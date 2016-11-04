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
    /// Interaction logic for SeansiView.xaml
    /// </summary>
    public partial class SeansiView : Window
    {
        SeansiViewModel viewModel = new SeansiViewModel();
        PricesViewModel viewModel2 = new PricesViewModel();
        public SeansiView(bool isSelecting)
        {
            InitializeComponent();
            DataContext = viewModel;
            cbc1.ItemsSource = viewModel.Films;
            cbc2.ItemsSource = viewModel.Halls;
            PricesDataGrid.DataContext = viewModel2;
            cbc3.ItemsSource = viewModel2.Seansi;
            if (isSelecting)
                SelectButton.Visibility = Visibility.Visible;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void SelectButton_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
