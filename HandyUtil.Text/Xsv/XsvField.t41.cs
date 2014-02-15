
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using HandyUtil.Extensions.System;

namespace HandyUtil.Text.Xsv
{
    public partial struct XsvField
    {

        public sbyte AsSByte()
        {
            return Source.ToSByte();
        }

        public sbyte AsSByte(sbyte defaultValue)
        {
            return Source.ToSByteOrDefault(defaultValue);
        }

		public sbyte? AsNullableSByte()
        {
            return Source.ToSByteOrNull();
        }

		public sbyte AsSByte(IFormatProvider formatProvider)
        {
            return Source.ToSByte(formatProvider);
        }

		public sbyte AsSByte(NumberStyles numberStyles)
        {
            return Source.ToSByte(numberStyles);
        }
		
		public sbyte AsSByte(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToSByte(numberStyles, formatProvider);
        }

		public sbyte? AsNullableSByte(NumberStyles numberStyles)
        {
            return Source.ToSByteOrNull(numberStyles);
        }
		
		public sbyte? AsNullableSByte(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToSByteOrNull(numberStyles, formatProvider);
        }

		public sbyte AsSByte(NumberStyles numberStyles, sbyte defaultValue)
        {
            return Source.ToSByteOrDefault(numberStyles, defaultValue);
        }
		
		public sbyte AsSByte(NumberStyles numberStyles, IFormatProvider formatProvider, sbyte defaultValue)
        {
            return Source.ToSByteOrDefault(numberStyles, formatProvider, defaultValue);
        }

        public XsvField(sbyte value, string format ="")
			:this()
        {
            Source = value.ToString(format);
        }
		
		public XsvField(sbyte value, string format, IFormatProvider formatProvider)
			:this()
        {
            Source = value.ToString(format, formatProvider);
        }

        public static explicit operator sbyte(XsvField field)
        {
            return field.AsSByte();
        }

		public static explicit operator sbyte?(XsvField field)
        {
            return field.AsNullableSByte();
        }

        public byte AsByte()
        {
            return Source.ToByte();
        }

        public byte AsByte(byte defaultValue)
        {
            return Source.ToByteOrDefault(defaultValue);
        }

		public byte? AsNullableByte()
        {
            return Source.ToByteOrNull();
        }

		public byte AsByte(IFormatProvider formatProvider)
        {
            return Source.ToByte(formatProvider);
        }

		public byte AsByte(NumberStyles numberStyles)
        {
            return Source.ToByte(numberStyles);
        }
		
		public byte AsByte(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToByte(numberStyles, formatProvider);
        }

		public byte? AsNullableByte(NumberStyles numberStyles)
        {
            return Source.ToByteOrNull(numberStyles);
        }
		
		public byte? AsNullableByte(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToByteOrNull(numberStyles, formatProvider);
        }

		public byte AsByte(NumberStyles numberStyles, byte defaultValue)
        {
            return Source.ToByteOrDefault(numberStyles, defaultValue);
        }
		
		public byte AsByte(NumberStyles numberStyles, IFormatProvider formatProvider, byte defaultValue)
        {
            return Source.ToByteOrDefault(numberStyles, formatProvider, defaultValue);
        }

        public XsvField(byte value, string format ="")
			:this()
        {
            Source = value.ToString(format);
        }
		
		public XsvField(byte value, string format, IFormatProvider formatProvider)
			:this()
        {
            Source = value.ToString(format, formatProvider);
        }

        public static explicit operator byte(XsvField field)
        {
            return field.AsByte();
        }

		public static explicit operator byte?(XsvField field)
        {
            return field.AsNullableByte();
        }

        public char AsChar()
        {
            return Source.ToChar();
        }

        public char AsChar(char defaultValue)
        {
            return Source.ToCharOrDefault(defaultValue);
        }

		public char? AsNullableChar()
        {
            return Source.ToCharOrNull();
        }

        public static explicit operator char(XsvField field)
        {
            return field.AsChar();
        }

		public static explicit operator char?(XsvField field)
        {
            return field.AsNullableChar();
        }

        public short AsInt16()
        {
            return Source.ToInt16();
        }

        public short AsInt16(short defaultValue)
        {
            return Source.ToInt16OrDefault(defaultValue);
        }

