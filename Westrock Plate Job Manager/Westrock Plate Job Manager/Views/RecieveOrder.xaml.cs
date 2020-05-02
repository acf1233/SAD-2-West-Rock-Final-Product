using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
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
using Westrock_Plate_Job_Manager.Models;
using Westrock_Plate_Job_Manager.Views.UserControls;

namespace Westrock_Plate_Job_Manager.Views
{
    /// <summary>
    /// Interaction logic for RecieveOrder.xaml
    /// </summary>
    public partial class RecieveOrder : Window
    {
        ObservableCollection<PlateOrder> orderList = new ObservableCollection<PlateOrder>();
        Presenter presenter = new Presenter();
        InventoryManager inventoryManager;

        public RecieveOrder(InventoryManager inventory)
        {
            InitializeComponent();

            orderList = presenter.GetPlateOrders();
            OrderGrid.ItemsSource = orderList;
            inventoryManager = inventory;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmOrderBtn_Click(object sender, RoutedEventArgs e)
        { 
            var order = (PlateOrder)OrderGrid.SelectedItem;            

           

            if (order != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show($"Mark order {order.OrderId} as complete?", "Receive Order", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    presenter.UpdatePlateOrders(order.OrderId);
                    orderList = presenter.GetPlateOrders();
                    OrderGrid.ItemsSource = orderList;
                    presenter.UpdatePlateInventoryOnConfirmOrder(order);
                }
            }
            else
            {
                MessageBox.Show("Please select an order.");
            }
        }

        //private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    inventoryManager.RefreshInventoryGrid();
        //}
    }
}
