using HandyUtil.Text.Xsv;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
#if net35
using HandyUtil.Extensions.System.Linq;
#endif

namespace HendyUtil.TextTest
{

    [TestClass]
    public class TypedXsvDataTest
    {
        enum Maker
        {
            Ａ,
            Ｔ,
            Ｈ,
            Ｓ
        }

        class ShipModel
        {
            public int 品番 { set; get; }
            public string 船種 { set; get; }
            public string 品名 { set; get; }
            public int 税込価格 { set; get; }
            public int 本体価格 { set; get; }
            public Maker メーカー { set; get; }
        }

        [TestMethod]
        public void ReadXsvTest_autoBinding()
        {
            var data =
                 "品番,船種,品名,税込価格,本体価格,メーカー" + Environment.NewLine
                + "103,戦艦,山城　やましろ,1785,\"1,700\",Ａ" + Environment.NewLine
                + "215,航空母艦,信濃　しなの,2940,\"2,800\",Ｔ" + Environment.NewLine
                + "442,駆逐艦,陽炎　かげろう,1050,\"1,000\",Ａ" + Environment.NewLine;

            var expected = new[]{ 
                new { 品番 = 103, 船種 = "戦艦", 品名 = "山城　やましろ", 税込価格 = 1785, 本体価格 = 1700, メーカー = Maker.Ａ },
                new { 品番 = 215, 船種 = "航空母艦", 品名 = "信濃　しなの", 税込価格 = 2940, 本体価格 = 2800, メーカー = Maker.Ｔ },
                new { 品番 = 442, 船種 = "駆逐艦", 品名 = "陽炎　かげろう", 税込価格 = 1050, 本体価格 = 1000, メーカー = Maker.Ａ }
            };

            var target = new TypedXsvData<ShipModel>(new[] { "," }, isAutoBinding: true);
            using (var reader = new System.IO.StringReader(data))
            {
                target.Read(reader);
            }

            foreach (var x in target.Rows.Zip(expected, (a, b) => new { row = a, exp = b }))
            {
                Assert.AreEqual(x.exp.品番, x.row.Fields.品番);
                Assert.AreEqual(x.exp.船種, x.row.Fields.船種);
                Assert.AreEqual(x.exp.品名, x.row.Fields.品名);
                Assert.AreEqual(x.exp.税込価格, x.row.Fields.税込価格);
                Assert.AreEqual(x.exp.本体価格, x.row.Fields.本体価格);
                Assert.AreEqual(x.exp.メーカー, x.row.Fields.メーカー);
            }
        }

        class ShipModelB : ShipModel
        {
            public string ふりがな { set; get; }
        }

        [TestMethod]
        public void ReadCsvTest_defaultHeader()
        {
            var data =
                "111,戦艦,榛名　はるな,2625,\"2,500\",Ｈ" + Environment.NewLine +
                "207,航空母艦,大鷹　たいよう,1575,\"1,500\",Ａ" + Environment.NewLine +
                "321,軽巡洋艦,鬼怒　きぬ,1365,\"1,300\",Ｔ";

            var expected = new[]{ 
                new { 品番 = 111, 船種 = "戦艦", 品名 = "榛名", ふりがな = "はるな", 税込価格 = 2625, 本体価格 = 2500, メーカー = Maker.Ｈ },
                new { 品番 = 207, 船種 = "航空母艦", 品名 = "大鷹", ふりがな = "たいよう", 税込価格 = 1575, 本体価格 = 1500, メーカー = Maker.Ａ },
                new { 品番 = 321, 船種 = "軽巡洋艦", 品名 = "鬼怒", ふりがな = "きぬ", 税込価格 = 1365, 本体価格 = 1300, メーカー = Maker.Ｔ }
            };

            var target = new TypedXsvData<ShipModelB>(
                new XsvDataSettings()
                {
                    HeaderExists = false,
                    Delimiters = new[] { ",", "　" },
                    DefaultColumnName = "defaultColumn_"
                }, isAutoBinding: true);

            using (var reader = new StringReader(data))
            {
                target.Read(reader);
            }

            foreach (var x in target.Rows.Zip(expected, (a, b) => new { row = a, exp = b }))
            {
                Assert.AreEqual(x.exp.品番, x.row["defaultColumn_0"].AsInt32());
                Assert.AreEqual(x.exp.船種, x.row["defaultColumn_1"].AsString());
                Assert.AreEqual(x.exp.品名, x.row["defaultColumn_2"].AsString());
                Assert.AreEqual(x.exp.ふりがな, x.row["defaultColumn_3"].AsString());
                Assert.AreEqual(x.exp.税込価格, x.row["defaultColumn_4"].AsInt32());
                Assert.AreEqual(x.exp.本体価格, x.row["defaultColumn_5"].AsInt32(NumberStyles.Currency));
                Assert.AreEqual(x.exp.メーカー, x.row["defaultColumn_6"].AsEnum<Maker>());
            }

            target.Settings.HeaderExists = false;
            target.Settings.HeaderStrings = new[] { "品番", "船種", "品名", "ふりがな", "税込価格", "本体価格", "メーカー" };
            using (var reader = new StringReader(data))
            {
                target.Read(reader);
            }

            foreach (var x in target.Rows.Zip(expected, (a, b) => new { row = a, exp = b }))
            {
                Assert.AreEqual(x.exp.品番, x.row.Fields.品番);
                Assert.AreEqual(x.exp.船種, x.row.Fields.船種);
                Assert.AreEqual(x.exp.品名, x.row.Fields.品名);
                Assert.AreEqual(x.exp.ふりがな, x.row.Fields.ふりがな);
                Assert.AreEqual(x.exp.税込価格, x.row.Fields.税込価格);
                Assert.AreEqual(x.exp.本体価格, x.row.Fields.本体価格);
                Assert.AreEqual(x.exp.メーカー, x.row.Fields.メーカー);
            }
        }

