using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HandyUtil.Text.Xsv
{

    public class XsvData<T> where T : XsvDataRow, new()
    {
        protected IList<T> _rows;

        public IList<T> Rows { get { return _rows; } }
        public ICollection<string> Delimiters { get; protected set; }
        public string DefaultColumnName { set; get; }

        public XsvData(ICollection<string> delimiters)
        {
            if (delimiters == null || delimiters.Count() == 0)
            { throw new ArgumentException("delimiters"); }

            this._rows = new List<T>();
            this.Delimiters = delimiters;
            this.DefaultColumnName = "_column_";
        }

        public XsvData(IList<T> rows, ICollection<string> delimiters)
            : this(delimiters)
        {
            this._rows = rows;
        }

        public void Read(TextReader reader, bool headerExists, IEnumerable<string> headerStrings = null)
        {
            if (reader == null)
            { throw new ArgumentNullException("reader"); }

            _rows.Clear();
            IEnumerable<string> header = null;
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                { break; }

                var tokens = Tokenize(line, this.Delimiters, () =>
                {
                    return reader.ReadLine() ?? string.Empty;
                });

                if (header == null)
                {
                    if (headerExists)
                    {
                        header = tokens.ToArray();
                        continue;
                    }
                    if (headerStrings == null)
                    {
                        header = Enumerable.Empty<string>();
                    }
                    else
                    {
                        header = headerStrings;
                    }
                }

                _rows.Add(CreateXsvRow(header, tokens));
            }

        }

        public void Write(TextWriter writer, bool headerExists, bool updateFields = true, string delimiter = null)
        {
            if (updateFields)
            {
                UpdateFields();
            }

            if (delimiter == null)
            {
                delimiter = Delimiters.First();
            }
            if (headerExists)
            {
                writer.WriteLine(Rows.First().OutputHeaders(Delimiters, delimiter));
            }
            foreach (var row in Rows)
            {
                writer.WriteLine(row.OutputFields(Delimiters, delimiter));
            }
        }

        public void Update()
        {
            UpdateFields();
        }

        protected virtual T CreateXsvRow(IEnumerable<string> header, IEnumerable<string> fields)
        {
            var row = new T();
            row.SetFields(header, fields, DefaultColumnName);
            return row;
        }

        protected virtual void UpdateFields()
        {
            foreach (var row in Rows)
            {
                row.Update();
            }
        }

        protected static IEnumerable<string> Tokenize(string line, ICollection<string> delimiters, Func<string> followingLineSelector)
        {
            if (string.IsNullOrEmpty(line))
            { yield break; }

            var hasWsDelimiter = delimiters.Any(s => s.TrimStart().Length != s.Length);
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
                        token += c;
                        var delimiter = delimiters.FirstOrDefault(s => token.ToString().EndsWith(s));
                        if (delimiter != null)
                        {
                            yield return token.Replace(delimiter, "").TrimEnd();
                            state = TokenState.AfterSeparator;
                            token = "";
                        }
                        break;

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

                        token = token.Substring(1, token.Length - 2).Replace("\"\"", "\"");
                        state = TokenState.NormalField;

                        goto RESWITCH;
                }

            }
            if (state == TokenState.QuotedField)
            {
                var next = Tokenize(token + Environment.NewLine + followingLineSelector(),
                    delimiters, followingLineSelector);
                foreach (var s in next)
                {
                    yield return s;
                }
            }
            else if (token != string.Empty || state == TokenState.AfterSeparator)
            {

                yield return (state == TokenState.EndQuote)
                    ? token.Substring(1, token.Length - 2).Replace("\"\"", "\"")
                    : token;
            }

        }

        private enum TokenState
        {
            Empty,
            AfterSeparator,
            NormalField,
            QuotedField,
            EndQuote
        }
    }
}
