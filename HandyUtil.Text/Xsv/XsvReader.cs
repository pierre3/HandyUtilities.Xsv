using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#if net40 || net45
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Threading;
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

        public string CommentString { get; set; }

        public XsvReader(Stream stream, Encoding encoding, string commentString = null)
        {
            if (stream == null)
            { throw new ArgumentNullException("stream"); }

            this.BaseReader = new StreamReader(stream, encoding);
            this.CommentString = commentString;
        }

        public XsvReader(Stream stream, string commentString = null)
        {
            if (stream == null)
            { throw new ArgumentNullException("stream"); }
            this.BaseReader = new StreamReader(stream);
            this.CommentString = commentString;
        }

        public XsvReader(TextReader reader, string commentString = null)
        {
            if (reader == null)
            { throw new ArgumentNullException("reader"); }
            this.BaseReader = reader;
            this.CommentString = commentString;
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
            if (string.IsNullOrEmpty(CommentString))
            {
                return BaseReader.ReadLine();
            }

            string line = "";
            do
            {
                line = BaseReader.ReadLine();
            } while (line != null && line.StartsWith(CommentString));
            return line;
        }

        public IEnumerable<string> ReadXsvLine(ICollection<string> delimiters)
        {
            return Parse(ReadLine(), delimiters, () => ReadLine());
        }

        public IEnumerable<IList<string>> ReadXsvToEnd(ICollection<string> delimiters)
        {
            while (!EndOfData)
            {
                yield return ReadXsvLine(delimiters).ToList();
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
                switch (state)
                {
                    case TokenState.Empty:
                    case TokenState.AfterSeparator:
                        state = TokenState.Empty;
                        if (char.IsWhiteSpace(c) && !delimiters.Any(s => s.StartsWith(c.ToString())))
                        { break; }

                        if (c == '"')
                        {
                            state = TokenState.QuotedField;
                            token += c;
                            break;
                        }
                        state = TokenState.NormalField;
                        goto case TokenState.NormalField;

                    case TokenState.NormalField:
                        {
                            token += c;
                            var delimiter = delimiters.FirstOrDefault(s => token.EndsWith(s));
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
                        goto case TokenState.AfterQuote;

                    case TokenState.AfterQuote:
                        {
                            token += c;
                            var delimiter = delimiters.FirstOrDefault(s => token.EndsWith(s));
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
        public async static Task<IList<string>> Parse(string line, ICollection<string> delimiters, Func<Task<string>> followingLineSelector)
        {
            var result = new List<string>();

            if (string.IsNullOrEmpty(line))
            { return result; }

            var state = TokenState.Empty;
            string token = "";

            foreach (var c in line)
            {
                switch (state)
                {
                    case TokenState.Empty:
                    case TokenState.AfterSeparator:
                        state = TokenState.Empty;
                        if (char.IsWhiteSpace(c) && !delimiters.Any(s => s.StartsWith(c.ToString())))
                        { break; }

                        if (c == '"')
                        {
                            state = TokenState.QuotedField;
                            token += c;
                            break;
                        }
                        state = TokenState.NormalField;
                        goto case TokenState.NormalField;

                    case TokenState.NormalField:
                        {
                            token += c;
                            var delimiter = delimiters.FirstOrDefault(s => token.EndsWith(s));
                            if (delimiter != null)
                            {
                                result.Add(token.Substring(0, token.Length - delimiter.Length).TrimEnd());
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

                        result.Add(token.Substring(1, token.Length - 2).Replace("\"\"", "\""));
                        token = "";
                        state = TokenState.AfterQuote;
                        goto case TokenState.AfterQuote;

                    case TokenState.AfterQuote:
                        {
                            token += c;
                            var delimiter = delimiters.FirstOrDefault(s => token.EndsWith(s));
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
            { return result; }

            if (state == TokenState.QuotedField)
            {
                var followingLine = await followingLineSelector().ConfigureAwait(false);
                var next = await Parse(token + Environment.NewLine + followingLine,
                    delimiters, followingLineSelector);
                result.AddRange(next);

            }
            else if (token != string.Empty || state == TokenState.AfterSeparator)
            {
                var tail = (state == TokenState.EndQuote)
                    ? token.TrimEnd().Substring(1, token.Length - 2).Replace("\"\"", "\"")
                    : token.TrimEnd();
                result.Add(tail);
            }
            return result;
        }

        public async Task<string> ReadLineAsync()
        {
            if (string.IsNullOrEmpty(CommentString))
            {
                return await BaseReader.ReadLineAsync();
            }

            string line = "";
            do
            {
                line = await BaseReader.ReadLineAsync();
            } while (line != null && line.StartsWith(CommentString));
            return line;

        }

        public async Task<IList<string>> ReadXsvLineAsync(ICollection<string> delimiters)
        {
            return await Parse(await ReadLineAsync().ConfigureAwait(false), delimiters, () => ReadLineAsync());
        }

        public async Task<IList<IList<string>>> ReadXsvToEndAsync(ICollection<string> delimiters)
        {
            var result = new List<IList<string>>();
            while (!EndOfData)
            {
                var line = await ReadXsvLineAsync(delimiters).ConfigureAwait(false);
                result.Add(line);
            }
            return result;
        }

        public IObservable<IList<string>> ReadXsvAsObservable(ICollection<string> delimiters)
        {
            return Observable.Create<IList<string>>(async (observer, ct) =>
            {
                var line = 1;
                try
                {
                    while (!EndOfData)
                    {
                        if (ct.IsCancellationRequested)
                        { break; }

                        var row = await ReadXsvLineAsync(delimiters).ConfigureAwait(false);
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
#else
#if net40
        public IObservable<IList<string>> ReadXsvAsObservable(ICollection<string> delimiters)
        {
            return ReadXsvAsObservable(delimiters, null);     
        }

        public IObservable<IList<string>> ReadXsvAsObservable(ICollection<string> delimiters, System.Reactive.Concurrency.IScheduler scheduler)
        {
            if (scheduler == null)
            {
                scheduler = System.Reactive.Concurrency.ThreadPoolScheduler.Instance;
            }

            return Observable.Create<IList<string>>(observer =>
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                var disposable = Observable.Start(()=>
                {
                    var line = 1;
                    try
                    {
                        while (!EndOfData)
                        {
                            if (cts.IsCancellationRequested)
                            { break; }
                            
                            var row = ReadXsvLine(delimiters).ToList();
                            observer.OnNext(row);
                            line++;
                        }
                        observer.OnCompleted();
                    }
                    catch (Exception e)
                    {
                        observer.OnError(new XsvReaderException(line, e.Message, e));
                    }    
                }, scheduler).Subscribe(_ => { }, observer.OnError, observer.OnCompleted);

                return new CompositeDisposable(disposable, new CancellationDisposable(cts));
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
