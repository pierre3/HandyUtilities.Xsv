using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HandyUtil.Text.Xsv
{
    public class XsvData : XsvData<XsvDataRow> {

        public XsvData(IList<XsvDataRow> rows,ICollection<string> delimiters )
            : base(rows,delimiters)
        {}

        public XsvData(ICollection<string> delimiters)
            : base(delimiters)
        {}
    }
}
