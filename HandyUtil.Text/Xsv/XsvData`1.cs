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

            var header = await ReadHeaderAsync(xsvReader, headerExists, headerStrings);
            return xsvReader.AsObservable(Delimiters).Subscribe(
                row =>
                {
                    _rows.Add(CreateXsvRow(header, row));
                },
                OnError ?? (_ => { }),
                OnCompleded ?? (() => { }));
        }

        protected async Task<IEnumerable<string>> ReadHeaderAsync(XsvReader xsvReader, bool headerExists, IEnumerable<string> headerStrings)
        {
            var header = headerStrings ?? Enumerable.Empty<string>();
            if (headerExists)
            {
                header = await xsvReader.ReadXsvLineAsync(Delimiters);
            }
            return header;
        }

        protected IEnumerable<string> ReadHeader(XsvReader xsvReader, bool headerExists, IEnumerable<string> headerStrings)
        {
            var header = headerStrings ?? Enumerable.Empty<string>();
            if (headerExists)
            {
                header = xsvReader.ReadXsvLine(Delimiters);
            }
            return header;
        }

        public async Task ReadAsync(XsvReader xsvReader, bool headerExists, IEnumerable<string> headerStrings=null){
            if (xsvReader == null)
            { throw new ArgumentNullException("xsvReader"); }
            _rows.Clear();
            
            var header = await ReadHeaderAsync(xsvReader, headerExists, headerStrings);
            var rows = await xsvReader.ReadXsvToEndAsync(Delimiters);
            foreach (var row in rows)
            {
                _rows.Add(CreateXsvRow(header, row));
            }
        }

        public void Read(XsvReader xsvReader, bool headerExists, IEnumerable<string> headerStrings = null)
        {
            if (xsvReader == null)
            { throw new ArgumentNullException("xsvReader"); }
            _rows.Clear();

            var header = ReadHeader(xsvReader, headerExists, headerStrings);
            var rows = xsvReader.ReadXsvToEnd(Delimiters);
            foreach (var row in rows)
            {
                _rows.Add(CreateXsvRow(header, row));
            }
        }

        public void Read(Stream stream, bool headerExists, IEnumerable<string> headerStrings = null, Encoding encoding = null)
        {
            if(stream==null)
            { throw new ArgumentNullException("stream"); }

            using (var xsvReader = new XsvReader(stream, encoding))
            {
                Read(xsvReader, headerExists, headerStrings);
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

       
    }
}
