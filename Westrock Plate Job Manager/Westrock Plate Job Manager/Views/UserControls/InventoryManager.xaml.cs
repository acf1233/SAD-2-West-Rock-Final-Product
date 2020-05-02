using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Westrock_Plate_Job_Manager.Models;

namespace Westrock_Plate_Job_Manager.Views.UserControls
{
    /// <summary>
    /// Interaction logic for InventoryManager.xaml
    /// </summary>
    public partial class InventoryManager : UserControl
    {
        ObservableCollection<PlateObject> platesList = new ObservableCollection<PlateObject>();
        Presenter presenter = new Presenter();

        public InventoryManager()
        {
            InitializeComponent();

            platesList = presenter.LoadInitialPlateInventory();
            PlateGrid.ItemsSource = platesList;
        }

        private void AddOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            //Open up new bullshit here
            OrderPlates orderPlatesPopUp = new OrderPlates();
            orderPlatesPopUp.ShowDialog();
            RefreshInventoryGrid();
        } //an error is thrown every time you exit from the add order pop-up.

        private void markAsReceived_Click(object sender, RoutedEventArgs e)
        {
            //Open other bullshit here
            RecieveOrder markOrderAsReceivedPopUp = new RecieveOrder(this);
            markOrderAsReceivedPopUp.ShowDialog();
            RefreshInventoryGrid();
        }

        private void clearFilter_Click(object sender, RoutedEventArgs e)
        {
            //Clear that mofuckin filter boi
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshInventoryGrid();
        }

        private void PlateGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if(e.Column.Header.ToString() == "ColorSet")
            {
                e.Cancel = true;
            }
        }

        public void RefreshInventoryGrid()
        {
            platesList = presenter.LoadInitialPlateInventory();
            PlateGrid.ItemsSource = platesList;
        }
    }
}
