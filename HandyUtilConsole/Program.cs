using HandyUtil.Extensions.System;
using HandyUtil.Extensions.System.Linq;
using HandyUtil.Text.Xsv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace HandyUtilConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("Press any key.");
                Console.WriteLine("[r]- ReadXsvAsync");
                Console.WriteLine("[o]- ReadXsvObservable");
                Console.WriteLine("[a]- TextFieldParser");
                Console.WriteLine("[p]- Perfomance");
                Console.WriteLine("[q]- Perfomance2");
                Console.WriteLine("[e]- Exit");
                var key = Console.ReadKey(false);
                switch (key.KeyChar)
                {
                    case 'o':
                        using (var reader = new XsvReader(new StreamReader(@".\ModelShips.txt")))
                        //using (var reader = new XsvReader(new StringReader(Properties.Resources.ModelShips)))
                        {
                            var id = System.Threading.Thread.CurrentThread.ManagedThreadId;
                            Console.WriteLine("start:" + id);
                            var disposable = ReadXsvObservable(reader);
                            Console.ReadKey(false);
                            Console.WriteLine("current:" + id);
                            disposable.Dispose();
                            Console.WriteLine("Disposed");
                            
                        }
                        break;
                    case 'r':
                        //using (var reader = new XsvReader(new StreamReader(@".\ModelShips.txt")))
                        using (var reader = new XsvReader(new StringReader(Properties.Resources.ModelShips)))
                        {
                            ReadXsvAsync(reader).Wait();
                        }
                        break;
                    case 'a':
                        var data = "No., Name, Price, Comment" + Environment.NewLine
                                 + "001, りんご, 98円, 青森産" + Environment.NewLine
                                 + "002, バナナ, 120円, \"    とっても!\r\nお,い,し,い,よ!\"" + Environment.NewLine
                                 + "" + Environment.NewLine
                                 + "004, \"うまい棒\"\"めんたい\"\"\", 10円," + Environment.NewLine
                                 + "005, バナメイ海老, 800円, \"300ｇ\r\n\r\nエビチリに\"";
                        Console.WriteLine("<< Source >>");
                        Console.WriteLine(data);
                        Console.WriteLine("");
                        Console.WriteLine("<< XsvReader.Parse() >>");
                        foreach (var row in Parse(data))
                        {
                            Console.WriteLine(row.ConcatWith(";") + "<n>");
                        }
                        Console.WriteLine("");                        
                        Console.WriteLine("<< TextFieldParser (TrimWhiteSpace=true) >>");
                        foreach (var row in ReadFields(data,trimWS:true))
                        {
                            Console.WriteLine(row.ConcatWith(";") + "<n>");
                        }
                        Console.WriteLine("");
                        Console.WriteLine("<< TextFieldParser (TrimWhiteSpace=false) >>");
                        foreach (var row in ReadFields(data,trimWS:false))
                        {
                            Console.WriteLine(row.ConcatWith(";") + "<n>");
                        }
                        break;
                    case 'p':
                        Console.WriteLine("");
                        var delimiters = new[] { "," };
                        var sw = System.Diagnostics.Stopwatch.StartNew();
                        for (int i = 0; i < 1000; i++)
                        {
                            using (var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(new StringReader(Properties.Resources.ModelShips)))
                            {
                                parser.Delimiters = delimiters;
                                parser.TrimWhiteSpace = true;
                                while (!parser.EndOfData)
                                {
                                    parser.ReadFields();
                                }
                            }
                        }
                        sw.Stop();
                        Console.WriteLine(string.Format("TextFieldParser: {0} ms", sw.ElapsedMilliseconds));
                        sw.Reset();
                        sw.Start();
                        for (int i = 0; i < 1000; i++)
                        {
                            using (var reader = new XsvReader(new StringReader(Properties.Resources.ModelShips)))
                            {
                                while (!reader.EndOfData)
                                {
                                    reader.ReadXsvLine(delimiters).ToArray();
                                }
                            }
                        }
                        sw.Stop();
                        Console.WriteLine(string.Format("XsvReader: {0} ms", sw.ElapsedMilliseconds));

                        break;
#if net45
                    case 'q':
                        ReadAsyncTest().Wait();
                        break;
#endif
                    case 'e':
                        return;
                    
                }
                Console.WriteLine("***-***-***-***");
            }
        }

