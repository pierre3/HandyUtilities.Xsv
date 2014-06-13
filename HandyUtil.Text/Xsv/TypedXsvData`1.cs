using System;
using System.Collections.Generic;
namespace HandyUtil.Text.Xsv
{
    public class TypedXsvData<T> : XsvData<XsvDataRow<T>> where T : new()
    {
        public bool IsAutoBinding { set; get; }

        public EventHandler<XsvFieldUpdatedEventArgs<T>> Attached;
        public EventHandler<XsvFieldUpdatedEventArgs<T>> Updated;

        public TypedXsvData(ICollection<string> delimiters, bool isAutoBinding = false)
            : base(delimiters)
        { this.IsAutoBinding = isAutoBinding; }

        public TypedXsvData(IList<XsvDataRow<T>> rows, ICollection<string> delimiters, bool isAutoBinding = false)
            : base(rows, delimiters)
        { this.IsAutoBinding = isAutoBinding; }

        public TypedXsvData(XsvDataSettings settings, bool isAutoBinding = false)
            : base(settings)
        { this.IsAutoBinding = isAutoBinding; }

        public TypedXsvData(IList<XsvDataRow<T>> rows, XsvDataSettings settings, bool isAutoBinding = false)
            : base(rows, settings)
        { this.IsAutoBinding = isAutoBinding; }

        protected override XsvDataRow<T> CreateXsvRow(IEnumerable<string> header, IEnumerable<string> fields)
        {
            var row = new XsvDataRow<T>(IsAutoBinding);
            row.Attached += OnAttached;
            row.Updated += OnUpdated;
            row.SetFields(header, fields, Settings.DefaultColumnName);
            return row;
        }

        private void OnAttached(object sender, XsvFieldUpdatedEventArgs<T> e)
        {
            var handler = Attached;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        private void OnUpdated(object sender, XsvFieldUpdatedEventArgs<T> e)
        {
            var handler = Updated;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
    }
}
