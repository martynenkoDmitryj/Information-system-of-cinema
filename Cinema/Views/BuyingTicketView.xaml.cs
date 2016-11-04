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
    /// Interaction logic for BuyingTicketView.xaml
    /// </summary>
    public partial class BuyingTicketView : Window
    {
        public BuyingTicketView(int seansID,int rowsCount)
        {

            InitializeComponent();
            ccc.RowsCount = rowsCount;
            ccc.IDSeansa = seansID;
            BuyingTicketViewModel vm = new BuyingTicketViewModel();
            ccc.DataContext = vm;
            //DataContext = vm;
            vm.PropertyChanged += Vm_PropertyChanged;
            Height = ccc.Height;

        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Ticket")
            {
                if (App.isReservation)
                {
                    NewReservation v = new NewReservation();
                    v.Show();
                }
                else {
                    PrintTicketView v = new PrintTicketView();
                    v.Show();
                }
                this.Close();

            }
        }
    }
}