        class ShipModelC : ShipModel
        {
            public double 税込価格_千円 { set; get; }
        }

        [TestMethod]
        public void ReadXsvTest_eventHandler()
        {
            var data =
                 "品番,船種,品名,税込価格_千円,本体価格,メーカー" + Environment.NewLine
                + "103,戦艦,山城　やましろ,1.785千円,\"1,700￥\",Ａ" + Environment.NewLine
                + "215,航空母艦,信濃　しなの,2.940千円,\"2,800￥\",Ｔ" + Environment.NewLine
                + "442,駆逐艦,陽炎　かげろう,1.050千円,\"1,000￥\",Ａ" + Environment.NewLine;

            var expected = new[]{ 
                new { 品番 = 103, 船種 = "戦艦", 品名 = "山城　やましろ", 税込価格_千円 = 1.785, 本体価格 = 1700, メーカー = Maker.Ａ },
                new { 品番 = 215, 船種 = "航空母艦", 品名 = "信濃　しなの", 税込価格_千円 = 2.940, 本体価格 = 2800, メーカー = Maker.Ｔ },
                new { 品番 = 442, 船種 = "駆逐艦", 品名 = "陽炎　かげろう", 税込価格_千円 = 1.050, 本体価格 = 1000, メーカー = Maker.Ａ }
            };

            var csv = new TypedXsvData<ShipModelC>(new[] { "," }, isAutoBinding: true);
            csv.Attached += (o, e) =>
            {
                var numFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
                numFormat.CurrencySymbol = "￥";
                e.Fields.本体価格 = e.Row["本体価格"].AsInt32(NumberStyles.Currency, numFormat);

                numFormat.CurrencySymbol = "千円";
                e.Fields.税込価格_千円 = e.Row["税込価格_千円"].AsDouble(NumberStyles.Currency, numFormat);

            };
            csv.Updated += (o, e) =>
            {
                e.Row["税込価格_千円"] = new XsvField(e.Fields.税込価格_千円, "0.000千円");
                e.Row["本体価格"] = new XsvField(e.Fields.本体価格, "0,000￥");
                e.Row["メーカー"] = new XsvField(e.Fields.メーカー);
            };
            using (var reader = new System.IO.StringReader(data))
            {
                csv.Read(reader);
            }

            foreach (var x in csv.Rows.Zip(expected, (a, b) => new { row = a, exp = b }))
            {
                Assert.AreEqual(x.exp.品番, x.row.Fields.品番);
                Assert.AreEqual(x.exp.船種, x.row.Fields.船種);
                Assert.AreEqual(x.exp.品名, x.row.Fields.品名);
                Assert.AreEqual(x.exp.税込価格_千円, x.row.Fields.税込価格_千円);
                Assert.AreEqual(x.exp.本体価格, x.row.Fields.本体価格);
                Assert.AreEqual(x.exp.メーカー, x.row.Fields.メーカー);
            }

            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                csv.Write(writer, ",");
                Assert.AreEqual(data, sb.ToString());
            }
        }


    }
}
