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

        private void ReadButtonClick(object sender, RoutedEventArgs e)
        {
            if (VM == null) { return; }

            VM.Clear();
            VM.ReadAsObservable();
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
                No = keyedRow["Ｎｏ."].ToInt32(),
                ClassOfShip = keyedRow["艦種"],
                Name = keyedRow["品名"],
                TaxIncludedPrice = keyedRow["税込価格"].ToInt32OrDefault(System.Globalization.NumberStyles.Currency, -1),
                Price = keyedRow["本体価格"].ToInt32OrDefault(System.Globalization.NumberStyles.Currency, -1),
                Maker = keyedRow["メーカー"]
            };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CsvData
    {
        private IDisposable disposable = System.Reactive.Disposables.Disposable.Empty;

        public ObservableCollection<ModelShip> Rows { set; get; }

        public CsvData()
        {
            Rows = new ObservableCollection<ModelShip>();
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
        }

        public void ReadAsObservable()
        {
            Cancel();

            var reader = new XsvReader(new System.IO.StreamReader(@".\ModelShips.txt"));
            var header = reader.ReadXsvLine(new[] { "," }).ToList();

            disposable = reader.ReadXsvAsObservable(new[] { "," })
                .Where(row => !row.First().StartsWith(";"))
                .Select(row => ModelShip.FromCsvRecord(header, row))
                .Where(row => row.Price >= 2500)
                .Where(row => row.ClassOfShip == "戦艦")
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
