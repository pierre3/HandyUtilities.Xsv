using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#if net40
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Reactive.Disposables;
#endif

namespace HandyUtil.Text.Xsv
{
    public class XsvReader : IDisposable
    {
        protected bool _endOfData = false;
        protected TextReader BaseReader { set; get; }

        public bool Disposed { get; protected set; }
        public bool EndOfData
        {
            get
            {
                if (!_endOfData)
                {
                    _endOfData = BaseReader.Peek() == -1;
                }
                return _endOfData;
            }
        }

        public XsvReader(Stream stream, Encoding encoding)
        {
            if (stream == null)
            { throw new ArgumentNullException("stream"); }

            BaseReader = new StreamReader(stream, encoding);
        }

        public XsvReader(Stream stream)
        {
            if (stream == null)
            { throw new ArgumentNullException("stream"); }
            BaseReader = new StreamReader(stream);
        }

        public XsvReader(TextReader reader)
        {
            if (reader == null)
            { throw new ArgumentNullException("reader"); }
            BaseReader = reader;
        }

        public void Dispose()
        {
            if (BaseReader != null)
            {
                BaseReader.Dispose();
                Disposed = true;
            }
        }

        public string ReadLine()
        {
            return BaseReader.ReadLine();
        }

        public IEnumerable<string> ReadXsvLine(ICollection<string> delimiters)
        {
            return Parse(ReadLine(), delimiters, () => ReadLine());
        }

        public IEnumerable<string[]> ReadXsvToEnd(ICollection<string> delimiters)
        {
            while (!EndOfData)
            {
                yield return ReadXsvLine(delimiters).ToArray();
            }
        }

        public static IEnumerable<string> Parse(string line, ICollection<string> delimiters, Func<string> followingLineSelector)
        {
            if (string.IsNullOrEmpty(line))
            { yield break; }

            var state = TokenState.Empty;
            string token = "";

            foreach (var c in line)
            {
            RESWITCH:
                switch (state)
                {
                    case TokenState.Empty:
                    case TokenState.AfterSeparator:
                        state = TokenState.Empty;
                        if (!delimiters.Any(s => s.StartsWith(c.ToString())) && char.IsWhiteSpace(c))
                        { break; }

                        if (c == '"')
                        {
                            state = TokenState.QuotedField;
                            token += c;
                            break;
                        }
                        state = TokenState.NormalField;
                        goto RESWITCH;

                    case TokenState.NormalField:
                        {
                            token += c;
                            var delimiter = delimiters.FirstOrDefault(s => token.ToString().EndsWith(s));
                            if (delimiter != null)
                            {
                                yield return token.Substring(0, token.Length - delimiter.Length).TrimEnd();
                                state = TokenState.AfterSeparator;
                                token = "";
                            }
                            break;
                        }

                    case TokenState.QuotedField:
                        token += c;
                        if (c == '"')
                        {
                            state = TokenState.EndQuote;
                        }
                        break;

                    case TokenState.EndQuote:
                        if (c == '"')
                        {
                            token += c;
                            state = TokenState.QuotedField;
                            break;
                        }

                        yield return token.Substring(1, token.Length - 2).Replace("\"\"", "\"");
                        token = "";
                        state = TokenState.AfterQuote;
                        goto RESWITCH;

                    case TokenState.AfterQuote:
                        {
                            token += c;
                            var delimiter = delimiters.FirstOrDefault(s => token.ToString().EndsWith(s));
                            if (delimiter != null)
                            {
                                state = TokenState.AfterSeparator;
                                token = "";
                            }
                            break;
                        }
                }
            }

            if (state == TokenState.AfterQuote)
            { yield break; }

            if (state == TokenState.QuotedField)
            {
                var next = Parse(token + Environment.NewLine + followingLineSelector(),
                    delimiters, followingLineSelector);
                foreach (var s in next)
                {
                    yield return s;
                }
            }
            else if (token != string.Empty || state == TokenState.AfterSeparator)
            {

                yield return (state == TokenState.EndQuote)
                    ? token.TrimEnd().Substring(1, token.Length - 2).Replace("\"\"", "\"")
                    : token.TrimEnd();
            }

        }

#if net45
        public async Task<string> ReadLineAsync()
        {
            return await BaseReader.ReadLineAsync();
        }

        public async Task<string[]> ReadXsvLineAsync(ICollection<string> delimiters)
        {
            return await Task.Run(() => ReadXsvLine(delimiters).ToArray());
        }

        public async Task<IList<string[]>> ReadXsvToEndAsync(ICollection<string> delimiters)
        {
            return await Task.Run(() => ReadXsvToEnd(delimiters).ToList());
        }

        public IObservable<string[]> ReadXsvObservable(ICollection<string> delimiters)
        {
            return Observable.Create<string[]>(async (observer, ct) =>
            {
                await SubscribeAsync(observer, ct, delimiters);
            });
        }

        protected async Task SubscribeAsync(IObserver<string[]> observer, CancellationToken ct, ICollection<string> delimiters)
        {
            var line = 1;
            try
            {
                while (!EndOfData)
                {
                    if (ct.IsCancellationRequested)
                    { break; }

                    var row = await ReadXsvLineAsync(delimiters);
                    observer.OnNext(row);
                    line++;
                }
                observer.OnCompleted();
            }
            catch (Exception e)
            {
                observer.OnError(new XsvReaderException(line, e.Message, e));
            }
        }
#else
#if net40
        public Task<string> ReadLineAsync()
        {
            return Task.Factory.StartNew(() => BaseReader.ReadLine());
        }

        public Task<string[]> ReadXsvLineAsync(ICollection<string> delimiters)
        {
            return Task.Factory.StartNew(() => ReadXsvLine(delimiters).ToArray());
        }

        public Task<IList<string[]>> ReadXsvToEndAsync(ICollection<string> delimiters)
        {
            return Task.Factory.StartNew(() => (IList<string[]>)ReadXsvToEnd(delimiters).ToList());
        }

        public IObservable<IEnumerable<string>> ReadXsvObservable(ICollection<string> delimiters)
        {
            return Observable.Create<IEnumerable<string>>(observer =>
            {
                var cts = new CancellationTokenSource();
                var disposable = Observable.FromAsync(ct => SubscribeAsync(observer, ct, delimiters))
                    .Subscribe(_ => { }, observer.OnError, observer.OnCompleted);
                return new CompositeDisposable(disposable, new CancellationDisposable(cts));
            });
        }

        protected Task SubscribeAsync(IObserver<string[]> observer, CancellationToken ct, ICollection<string> delimiters)
        {
            return Task.Factory.StartNew(() =>
            {
                var line = 1;
                try
                {
                    while (!EndOfData)
                    {
                        if (ct.IsCancellationRequested)
                        { break; }

                        var row = ReadXsvLine(delimiters).ToArray();
                        observer.OnNext(row);
                        line++;
                    }
                    observer.OnCompleted();
                }
                catch (Exception e)
                {
                    observer.OnError(new XsvReaderException(line, e.Message, e));
                }
            });
        }
#endif
#endif

        private enum TokenState
        {
            Empty,
            AfterSeparator,
            NormalField,
            QuotedField,
            EndQuote,
            AfterQuote
        }

    }
}
