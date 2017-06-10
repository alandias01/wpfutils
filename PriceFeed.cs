using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WPFUtils
{
    public class StockData : INotifyPropertyChanged
    {
        private string symbol;
        public string Symbol
        {
            get
            {
                return symbol;
            }
            set
            {
                symbol = value;
                this.OnPropertyChanged("Symbol");
            }
        }

        private double price;
        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
                this.OnPropertyChanged("Price");
            }
        }

        public StockData(string s, double p)
        {
            this.Symbol = s;
            this.Price = p;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
        
    public class PriceFeed
    {
        private readonly Timer timer = new Timer();
        private readonly Random rand = new Random();
        private IEnumerable<StockData> listStockData; 

        public event EventHandler<IEnumerable<StockData>> StockUpdate;
        
        public PriceFeed()
        {                     
        }
        
        public void Start(int interval)
        {
            this.InitFirstStockDataPush();
            this.InitTimer(interval);
        }

        private void InitFirstStockDataPush()
        {
            listStockData = new List<StockData>() {
                new StockData("IBM", 900),
                new StockData("GME", 70),
                new StockData("AXA", 50),
                new StockData("ITG", 15),
                new StockData("ATT", 30) };

            this.OnStockUpdate(listStockData);
        }

        private void InitTimer(int interval)
        {            
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = interval;
            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var stockItem in this.listStockData)
            {
                stockItem.Price += rand.Next(-3, 3);
            }

            this.OnStockUpdate(this.listStockData);
            timer.Start();  //Ensures that this timer doesn't elapse until this method fully completes
        }

        private void OnStockUpdate(IEnumerable<StockData> e)
        {
            if (this.StockUpdate != null)
            {
                this.StockUpdate(this, e);
            }
        }
    }

    public class SampleMainWindow
    {
        public Dictionary<string, StockData> DictionaryStockData = new Dictionary<string, StockData>();
        public ObservableCollection<StockData> ListStockData { get; set; }

        public SampleMainWindow()
        {
            //InitializeComponent();
            ListStockData = new ObservableCollection<StockData>();
            //this.DataContext = this;

            var pf = new PriceFeed();
            pf.StockUpdate += Pf_StockUpdate;
            pf.Start(1000);
        }

        private void Pf_StockUpdate(object sender, IEnumerable<StockData> e)
        {
            foreach (var stockItem in e)
            {
                StockData foundStockDataItem;
                if (DictionaryStockData.TryGetValue(stockItem.Symbol, out foundStockDataItem))
                {
                    foundStockDataItem.Price = stockItem.Price;
                }
                else
                {
                    DictionaryStockData.Add(stockItem.Symbol, stockItem);
                    ListStockData.Add(stockItem);
                }
            }
        }
    }


}
