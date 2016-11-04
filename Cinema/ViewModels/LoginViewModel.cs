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
    public class LoginViewModel : INotifyPropertyChanged, IDisposable
    {
        public CinemaEntities _ctx;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ClickCommand { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string AccessLevel { get; set; }

        public LoginViewModel()
        {
            _ctx = new CinemaEntities();
            ClickCommand = new RelayCommand(arg => ClickMethod());

        }

        private void ClickMethod()
        {
            Пользователи p = _ctx.Пользователи.FirstOrDefault(u => u.Логин == Login);
            if (p != null)
            {
                if (p.Пароль == Password)
                {
                    App.AccessLevel = p.УровеньДоступа;
                    RaisePropertyChanged("Success");
                }
                else
                {
                    RaisePropertyChanged("InvalidPassword");

                }
            }
            else
            {
                RaisePropertyChanged("NotExistUser");
            }
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
