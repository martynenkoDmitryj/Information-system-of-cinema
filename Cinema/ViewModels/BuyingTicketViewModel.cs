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
    public class BuyingTicketViewModel : INotifyPropertyChanged, IDisposable
    {
        public CinemaEntities _ctx;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Билеты> Tickets { get; set; }
        public ObservableCollection<Бронь> Reservations { get; set; }

        public ICommand Command1 { get; set; }
        public ICommand Command2 { get; set; }

        public int SelectedRow { get; set; }
        public int SelectedPlace { get; set; }
        public int SeansID { get; set; }

        public BuyingTicketViewModel()
        {
            _ctx = new CinemaEntities();
            Tickets = new ObservableCollection<Билеты>(_ctx.Билеты);
            Reservations = new ObservableCollection<Бронь>(_ctx.Бронь);
            Tickets.CollectionChanged += Tickets_CollectionChanged;
            Reservations.CollectionChanged += Reservations_CollectionChanged;
            Command1 = new RelayCommand(arg => Method1());

        }

        private async void Reservations_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (Бронь s in e.OldItems)
                    {
                        _ctx.Бронь.Remove(s);
                    }
                    break;
            }


            await SaveChanges();
        }

        public async Task SaveChanges()
        {
            await _ctx.SaveChangesAsync();
        }

        private async void Tickets_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Билеты s in e.NewItems)
                    {
                        _ctx.Билеты.Add(new Билеты() { IDСеанса = s.IDСеанса, IDЗала = s.IDЗала, Место = s.Место, Ряд = s.Ряд, Бронь = s.Бронь });
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (Билеты s in e.OldItems)
                    {
                        _ctx.Билеты.Remove(s);
                    }
                    break;

               
            }
          await  SaveChanges();
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
        private void Method1()
        {
            Tickets.Add(new Билеты() { IDСеанса = App.A, IDЗала = _ctx.Сеансы.FirstOrDefault(k => k.ID == App.A).IDЗала, Место = SelectedPlace, Ряд = SelectedRow, Бронь = false });
            RaisePropertyChanged("Ticket");
        }
    }
}
