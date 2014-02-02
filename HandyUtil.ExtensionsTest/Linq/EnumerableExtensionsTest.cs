using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using HandyUtil.Extensions.System.Linq;
using System.Globalization;
namespace HendyUtil.ExtensionsTest
{
    [TestClass]
    public class EnumerableExtensionsTest
    {
        [TestMethod]
        public void ZipTest_1()
        {
            var s = new string[] { "first", "second", "third", "fourth", "fifth", "sixth", "seventh" };
            var i = Enumerable.Range(1, 10);

            var actual = s.Zip(i, (str, num) => new { str, num }, "empty", 0).ToArray();
            Console.WriteLine(actual.ConcatWith(","));

            var expected = new[]
            {
                new { str = "first", num = 1 }, new { str = "second", num = 2 },
                new { str = "third", num = 3 }, new { str = "fourth", num = 4 },
                new { str = "fifth", num = 5 }, new { str = "sixth", num = 6 },
                new { str = "seventh", num = 7 }, new { str = "empty", num = 8 },
                new { str = "empty", num = 9 }, new { str = "empty", num = 10 }
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ZipTest_2()
        {
            var s = new string[] { "first", "second", "third", "fourth", "fifth", "sixth", "seventh" };
            var i = Enumerable.Range(0, 10);

            var actual = s.Zip(i, (_, str, num) => new { str, num }, n => "follower_" + n, _ => default(int)).ToArray();
            Console.WriteLine(actual.ConcatWith(","));

            var expected = new[]
            {
                new { str = "first", num = 0 }, new { str = "second", num = 1 },
                new { str = "third", num = 2 }, new { str = "fourth", num = 3 },
                new { str = "fifth", num = 4 }, new { str = "sixth", num = 5 },
                new { str = "seventh", num = 6 }, new { str = "follower_7", num = 7 },
                new { str = "follower_8", num = 8 }, new { str = "follower_9", num = 9 }
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConcatWithTest_1()
        {
            var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var actual = array.ConcatWith(", ");
            var expected = "1, 2, 3, 4, 5, 6, 7, 8, 9, 10";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConcatWithTest_2()
        {
            var array = new[] { 100, 1200, 322, 6421, 10, 4052, 666, 111, 4000, 80 };

            var actual = array.ConcatWith("; ", "0,0G");
            var expected = "100G; 1,200G; 322G; 6,421G; 10G; 4,052G; 666G; 111G; 4,000G; 80G";

            Assert.AreEqual(expected, actual);
        }

        class sampleClass01 : IFormattable
        {
            public string Name { set; get; }
            public int Id { set; get; }
            public float Power { set; get; }
            public override string ToString()
            {
                return string.Format("{0}: {1}; {2}P", Name, Id, Power);
            }

            public string ToString(string format, IFormatProvider formatProvider)
            {
                if (formatProvider == null)
                {
                    return ToString();
                }
                var formatter = (ICustomFormatter)formatProvider.GetFormat(typeof(ICustomFormatter));
                if (formatter == null)
                {
                    return ToString();
                }
                return formatter.Format(format, this, formatProvider);
            }

            public class Formatter : IFormatProvider, ICustomFormatter
            {
                public object GetFormat(Type formatType)
                {
                    if (formatType == typeof(ICustomFormatter))
                    { return this; }
                    else
                    { return null; }
                }

                public string Format(string format, object arg, IFormatProvider formatProvider)
                {
                    if (arg.GetType() != typeof(sampleClass01))
                    {
                        if (arg is IFormattable)
                            return ((IFormattable)arg).ToString(format, CultureInfo.CurrentCulture);
                        else if (arg != null)
                            return arg.ToString();
                        else
                            return String.Empty;
                    }
                    var sample = arg as sampleClass01;
                    if (format == "N")
                    {
                        return "Name: " + sample.Name;
                    }
                    if (format == "I")
                    {
                        return "ID: " + sample.Id;
                    }
                    if (format == "P")
                    {
                        return "Power: " + sample.Power;
                    }
                    return sample.ToString();
                }
            }
        }


        [TestMethod]
        public void ConcatWithTest_3()
        {
            var samples = new sampleClass01[]{
                new sampleClass01(){Name="taro",Id=1234,Power=45.23f},
                new sampleClass01(){Name="jiro",Id=2222,Power=99.99f},
                new sampleClass01(){Name="saburo",Id=9999,Power=10.11f}
            };
            var actual = samples.ConcatWith("//");
            var expected = "taro: 1234; 45.23P//jiro: 2222; 99.99P//saburo: 9999; 10.11P";
            Assert.AreEqual(expected, actual);

            actual = samples.ConcatWith(", ", "N", new sampleClass01.Formatter());
            expected = "Name: taro, Name: jiro, Name: saburo";
            Assert.AreEqual(expected, actual);

            Console.WriteLine(new[] { 1000, 1980, 3980, 4500, 6398 }.ConcatWith(", ", "C"));

            var sqrts = Enumerable.Range(1, 10).Select(n => Math.Sqrt(n));
            Console.WriteLine(sqrts.ConcatWith(", ", "E03"));

            var days = new[]{
                new DateTime(2013,1,1),
                new DateTime(2013,2,2),
                new DateTime(2013,3,3)
            };
            var culture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            Console.WriteLine(days.ConcatWith(" / ", "D", culture));
            
        }
    }
}
