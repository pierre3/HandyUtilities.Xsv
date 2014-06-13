using HandyUtil.Extensions.System;
using HandyUtil.Extensions.System.Linq;
using System.Collections.Generic;
using System.Linq;

namespace HandyUtil.Text.Xsv
{

    public partial class XsvDataRow : IDictionary<string, XsvField>
    {
        protected IDictionary<string, XsvField> _items;

        public XsvDataRow()
        {
            _items = new Dictionary<string, XsvField>();
        }

        protected virtual void AttachFields()
        { }

        protected virtual void UpdateFields()
        { }

        public void Update()
        {
            UpdateFields();
        }

        public void SetFields(IEnumerable<string> columnHeader, IEnumerable<string> row, string defaultColumnName)
        {
            _items = columnHeader.Zip(row, (n, a, b) => new { Key = a, Value = new XsvField(b) },
                n => defaultColumnName + n, _ => "").ToDictionary(kv => kv.Key, kv => kv.Value);

            AttachFields();
        }

        public void SetFields(IEnumerable<KeyValuePair<string, XsvField>> collection)
        {
            _items = collection.ToDictionary(item => item.Key, item => item.Value);
        }
        
        public string OutputHeaders(IEnumerable<string> delimiters, string delimiter)
        {
            return _items.Keys.Select(head => head.MakeXsvField(delimiters)).ConcatWith(delimiter);
        }

        public string OutputFields(IEnumerable<string> delimiters, string delimiter, bool updateFields = true)
        {
            if (updateFields)
            { UpdateFields(); }

            return _items.Values.Select(field => field.ToString(delimiters)).ConcatWith(delimiter);
        }

        public override string ToString()
        {
            return _items.ConcatWith(", ");
        }

        #region Implements of IDictionary<XsvHeader, XsvField> Interface

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
            ((ICollection<KeyValuePair<string, XsvField>>)_items).Add(item);
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
            ((ICollection<KeyValuePair<string, XsvField>>)_items).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, XsvField> item)
        {
            return ((ICollection<KeyValuePair<string, XsvField>>)_items).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, XsvField>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)_items).GetEnumerator();
        }
        #endregion
    }
}