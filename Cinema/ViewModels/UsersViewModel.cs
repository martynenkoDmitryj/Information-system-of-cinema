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
using System.Windows.Data;
using System.Windows.Input;

namespace Cinema.ViewModels
{
    public class UsersViewModel : INotifyPropertyChanged, IDisposable
    {
        public ObservableCollection<Пользователи> Users { get; set; }
        private CinemaEntities _ctx;
        public event PropertyChangedEventHandler PropertyChanged;
        public string NewLogin { get; set; }
        public string NewPassword { get; set; }
        public string NewAccessLevel { get; set; }
        public ICommand ClickCommand { get; set; }
        public ICommand ClosingCommand { get; set; }

        public UsersViewModel()
        {
            _ctx = new CinemaEntities();
           
            Users =  new ObservableCollection<Пользователи>(_ctx.Пользователи);
            ClickCommand = new RelayCommand(arg => ClickMethod());
            ClosingCommand = new RelayCommand(arg => ClosingMethod());

            Users.CollectionChanged += Users_CollectionChanged;
            

        }
        private async void ClosingMethod()
        {
            await SaveChanges();
        }
        private  void ClickMethod()
        {
           Users.Add(new Пользователи() { Логин = NewLogin, Пароль = NewPassword, УровеньДоступа=NewAccessLevel });
        }

        private async void Users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Пользователи s in e.NewItems)
                    {
                        _ctx.Пользователи.Add(new Пользователи() { Логин = s.Логин, Пароль = s.Пароль, УровеньДоступа = s.УровеньДоступа });
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (Пользователи s in e.OldItems)
                    {
                        _ctx.Пользователи.Remove(s);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    ReadOnlyObservableCollection<Пользователи> col = sender as ReadOnlyObservableCollection<Пользователи>;
                    _ctx.Пользователи.RemoveRange(_ctx.Пользователи);
                    if (col != null)
                    {
                        foreach (Пользователи s in col)
                        {
                            _ctx.Пользователи.Add(new Пользователи() { Логин = s.Логин, Пароль = s.Пароль, УровеньДоступа = s.УровеньДоступа });
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
