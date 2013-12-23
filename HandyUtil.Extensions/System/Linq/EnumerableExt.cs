using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandyUtil.Extensions.System.Linq
{
    public static partial class EnumerableExt
    {
        public static string ConcatWith<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join<T>(separator, source);
        }

        public static string ConcatWith<T>(this IEnumerable<T> source, string separator, string format, IFormatProvider provider = null) where T : IFormattable
        {
            return string.Join(separator, source.Select(value => value.ToString(format, provider)));
        }

        public static IEnumerable<TResult> Zip<T1, T2, TResult>(this IEnumerable<T1> left, IEnumerable<T2> right,
            Func<T1, T2, TResult> selector, T1 defaultL, T2 defaultR)
        {
            using (var enumeratorL = left.GetEnumerator())
            using (var enumeratorR = right.GetEnumerator())
            {
                T1 valueL;
                T2 valueR;
                while (true)
                {
                    var hasValueL = enumeratorL.MoveNext();
                    var hasValueR = enumeratorR.MoveNext();
                    if (!hasValueL && !hasValueR)
                    { yield break; }
                    valueL = hasValueL ? enumeratorL.Current : defaultL;
                    valueR = hasValueR ? enumeratorR.Current : defaultR;
                    yield return selector(valueL, valueR);
                }
            }
        }

        public static IEnumerable<TResult> Zip<T1, T2, TResult>(this IEnumerable<T1> left, IEnumerable<T2> right,
            Func<T1, T2, TResult> selector, Func<int, T1> followingSelectorL, Func<int, T2> followingSelectorR)
        {
            using (var eL = left.GetEnumerator())
            using (var eR = right.GetEnumerator())
            {
                T1 valueL;
                T2 valueR;
                int count = 0;
                while (true)
                {
                    var hasValueL = eL.MoveNext();
                    var hasValueR = eR.MoveNext();

                    if (!hasValueL && !hasValueR)
                    {
                        yield break;
                    }
                    else if (!hasValueL)
                    {
                        valueL = followingSelectorL(count);
                        valueR = eR.Current;
                    }
                    else if (!hasValueR)
                    {
                        valueL = eL.Current;
                        valueR = followingSelectorR(count);
                    }
                    else
                    {
                        valueL = eL.Current;
                        valueR = eR.Current;
                    }
                    yield return selector(valueL, valueR);
                    count++;
                }
            }
        }

        public static IEnumerable<T> Infinite<T>(T value)
        {
            while (true)
            {
                yield return value;
            }
        }
    }
}
