using HandyUtil.Extensions.System;
using System;
using System.Collections.Generic;

namespace HandyUtil.Text.Xsv
{
    public partial struct XsvField : IEquatable<XsvField>
    {
        public string Source { private set; get; }

        public XsvField(object value)
            : this()
        {
            Source = value.ToString();
        }

        public XsvField(Enum value)
            : this()
        {
            this.Source = value.ToString();
        }

        public XsvField(XsvField xsvField)
            :this()
        {
            this.Source = xsvField.Source;
        }

        public string AsString()
        {
            return Source;
        }

        public string AsString(string defaultValue)
        {
            return string.IsNullOrEmpty(Source) ? defaultValue : Source;
        }

        public override string ToString()
        {
            return Source;
        }

        public string ToString(IEnumerable<string> delimiters)
        {
            return Source.MakeXsvField(delimiters);
        }

        public static explicit operator string(XsvField field)
        {
            return field.AsString();
        }

        public TEnum AsEnum<TEnum>(bool ignoreCase = true) where TEnum : struct
        {
            return Source.ToEnum<TEnum>(ignoreCase);
        }

        public TEnum? AsNullableEnum<TEnum>(bool ignoreCase = true) where TEnum : struct
        {
            return Source.ToEnumOrNull<TEnum>(ignoreCase);
        }

        public TEnum AsEnum<TEnum>(TEnum defaultValue, bool ignoreCase = true) where TEnum : struct
        {
            return Source.ToEnumOrDefault<TEnum>(defaultValue, ignoreCase);
        }

        public object AsEnum(Type enumType, bool ignoreCase = true)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumType must be an enum type.");
            }
            return Enum.Parse(enumType, Source);
        }

        public object AsNullableEnum(Type enumType, bool ignoreCase = true)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumType must be an enum type.");
            }
            try
            {
                return Enum.Parse(enumType, Source);
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (OverflowException)
            {
                return null;
            }
        }

        public object AsEnum(Type enumType, object defaultValue, bool ignoreCase = true)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumType must be an enum type.", "enumType");
            }
            if (defaultValue.GetType() != enumType)
            {
                throw new ArgumentException("Type of defaultValue is different from enumType.", "defaultValue");
            }
            try
            {
                return Enum.Parse(enumType, Source);
            }
            catch (ArgumentException)
            {
                return defaultValue;
            }
            catch (OverflowException)
            {
                return defaultValue;
            }
        }

        public bool Equals(XsvField other)
        {
            return this.Source == other.Source;
        }

        public static bool operator ==(XsvField a, XsvField b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(XsvField a, XsvField b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return this.Equals((XsvField)obj);
        }

        public override int GetHashCode()
        {
            return Source.GetHashCode();
        }
    }
}
