using HandyUtil.Extensions.System;
using HandyUtil.Text.Xsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;

namespace WpfApp
{

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private CsvData VM { get { return DataContext as CsvData; } }

        public MainWindow()
        {
            InitializeComponent();
        }

        //private void ReadButtonClick(object sender, RoutedEventArgs e)
        private async void ReadButtonClick(object sender, RoutedEventArgs e)
        {
            if (VM == null) { return; }

            VM.Clear();
            //VM.ReadAsObservable();
            await VM.ReadAsync();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            if (VM == null) { return; }
            VM.Cancel();
        }
    }

    /// <summary>
    /// １レコード分のデータ
    /// </summary>
    public class ModelShip
    {
        public int No { set; get; }
        public string ClassOfShip { set; get; }
        public string Name { set; get; }
        public int TaxIncludedPrice { set; get; }
        public int Price { set; get; }
        public string Maker { set; get; }

        public static ModelShip FromCsvRecord(IList<string> header, IList<string> row)
        {
            var keyedRow = header.Zip(row, (h, r) => new { h, r }).ToDictionary(a => a.h, a => a.r);
            return new ModelShip()
            {
                No = keyedRow["No"].ToInt32(),
                ClassOfShip = keyedRow["ClassOfShip"],
                Name = keyedRow["Name"],
                TaxIncludedPrice = keyedRow["TaxIncludedPrice"].ToInt32OrDefault(System.Globalization.NumberStyles.Currency, -1),
                Price = keyedRow["Price"].ToInt32OrDefault(System.Globalization.NumberStyles.Currency, -1),
                Maker = keyedRow["Maker"]
            };
        }
    }

    /// <summary>
    /// CsvData
    /// </summary>
    public class CsvData
    {
        private IDisposable disposable = System.Reactive.Disposables.Disposable.Empty;
        private System.Threading.CancellationTokenSource cts;
        
        public ObservableCollection<ModelShip> Rows { set; get; }
        public ObservableCollection<XsvDataRow<ModelShip>> XsvRows { set; get; }

        public CsvData()
        {
            Rows = new ObservableCollection<ModelShip>();
            XsvRows = new ObservableCollection<XsvDataRow<ModelShip>>();
            cts = new System.Threading.CancellationTokenSource();
        }

        public void Clear() 
        {
            Rows.Clear();
        }

        public void Cancel()
        {
            System.Diagnostics.Debug.WriteLine("cancel");
            if (disposable != null)
            {
                disposable.Dispose();
                disposable = null;
            }
            if (cts != null)
            {
                cts.Cancel();
                cts = new System.Threading.CancellationTokenSource();
            }
        }

        public async Task ReadAsync()
        {
            var data = new TypedXsvData<ModelShip>(this.XsvRows, 
                new XsvDataSettings() { CommentToken=";" }, true);
            using (var reader = new System.IO.StreamReader(@".\ModelShips.txt"))
            {
                await data.ReadAsync(reader,cts.Token);
            }
        }
        
        public void ReadAsObservable()
        {
            Cancel();

            var reader = new XsvReader(new System.IO.StreamReader(@".\ModelShips.txt"));
            var header = reader.ReadXsvLine(new[] { "," }).ToList();

            disposable = reader.ReadXsvAsObservable(new[] { "," })
                .Where(row => !row.First().StartsWith(";"))
                .Select(row => ModelShip.FromCsvRecord(header, row))
                //.Where(row => row.Price >= 2500)
                //.Where(row => row.ClassOfShip == "戦艦")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(
                    row => 
                    {
                        System.Diagnostics.Debug.WriteLine("OnNext");
                        this.Rows.Add(row); 
                    },
                    error => 
                    {
                        System.Diagnostics.Debug.WriteLine("OnError:" + error);
                        reader.Dispose();
                    },
                    () =>
                    {
                        System.Diagnostics.Debug.WriteLine("OnCompleted");
                        reader.Dispose();
                    });
        }
    }
}
