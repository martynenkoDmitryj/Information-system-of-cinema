using Cinema.Models;
using Cinema.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Cinema
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string AccessLevel { get; set; }
        public static int A { get; set; }
        public static int B { get; set; }
        public static bool isReservation { get; set; }

        public static ObservableCollection<СтоимостьБилетов> Prices { get; set; }
    }
    public interface IWindowFactory
    {
        void CreateNewWindow();
    }

    public class ProductionWindowFactory : IWindowFactory
    {
        public ProductionWindowFactory()
        {

        }

        public void CreateNewWindow()
        {
            
                BuyingTicketView v = new BuyingTicketView(App.A, App.B);
                v.Show();
           
        }
    }
}

