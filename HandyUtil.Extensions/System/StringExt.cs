using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            ThrowIfNotEnumType(type);

            return (TEnum)Enum.Parse(type, str, ignoreCase);
        }

        public static TEnum? ToEnumOrNull<TEnum>(this string str, bool ignoreCase = true) where TEnum : struct
        {
            var type = typeof(TEnum);
            ThrowIfNotEnumType(type);

            TEnum result;
            if (TryParseEnum(str, ignoreCase, out result) && Enum.IsDefined(type, result))
            {
                return result;
            }
            return null;
        }

        public static TEnum ToEnumOrDefault<TEnum>(this string str, TEnum defaultValue = default(TEnum), bool ignoreCase = true) where TEnum : struct
        {
            var type = typeof(TEnum);
            ThrowIfNotEnumType(type);

            TEnum result;
            if (TryParseEnum(str, ignoreCase, out result) && Enum.IsDefined(type, result))
            {
                return result;
            }
            return defaultValue;
        }

        private static void ThrowIfNotEnumType(Type type)
        {
            if (!type.IsEnum)
            {
#if net40
                throw new TypeAccessException("TEnum must be an enum type.");
#else
                throw new ArgumentException("TEnum must be an enum type.");
#endif
            }

        }

        private static bool TryParseEnum<TEnum>(string str, bool ignoreCase, out TEnum result) where TEnum:struct
        {
#if net40
            return Enum.TryParse(str, ignoreCase, out result);
#else
            try
            {
                result = (TEnum)Enum.Parse(typeof(TEnum), str, ignoreCase);
                return true;
            }catch(ArgumentException)
            {
                result = default(TEnum);
                return false;
            }
#endif
        }
    }
}
