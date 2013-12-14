using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyUtil.Extensions.System
{
    public partial class StringExt
    {
        public static byte[] GetBytes(this string source)
        {
            return Encoding.Default.GetBytes(source);
        }

        public static byte[] GetBytes(this string source, Encoding encoding)
        {
            return encoding.GetBytes(source);
        }

        public static string Enclose(this string s, string bothEnds)
        {
            return bothEnds + s + bothEnds;
        }

        public static string Enclose(this string s, string leftEnds, string rightEnds)
        {
            return leftEnds + s + rightEnds;
        }

        public static string MakeXsvField(this string str, IEnumerable<string> delimiters)
        {

            var result = str.Replace("\"", "\"\"");
            var trimed = result.Trim();
            if (result.Length != trimed.Length
                || new[] { "\"", "\r", "\n" }.Concat(delimiters).Any(s => result.Contains(s)))
            {
                result = result.Enclose("\"");
            }
            return result;
        }

        public static TEnum ToEnum<TEnum>(this string str, bool ignoreCase = true) where TEnum : struct
        {
            var type = typeof(TEnum);
            if (!type.IsEnum)
            {
                throw new TypeAccessException("TEnum must be an enum type.");
            }
            return (TEnum)Enum.Parse(type, str, ignoreCase);
        }

        public static TEnum? ToEnumOrNull<TEnum>(this string str, bool ignoreCase = true) where TEnum : struct
        {
            var type = typeof(TEnum);
            if (!type.IsEnum)
            {
                throw new TypeAccessException("TEnum must be an enum type.");
            }

            TEnum result;
            if (Enum.TryParse(str, ignoreCase, out result) && Enum.IsDefined(type, result))
            {
                return result;
            }
            return null;
        }

        public static TEnum ToEnumOrDefault<TEnum>(this string str, TEnum defaultValue = default(TEnum), bool ignoreCase = true) where TEnum : struct
        {
            var type = typeof(TEnum);
            if (!type.IsEnum)
            {
                throw new TypeAccessException("TEnum must be an enum type.");
            }

            TEnum result;
            if (Enum.TryParse(str, ignoreCase, out result) && Enum.IsDefined(type, result))
            {
                return result;
            }
            return defaultValue;
        }

    }
}
