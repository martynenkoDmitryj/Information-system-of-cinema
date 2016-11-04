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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cinema.Models;
using Cinema.ViewModels;

namespace Cinema
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoginViewModel vm = new LoginViewModel();
            DataContext = vm;
            vm.PropertyChanged += Vm_PropertyChanged;
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "Success":
                    this.Hide();
                    Window1 w = new Window1();
                    w.Show();
                    break;
                case "InvalidPassword":
                    MessageBox.Show("Неверный пароль!");
                    break;
                case "NotExistUser":
                    MessageBox.Show("Пользователь не найден!");
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
     
        }
    }
}
