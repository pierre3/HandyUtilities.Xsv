
namespace HandyUtil.Text.Xsv
{
    public class XsvWriteSettings
    {
        public static readonly XsvWriteSettings Default = new XsvWriteSettings()
        {
            OutputsHeader = true,
            UpdatesField = true,
            SynchronisesColumn = true
        };
        public bool OutputsHeader { set; get; }
        public bool UpdatesField { set; get; }
        public bool SynchronisesColumn { set; get; }
    }
}
