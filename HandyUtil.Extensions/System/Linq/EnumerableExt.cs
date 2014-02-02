using System;
using System.Collections.Generic;
using System.Linq;

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

        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector, TFirst default1st, TSecond default2nd)
        {
            if (first == null)
            { throw new ArgumentNullException("first"); }
            if (second == null)
            { throw new ArgumentNullException("second"); }
            if (resultSelector == null)
            { throw new ArgumentNullException("selector"); }

            return _Zip(first, second, resultSelector, default1st, default2nd);
        }

        private static IEnumerable<TResult> _Zip<TFirst, TSecond, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector, TFirst default1st, TSecond default2nd)
        {
            using (var enumerator1 = first.GetEnumerator())
            using (var enumerator2 = second.GetEnumerator())
            {
                TFirst value1st;
                TSecond value2nd;
                while (true)
                {
                    var hasValue1st = enumerator1.MoveNext();
                    var hasValue2nd = enumerator2.MoveNext();
                    if (!hasValue1st && !hasValue2nd)
                    { yield break; }
                    value1st = hasValue1st ? enumerator1.Current : default1st;
                    value2nd = hasValue2nd ? enumerator2.Current : default2nd;
                    yield return resultSelector(value1st, value2nd);
                }
            }
        }

        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second,
            Func<int, TFirst, TSecond, TResult> resultSelector, Func<int, TFirst> followingSelector1st, Func<int, TSecond> followingSelector2nd)
        {
            if (first == null)
            { throw new ArgumentNullException("first"); }
            if (second == null)
            { throw new ArgumentNullException("second"); }
            if (resultSelector == null)
            { throw new ArgumentNullException("selector"); }
            if (followingSelector1st == null)
            { throw new ArgumentNullException("followingSelector1st"); }
            if (followingSelector2nd == null)
            { throw new ArgumentNullException("followingSelector2nd"); }
            
            return _Zip(first, second, resultSelector, followingSelector1st, followingSelector2nd);
        }

        private static IEnumerable<TResult> _Zip<TFirst, TSecond, TResult>(IEnumerable<TFirst> first, IEnumerable<TSecond> second,
            Func<int, TFirst, TSecond, TResult> resultSelector, Func<int, TFirst> followingSelector1st, Func<int, TSecond> followingSelector2nd)
        {
            using (var e1st = first.GetEnumerator())
            using (var e2nd = second.GetEnumerator())
            {
                TFirst value1st;
                TSecond value2nd;
                int count = 0;
                while (true)
                {
                    var hasValue1st = e1st.MoveNext();
                    var hasValue2nd = e2nd.MoveNext();

                    if (!hasValue1st && !hasValue2nd)
                    {
                        yield break;
                    }
                    else if (!hasValue1st)
                    {
                        value1st = followingSelector1st(count);
                        value2nd = e2nd.Current;
                    }
                    else if (!hasValue2nd)
                    {
                        value1st = e1st.Current;
                        value2nd = followingSelector2nd(count);
                    }
                    else
                    {
                        value1st = e1st.Current;
                        value2nd = e2nd.Current;
                    }
                    yield return resultSelector(count, value1st, value2nd);
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
