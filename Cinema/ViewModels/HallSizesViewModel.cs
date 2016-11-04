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
    public class HallSizesViewModel : INotifyPropertyChanged, IDisposable
    {
        public ObservableCollection<РазмерыЗалов> HallSizes { get; set; }
        private CinemaEntities _ctx;
        public event PropertyChangedEventHandler PropertyChanged;
        public string NewName { get; set; }
        public int NewCount { get; set; }
        public ICommand ClickCommand { get; set; }
        public ICommand ClosingCommand { get; set; }

        public HallSizesViewModel()
        {
            _ctx = new CinemaEntities();
            HallSizes = new ObservableCollection<РазмерыЗалов>(_ctx.РазмерыЗалов);
            ClickCommand = new RelayCommand(arg => ClickMethod());
            ClosingCommand = new RelayCommand(arg => ClosingMethod());

            HallSizes.CollectionChanged += HallSizes_CollectionChanged;


        }

        private async void ClosingMethod()
        {
            await SaveChanges();
        }

        private void ClickMethod()
        {
            HallSizes.Add(new РазмерыЗалов() { Наименование = NewName, КоличествоРядов = NewCount });
        }

        private async void HallSizes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {


            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (РазмерыЗалов s in e.NewItems)
                    {
                        _ctx.РазмерыЗалов.Add(new РазмерыЗалов() { Наименование = s.Наименование, КоличествоРядов = s.КоличествоРядов });
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (РазмерыЗалов s in e.OldItems)
                    {
                        _ctx.РазмерыЗалов.Remove(s);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    ReadOnlyObservableCollection<РазмерыЗалов> col = sender as ReadOnlyObservableCollection<РазмерыЗалов>;
                    _ctx.РазмерыЗалов.RemoveRange(_ctx.РазмерыЗалов);
                    if (col != null)
                    {
                        foreach (РазмерыЗалов s in col)
                        {
                            _ctx.РазмерыЗалов.Add(new РазмерыЗалов() { Наименование = s.Наименование, КоличествоРядов = s.КоличествоРядов });
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
