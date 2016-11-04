using Cinema.Models;
using Cinema.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cinema.ViewModels
{
    public class PricesViewModel : INotifyPropertyChanged, IDisposable
    {
        public ObservableCollection<СтоимостьБилетов> Prices { get; set; }
        public ObservableCollection<Сеансы> Seansi { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public CinemaEntities _ctx;
        public int NewSeans { get; set; }
        public decimal NewPrice { get; set; }
        public ICommand ClickCommand { get; set; }
        public ICommand ClosingCommand { get; set; }

        public PricesViewModel()
        {
            _ctx = new CinemaEntities();
            Prices = new ObservableCollection<СтоимостьБилетов>(_ctx.СтоимостьБилетов);
            Seansi = new ObservableCollection<Сеансы>(_ctx.Сеансы);
            ClickCommand = new RelayCommand(arg => ClickMethod());
            ClosingCommand = new RelayCommand(arg => ClosingMethod());
            Prices.CollectionChanged += Prices_CollectionChanged;
            App.Prices = Prices;

        }
        private async void ClosingMethod()
        {
            await SaveChanges();
        }
        private void ClickMethod()
        {
            Prices.Add(new СтоимостьБилетов() { IDСеанса = NewSeans, Стоимость = NewPrice });
        }

        private async void Prices_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (СтоимостьБилетов s in e.NewItems)
                    {
                        _ctx.СтоимостьБилетов.Add(new СтоимостьБилетов() { IDСеанса = s.IDСеанса, Стоимость = s.Стоимость });
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (СтоимостьБилетов s in e.OldItems)
                    {
                        _ctx.СтоимостьБилетов.Remove(s);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    ReadOnlyObservableCollection<СтоимостьБилетов> col = sender as ReadOnlyObservableCollection<СтоимостьБилетов>;
                    _ctx.СтоимостьБилетов.RemoveRange(_ctx.СтоимостьБилетов);
                    if (col != null)
                    {
                        foreach (СтоимостьБилетов s in col)
                        {
                            _ctx.СтоимостьБилетов.Add(new СтоимостьБилетов() { IDСеанса = s.IDСеанса, Стоимость = s.Стоимость });
                        }
                    }
                    break;
            }


            await SaveChanges();
        }

        public async Task SaveChanges()
        {
            await _ctx.SaveChangesAsync();
        }

        private async void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
            {
                return;
            }
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            await SaveChanges();
        }


        public void Dispose()
        {
            // мы должны освободить ресурсы контекста при удалении ViewModel
            _ctx.Dispose();
        }

    }
}