		public short? AsNullableInt16()
        {
            return Source.ToInt16OrNull();
        }

		public short AsInt16(IFormatProvider formatProvider)
        {
            return Source.ToInt16(formatProvider);
        }

		public short AsInt16(NumberStyles numberStyles)
        {
            return Source.ToInt16(numberStyles);
        }
		
		public short AsInt16(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToInt16(numberStyles, formatProvider);
        }

		public short? AsNullableInt16(NumberStyles numberStyles)
        {
            return Source.ToInt16OrNull(numberStyles);
        }
		
		public short? AsNullableInt16(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToInt16OrNull(numberStyles, formatProvider);
        }

		public short AsInt16(NumberStyles numberStyles, short defaultValue)
        {
            return Source.ToInt16OrDefault(numberStyles, defaultValue);
        }
		
		public short AsInt16(NumberStyles numberStyles, IFormatProvider formatProvider, short defaultValue)
        {
            return Source.ToInt16OrDefault(numberStyles, formatProvider, defaultValue);
        }

        public XsvField(short value, string format ="")
			:this()
        {
            Source = value.ToString(format);
        }
		
		public XsvField(short value, string format, IFormatProvider formatProvider)
			:this()
        {
            Source = value.ToString(format, formatProvider);
        }

        public static explicit operator short(XsvField field)
        {
            return field.AsInt16();
        }

		public static explicit operator short?(XsvField field)
        {
            return field.AsNullableInt16();
        }

        public ushort AsUInt16()
        {
            return Source.ToUInt16();
        }

        public ushort AsUInt16(ushort defaultValue)
        {
            return Source.ToUInt16OrDefault(defaultValue);
        }

		public ushort? AsNullableUInt16()
        {
            return Source.ToUInt16OrNull();
        }

		public ushort AsUInt16(IFormatProvider formatProvider)
        {
            return Source.ToUInt16(formatProvider);
        }

		public ushort AsUInt16(NumberStyles numberStyles)
        {
            return Source.ToUInt16(numberStyles);
        }
		
		public ushort AsUInt16(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToUInt16(numberStyles, formatProvider);
        }

		public ushort? AsNullableUInt16(NumberStyles numberStyles)
        {
            return Source.ToUInt16OrNull(numberStyles);
        }
		
		public ushort? AsNullableUInt16(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToUInt16OrNull(numberStyles, formatProvider);
        }

		public ushort AsUInt16(NumberStyles numberStyles, ushort defaultValue)
        {
            return Source.ToUInt16OrDefault(numberStyles, defaultValue);
        }
		
		public ushort AsUInt16(NumberStyles numberStyles, IFormatProvider formatProvider, ushort defaultValue)
        {
            return Source.ToUInt16OrDefault(numberStyles, formatProvider, defaultValue);
        }

        public XsvField(ushort value, string format ="")
			:this()
        {
            Source = value.ToString(format);
        }
		
		public XsvField(ushort value, string format, IFormatProvider formatProvider)
			:this()
        {
            Source = value.ToString(format, formatProvider);
        }

        public static explicit operator ushort(XsvField field)
        {
            return field.AsUInt16();
        }

		public static explicit operator ushort?(XsvField field)
        {
            return field.AsNullableUInt16();
        }

        public int AsInt32()
        {
            return Source.ToInt32();
        }

        public int AsInt32(int defaultValue)
        {
            return Source.ToInt32OrDefault(defaultValue);
        }

		public int? AsNullableInt32()
        {
            return Source.ToInt32OrNull();
        }

		public int AsInt32(IFormatProvider formatProvider)
        {
            return Source.ToInt32(formatProvider);
        }

		public int AsInt32(NumberStyles numberStyles)
        {
            return Source.ToInt32(numberStyles);
        }
		
		public int AsInt32(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToInt32(numberStyles, formatProvider);
        }

		public int? AsNullableInt32(NumberStyles numberStyles)
        {
            return Source.ToInt32OrNull(numberStyles);
        }
		
		public int? AsNullableInt32(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToInt32OrNull(numberStyles, formatProvider);
        }

		public int AsInt32(NumberStyles numberStyles, int defaultValue)
        {
            return Source.ToInt32OrDefault(numberStyles, defaultValue);
        }
		
