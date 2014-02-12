
using HandyUtil.Extensions.System;
using HandyUtil.Extensions.System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HandyUtil.Text.Xsv
{
    public class XsvColumnHeaders : IList<string>
    {
        protected IList<string> _items;

        public XsvColumnHeaders(IEnumerable<string> source)
        {
            _items = source.Distinct().ToList();
        }

        public string OutputString(IEnumerable<string> delimiters, string delimiter) 
        {
            return _items.Select(item=>item.MakeXsvField(delimiters)).ConcatWith(delimiter);
        }

        public IEnumerable<T> SynchronizeColumns<T>(IEnumerable<T> rows) where T:XsvDataRow
        {
            foreach (var row in rows)
            {
                var nextRow = _items.Select(header =>
                {
                    if (row.ContainsKey(header))
                    {
                        return new KeyValuePair<string, XsvField>(header, row[header]);
                    }
                    return new KeyValuePair<string, XsvField>(header, new XsvField(""));
                });
                row.SetFields(nextRow);
                yield return row;
            }
        }

        public int IndexOf(string item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            if (_items.Contains(item))
            {
                throw new ArgumentException("Same key already exists.", item);
            }
            _items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        public string this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                _items[index] = value;
            }
        }

        public void Add(string item)
        {
            if (_items.Contains(item))
            {
                throw new ArgumentException("Same key already exists.", item);
            }
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(string item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
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

        public bool Remove(string item)
        {
            return _items.Remove(item);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
