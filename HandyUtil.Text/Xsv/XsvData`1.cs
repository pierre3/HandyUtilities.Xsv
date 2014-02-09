using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections;

namespace HandyUtil.Text.Xsv
{
    public class XsvData<T> where T : XsvDataRow, new()
    {
        protected XsvColumnHeaders<T> _columnHeaders;
        protected IList<T> _rows;

        public IEnumerable<KeyValuePair<string, XsvField>> ColumnHeaders
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

        public async Task<IDisposable> ReadAsyncObservable(XsvReader xsvReader, bool headerExists, IEnumerable<string> headerStrings = null,
            Action<Exception> OnError = null, Action OnCompleded = null)
        {
            if (xsvReader == null)
            { throw new ArgumentNullException("xsvReader"); }

            _rows.Clear();

            var headers = await ReadHeaderAsync(xsvReader, headerExists, headerStrings);
            _columnHeaders = new XsvColumnHeaders<T>(headers);
            return xsvReader.AsObservable(Delimiters).Subscribe(
                row =>
                {
                    _rows.Add(CreateXsvRow(headers, row));
                },
                OnError ?? (_ => { }),
                OnCompleded ?? (() => { }));
        }

        protected async Task<IList<string>> ReadHeaderAsync(XsvReader xsvReader, bool headerExists, IEnumerable<string> headerStrings)
        {
            var header = headerStrings ?? Enumerable.Empty<string>();
            if (headerExists)
            {
                header = await xsvReader.ReadXsvLineAsync(Delimiters);
            }
            return header.ToList();
        }

        protected IList<string> ReadHeader(XsvReader xsvReader, bool headerExists, IEnumerable<string> headerStrings)
        {
            var header = headerStrings ?? Enumerable.Empty<string>();
            if (headerExists)
            {
                header = xsvReader.ReadXsvLine(Delimiters);
            }
            return header.ToList();
        }

        public async Task ReadAsync(XsvReader xsvReader, bool headerExists, IEnumerable<string> headerStrings = null)
        {
            if (xsvReader == null)
            { throw new ArgumentNullException("xsvReader"); }
            _rows.Clear();

            var headers = await ReadHeaderAsync(xsvReader, headerExists, headerStrings);
            _columnHeaders = new XsvColumnHeaders<T>(headers);
            var rows = await xsvReader.ReadXsvToEndAsync(Delimiters);
            foreach (var row in rows)
            {
                _rows.Add(CreateXsvRow(headers, row));
            }
        }

        public void Read(XsvReader xsvReader, bool headerExists, IEnumerable<string> headerStrings = null)
        {
            if (xsvReader == null)
            { throw new ArgumentNullException("xsvReader"); }
            _rows.Clear();

            var headers = ReadHeader(xsvReader, headerExists, headerStrings);
            _columnHeaders = new XsvColumnHeaders<T>(headers);
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

            if (settings.SynchronisesColumns)
            {
                SynchronizeColumns();
            }

            if (settings.UpdateFields)
            {
                UpdateFields();
            }

            if (settings.OutputsHeader)
            {
                writer.WriteLine(_columnHeaders.OutputString(Delimiters, delimiter));
            }

            foreach (var row in Rows)
            {
                writer.WriteLine(row.OutputFields(Delimiters, delimiter));
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

        public void SetColumnHeaders(IEnumerable<KeyValuePair<string, XsvField>> source)
        {
            _columnHeaders = new XsvColumnHeaders<T>(source);
        }

        public void AddColumnHeader(string name, XsvField defaultField)
        {
            _columnHeaders.Add(name, defaultField);
        }

        public bool RemoveColumnHeader(string name)
        {
            return _columnHeaders.Remove(name);
        }

        public bool RemoveColumnHeader(KeyValuePair<string, XsvField> item)
        {
            return _columnHeaders.Remove(item);
        }

        public void InsertColumnHeader(int index, string name, XsvField defaultField)
        {
            (_columnHeaders as IList<KeyValuePair<string, XsvField>>).Insert(index, new KeyValuePair<string, XsvField>(name, defaultField));
        }

        public class WriterSettings
        {
            public static readonly WriterSettings Default = new WriterSettings()
            {
                OutputsHeader = true,
                UpdateFields = true,
                SynchronisesColumns = true
            };
            public bool OutputsHeader { set; get; }
            public bool UpdateFields { set; get; }
            public bool SynchronisesColumns { set; get; }
        }
    }
}
