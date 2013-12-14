using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace HandyUtil.Extensions.System
{
	public static partial class StringExt
	{
		
		#region string to sbyte methods	
		public static sbyte ToSByte(this string s)
		{
			return sbyte.Parse(s);
		}

		public static sbyte? ToSByteOrNull(this string s)
		{
			sbyte result;
			if(sbyte.TryParse(s,out result))
			{
				return result;
			}
			return null;
		}
		
		public static sbyte ToSByteOrDefault(this string s, sbyte defaultValue = default(sbyte))
		{
			sbyte result;
			if(sbyte.TryParse(s,out result))
			{
				return result;
			}
			return defaultValue;
		}
			
		public static sbyte ToSByte(this string s, IFormatProvider formatProvider)
		{
			return sbyte.Parse(s, formatProvider);
		}

		public static sbyte ToSByte(this string s, NumberStyles numberStyles)
		{
			return sbyte.Parse(s, numberStyles);
		}

		public static sbyte ToSByte(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			return sbyte.Parse(s, numberStyles, formatProvider);
		}

		public static sbyte? ToSByteOrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			sbyte result;
			if(sbyte.TryParse(s, numberStyles, formatProvider,out result))
			{
				return result;
			}
			return null;
		}

		public static sbyte? ToSByteOrNull(this string s, NumberStyles numberStyles)
		{
			sbyte result;
			if(sbyte.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return null;
		}

		public static sbyte ToSByteOrDefault(this string s, NumberStyles numberStyles, sbyte defaultValue = default(sbyte))
		{
			sbyte result;
			if(sbyte.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return defaultValue;
		}

		public static sbyte ToSByteOrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, sbyte defaultValue = default(sbyte))
		{
			sbyte result;
			if(sbyte.TryParse(s,numberStyles, formatProvider, out result))
			{
				return result;
			}
			return defaultValue;
		}
		#endregion
		
		#region string to byte methods	
		public static byte ToByte(this string s)
		{
			return byte.Parse(s);
		}

		public static byte? ToByteOrNull(this string s)
		{
			byte result;
			if(byte.TryParse(s,out result))
			{
				return result;
			}
			return null;
		}
		
		public static byte ToByteOrDefault(this string s, byte defaultValue = default(byte))
		{
			byte result;
			if(byte.TryParse(s,out result))
			{
				return result;
			}
			return defaultValue;
		}
			
		public static byte ToByte(this string s, IFormatProvider formatProvider)
		{
			return byte.Parse(s, formatProvider);
		}

		public static byte ToByte(this string s, NumberStyles numberStyles)
		{
			return byte.Parse(s, numberStyles);
		}

		public static byte ToByte(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			return byte.Parse(s, numberStyles, formatProvider);
		}

		public static byte? ToByteOrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			byte result;
			if(byte.TryParse(s, numberStyles, formatProvider,out result))
			{
				return result;
			}
			return null;
		}

		public static byte? ToByteOrNull(this string s, NumberStyles numberStyles)
		{
			byte result;
			if(byte.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return null;
		}

		public static byte ToByteOrDefault(this string s, NumberStyles numberStyles, byte defaultValue = default(byte))
		{
			byte result;
			if(byte.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return defaultValue;
		}

		public static byte ToByteOrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, byte defaultValue = default(byte))
		{
			byte result;
			if(byte.TryParse(s,numberStyles, formatProvider, out result))
			{
				return result;
			}
			return defaultValue;
		}
		#endregion
		
		#region string to char methods	
		public static char ToChar(this string s)
		{
			return char.Parse(s);
		}

		public static char? ToCharOrNull(this string s)
		{
			char result;
			if(char.TryParse(s,out result))
			{
				return result;
			}
			return null;
		}
		
		public static char ToCharOrDefault(this string s, char defaultValue = default(char))
		{
			char result;
			if(char.TryParse(s,out result))
			{
				return result;
			}
			return defaultValue;
		}
		#endregion
		
		#region string to short methods	
		public static short ToInt16(this string s)
		{
			return short.Parse(s);
		}

		public static short? ToInt16OrNull(this string s)
		{
			short result;
			if(short.TryParse(s,out result))
			{
				return result;
			}
			return null;
		}
		
		public static short ToInt16OrDefault(this string s, short defaultValue = default(short))
		{
			short result;
			if(short.TryParse(s,out result))
			{
				return result;
			}
			return defaultValue;
		}
			
		public static short ToInt16(this string s, IFormatProvider formatProvider)
		{
			return short.Parse(s, formatProvider);
		}

		public static short ToInt16(this string s, NumberStyles numberStyles)
		{
			return short.Parse(s, numberStyles);
		}

		public static short ToInt16(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			return short.Parse(s, numberStyles, formatProvider);
		}

		public static short? ToInt16OrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			short result;
			if(short.TryParse(s, numberStyles, formatProvider,out result))
			{
				return result;
			}
			return null;
		}

		public static short? ToInt16OrNull(this string s, NumberStyles numberStyles)
		{
			short result;
			if(short.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return null;
		}

		public static short ToInt16OrDefault(this string s, NumberStyles numberStyles, short defaultValue = default(short))
		{
			short result;
			if(short.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return defaultValue;
		}

		public static short ToInt16OrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, short defaultValue = default(short))
		{
			short result;
			if(short.TryParse(s,numberStyles, formatProvider, out result))
			{
				return result;
			}
			return defaultValue;
		}
		#endregion
		
		#region string to ushort methods	
		public static ushort ToUInt16(this string s)
		{
			return ushort.Parse(s);
		}

		public static ushort? ToUInt16OrNull(this string s)
		{
			ushort result;
			if(ushort.TryParse(s,out result))
			{
				return result;
			}
			return null;
		}
		
		public static ushort ToUInt16OrDefault(this string s, ushort defaultValue = default(ushort))
		{
			ushort result;
			if(ushort.TryParse(s,out result))
			{
				return result;
			}
			return defaultValue;
		}
			
		public static ushort ToUInt16(this string s, IFormatProvider formatProvider)
		{
			return ushort.Parse(s, formatProvider);
		}

		public static ushort ToUInt16(this string s, NumberStyles numberStyles)
		{
			return ushort.Parse(s, numberStyles);
		}

		public static ushort ToUInt16(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			return ushort.Parse(s, numberStyles, formatProvider);
		}

		public static ushort? ToUInt16OrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			ushort result;
			if(ushort.TryParse(s, numberStyles, formatProvider,out result))
			{
				return result;
			}
			return null;
		}

		public static ushort? ToUInt16OrNull(this string s, NumberStyles numberStyles)
		{
			ushort result;
			if(ushort.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return null;
		}

		public static ushort ToUInt16OrDefault(this string s, NumberStyles numberStyles, ushort defaultValue = default(ushort))
		{
			ushort result;
			if(ushort.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return defaultValue;
		}

		public static ushort ToUInt16OrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, ushort defaultValue = default(ushort))
		{
			ushort result;
			if(ushort.TryParse(s,numberStyles, formatProvider, out result))
			{
				return result;
			}
			return defaultValue;
		}
		#endregion
		
		#region string to int methods	
		public static int ToInt32(this string s)
		{
			return int.Parse(s);
		}

		public static int? ToInt32OrNull(this string s)
		{
			int result;
			if(int.TryParse(s,out result))
			{
				return result;
			}
			return null;
		}
		
		public static int ToInt32OrDefault(this string s, int defaultValue = default(int))
		{
			int result;
			if(int.TryParse(s,out result))
			{
				return result;
			}
			return defaultValue;
		}
			
		public static int ToInt32(this string s, IFormatProvider formatProvider)
		{
			return int.Parse(s, formatProvider);
		}

		public static int ToInt32(this string s, NumberStyles numberStyles)
		{
			return int.Parse(s, numberStyles);
		}

		public static int ToInt32(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			return int.Parse(s, numberStyles, formatProvider);
		}

		public static int? ToInt32OrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			int result;
			if(int.TryParse(s, numberStyles, formatProvider,out result))
			{
				return result;
			}
			return null;
		}

		public static int? ToInt32OrNull(this string s, NumberStyles numberStyles)
		{
			int result;
			if(int.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return null;
		}

		public static int ToInt32OrDefault(this string s, NumberStyles numberStyles, int defaultValue = default(int))
		{
			int result;
			if(int.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return defaultValue;
		}

		public static int ToInt32OrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, int defaultValue = default(int))
		{
			int result;
			if(int.TryParse(s,numberStyles, formatProvider, out result))
			{
				return result;
			}
			return defaultValue;
		}
		#endregion
		
		#region string to uint methods	
		public static uint ToUInt32(this string s)
		{
			return uint.Parse(s);
		}

		public static uint? ToUInt32OrNull(this string s)
		{
			uint result;
			if(uint.TryParse(s,out result))
			{
				return result;
			}
			return null;
		}
		
		public static uint ToUInt32OrDefault(this string s, uint defaultValue = default(uint))
		{
			uint result;
			if(uint.TryParse(s,out result))
			{
				return result;
			}
			return defaultValue;
		}
			
		public static uint ToUInt32(this string s, IFormatProvider formatProvider)
		{
			return uint.Parse(s, formatProvider);
		}

		public static uint ToUInt32(this string s, NumberStyles numberStyles)
		{
			return uint.Parse(s, numberStyles);
		}

		public static uint ToUInt32(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			return uint.Parse(s, numberStyles, formatProvider);
		}

		public static uint? ToUInt32OrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			uint result;
			if(uint.TryParse(s, numberStyles, formatProvider,out result))
			{
				return result;
			}
			return null;
		}

		public static uint? ToUInt32OrNull(this string s, NumberStyles numberStyles)
		{
			uint result;
			if(uint.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return null;
		}

		public static uint ToUInt32OrDefault(this string s, NumberStyles numberStyles, uint defaultValue = default(uint))
		{
			uint result;
			if(uint.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return defaultValue;
		}

		public static uint ToUInt32OrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, uint defaultValue = default(uint))
		{
			uint result;
			if(uint.TryParse(s,numberStyles, formatProvider, out result))
			{
				return result;
			}
			return defaultValue;
		}
		#endregion
		
		#region string to long methods	
		public static long ToInt64(this string s)
		{
			return long.Parse(s);
		}

		public static long? ToInt64OrNull(this string s)
		{
			long result;
			if(long.TryParse(s,out result))
			{
				return result;
			}
			return null;
		}
		
		public static long ToInt64OrDefault(this string s, long defaultValue = default(long))
		{
			long result;
			if(long.TryParse(s,out result))
			{
				return result;
			}
			return defaultValue;
		}
			
		public static long ToInt64(this string s, IFormatProvider formatProvider)
		{
			return long.Parse(s, formatProvider);
		}

		public static long ToInt64(this string s, NumberStyles numberStyles)
		{
			return long.Parse(s, numberStyles);
		}

		public static long ToInt64(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			return long.Parse(s, numberStyles, formatProvider);
		}

		public static long? ToInt64OrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			long result;
			if(long.TryParse(s, numberStyles, formatProvider,out result))
			{
				return result;
			}
			return null;
		}

		public static long? ToInt64OrNull(this string s, NumberStyles numberStyles)
		{
			long result;
			if(long.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return null;
		}

		public static long ToInt64OrDefault(this string s, NumberStyles numberStyles, long defaultValue = default(long))
		{
			long result;
			if(long.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return defaultValue;
		}

		public static long ToInt64OrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, long defaultValue = default(long))
		{
			long result;
			if(long.TryParse(s,numberStyles, formatProvider, out result))
			{
				return result;
			}
			return defaultValue;
		}
		#endregion
		
		#region string to ulong methods	
		public static ulong ToUInt64(this string s)
		{
			return ulong.Parse(s);
		}

		public static ulong? ToUInt64OrNull(this string s)
		{
			ulong result;
			if(ulong.TryParse(s,out result))
			{
				return result;
			}
			return null;
		}
		
		public static ulong ToUInt64OrDefault(this string s, ulong defaultValue = default(ulong))
		{
			ulong result;
			if(ulong.TryParse(s,out result))
			{
				return result;
			}
			return defaultValue;
		}
			
		public static ulong ToUInt64(this string s, IFormatProvider formatProvider)
		{
			return ulong.Parse(s, formatProvider);
		}

		public static ulong ToUInt64(this string s, NumberStyles numberStyles)
		{
			return ulong.Parse(s, numberStyles);
		}

		public static ulong ToUInt64(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			return ulong.Parse(s, numberStyles, formatProvider);
		}

		public static ulong? ToUInt64OrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			ulong result;
			if(ulong.TryParse(s, numberStyles, formatProvider,out result))
			{
				return result;
			}
			return null;
		}

		public static ulong? ToUInt64OrNull(this string s, NumberStyles numberStyles)
		{
			ulong result;
			if(ulong.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return null;
		}

		public static ulong ToUInt64OrDefault(this string s, NumberStyles numberStyles, ulong defaultValue = default(ulong))
		{
			ulong result;
			if(ulong.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return defaultValue;
		}

		public static ulong ToUInt64OrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, ulong defaultValue = default(ulong))
		{
			ulong result;
			if(ulong.TryParse(s,numberStyles, formatProvider, out result))
			{
				return result;
			}
			return defaultValue;
		}
		#endregion
		
		#region string to float methods	
		public static float ToFloat(this string s)
		{
			return float.Parse(s);
		}

		public static float? ToFloatOrNull(this string s)
		{
			float result;
			if(float.TryParse(s,out result))
			{
				return result;
			}
			return null;
		}
		
		public static float ToFloatOrDefault(this string s, float defaultValue = default(float))
		{
			float result;
			if(float.TryParse(s,out result))
			{
				return result;
			}
			return defaultValue;
		}
			
		public static float ToFloat(this string s, IFormatProvider formatProvider)
		{
			return float.Parse(s, formatProvider);
		}

		public static float ToFloat(this string s, NumberStyles numberStyles)
		{
			return float.Parse(s, numberStyles);
		}

		public static float ToFloat(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			return float.Parse(s, numberStyles, formatProvider);
		}

		public static float? ToFloatOrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			float result;
			if(float.TryParse(s, numberStyles, formatProvider,out result))
			{
				return result;
			}
			return null;
		}

		public static float? ToFloatOrNull(this string s, NumberStyles numberStyles)
		{
			float result;
			if(float.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return null;
		}

		public static float ToFloatOrDefault(this string s, NumberStyles numberStyles, float defaultValue = default(float))
		{
			float result;
			if(float.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return defaultValue;
		}

		public static float ToFloatOrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, float defaultValue = default(float))
		{
			float result;
			if(float.TryParse(s,numberStyles, formatProvider, out result))
			{
				return result;
			}
			return defaultValue;
		}
		#endregion
		
		#region string to double methods	
		public static double ToDouble(this string s)
		{
			return double.Parse(s);
		}

		public static double? ToDoubleOrNull(this string s)
		{
			double result;
			if(double.TryParse(s,out result))
			{
				return result;
			}
			return null;
		}
		
		public static double ToDoubleOrDefault(this string s, double defaultValue = default(double))
		{
			double result;
			if(double.TryParse(s,out result))
			{
				return result;
			}
			return defaultValue;
		}
			
		public static double ToDouble(this string s, IFormatProvider formatProvider)
		{
			return double.Parse(s, formatProvider);
		}

		public static double ToDouble(this string s, NumberStyles numberStyles)
		{
			return double.Parse(s, numberStyles);
		}

		public static double ToDouble(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			return double.Parse(s, numberStyles, formatProvider);
		}

		public static double? ToDoubleOrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			double result;
			if(double.TryParse(s, numberStyles, formatProvider,out result))
			{
				return result;
			}
			return null;
		}

		public static double? ToDoubleOrNull(this string s, NumberStyles numberStyles)
		{
			double result;
			if(double.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return null;
		}

		public static double ToDoubleOrDefault(this string s, NumberStyles numberStyles, double defaultValue = default(double))
		{
			double result;
			if(double.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return defaultValue;
		}

		public static double ToDoubleOrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, double defaultValue = default(double))
		{
			double result;
			if(double.TryParse(s,numberStyles, formatProvider, out result))
			{
				return result;
			}
			return defaultValue;
		}
		#endregion
		
		#region string to decimal methods	
		public static decimal ToDecimal(this string s)
		{
			return decimal.Parse(s);
		}

		public static decimal? ToDecimalOrNull(this string s)
		{
			decimal result;
			if(decimal.TryParse(s,out result))
			{
				return result;
			}
			return null;
		}
		
		public static decimal ToDecimalOrDefault(this string s, decimal defaultValue = default(decimal))
		{
			decimal result;
			if(decimal.TryParse(s,out result))
			{
				return result;
			}
			return defaultValue;
		}
			
		public static decimal ToDecimal(this string s, IFormatProvider formatProvider)
		{
			return decimal.Parse(s, formatProvider);
		}

		public static decimal ToDecimal(this string s, NumberStyles numberStyles)
		{
			return decimal.Parse(s, numberStyles);
		}

		public static decimal ToDecimal(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			return decimal.Parse(s, numberStyles, formatProvider);
		}

		public static decimal? ToDecimalOrNull(this string s, NumberStyles numberStyles, IFormatProvider formatProvider)
		{
			decimal result;
			if(decimal.TryParse(s, numberStyles, formatProvider,out result))
			{
				return result;
			}
			return null;
		}

		public static decimal? ToDecimalOrNull(this string s, NumberStyles numberStyles)
		{
			decimal result;
			if(decimal.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return null;
		}

		public static decimal ToDecimalOrDefault(this string s, NumberStyles numberStyles, decimal defaultValue = default(decimal))
		{
			decimal result;
			if(decimal.TryParse(s, numberStyles, null, out result))
			{
				return result;
			}
			return defaultValue;
		}

		public static decimal ToDecimalOrDefault(this string s, NumberStyles numberStyles, IFormatProvider formatProvider, decimal defaultValue = default(decimal))
		{
			decimal result;
			if(decimal.TryParse(s,numberStyles, formatProvider, out result))
			{
				return result;
			}
			return defaultValue;
		}
		#endregion
		
		#region string to DateTime methods	
		public static DateTime ToDateTime(this string s)
		{
			return DateTime.Parse(s);
		}

		public static DateTime? ToDateTimeOrNull(this string s)
		{
			DateTime result;
			if(DateTime.TryParse(s,out result))
			{
				return result;
			}
			return null;
		}
		
		public static DateTime ToDateTimeOrDefault(this string s, DateTime defaultValue = default(DateTime))
		{
			DateTime result;
			if(DateTime.TryParse(s,out result))
			{
				return result;
			}
			return defaultValue;
		}
	
		public static DateTime ToDateTime(this string s, IFormatProvider formatProvider)
		{
			return DateTime.Parse(s, formatProvider);
		}

		public static DateTime ToDateTime(this string s, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
		{
			return DateTime.Parse(s, formatProvider, dateTimeStyles);
		}

		public static DateTime? ToDateTimeOrNull(this string s, DateTimeStyles dateTimeStyles)
		{
			DateTime result;
			if(DateTime.TryParse(s, null, dateTimeStyles, out result))
			{
				return result;
			}
			return null;
		}

		public static DateTime ToDateTimeOrDefault(this string s, DateTimeStyles dateTimeStyles, DateTime defaultValue = default(DateTime))
		{
			DateTime result;
			if(DateTime.TryParse(s, null, dateTimeStyles, out result))
			{
				return result;
			}
			return defaultValue;
		}

		public static DateTime? ToDateTimeOrNull(this string s, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
		{
			DateTime result;
			if(DateTime.TryParse(s, formatProvider, dateTimeStyles, out result))
			{
				return result;
			}
			return null;
		}

		public static DateTime ToDateTimeOrDefault(this string s,IFormatProvider formatProvider, DateTimeStyles dateTimeStyles, DateTime defaultValue = default(DateTime))
		{
			DateTime result;
			if(DateTime.TryParse(s, formatProvider, dateTimeStyles, out result))
			{
				return result;
			}
			return defaultValue;
		}
		#endregion
		
		#region string to bool methods	
		public static bool ToBoolean(this string s)
		{
			return bool.Parse(s);
		}

		public static bool? ToBooleanOrNull(this string s)
		{
			bool result;
			if(bool.TryParse(s,out result))
			{
				return result;
			}
			return null;
		}
		
		public static bool ToBooleanOrDefault(this string s, bool defaultValue = default(bool))
		{
			bool result;
			if(bool.TryParse(s,out result))
			{
				return result;
			}
			return defaultValue;
		}
		#endregion
	}
}

