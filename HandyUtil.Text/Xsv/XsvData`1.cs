using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;

#if net40
using System.Threading.Tasks;
using System.Threading;
using System.Reactive.Disposables;
#endif

namespace HandyUtil.Text.Xsv
{
    public class XsvData<T> where T : XsvDataRow, new()
    {
        protected XsvColumnHeaders _columnHeaders;
        protected IList<T> _rows;

        public IEnumerable<string> ColumnHeaders
        {
            get { return _columnHeaders.AsEnumerable(); }
        }

        public IList<T> Rows
        {
            get { return _rows; }
        }

        public ICollection<string> Delimiters { get; protected set; }

        public string DefaultColumnName { set; get; }

        public XsvData(ICollection<string> delimiters, string defaultColumnName = "_column_")
        {
            if (delimiters == null || delimiters.Count() == 0)
            { throw new ArgumentException("delimiters"); }

            this._rows = new List<T>();
            this.Delimiters = delimiters;
            this.DefaultColumnName = defaultColumnName;
        }

        public XsvData(IList<T> rows, ICollection<string> delimiters)
            : this(delimiters)
        {
            this._rows = rows;
        }

        protected IList<string> ReadHeader(XsvReader xsvReader, bool headerExists, IEnumerable<string> headerStrings = null)
        {
            var header = headerStrings ?? Enumerable.Empty<string>();
            if (headerExists)
            {
                header = xsvReader.ReadXsvLine(Delimiters);
            }
            return header.ToList();
        }

        public void Read(XsvReader xsvReader, bool headerExists, IEnumerable<string> headerStrings = null)
        {
            if (xsvReader == null)
            { throw new ArgumentNullException("xsvReader"); }
            _rows.Clear();

            var headers = ReadHeader(xsvReader, headerExists, headerStrings);
            _columnHeaders = new XsvColumnHeaders(headers);
            var rows = xsvReader.ReadXsvToEnd(Delimiters);
            foreach (var row in rows)
            {
                _rows.Add(CreateXsvRow(headers, row));
            }
        }

        public void Read(Stream stream, bool headerExists, IEnumerable<string> headerStrings = null, Encoding encoding = null)
        {
            if (stream == null)
            { throw new ArgumentNullException("stream"); }

            using (var xsvReader = new XsvReader(stream, encoding))
            {
                Read(xsvReader, headerExists, headerStrings);
            }
        }

#if net45
        public async Task<IDisposable> ReadAsyncObservable(XsvReader xsvReader, bool headerExists, IEnumerable<string> headerStrings = null,
            Action<string[]> OnNext = null, Action<Exception> OnError = null, Action OnCompleded = null)
        {
            if (xsvReader == null)
            { throw new ArgumentNullException("xsvReader"); }

            _rows.Clear();

            var headers = await ReadHeaderAsync(xsvReader, headerExists, headerStrings);
            _columnHeaders = new XsvColumnHeaders(headers);
            return xsvReader.ReadXsvObservable(Delimiters).Subscribe(
                row =>
                {
                    _rows.Add(CreateXsvRow(headers, row));
                    if (OnNext != null)
                    {
                        OnNext(row);
                    }
                },
                OnError ?? (_ => { }),
                OnCompleded ?? (() => { }));
        }

        protected async Task<IList<string>> ReadHeaderAsync(XsvReader xsvReader, bool headerExists, IEnumerable<string> headerStrings = null)
        {
            var header = headerStrings ?? Enumerable.Empty<string>();
            if (headerExists)
            {
                header = await xsvReader.ReadXsvLineAsync(Delimiters);
            }
            return header.ToList();
        }

        public async Task ReadAsync(XsvReader xsvReader, bool headerExists, IEnumerable<string> headerStrings = null)
        {
            if (xsvReader == null)
            { throw new ArgumentNullException("xsvReader"); }
            _rows.Clear();

            var headers = await ReadHeaderAsync(xsvReader, headerExists, headerStrings);
            _columnHeaders = new XsvColumnHeaders(headers);
            var rows = await xsvReader.ReadXsvToEndAsync(Delimiters);
            foreach (var row in rows)
            {
                _rows.Add(CreateXsvRow(headers, row));
            }
        }
#else
#if net40
        protected Task<IList<string>> ReadHeaderAsync(XsvReader xsvReader, bool headerExists, IEnumerable<string> headerStrings = null)
        {
            return Task.Factory.StartNew(() =>
            {
                var header = headerStrings ?? Enumerable.Empty<string>();
                if (headerExists)
                {
                    header = xsvReader.ReadXsvLine(Delimiters);
                }
                return (IList<string>)header.ToList();
            });
        }

        public Task ReadAsync(XsvReader xsvReader, bool headerExists, IEnumerable<string> headerStrings = null)
        {
            if (xsvReader == null)
            { throw new ArgumentNullException("xsvReader"); }
            _rows.Clear();

            return ReadHeaderAsync(xsvReader, headerExists, headerStrings).ContinueWith(readHeaderTask=>
            {
                var headers = readHeaderTask.Result;
                _columnHeaders = new XsvColumnHeaders(headers);

                return xsvReader.ReadXsvToEndAsync(Delimiters).ContinueWith(readTask =>
                {
                    var rows = readTask.Result;
                    foreach (var row in rows)
                    {
                        _rows.Add(CreateXsvRow(headers, row));
                    }
                });
            });
        }
#endif
#endif

        public void Write(TextWriter writer, string delimiter = null, WriterSettings settings = null)
        {
            if (settings == null)
            {
                settings = WriterSettings.Default;
            }
            if (delimiter == null)
            {
                delimiter = Delimiters.First();
            }

            if (settings.ColumnSynchronises)
            {
                SynchronizeColumns();
            }

            if (settings.HeaderOutputs)
            {
                writer.WriteLine(_columnHeaders.OutputString(Delimiters, delimiter));
            }

            foreach (var row in Rows)
            {
                writer.WriteLine(row.OutputFields(Delimiters, delimiter, settings.FieldUpdates));
            }
        }

        public void SynchronizeColumns()
        {
            _rows = _columnHeaders.SynchronizeColumns(_rows).ToList();
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

        public void SetColumnHeaders(IEnumerable<string> source)
        {
            _columnHeaders = new XsvColumnHeaders(source);
        }

        public void AddColumnHeader(string name)
        {
            _columnHeaders.Add(name);
        }

        public bool RemoveColumnHeader(string name)
        {
            return _columnHeaders.Remove(name);
        }

        public void InsertColumnHeader(int index, string name)
        {
            _columnHeaders.Insert(index, name);
        }

        public void SwapColumnHeader(string column1, string column2)
        {
            if (column1 == column2)
            { return; }

            var indexOf1 = _columnHeaders.IndexOf(column1);
            if (indexOf1 < 0)
            {
                throw new ArgumentException("column1");
            }
            var indexOf2 = _columnHeaders.IndexOf(column2);
            if (indexOf2 < 0)
            {
                throw new ArgumentException("column2");
            }
            if (indexOf1 < indexOf2)
            {
                _columnHeaders.RemoveAt(indexOf1);
                _columnHeaders.RemoveAt(indexOf2 - 1);
                _columnHeaders.Insert(indexOf1, column2);
                _columnHeaders.Insert(indexOf2, column1);
            }
            else
            {
                _columnHeaders.RemoveAt(indexOf2);
                _columnHeaders.RemoveAt(indexOf1 - 1);
                _columnHeaders.Insert(indexOf2, column1);
                _columnHeaders.Insert(indexOf1, column2);
            }

        }

        public class WriterSettings
        {
            public static readonly WriterSettings Default = new WriterSettings()
            {
                HeaderOutputs = true,
                FieldUpdates = true,
                ColumnSynchronises = true
            };
            public bool HeaderOutputs { set; get; }
            public bool FieldUpdates { set; get; }
            public bool ColumnSynchronises { set; get; }
        }
    }
}
