using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyUtil.Text.Xsv
{
    public class XsvFieldUpdatedEventArgs<T> : EventArgs where T:new()
    {
        public T Fields { protected set; get; }
        public XsvDataRow<T> Row {protected set; get;}

        public XsvFieldUpdatedEventArgs(XsvDataRow<T> row, T fields)
        {
            this.Row = row;
            this.Fields = fields;
        }
    }
}
