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
    public class HallsViewModel : INotifyPropertyChanged, IDisposable
    {
        public ObservableCollection<Залы> Halls { get; set; }
        public ObservableCollection<РазмерыЗалов> Размеры { get; set; }

        private CinemaEntities _ctx;

        public int NewNumber { get; set; }
        public string NewSize { get; set; }
        public ICommand ClickCommand { get; set; }
        public ICommand ClosingCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public HallsViewModel()
        {
            _ctx = new CinemaEntities();

            Halls = new ObservableCollection<Залы>(_ctx.Залы);
            Размеры = new ObservableCollection<РазмерыЗалов>(_ctx.РазмерыЗалов);

            ClickCommand = new RelayCommand(arg => ClickMethod());
            Halls.CollectionChanged += Halls_CollectionChanged;
            ClosingCommand = new RelayCommand(arg => ClosingMethod());



        }
        private void ClosingMethod()
        {
            SaveChanges();
        }
        private void ClickMethod()
        {
            // _ctx.РазмерыЗалов.FirstOrDefault(a => a.Наименование == NewSize).ID
            Halls.Add(new Залы() { НомерЗала = NewNumber, РазмерыЗалов = _ctx.РазмерыЗалов.FirstOrDefault(a => a.Наименование == NewSize), IDРазмера = _ctx.РазмерыЗалов.FirstOrDefault(a => a.Наименование == NewSize).ID });
        }

        private void Halls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {


            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Залы s in e.NewItems)
                    {
                        _ctx.Залы.Add(new Залы() { НомерЗала = s.НомерЗала, IDРазмера = s.IDРазмера });
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (Залы s in e.OldItems)
                    {

                        _ctx.Залы.Remove(s);
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    ReadOnlyObservableCollection<Залы> col = sender as ReadOnlyObservableCollection<Залы>;
                    _ctx.Залы.RemoveRange(_ctx.Залы);
                    if (col != null)
                    {
                        foreach (Залы s in col)
                        {
                            _ctx.Залы.Add(new Залы() { НомерЗала = s.НомерЗала, IDРазмера = s.IDРазмера });
                        }
                    }
                    break;
            }


            SaveChanges();
        }

        public void SaveChanges()
        {
            _ctx.SaveChanges();
        }


        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
            {
                return;
            }
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            SaveChanges();
        }


        public void Dispose()
        {
            // мы должны освободить ресурсы контекста при удалении ViewModel
            _ctx.Dispose();
        }
    }
}