		public int AsInt32(NumberStyles numberStyles, IFormatProvider formatProvider, int defaultValue)
        {
            return Source.ToInt32OrDefault(numberStyles, formatProvider, defaultValue);
        }

        public XsvField(int value, string format ="")
			:this()
        {
            Source = value.ToString(format);
        }
		
		public XsvField(int value, string format, IFormatProvider formatProvider)
			:this()
        {
            Source = value.ToString(format, formatProvider);
        }

        public static explicit operator int(XsvField field)
        {
            return field.AsInt32();
        }

		public static explicit operator int?(XsvField field)
        {
            return field.AsNullableInt32();
        }

        public uint AsUInt32()
        {
            return Source.ToUInt32();
        }

        public uint AsUInt32(uint defaultValue)
        {
            return Source.ToUInt32OrDefault(defaultValue);
        }

		public uint? AsNullableUInt32()
        {
            return Source.ToUInt32OrNull();
        }

		public uint AsUInt32(IFormatProvider formatProvider)
        {
            return Source.ToUInt32(formatProvider);
        }

		public uint AsUInt32(NumberStyles numberStyles)
        {
            return Source.ToUInt32(numberStyles);
        }
		
		public uint AsUInt32(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToUInt32(numberStyles, formatProvider);
        }

		public uint? AsNullableUInt32(NumberStyles numberStyles)
        {
            return Source.ToUInt32OrNull(numberStyles);
        }
		
		public uint? AsNullableUInt32(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToUInt32OrNull(numberStyles, formatProvider);
        }

		public uint AsUInt32(NumberStyles numberStyles, uint defaultValue)
        {
            return Source.ToUInt32OrDefault(numberStyles, defaultValue);
        }
		
		public uint AsUInt32(NumberStyles numberStyles, IFormatProvider formatProvider, uint defaultValue)
        {
            return Source.ToUInt32OrDefault(numberStyles, formatProvider, defaultValue);
        }

        public XsvField(uint value, string format ="")
			:this()
        {
            Source = value.ToString(format);
        }
		
		public XsvField(uint value, string format, IFormatProvider formatProvider)
			:this()
        {
            Source = value.ToString(format, formatProvider);
        }

        public static explicit operator uint(XsvField field)
        {
            return field.AsUInt32();
        }

		public static explicit operator uint?(XsvField field)
        {
            return field.AsNullableUInt32();
        }

        public long AsInt64()
        {
            return Source.ToInt64();
        }

        public long AsInt64(long defaultValue)
        {
            return Source.ToInt64OrDefault(defaultValue);
        }

		public long? AsNullableInt64()
        {
            return Source.ToInt64OrNull();
        }

		public long AsInt64(IFormatProvider formatProvider)
        {
            return Source.ToInt64(formatProvider);
        }

		public long AsInt64(NumberStyles numberStyles)
        {
            return Source.ToInt64(numberStyles);
        }
		
		public long AsInt64(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToInt64(numberStyles, formatProvider);
        }

		public long? AsNullableInt64(NumberStyles numberStyles)
        {
            return Source.ToInt64OrNull(numberStyles);
        }
		
		public long? AsNullableInt64(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToInt64OrNull(numberStyles, formatProvider);
        }

		public long AsInt64(NumberStyles numberStyles, long defaultValue)
        {
            return Source.ToInt64OrDefault(numberStyles, defaultValue);
        }
		
		public long AsInt64(NumberStyles numberStyles, IFormatProvider formatProvider, long defaultValue)
        {
            return Source.ToInt64OrDefault(numberStyles, formatProvider, defaultValue);
        }

        public XsvField(long value, string format ="")
			:this()
        {
            Source = value.ToString(format);
        }
		
		public XsvField(long value, string format, IFormatProvider formatProvider)
			:this()
        {
            Source = value.ToString(format, formatProvider);
        }

        public static explicit operator long(XsvField field)
        {
            return field.AsInt64();
        }

		public static explicit operator long?(XsvField field)
        {
            return field.AsNullableInt64();
        }

        public ulong AsUInt64()
        {
            return Source.ToUInt64();
        }

        public ulong AsUInt64(ulong defaultValue)
        {
            return Source.ToUInt64OrDefault(defaultValue);
        }

