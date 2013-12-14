using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace HandyUtil.Extensions.System.Linq
{
	public static partial class EnumerableExt
	{

		public static string ConcatWith(this IEnumerable<sbyte> source, string separator, string format)
        {
            return source.Select(s => s.ToString(format)).Aggregate((buf, s) => buf + separator + s);
        }

		public static string ConcatWith(this IEnumerable<byte> source, string separator, string format)
        {
            return source.Select(s => s.ToString(format)).Aggregate((buf, s) => buf + separator + s);
        }

		public static string ConcatWith(this IEnumerable<short> source, string separator, string format)
        {
            return source.Select(s => s.ToString(format)).Aggregate((buf, s) => buf + separator + s);
        }

		public static string ConcatWith(this IEnumerable<ushort> source, string separator, string format)
        {
            return source.Select(s => s.ToString(format)).Aggregate((buf, s) => buf + separator + s);
        }

		public static string ConcatWith(this IEnumerable<int> source, string separator, string format)
        {
            return source.Select(s => s.ToString(format)).Aggregate((buf, s) => buf + separator + s);
        }

		public static string ConcatWith(this IEnumerable<uint> source, string separator, string format)
        {
            return source.Select(s => s.ToString(format)).Aggregate((buf, s) => buf + separator + s);
        }

		public static string ConcatWith(this IEnumerable<long> source, string separator, string format)
        {
            return source.Select(s => s.ToString(format)).Aggregate((buf, s) => buf + separator + s);
        }

		public static string ConcatWith(this IEnumerable<ulong> source, string separator, string format)
        {
            return source.Select(s => s.ToString(format)).Aggregate((buf, s) => buf + separator + s);
        }

		public static string ConcatWith(this IEnumerable<float> source, string separator, string format)
        {
            return source.Select(s => s.ToString(format)).Aggregate((buf, s) => buf + separator + s);
        }

		public static string ConcatWith(this IEnumerable<double> source, string separator, string format)
        {
            return source.Select(s => s.ToString(format)).Aggregate((buf, s) => buf + separator + s);
        }

		public static string ConcatWith(this IEnumerable<decimal> source, string separator, string format)
        {
            return source.Select(s => s.ToString(format)).Aggregate((buf, s) => buf + separator + s);
        }

		public static string ConcatWith(this IEnumerable<DateTime> source, string separator, string format)
        {
            return source.Select(s => s.ToString(format)).Aggregate((buf, s) => buf + separator + s);
        }
	}
}

