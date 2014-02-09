using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HandyUtil.Extensions.System;
using HandyUtil.Extensions.System.Linq;
using HandyUtil.Text.Xsv;
using System.IO;
using System.Text;
using System.Linq;

namespace Utility.TextTest
{
    [TestClass]
    public class XsvDataTest
    {
        [TestMethod]
        public void TokenizeTest()
        {
            const string quot = "\"";
            const string quot2 = quot + quot;
            const string ret = "\r\n";
            const string sep = ";";
            const string sep2 = ",";

            string[] data = { 
                "columnA" + sep,
                "columnB" + sep2,
                "columnC" + sep,
                "columnD" + sep2,
                "columnE" + ret,

                "　　リンゴ " + sep2,
                "" + sep,
                (quot2 + "み" + quot2 + "かん").Enclose(quot) + sep2,
                ("バナ" + ret + "ナ").Enclose(quot) + sep,
                "  パイナップル  " + sep + ret,

                "" + sep,
                "" + sep2,
                "" + sep2,
                "" + sep,
                "" + ret,

                "  ABCD  ".Enclose(quot) + sep,
                ("E" + sep + "FG").Enclose(quot) + sep2,
                ("HIJ" + ret + "K" + ret + ret + "LMN").Enclose(quot) + sep,
                "OPQR"+sep2,
                ""
            };
            var testData = data.ConcatWith("");
            Console.WriteLine(testData);
            Console.WriteLine("");

            using (var reader = new XsvReader(new System.IO.StringReader(testData)))
            {
                var target = new XsvData<XsvDataRow>(new[] { sep, sep2 });
                target.Read(reader, true);
                foreach (var row in target.Rows)
                {
                    Console.WriteLine(row.ToString() + " <<");
                }
                Console.WriteLine("");
                foreach (var row in target.Rows)
                {
                    Console.WriteLine(row.OutputFields(new[] { ":" }, ":") + " <<");
                }

                var sb = new StringBuilder();
                using (var writer = new StringWriter(sb))
                {
                    target.Write(writer);
                }
                Console.WriteLine("----------");
                Console.WriteLine(sb.ToString());
            }
        }

        enum Maker
        {
            UNKNOWN,
            Ａ,
            Ｔ,
            Ｈ,
            Ｓ
        }

        class ShipModelDataRow : XsvDataRow
        {
            public int 品番 { set; get; }
            public string 船種 { set; get; }
            public string 品名 { set; get; }
            public int 税込価格 { set; get; }
            public int 本体価格 { set; get; }
            public Maker メーカー { set; get; }

            public ShipModelDataRow()
                : base()
            { }

            protected override void AttachFields()
            {
                this.品番 = this["品番"].AsInt32(defaultValue:0);
                this.船種 = this["船種"].AsString(defaultValue:"不明");
                this.品名 = this["品名"].AsString(defaultValue:"名無し");
                this.税込価格 = this["税込価格"].AsInt32(defaultValue:-1);
                this.本体価格 = this["本体価格"].AsInt32(numberStyles:System.Globalization.NumberStyles.Currency, defaultValue:-1);
                this.メーカー = this["メーカー"].AsEnum<Maker>(defaultValue:Maker.UNKNOWN);
            }
            protected override void UpdateFields()
            {

            }
        }

