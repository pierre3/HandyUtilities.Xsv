using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#if net40 || net45
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Threading;
#endif

namespace HandyUtil.Text.Xsv
{

    public class XsvData<T> where T : XsvDataRow, new()
    {
        protected XsvColumnHeaders _columnHeaders;
        protected IList<T> _items;

        public IEnumerable<string> ColumnHeaders
        {
            get { return _columnHeaders.AsEnumerable(); }
        }

        public IList<T> Rows
        {
            get { return _items; }
        }

        public XsvDataSettings Settings { get; protected set; }

        public XsvData()
        {
            this.Settings = new XsvDataSettings();
            this._items = new List<T>();
        }

        public XsvData(XsvDataSettings settings)
        {
            if (settings.Delimiters == null || settings.Delimiters.Count() == 0)
            { throw new ArgumentException("settings.Delimiters"); }

            this.Settings = settings;
            this._items = new List<T>();
        }

        public XsvData(ICollection<string> delimiters)
            : this(new XsvDataSettings() { Delimiters = delimiters })
        { }

        public XsvData(IList<T> rows, ICollection<string> delimiters)
            : this(delimiters)
        {
            this._items = rows;
        }

        public XsvData(IList<T> rows, XsvDataSettings settings)
            : this(settings)
        {
            this._items = rows;
        }

        public void Read(TextReader reader)
        {
            if (reader == null)
            { throw new ArgumentNullException("reader"); }

            using (var xsvReader = new XsvReader(reader, Settings.CommentToken))
            {
                var headers = ReadHeader(xsvReader);
                _columnHeaders = new XsvColumnHeaders(headers);
                var rows = xsvReader.ReadXsvToEnd(Settings.Delimiters);

                _items.Clear();
                foreach (var row in rows)
                {
                    _items.Add(CreateXsvRow(headers, row));
                }
            }
        }

        public void Read(Stream stream, Encoding encoding = null)
        {
            if (stream == null)
            { throw new ArgumentNullException("stream"); }

            using (var reader = new StreamReader(stream, encoding))
            {
                Read(reader);
            }
        }

#if net45
        
        public async Task ReadAsync(TextReader reader)
        {
            if (reader == null)
            { throw new ArgumentNullException("reader"); }

            using (var xsvReader = new XsvReader(reader, Settings.CommentToken))
            {
                var headers = await ReadHeaderAsync(xsvReader);
                _columnHeaders = new XsvColumnHeaders(headers);
                var rows = await xsvReader.ReadXsvToEndAsync(Settings.Delimiters);

                _items.Clear();
                foreach (var row in rows)
                {
                    _items.Add(CreateXsvRow(headers, row));
                }
            }
        }

        public async Task ReadAsync(Stream stream, Encoding encoding = null)
        {
            if (stream == null)
            { throw new ArgumentNullException("stream"); }

            using (var reader = new StreamReader(stream, encoding))
            {
                await ReadAsync(reader);
            }
        }

        public async Task ReadAsync(TextReader reader, CancellationToken cancellationToken)
        {
            if (reader == null)
            { throw new ArgumentNullException("reader"); }

            if (cancellationToken == null)
            { throw new ArgumentNullException("cancellationToken"); }

            if (cancellationToken.IsCancellationRequested)
            { return; }

            using (var xsvReader = new XsvReader(reader, Settings.CommentToken))
            {
                var headers = await ReadHeaderAsync(xsvReader);
                _columnHeaders = new XsvColumnHeaders(headers);

                _items.Clear();
                while (!xsvReader.EndOfData)
                {
                    if (cancellationToken.IsCancellationRequested) 
                    { return; }

                    var row = await xsvReader.ReadXsvLineAsync(Settings.Delimiters);
                    _items.Add(CreateXsvRow(headers, row));
                }
            }
        }

        public async Task ReadAsync(Stream stream, CancellationToken cancellationToken, Encoding encoding = null)
        {
            if (stream == null)
            { throw new ArgumentNullException("stream"); }

            if (cancellationToken == null)
            { throw new ArgumentNullException("cancellationToken"); }

            if (cancellationToken.IsCancellationRequested)
            { return; }

            using (var reader = new StreamReader(stream, encoding))
            {
                await ReadAsync(reader, cancellationToken);
            }
        }
#endif

        public void Write(TextWriter writer, string delimiter = null, XsvWriteSettings writerSettings = null)
        {
            if (writerSettings == null)
            {
                writerSettings = XsvWriteSettings.Default;
            }
            if (delimiter == null)
            {
                delimiter = Settings.Delimiters.First();
            }

            if (writerSettings.SynchronisesColumn)
            {
                SynchronizeColumns();
            }

            if (writerSettings.OutputsHeader)
            {
                writer.WriteLine(_columnHeaders.OutputString(Settings.Delimiters, delimiter));
            }

            foreach (var row in Rows)
            {
                writer.WriteLine(row.OutputFields(Settings.Delimiters, delimiter, writerSettings.UpdatesField));
            }
        }

        public void SynchronizeColumns()
        {
            _items = _columnHeaders.SynchronizeColumns(_items).ToList();
        }

        public void Update()
        {
            UpdateFields();
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

        protected virtual T CreateXsvRow(IEnumerable<string> header, IEnumerable<string> fields)
        {
            var row = new T();
            row.SetFields(header, fields, Settings.DefaultColumnName);
            return row;
        }

        protected virtual void UpdateFields()
        {
            foreach (var row in Rows)
            {
                row.Update();
            }
        }

        protected IList<string> ReadHeader(XsvReader xsvReader)
        {
            if (xsvReader == null)
            { throw new ArgumentNullException("xsvReader"); }

            var header = Settings.HeaderStrings ?? new List<string>();
            if (Settings.HeaderExists)
            {
                header = xsvReader.ReadXsvLine(Settings.Delimiters).ToList();
            }
            return header;
        }

#if net45
        protected async Task<IList<string>> ReadHeaderAsync(XsvReader xsvReader)
        {
            var header = Settings.HeaderStrings ?? Enumerable.Empty<string>();
            if (Settings.HeaderExists)
            {
                header = await xsvReader.ReadXsvLineAsync(Settings.Delimiters);
            }
            return header.ToList();
        }
#endif

        
    }
}
