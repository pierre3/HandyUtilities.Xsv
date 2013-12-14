using System;
using System.Text;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HandyUtil.Extensions.System;

namespace HendyUtil.ExtensionsTest
{
    [TestClass]
    public class StringExtTest
    {
        [TestMethod]
        public void ToInt32Test_1()
        {
            var numFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            numFormat.CurrencySymbol = "ルピー";
            numFormat.CurrencyGroupSeparator = "`";
            numFormat.CurrencyGroupSizes = new[] { 4 };

            Assert.AreEqual(12345, "12345".ToInt32());
            Assert.AreEqual(12345, @"12,345\".ToInt32(NumberStyles.Currency));
            Assert.AreEqual(12345, "12345".ToInt32(numFormat));
            Assert.AreEqual(123456789, "1`2345`6789ルピー".ToInt32(NumberStyles.Currency, numFormat));

            Assert.AreEqual(999, "999".ToInt32OrNull());
            Assert.AreEqual(999999, @"999,999\".ToInt32OrNull(NumberStyles.Currency));
            Assert.AreEqual(999999999, "9`9999`9999ルピー".ToInt32OrNull(NumberStyles.Currency, numFormat));

            Assert.AreEqual(default(int), "xxxx".ToInt32OrDefault());
            Assert.AreEqual(default(int), "xxxx".ToInt32OrDefault(NumberStyles.Currency));
            Assert.AreEqual(default(int), "xxxx".ToInt32OrDefault(NumberStyles.Currency, numFormat));

            Assert.AreEqual(int.MinValue, "xxxx".ToInt32OrDefault(int.MinValue));
            Assert.AreEqual(-1, "xxxx".ToInt32OrDefault(NumberStyles.Currency,-1));
            Assert.AreEqual(int.MaxValue, "xxxx".ToInt32OrDefault(NumberStyles.Currency, numFormat,int.MaxValue));

            Assert.IsNull("!00".ToInt32OrNull());
            Assert.IsNull(@"QQQ,999\".ToInt32OrNull(NumberStyles.Currency));
            Assert.IsNull("9!9999!9999ルピー".ToInt32OrNull(NumberStyles.Currency, numFormat));
        }

        [TestMethod]
        [ExpectedException(typeof(System.FormatException))]
        public void ToInt32Test_2()
        {
            "XXXXX".ToInt32();
        }
        [TestMethod]
        [ExpectedException(typeof(System.FormatException))]
        public void ToInt32Test_3()
        {
            @"XXXXX\".ToInt32(NumberStyles.Currency);
        }
        [TestMethod]
        [ExpectedException(typeof(System.FormatException))]
        public void ToInt32Test_4()
        {
            "ABCDE".ToInt32(CultureInfo.CurrentCulture.NumberFormat);
        }
        [TestMethod]
        [ExpectedException(typeof(System.FormatException))]
        public void ToInt32Test_5()
        {
            var numFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            numFormat.CurrencySymbol = "ルピー";
            numFormat.CurrencyGroupSeparator = "`";
            numFormat.CurrencyGroupSizes = new[] { 4 };
            "123,456,789G".ToInt32(NumberStyles.Currency, numFormat);
        }
    }
}
