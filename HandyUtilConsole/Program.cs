using HandyUtil.Extensions.System;
using HandyUtil.Extensions.System.Linq;
using HandyUtil.Text.Xsv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                Console.WriteLine("[e]- Exit");
                var key = Console.ReadKey(false);
                switch (key.KeyChar)
                {
                    case 'o':
                        using (var reader = new XsvReader(new StringReader(Properties.Resources.ModelShips)))
                        {
                            var disposable = ReadXsvObservable(reader);

                            Console.ReadKey(false);
                            disposable.Dispose();
                            Console.WriteLine("Disposed");
                        }
                        break;
                    case 'r':
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
                    case 'e':
                        return;
                    
                }
                Console.WriteLine("***-***-***-***");
            }
        }
        
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

        static IDisposable ReadXsvObservable(XsvReader reader)
        {
            var csv = new List<string[]>();

            var header = reader.ReadXsvLine(new[] { "," }).ToArray();
            csv.Add(header);

            return reader.ReadXsvObservable(new[] { "," }).Subscribe(row =>
            {
                csv.Add(row);
                Console.WriteLine("OnNext => " + row.ConcatWith(", "));
                Delay(100).Wait();
            },
            e => Console.WriteLine("OnError " + e.Message),
            () =>
            {
                Console.WriteLine("OnCompleted.");
                Console.WriteLine("");
                foreach (var row in csv)
                {
                    Console.WriteLine(row.Select(s => s.MakeXsvField(new[] { "," })).ConcatWith(", "));
                }
            });

        }
#if net45
        static async Task ReadXsvAsync(XsvReader reader)
        {
            var result = await reader.ReadXsvToEndAsync(new[] { "," });

            foreach (var row in result)
            {
                Console.WriteLine(row.Select(s => s.MakeXsvField(new[] { "," })).ConcatWith(", "));
            }
        }
#else
#if net40
        static Task ReadXsvAsync(XsvReader reader)
        {
            return reader.ReadXsvToEndAsync(new[] { "," }).ContinueWith(task =>
            {
                foreach (var row in task.Result )
                {
                    Console.WriteLine(row.Select(s => s.MakeXsvField(new[] { "," })).ConcatWith(", "));
                }
            });
        }
#endif
#endif
        static Task Delay(int ms)
        {
            return Task.Factory.StartNew(() =>
            {
                long startThicks = DateTime.Now.Ticks;
                while (DateTime.Now.Ticks - startThicks < ms * 10000) ;
                
            });
        }

    }
}
