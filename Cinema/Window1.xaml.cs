using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Cinema.Views;

namespace Cinema
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            UsersView v = new UsersView();
            v.Show();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            FilmsView v = new FilmsView();
            v.Show();
        }

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            HallsView v = new HallsView();
            v.Show();
        }

        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            SeansiView v = new SeansiView(false);
            v.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void button_Copy3_Click(object sender, RoutedEventArgs e)
        {
            App.isReservation = false;
            SeansiView v = new SeansiView(true);
            v.Show();
        }

        private void button_Copy4_Click(object sender, RoutedEventArgs e)
        {
            App.isReservation = true;
            SeansiView v = new SeansiView(true);
            v.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(App.AccessLevel == "Администратор")
            {
                button.Visibility = Visibility.Visible;
                Height = 206;
            }
        }
    }
}