		public ulong? AsNullableUInt64()
        {
            return Source.ToUInt64OrNull();
        }

		public ulong AsUInt64(IFormatProvider formatProvider)
        {
            return Source.ToUInt64(formatProvider);
        }

		public ulong AsUInt64(NumberStyles numberStyles)
        {
            return Source.ToUInt64(numberStyles);
        }
		
		public ulong AsUInt64(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToUInt64(numberStyles, formatProvider);
        }

		public ulong? AsNullableUInt64(NumberStyles numberStyles)
        {
            return Source.ToUInt64OrNull(numberStyles);
        }
		
		public ulong? AsNullableUInt64(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToUInt64OrNull(numberStyles, formatProvider);
        }

		public ulong AsUInt64(NumberStyles numberStyles, ulong defaultValue)
        {
            return Source.ToUInt64OrDefault(numberStyles, defaultValue);
        }
		
		public ulong AsUInt64(NumberStyles numberStyles, IFormatProvider formatProvider, ulong defaultValue)
        {
            return Source.ToUInt64OrDefault(numberStyles, formatProvider, defaultValue);
        }

        public XsvField(ulong value, string format ="")
			:this()
        {
            Source = value.ToString(format);
        }
		
		public XsvField(ulong value, string format, IFormatProvider formatProvider)
			:this()
        {
            Source = value.ToString(format, formatProvider);
        }

        public static explicit operator ulong(XsvField field)
        {
            return field.AsUInt64();
        }

		public static explicit operator ulong?(XsvField field)
        {
            return field.AsNullableUInt64();
        }

        public float AsFloat()
        {
            return Source.ToFloat();
        }

        public float AsFloat(float defaultValue)
        {
            return Source.ToFloatOrDefault(defaultValue);
        }

		public float? AsNullableFloat()
        {
            return Source.ToFloatOrNull();
        }

		public float AsFloat(IFormatProvider formatProvider)
        {
            return Source.ToFloat(formatProvider);
        }

		public float AsFloat(NumberStyles numberStyles)
        {
            return Source.ToFloat(numberStyles);
        }
		
		public float AsFloat(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToFloat(numberStyles, formatProvider);
        }

		public float? AsNullableFloat(NumberStyles numberStyles)
        {
            return Source.ToFloatOrNull(numberStyles);
        }
		
		public float? AsNullableFloat(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToFloatOrNull(numberStyles, formatProvider);
        }

		public float AsFloat(NumberStyles numberStyles, float defaultValue)
        {
            return Source.ToFloatOrDefault(numberStyles, defaultValue);
        }
		
		public float AsFloat(NumberStyles numberStyles, IFormatProvider formatProvider, float defaultValue)
        {
            return Source.ToFloatOrDefault(numberStyles, formatProvider, defaultValue);
        }

        public XsvField(float value, string format ="")
			:this()
        {
            Source = value.ToString(format);
        }
		
		public XsvField(float value, string format, IFormatProvider formatProvider)
			:this()
        {
            Source = value.ToString(format, formatProvider);
        }

        public static explicit operator float(XsvField field)
        {
            return field.AsFloat();
        }

		public static explicit operator float?(XsvField field)
        {
            return field.AsNullableFloat();
        }

        public double AsDouble()
        {
            return Source.ToDouble();
        }

        public double AsDouble(double defaultValue)
        {
            return Source.ToDoubleOrDefault(defaultValue);
        }

		public double? AsNullableDouble()
        {
            return Source.ToDoubleOrNull();
        }

		public double AsDouble(IFormatProvider formatProvider)
        {
            return Source.ToDouble(formatProvider);
        }

		public double AsDouble(NumberStyles numberStyles)
        {
            return Source.ToDouble(numberStyles);
        }
		
		public double AsDouble(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToDouble(numberStyles, formatProvider);
        }

		public double? AsNullableDouble(NumberStyles numberStyles)
        {
            return Source.ToDoubleOrNull(numberStyles);
        }
		
		public double? AsNullableDouble(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToDoubleOrNull(numberStyles, formatProvider);
        }

		public double AsDouble(NumberStyles numberStyles, double defaultValue)
        {
            return Source.ToDoubleOrDefault(numberStyles, defaultValue);
        }
		
