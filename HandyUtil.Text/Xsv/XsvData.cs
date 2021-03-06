﻿using System.Collections.Generic;

namespace HandyUtil.Text.Xsv
{
    public class XsvData : XsvData<XsvDataRow>
    {
        public XsvData(IList<XsvDataRow> rows, ICollection<string> delimiters)
            : base(rows, delimiters)
        { }

        public XsvData(ICollection<string> delimiters)
            : base(delimiters)
        { }

        public XsvData(IList<XsvDataRow> rows, XsvDataSettings settings)
            : base(rows, settings)
        { }

        public XsvData(XsvDataSettings settings)
            : base(settings)
        { }
    }
}