#if net45
        static async Task ReadAsyncTest() 
        {
            var sw = new System.Diagnostics.Stopwatch();
            var lines = String.Join(Environment.NewLine, Enumerable.Range(1, 10000)
                .Select(_ => Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString()));
            using (var reader = new XsvReader(new StringReader(lines)))
            {
                while (!reader.EndOfData)
                {
                    await reader.ReadLineAsync().ConfigureAwait(false);
                }
            }

            sw.Restart();
            using (var reader = new XsvReader(new StringReader(lines)))
            {
                while (!reader.EndOfData)
                {
                    await reader.ReadLineAsync().ConfigureAwait(false);
                }
            }
            sw.Stop();
            Console.WriteLine(string.Format("ReadlineAsync: {0} ms", sw.ElapsedMilliseconds));

            //sw.Restart();
            //using (var reader = new XsvReader(new StringReader(lines)))
            //{
            //    while (!reader.EndOfData)
            //    {
            //        await reader.ReadLineAsync2().ConfigureAwait(false);
            //    }
            //}
            //sw.Stop();
            //Console.WriteLine(string.Format("ReadlineAsync2: {0} ms", sw.ElapsedMilliseconds));
        }

        static IDisposable ReadXsvObservable(XsvReader reader)
        {
            var csv = new List<IList<string>>();

            var header = reader.ReadXsvLine(new[] { "," }).ToArray();
            csv.Add(header);

            return reader.ReadXsvAsObservable(new[] { "," }).Subscribe(row =>
            {
                csv.Add(row);
                Console.WriteLine("OnNext:" + System.Threading.Thread.CurrentThread.ManagedThreadId + " => " + row.ConcatWith(", "));
            },
            e => Console.WriteLine("OnError " + e.Message),
            () =>
            {
                Console.WriteLine("OnCompleted:" +System.Threading.Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("");
                foreach (var row in csv)
                {
                    Console.WriteLine(row.Select(s => s.MakeXsvField(new[] { "," })).ConcatWith(", "));
                }
            });

        }
#endif

        static IEnumerable<string[]> Parse(string data) 
        {
            using (var sr = new StringReader(data))
            {
                var line = sr.ReadLine();
                while (line != null)
                {
                    yield return XsvReader.Parse(line, new[] { "," }, () => sr.ReadLine()).ToArray();
                    line = sr.ReadLine();
                }
            }
        }

        static IEnumerable<string[]> ReadFields(string data,bool trimWS) {
            using (var tfp = new Microsoft.VisualBasic.FileIO.TextFieldParser(new StringReader(data)))
            {
                tfp.Delimiters = new[] { "," };
                tfp.TrimWhiteSpace = trimWS;
                while (!tfp.EndOfData)
                {
                    yield return tfp.ReadFields();
                }
            }
        }

        
#if net45
        static async Task ReadXsvAsync(XsvReader reader)
        {
            var currentId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("start:" + System.Threading.Thread.CurrentThread.ManagedThreadId);
            var result = await reader.ReadXsvToEndAsync(new[] { "," });
            Console.WriteLine("end:" + System.Threading.Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("current:" + currentId);
            foreach (var row in result)
            {
                Console.WriteLine(row.Select(s => s.MakeXsvField(new[] { "," })).ConcatWith(", "));
            }
        }
#else
#if net40
        static IDisposable ReadXsvObservable(XsvReader reader)
        {
            var csv = new List<IList<string>>();

            var header = reader.ReadXsvLine(new[] { "," }).ToArray();
            csv.Add(header);

            return reader.ReadXsvAsObservable(new[] { "," })
                .Subscribe(row =>
                {
                    csv.Add(row);
                    Console.WriteLine("OnNext:" + System.Threading.Thread.CurrentThread.ManagedThreadId + " => " + row.ConcatWith(", "));
                },
            e => Console.WriteLine("OnError " + e.Message),
            () =>
            {
                Console.WriteLine("OnCompleted:" + System.Threading.Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("");
                foreach (var row in csv)
                {
                    Console.WriteLine(row.Select(s => s.MakeXsvField(new[] { "," })).ConcatWith(", "));
                }
            });

        }

        static Task ReadXsvAsync(XsvReader reader)
        {
            return Task.Factory.StartNew(() => { Console.WriteLine("Not Supported."); });
        }
#endif
#endif
        
    }
}
