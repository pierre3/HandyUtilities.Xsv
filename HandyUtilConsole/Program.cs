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
                    case 'e':
                        return;

                }
                Console.WriteLine("***-***-***-***");
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
