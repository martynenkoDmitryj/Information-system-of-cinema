using Cinema.ViewModels;
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

namespace Cinema.Views
{
    /// <summary>
    /// Interaction logic for HallsView.xaml
    /// </summary>
    public partial class HallsView : Window
    {
        public HallsView()
        {
            InitializeComponent();
            HallsViewModel vm = new HallsViewModel();
            DataContext = vm;
            Grid2.DataContext = new HallSizesViewModel();
            cbc1.ItemsSource = vm.Размеры; 
        }
    }
}
