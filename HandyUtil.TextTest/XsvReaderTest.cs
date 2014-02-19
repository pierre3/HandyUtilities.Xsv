using HandyUtil.Extensions.System.Linq;
using HandyUtil.Text.Xsv;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#if net40
using System.Threading.Tasks;
#endif

namespace HandyUtil.TextTest
{
    [TestClass]
    public class XsvReaderTest
    {
        static string testData01
            = "column_1, column_2, column_3, column_4" + Environment.NewLine
            + "  first  ,  second  ,  third  ,  fourth " + Environment.NewLine
            + "0001\t,\t0002\t,\t0003\t,\t0004" + Environment.NewLine
            + @" ""o""""n""""e"" , ""t,w,o"" , ""  three  "" , ""four
four

four

four"" ";

        static string[][] expected01 = new[]{
            new []{ "column_1", "column_2", "column_3", "column_4" },
            new []{ "first", "second", "third", "fourth" },
            new []{ "0001", "0002", "0003", "0004" },
            new []{ "o\"n\"e", "t,w,o", "  three  ",
                    "four" + Environment.NewLine 
                    + "four" + Environment.NewLine + Environment.NewLine 
                    + "four" + Environment.NewLine + Environment.NewLine 
                    + "four" 
                  }};

        static string testData02
           = "column_1\tcolumn_2 column_3\tcolumn_4" + Environment.NewLine
           + "\"  first\"\t\"second  \" \"  third  \"\t\"four\tth \"" + Environment.NewLine
           + " 0002 \t 0005 0006\t" + Environment.NewLine
           + @"""o""""n""""e"" ""t w o"" three ""f
 o
  u
   r""";

        static string[][] expected02 = new[]{
            new []{ "column_1", "column_2", "column_3", "column_4" },
            new []{ "  first", "second  ", "  third  ", "four\tth " },
            new []{ "", "0002", "", "", "0005","0006", "" },
            new []{ "o\"n\"e", "t w o", "three",
                      "f" + Environment.NewLine 
                    + " o" + Environment.NewLine
                    + "  u" + Environment.NewLine
                    + "   r" 
                  }};

        [TestMethod]
        public void ReadXsvLineTest01()
        {
            var result = new List<string[]>();
            using (var reader = new XsvReader(new StringReader(testData01)))
            {
                while (!reader.EndOfData)
                {
                    var row = reader.ReadXsvLine(new[] { "," }).ToArray();
                    result.Add(row);
                }
            }
            foreach (var row in result.Zip(expected01, (l, r) => new { expected = r, actual = l }))
            {
                Console.WriteLine(row.actual.ConcatWith("|") + "\\n");
                CollectionAssert.AreEqual(row.expected, row.actual);
            }
        }

        [TestMethod]
        public void ReadXsvLineTest02()
        {
            var result = new List<string[]>();
            using (var reader = new XsvReader(new StringReader(testData02)))
            {
                while (!reader.EndOfData)
                {
                    var row = reader.ReadXsvLine(new[] { " ", "\t" }).ToArray();
                    result.Add(row);
                }
            }
            foreach (var row in result.Zip(expected02, (l, r) => new { expected = r, actual = l }))
            {
                Console.WriteLine(row.actual.ConcatWith("|") + "\\n");
                CollectionAssert.AreEqual(row.expected, row.actual);
            }
        }
#if net40
        [TestMethod]
        public void ReadXsvObservableTest()
        {
            var csv = new List<string[]>();

            using (var reader = new XsvReader(new StringReader(testData01)))
            {
                csv.Add(reader.ReadXsvLine(new[] { "," }).ToArray());

                IDisposable disposable = reader.ReadXsvObservable(new[] { "," }).Subscribe(row =>
                {
                    csv.Add(row);
                    Console.WriteLine("OnNext");
                    Console.WriteLine(row.ConcatWith(", "));
                },
                e => Console.WriteLine("OnError " + e.Message),
                () =>
                {
                    Console.WriteLine("OnCompleted.");
                    foreach (var row in csv.Zip(expected01, (l, r) => new { expected = r, actual = l }))
                    {
                        Console.WriteLine(row.actual.ConcatWith("|") + "\\n");
                        CollectionAssert.AreEqual(row.expected, row.actual);
                    }
                });
            }
        }
#endif
#if net45
        [TestMethod]
        public async Task ReadXsvToEndAsyncTest()
        {
            using (var reader2 = new XsvReader(new StringReader(testData01)))
            {
                var rows = await reader2.ReadXsvToEndAsync(new[] { "," });

                foreach (var row in rows.Zip(expected01, (l, r) => new { expected = r, actual = l }))
                {
                    Console.WriteLine(row.actual.ConcatWith("|") + "\\n");
                    CollectionAssert.AreEqual(row.expected, row.actual);
                }
            }
        }
#else
#if net40
        [TestMethod]
        public void ReadXsvToEndAsyncTest()
        {
            using (var reader2 = new XsvReader(new StringReader(testData01)))
            {
                reader2.ReadXsvToEndAsync(new[] { "," }).ContinueWith(readTask =>
                {
                    var rows = readTask.Result;
                    foreach (var row in rows.Zip(expected01, (l, r) => new { expected = r, actual = l }))
                    {
                        Console.WriteLine(row.actual.ConcatWith("|") + "\\n");
                        CollectionAssert.AreEqual(row.expected, row.actual);
                    }
                });
            }
        }
#endif
#endif
    }
}
