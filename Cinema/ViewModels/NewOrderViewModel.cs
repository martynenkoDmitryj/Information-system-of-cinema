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
    public class NewOrderViewModel : INotifyPropertyChanged, IDisposable
    {
        private CinemaEntities _ctx;
        public string NewName { get; set; }
        public string NewPhone { get; set; }
        public ICommand ClickCommand { get; set; }
        public ObservableCollection<Билеты> Tickets { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public NewOrderViewModel()
        {
            _ctx = new CinemaEntities();
            Tickets = new ObservableCollection<Билеты>(_ctx.Билеты);
            ClickCommand = new RelayCommand(arg => ClickMethod());
        }

        private void ClickMethod()
        {
            _ctx.Бронь.Add(new Бронь() { ФИО = NewName, Телефон = NewPhone, IDБилета = Tickets.Last().ID });
            _ctx.SaveChanges();
            RaisePropertyChanged("Collection");
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
            {
                return;
            }
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            
        }


        public void Dispose()
        {
            // мы должны освободить ресурсы контекста при удалении ViewModel
            _ctx.Dispose();
        }
    }
}
