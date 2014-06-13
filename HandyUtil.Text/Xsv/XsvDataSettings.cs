using System.Collections.Generic;

namespace HandyUtil.Text.Xsv
{
    public class XsvDataSettings
    {
        public ICollection<string> Delimiters { set; get; }
        public bool HeaderExists { set; get; }
        public IList<string> HeaderStrings { set; get; }
        public string DefaultColumnName { set; get; }
        public string CommentToken { set; get; }
        public XsvDataSettings()
        {
            this.HeaderExists = true;
            this.Delimiters = new[] { "," };
            this.DefaultColumnName = "_column_";
            this.CommentToken = null;
            this.HeaderStrings = null;
        }
    }
}
