using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Westrock_Plate_Job_Manager.Views
{
    /// <summary>
    /// Interaction logic for OrderPlates.xaml
    /// </summary>
    public partial class OrderPlates : Window
    {
        Presenter presenter = new Presenter();

        public OrderPlates()
        {
            InitializeComponent();
            plateTypeCbo.ItemsSource = presenter.GetPlateTypes();
            dateOrderDtp.SelectedDate = DateTime.Now.Date;
            plateSizeCbo.IsEnabled = false;
            plateCaliperCbo.IsEnabled = false;
            plateQuantityUpDown.Value = 0;
        }

        private void plateTypeCbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterPlateSizeComboBox();
        }

        private void FilterPlateSizeComboBox()
        {
            plateSizeCbo.ItemsSource = null;
            plateCaliperCbo.Items.Clear();
            plateCaliperCbo.IsEnabled = false;
            if ((string)plateTypeCbo.SelectedItem == "EFX")
            {
                plateSizeCbo.ItemsSource = presenter.GetListOfPlateSizesFromType("EFX");
                plateSizeCbo.IsEnabled = true;
            }

            else if((string)plateTypeCbo.SelectedItem == "DFM")
            {
                plateSizeCbo.ItemsSource = presenter.GetListOfPlateSizesFromType("DFM");
                plateSizeCbo.IsEnabled = true;
            }
        }

        private void plateSizeCbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterPlateCaliperComboBox();
        }

        private void FilterPlateCaliperComboBox()
        {
            // for now the possible combinations of plate type and caliper are easily calulable, so we can hardcode the caliper values into the combo box dynamically based upon what is in the plate type 
            plateCaliperCbo.Items.Clear();
           if((string)plateTypeCbo.SelectedItem == "EFX")
           {
               plateCaliperCbo.Items.Add(.067);
           }

           else if((string)plateTypeCbo.SelectedItem == "DFM" )
           {
                if ((string)plateSizeCbo.SelectedItem == "30 x 25" || (string)plateSizeCbo.SelectedItem == "42 x 30" )
                {
                    plateCaliperCbo.Items.Add(.067);
                }

                else if ((string)plateSizeCbo.SelectedItem == "42 x 50")
                {
                    plateCaliperCbo.Items.Add(.107);
                }

                else
                {
                    plateCaliperCbo.Items.Add(.067);
                    plateCaliperCbo.Items.Add(.107);
                }
           }
                      
           plateCaliperCbo.IsEnabled = true;
        }

        private void submitOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            if (plateSizeCbo.SelectedItem == null || plateTypeCbo.SelectedItem == null || plateCaliperCbo.SelectedItem == null || dateOrderDtp.Text == "")
            {
                MessageBox.Show("You must fill out all of the fields before you can submit an order.");
            }
            
            else
            {
                PlateOrder order = new PlateOrder();
                order.Type = plateTypeCbo.Text;
                var sizeSplitted = plateSizeCbo.Text.Split(' ');
                order.Height = Convert.ToDouble(sizeSplitted[0]);
                order.Width = Convert.ToDouble(sizeSplitted[2]);
                order.Caliper = Convert.ToDouble(plateCaliperCbo.Text);
                order.Quantity = (int)plateQuantityUpDown.Value;
                order.OrderDate = (DateTime)dateOrderDtp.SelectedDate;
                var addOrderResult = presenter.AddOrder(order);
                presenter.UpdatePlateInventoryOnOrder(order);
                MessageBox.Show(addOrderResult);
                this.Close();
            }
        }
    }
}
