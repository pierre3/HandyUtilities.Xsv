
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandyUtil.Extensions.System;
using HandyUtil.Extensions.System.Linq;

namespace HandyUtil.Text.Xsv
{
    public class XsvColumnHeaders<T> : IDictionary<string, XsvField> where T:XsvDataRow, new()
    {
        protected T _items;

        public XsvColumnHeaders(IEnumerable<string> source)
        {
            this._items = new T();
            this._items.SetFields(source, Enumerable.Empty<string>(), "unnamed");
        }

        public XsvColumnHeaders(IEnumerable<KeyValuePair<string,XsvField>> source)
        {
            this._items = new T();
            this._items.SetFields(source);
        }

        public string OutputString(IEnumerable<string> delimiters, string delimiter) 
        {
            return _items.Keys.Select(head => head.MakeXsvField(delimiters)).ConcatWith(delimiter);
        }

        public IEnumerable<T> SynchronizeColumns(IEnumerable<T> rows)
        {
            foreach (var row in rows)
            {
                var nextRow = _items.Select(header =>
                {
                    if (row.ContainsKey(header.Key))
                    {
                        return new KeyValuePair<string, XsvField>(header.Key, row[header.Key]);
                    }
                    return new KeyValuePair<string, XsvField>(header.Key, header.Value);
                });
                row.SetFields(nextRow);
                yield return row;
            }
        }

        public void Add(string key, XsvField value)
        {
            _items.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _items.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return _items.Keys; }
        }

        public bool Remove(string key)
        {
            return _items.Remove(key);
        }

        public bool TryGetValue(string key, out XsvField value)
        {
            return _items.TryGetValue(key, out value);
        }

        public ICollection<XsvField> Values
        {
            get { return _items.Values; }
        }

        public XsvField this[string key]
        {
            get
            {
                return _items[key];
            }
            set
            {
                _items[key] = value;
            }
        }

        public void Add(KeyValuePair<string, XsvField> item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(KeyValuePair<string, XsvField> item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, XsvField>[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public bool IsReadOnly
        {
            get { return _items.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, XsvField> item)
        {
            return _items.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, XsvField>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
