using HandyUtil.Extensions.System;
using HandyUtil.Extensions.System.Linq;
using HandyUtil.Text.Xsv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace HandyUtilConsole
{
    class Program
    {
        static string testData01
           = "column_1, column_2, column_3, column_4" + Environment.NewLine
           + "  first  ,  second  ,  third  ,  fourth " + Environment.NewLine
           + "0001\t,\t0002\t,\t0003\t,\t0004" + Environment.NewLine
           + @" ""o""""n""""e"" , ""t,w,o"" , ""  three  "" , ""four
four

four

four"" ";

        static void Main(string[] args)
        {
            var csv = new List<Dictionary<string, string>>();
            
            using (var reader = new XsvReader(new StringReader(testData01)))
            {
                var header = reader.ReadXsvLine(new[] { "," });

                IDisposable disposable = reader.AsObservable(new[] { "," }).Subscribe(row =>
                {
                    csv.Add(header.Zip(row, (key, value) => new { key, value }).ToDictionary(a => a.key, a => a.value));
                    Console.WriteLine("OnNext");
                },
                e => Console.WriteLine("OnError " + e.Message),
                () =>
                {
                    Console.WriteLine("OnCompleted.");
                    Console.WriteLine("");
                    foreach (var row in csv)
                    {
                        Console.WriteLine(row.Keys.Select(s => s.MakeXsvField(new[] { ", " })).ConcatWith(", ") + "↵");
                        Console.WriteLine(row.Values.Select(s => s.MakeXsvField(new[] { ", " })).ConcatWith(", ") + "↵");
                        Console.WriteLine(".");
                    }
                });
                Console.ReadKey();

                disposable.Dispose();
            }
            Console.WriteLine("Disposed");
            
            using (var reader2 = new XsvReader(new StringReader(testData01)))
            {
                var task = reader2.ReadXsvToEndAsync(new[] { "," });
                task.Wait();
                foreach (var row in task.Result)
                {
                    Console.WriteLine(row.Select(s => s.MakeXsvField(new[] { ", " })).ConcatWith(", ") + "↵");
                    Console.WriteLine(".");
                }
            }

            Console.ReadKey();

        }


    }
}