		public double AsDouble(NumberStyles numberStyles, IFormatProvider formatProvider, double defaultValue)
        {
            return Source.ToDoubleOrDefault(numberStyles, formatProvider, defaultValue);
        }

        public XsvField(double value, string format ="")
			:this()
        {
            Source = value.ToString(format);
        }
		
		public XsvField(double value, string format, IFormatProvider formatProvider)
			:this()
        {
            Source = value.ToString(format, formatProvider);
        }

        public static explicit operator double(XsvField field)
        {
            return field.AsDouble();
        }

		public static explicit operator double?(XsvField field)
        {
            return field.AsNullableDouble();
        }

        public decimal AsDecimal()
        {
            return Source.ToDecimal();
        }

        public decimal AsDecimal(decimal defaultValue)
        {
            return Source.ToDecimalOrDefault(defaultValue);
        }

		public decimal? AsNullableDecimal()
        {
            return Source.ToDecimalOrNull();
        }

		public decimal AsDecimal(IFormatProvider formatProvider)
        {
            return Source.ToDecimal(formatProvider);
        }

		public decimal AsDecimal(NumberStyles numberStyles)
        {
            return Source.ToDecimal(numberStyles);
        }
		
		public decimal AsDecimal(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToDecimal(numberStyles, formatProvider);
        }

		public decimal? AsNullableDecimal(NumberStyles numberStyles)
        {
            return Source.ToDecimalOrNull(numberStyles);
        }
		
		public decimal? AsNullableDecimal(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            return Source.ToDecimalOrNull(numberStyles, formatProvider);
        }

		public decimal AsDecimal(NumberStyles numberStyles, decimal defaultValue)
        {
            return Source.ToDecimalOrDefault(numberStyles, defaultValue);
        }
		
		public decimal AsDecimal(NumberStyles numberStyles, IFormatProvider formatProvider, decimal defaultValue)
        {
            return Source.ToDecimalOrDefault(numberStyles, formatProvider, defaultValue);
        }

        public XsvField(decimal value, string format ="")
			:this()
        {
            Source = value.ToString(format);
        }
		
		public XsvField(decimal value, string format, IFormatProvider formatProvider)
			:this()
        {
            Source = value.ToString(format, formatProvider);
        }

        public static explicit operator decimal(XsvField field)
        {
            return field.AsDecimal();
        }

		public static explicit operator decimal?(XsvField field)
        {
            return field.AsNullableDecimal();
        }

        public DateTime AsDateTime()
        {
            return Source.ToDateTime();
        }

        public DateTime AsDateTime(DateTime defaultValue)
        {
            return Source.ToDateTimeOrDefault(defaultValue);
        }

		public DateTime? AsNullableDateTime()
        {
            return Source.ToDateTimeOrNull();
        }

		public DateTime AsDateTime(IFormatProvider formatProvider)
        {
            return Source.ToDateTime(formatProvider);
        }
		
		public DateTime AsDateTime(IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            return Source.ToDateTime(formatProvider, dateTimeStyles);
        }
		
		public DateTime? AsNullableDateTime(IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            return Source.ToDateTimeOrNull(formatProvider, dateTimeStyles);
        }

		public DateTime AsDateTime(IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, DateTime defaultValue)
        {
            return Source.ToDateTimeOrDefault(formatProvider, dateTimeStyles, defaultValue);
        }

		public XsvField(DateTime value, string format ="")
			:this()
        {
            Source = value.ToString(format);
        }

		public XsvField(DateTime value, string format, IFormatProvider formatProvider)
			:this()
        {
            Source = value.ToString(format, formatProvider);
        }

        public static explicit operator DateTime(XsvField field)
        {
            return field.AsDateTime();
        }

		public static explicit operator DateTime?(XsvField field)
        {
            return field.AsNullableDateTime();
        }

        public bool AsBoolean()
        {
            return Source.ToBoolean();
        }

        public bool AsBoolean(bool defaultValue)
        {
            return Source.ToBooleanOrDefault(defaultValue);
        }

		public bool? AsNullableBoolean()
        {
            return Source.ToBooleanOrNull();
        }

        public static explicit operator bool(XsvField field)
        {
            return field.AsBoolean();
        }

		public static explicit operator bool?(XsvField field)
        {
            return field.AsNullableBoolean();
        }

    }
}