        [TestMethod]
        public void ReadCsvTest_indexer()
        {
            string data = "品番,船種,品名,税込価格,本体価格,メーカー" + Environment.NewLine
                + "110,戦艦,比叡　ひえい,2625,\"2,500\",Ｈ" + Environment.NewLine
                + "119,航空戦艦,伊勢　いせ,3360,\"3,200\",Ｈ" + Environment.NewLine
                + "216,航空母艦,瑞鳳　ずいほう,2100,\"2,000\",Ｈ" + Environment.NewLine
                + "320,軽巡洋艦,名取　なとり,1365,\"1,300\",Ｔ" + Environment.NewLine
                + "334,重巡洋艦,那智　なち,1890,\"1,800\",Ｈ" + Environment.NewLine
                + "429,駆逐艦,桜　さくら,630,600,Ｔ";

            var expected = new[]{ 
                new { 品番 = 110, 船種 = "戦艦", 品名 = "比叡　ひえい", 税込価格 = 2625, 本体価格 = 2500, メーカー = Maker.Ｈ },
                new { 品番 = 119, 船種 = "航空戦艦", 品名 = "伊勢　いせ", 税込価格 = 3360, 本体価格 = 3200, メーカー = Maker.Ｈ },
                new { 品番 = 216, 船種 = "航空母艦", 品名 = "瑞鳳　ずいほう", 税込価格 = 2100, 本体価格 = 2000, メーカー = Maker.Ｈ},
                new { 品番 = 320, 船種 = "軽巡洋艦", 品名 = "名取　なとり", 税込価格 = 1365, 本体価格 = 1300, メーカー = Maker.Ｔ },
                new { 品番 = 334, 船種 = "重巡洋艦", 品名 = "那智　なち", 税込価格 = 1890, 本体価格 = 1800, メーカー = Maker.Ｈ },
                new { 品番 = 429, 船種 = "駆逐艦", 品名 = "桜　さくら", 税込価格 = 630, 本体価格 = 600, メーカー = Maker.Ｔ }
            };

            var target = new XsvData(new[] { "," });
            using (var reader = new XsvReader(new System.IO.StringReader(data)))
            {
                target.Read(reader, headerExists: true);
            }

            foreach (var x in target.Rows.Zip(expected, (a, b) => new { row = a, exp = b }))
            {
                Assert.AreEqual(x.exp.品番, x.row["品番"].AsInt32());
                Assert.AreEqual(x.exp.船種, x.row["船種"].AsString());
                Assert.AreEqual(x.exp.品名, x.row["品名"].AsString());
                Assert.AreEqual(x.exp.税込価格, x.row["税込価格"].AsInt32());
                Assert.AreEqual(x.exp.本体価格, x.row["本体価格"].AsInt32(System.Globalization.NumberStyles.Currency));
                Assert.AreEqual(x.exp.メーカー, x.row["メーカー"].AsEnum<Maker>());
            }
        }

        [TestMethod]
        public void ReadCsvTest_inhelitedXsvDataRow()
        {
            string data = "品番,船種,品名,税込価格,本体価格,メーカー" + Environment.NewLine
                + "110,戦艦,比叡　ひえい,2625,\"2,500\",Ｈ" + Environment.NewLine
                + "119,航空戦艦,伊勢　いせ,3360,\"3,200\",Ｈ" + Environment.NewLine
                + "216,航空母艦,瑞鳳　ずいほう,2100,\"2,000\",Ｈ" + Environment.NewLine
                + "320,軽巡洋艦,名取　なとり,1365,\"1,300\",Ｔ" + Environment.NewLine
                + "334,重巡洋艦,那智　なち,1890,\"1,800\",Ｈ" + Environment.NewLine
                + "xxx,,,xxx,xxx,xxx";

            var expected = new[]{ 
                new { 品番 = 110, 船種 = "戦艦", 品名 = "比叡　ひえい", 税込価格 = 2625, 本体価格 = 2500, メーカー = Maker.Ｈ },
                new { 品番 = 119, 船種 = "航空戦艦", 品名 = "伊勢　いせ", 税込価格 = 3360, 本体価格 = 3200, メーカー = Maker.Ｈ },
                new { 品番 = 216, 船種 = "航空母艦", 品名 = "瑞鳳　ずいほう", 税込価格 = 2100, 本体価格 = 2000, メーカー = Maker.Ｈ},
                new { 品番 = 320, 船種 = "軽巡洋艦", 品名 = "名取　なとり", 税込価格 = 1365, 本体価格 = 1300, メーカー = Maker.Ｔ },
                new { 品番 = 334, 船種 = "重巡洋艦", 品名 = "那智　なち", 税込価格 = 1890, 本体価格 = 1800, メーカー = Maker.Ｈ },
                new { 品番 = 0, 船種 = "不明", 品名 = "名無し", 税込価格 = -1, 本体価格 = -1, メーカー = Maker.UNKNOWN }
            };

            var target = new XsvData<ShipModelDataRow>(new[] { "," });
            using (var reader = new XsvReader(new System.IO.StringReader(data)))
            {
                target.Read(reader, true);
            }

            foreach (var x in target.Rows.Zip(expected, (a, b) => new { row = a, exp = b }))
            {
                Assert.AreEqual(x.exp.品番, x.row.品番);
                Assert.AreEqual(x.exp.船種, x.row.船種);
                Assert.AreEqual(x.exp.品名, x.row.品名);
                Assert.AreEqual(x.exp.税込価格, x.row.税込価格);
                Assert.AreEqual(x.exp.本体価格, x.row.本体価格);
                Assert.AreEqual(x.exp.メーカー, x.row.メーカー);
            }

        }



    }
}
