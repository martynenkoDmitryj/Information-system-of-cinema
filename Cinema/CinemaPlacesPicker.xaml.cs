using Cinema.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Cinema.ViewModels;
using System.Windows.Input;
using Cinema.ViewModels.Commands;

namespace Cinema
{
    /// <summary>
    /// Interaction logic for CinemaPlacesPicker.xaml
    /// </summary>
    public partial class CinemaPlacesPicker : UserControl
    {
        public int _rowsCount;

        public static DependencyProperty SelectedRowProperty = DependencyProperty.Register("SelectedRow", typeof(int), typeof(CinemaPlacesPicker), new PropertyMetadata(null));
        public static DependencyProperty IDSeansaProperty = DependencyProperty.Register("IDSeansa", typeof(int), typeof(CinemaPlacesPicker), new PropertyMetadata(null));

        public static DependencyProperty SelectedPlaceCommandProperty = DependencyProperty.Register("SelectedPlaceCommand", typeof(ICommand), typeof(CinemaPlacesPicker), new PropertyMetadata(null));
        public static DependencyProperty SelectedPlaceProperty = DependencyProperty.Register("SelectedPlace", typeof(int), typeof(CinemaPlacesPicker), new PropertyMetadata(null));

        public ObservableCollection<Бронь> Reservations { get; set; }
        public ObservableCollection<Билеты> Tickets
        {
            get;
            set;
        }
        public int SelectedRow
        {
            get
            {
                return (int)GetValue(SelectedRowProperty);
            }
            set
            {
                SetValue(SelectedRowProperty, value);

            }
        }
        public int SelectedPlace
        {
            get
            {
                return (int)GetValue(SelectedPlaceProperty);
            }
            set
            {
                SetValue(SelectedPlaceProperty, value);

            }
        }
        public int IDSeansa
        {
            get
            {
                return (int)GetValue(IDSeansaProperty);
            }
            set
            {
                SetValue(IDSeansaProperty, value);

            }
        }
        public ICommand SelectedPlaceCommand
        {
            get
            {
                return (ICommand)GetValue(SelectedPlaceCommandProperty);
            }
            set
            {
                SetValue(SelectedPlaceCommandProperty, value);
            }
        }

        public int RowsCount {
            get
            {
                return _rowsCount;
            }
            set
            {
                _rowsCount = value;
                Height = 60 * RowsCount + 10;
                MainStackPanel.Children.Clear();
                for(int i = 0; i < _rowsCount; i++)
                {
                    StackPanel p = new StackPanel();
                    p.Height = 50;
                    p.Width = 630;
                    p.Children.Add(new Label() { Content = (i + 1).ToString() + ":", Margin = new Thickness(0, 12.5, 0, 12.5) });
                    for(int j = 0; j < 10; j++)
                    {
                        p.Children.Add(new Button() { Width = 50, Margin = new Thickness(5, 5, 5, 5), Content = j + 1 });
                       
                    }
                    p.Orientation = Orientation.Horizontal;
                    MainStackPanel.Children.Add(p);
                }
            }
        }
        public CinemaPlacesPicker()
        {
            InitializeComponent();
            DataContextChanged += CinemaPlacesPicker_DataContextChanged;
            Tickets = new ObservableCollection<Билеты>();
            
            Reservations = new ObservableCollection<Бронь>();
            Height = (60 * RowsCount) + 10;
           
        }

        private void CinemaPlacesPicker_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null)
                Tickets = (DataContext as BuyingTicketViewModel).Tickets;
            foreach (StackPanel s in MainStackPanel.Children.OfType<StackPanel>())
            {
                foreach (Button b in s.Children.OfType<Button>())
                {
                    b.Background = Brushes.LightGreen;
                    b.Click += (o, a) =>
                    {
                        string str = s.Children.OfType<Label>().First().Content.ToString().Remove(s.Children.OfType<Label>().First().Content.ToString().Length - 1);
                        SelectedRow = int.Parse(str);
                        SelectedPlace = int.Parse(b.Content.ToString());
                        if (b.Background == Brushes.LightGreen)
                        {
                            SelectedPlaceCommand.Execute(this);
                            
                        }
                        else if (b.Background == Brushes.LightYellow)
                        {
                            if (MessageBox.Show("Место забронировано! Желаете удалить бронь?", "Место забронировано", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                //удалить бронировку
                                var l = (DataContext as BuyingTicketViewModel).Tickets.FirstOrDefault(v => v.IDСеанса == IDSeansa);
                                l.Бронь = false;
                                (DataContext as BuyingTicketViewModel).Reservations.Remove((DataContext as BuyingTicketViewModel).Reservations.FirstOrDefault(v => v.IDБилета == l.ID));
                                (DataContext as BuyingTicketViewModel).SaveChanges();
                                SelectedPlaceCommand.Execute(this);


                            }

                        }
                    };
                    foreach (Билеты t in Tickets)
                    {
                        if (t.IDСеанса == IDSeansa && t.Ряд.ToString() + ":" == s.Children.OfType<Label>().First().Content.ToString()
                            && t.Место.ToString() == b.Content.ToString())
                        {
                            if (t.Бронь == true)
                                b.Background = Brushes.LightYellow;
                            else b.Background = Brushes.IndianRed;

                            

                        }
                    }
                }
            }
        }
    }
}
