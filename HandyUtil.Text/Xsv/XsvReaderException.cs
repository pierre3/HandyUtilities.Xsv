using System;

namespace HandyUtil.Text.Xsv
{
    public class XsvReaderException : Exception
    {
        public int Line { protected set; get; }
        public XsvReaderException(int line)
        {
            this.Line = line;
        }

        public XsvReaderException(int line, string message)
            :base(message)
        {
            this.Line = line;
        }

        public XsvReaderException(int line, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Line = line;
        }

        public XsvReaderException(int line, System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            this.Line = line;
        }
    }
}
