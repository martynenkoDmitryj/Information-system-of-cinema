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
    public class SeansiViewModel : INotifyPropertyChanged, IDisposable
    {

        public ObservableCollection<Сеансы> Seansi { get; set; }
        public ObservableCollection<Фильмы> Films { get; set; }
        public ObservableCollection<Залы> Halls { get; set; }

        private CinemaEntities _ctx;
        public event PropertyChangedEventHandler PropertyChanged;
        public string NewTime { get; set; }
        public int NewHall { get; set; }
        public int NewFilm { get; set; }
        public bool NewIsFirst { get; set; }
        public DateTime NewDate { get; set; }
        public decimal NewPrice { get; set; }
        public Сеансы SelectedSeans { get; set; }
        public ICommand ClickCommand { get; set; }
        public ICommand MoveCommand { get; set; }
        public ICommand SelectCommand { get; set; }
        public ICommand ClosingCommand { get; set; }

        public SeansiViewModel()
        {
            _ctx = new CinemaEntities();
            Seansi = new ObservableCollection<Сеансы>(_ctx.Сеансы);
            Films = new ObservableCollection<Фильмы>(_ctx.Фильмы);
            Halls = new ObservableCollection<Залы>(_ctx.Залы);
            ClickCommand = new RelayCommand(arg => ClickMethod());
            MoveCommand = new RelayCommand(arg => MoveMethod());
            SelectCommand = new RelayCommand(arg => SelectMethod());
            ClosingCommand = new RelayCommand(arg => ClosingMethod());
            Seansi.CollectionChanged += Seansi_CollectionChanged;
            NewDate = DateTime.Now;


        }
        private async void ClosingMethod()
        {
            await SaveChanges();
        }
        private void SelectMethod()
        {
            ProductionWindowFactory f = new ProductionWindowFactory();
            Залы z = _ctx.Залы.FirstOrDefault(a => a.ID == SelectedSeans.IDЗала);
            App.A = SelectedSeans.ID;
            App.B = _ctx.РазмерыЗалов.FirstOrDefault(b => b.ID == z.IDРазмера).КоличествоРядов.Value;
            f.CreateNewWindow();
        }

        private  void MoveMethod()
        {
            if (SelectedSeans.ID == 0)
            {
                App.Prices.Add(new СтоимостьБилетов() { IDСеанса = _ctx.Сеансы.ToList().Last().ID, Стоимость = NewPrice });
            }
            else
            {
                App.Prices.Add(new СтоимостьБилетов() { IDСеанса = SelectedSeans.ID, Стоимость = NewPrice });
            }

        }

        private void ClickMethod()
        {
            Seansi.Add(new Сеансы() { IDФильма = NewFilm, IDЗала = NewHall, Дата=NewDate,Время=NewTime,Премьера=NewIsFirst });
        }

        private async void Seansi_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Сеансы s in e.NewItems)
                    {
                        _ctx.Сеансы.Add(new Сеансы() { IDФильма = s.IDФильма, IDЗала = s.IDЗала, Дата = s.Дата, Время = s.Время, Премьера = s.Премьера });
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (Сеансы s in e.OldItems)
                    {
                        _ctx.Сеансы.Remove(s);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    ReadOnlyObservableCollection<Сеансы> col = sender as ReadOnlyObservableCollection<Сеансы>;
                    _ctx.Сеансы.RemoveRange(_ctx.Сеансы);
                    if (col != null)
                    {
                        foreach (Сеансы s in col)
                        {
                            _ctx.Сеансы.Add(new Сеансы() { IDФильма = s.IDФильма, IDЗала = s.IDЗала, Дата = s.Дата, Время = s.Время, Премьера = s.Премьера });
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
