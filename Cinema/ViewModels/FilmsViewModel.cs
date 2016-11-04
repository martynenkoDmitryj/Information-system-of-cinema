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
    public class FilmsViewModel : INotifyPropertyChanged, IDisposable
    {
        public ObservableCollection<Фильмы> Films { get; set; }
        private CinemaEntities _ctx;
        public event PropertyChangedEventHandler PropertyChanged;
        public string NewName { get; set; }
        public string NewGenre { get; set; }
        public string NewDuration { get; set; }
        public int NewYear { get; set; }
        public string NewCountry { get; set; }
        public string NewAuthors { get; set; }
        public string NewDescription { get; set; }
        public ICommand ClickCommand { get; set; }
        public ICommand ClosingCommand { get; set; }

        public FilmsViewModel()
        {
            _ctx = new CinemaEntities();

            Films = new ObservableCollection<Фильмы>(_ctx.Фильмы);
            ClickCommand = new RelayCommand(arg => ClickMethod());
            ClosingCommand = new RelayCommand(arg => ClosingMethod());

            Films.CollectionChanged += Films_CollectionChanged;
            

        }

        private async void ClosingMethod()
        {
            await SaveChanges();
        }

        private void ClickMethod()
        {
            Films.Add(new Фильмы() { Авторы = NewAuthors, Год=NewYear, Длительность=NewDuration, Жанр = NewGenre, Название = NewName, Описание=NewDescription, Страна = NewCountry });
        }

        private async void Films_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {


            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Фильмы s in e.NewItems)
                    {
                        _ctx.Фильмы.Add(new Фильмы() { Авторы = s.Авторы, Год = s.Год, Длительность = s.Длительность, Жанр = s.Жанр, Название = s.Название, Описание = s.Описание, Страна = s.Страна});
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (Фильмы s in e.OldItems)
                    {
                        _ctx.Фильмы.Remove(s);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    ReadOnlyObservableCollection<Фильмы> col = sender as ReadOnlyObservableCollection<Фильмы>;
                    _ctx.Фильмы.RemoveRange(_ctx.Фильмы);
                    if (col != null)
                    {
                        foreach (Фильмы s in col)
                        {
                            _ctx.Фильмы.Add(new Фильмы() { Авторы = s.Авторы, Год = s.Год, Длительность = s.Длительность, Жанр = s.Жанр, Название = s.Название, Описание = s.Описание, Страна = s.Страна });
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
