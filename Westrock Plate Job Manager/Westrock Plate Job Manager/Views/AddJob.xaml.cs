using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    /// Interaction logic for AddJob.xaml
    /// </summary>
    public partial class AddJob : Window
    {
        ObservableCollection<JobPlateObject> JobPlates = new ObservableCollection<JobPlateObject>();
        Presenter presenter = new Presenter();

        bool plateTypeFilled = false;
        bool plateCaliperFilled = false;
        bool imageWidthFilled = false;
        bool imageHeightFilled = false;

        public AddJob()
        {
            InitializeComponent();
            plateTypeCbo.ItemsSource = presenter.GetPlateTypes();
            colorComboBox.Items.Add("Blue");
            colorComboBox.Items.Add("Red");
            colorComboBox.Items.Add("Yellow");
            colorComboBox.Items.Add("Green");
            colorComboBox.Items.Add("Orange");
            colorComboBox.Items.Add("Pink");
            colorComboBox.Items.Add("Purple");
            colorComboBox.Items.Add("Brown");
            colorComboBox.Items.Add("Black");
            colorComboBox.Items.Add("White");
            colorQuantityUpDown.Value = 0;
            dateDueDtp.Minimum = DateTime.Now;
            dateDueDtp.Value = DateTime.Now;
            plateCaliperCbo.IsEnabled = false;
            jobStatusCbo.Items.Add("Added");
            jobStatusCbo.Items.Add("Halted");
            jobStatusCbo.Items.Add("InProgress");
            jobStatusCbo.Items.Add("Complete");
            jobStatusCbo.SelectedIndex = 0;
        }

        private void AddPlatesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (colorComboBox.SelectedItem != null && colorQuantityUpDown.Value != 0)
            {
                var plateToAdd = new JobPlateObject()
                {
                    Color = colorComboBox.SelectedItem.ToString(),
                    Quantity = (int)colorQuantityUpDown.Value
                };

                JobPlates.Add(plateToAdd);
                colorComboBox.Items.Remove(plateToAdd.Color.ToString());
                colorQuantityUpDown.Value = 0;
                plateColorGrid.ItemsSource = JobPlates;
                plateColorGrid.Items.Refresh();
                plateTotalTxt.Text = UpdatePlateTotal().ToString();
            }
        }

        private int UpdatePlateTotal()
        {
            int sum = 0;
            for(int i = 0;i < plateColorGrid.Items.Count; i++)
            {
                var plate = (JobPlateObject)plateColorGrid.Items[i];
                sum += plate.Quantity;
            }
            return sum;
        }

        private void PlateColorGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Color" || e.Column.Header.ToString() == "Quantity")
            {
                e.Cancel = false;
            }

            else
            {
                e.Cancel = true;
            }
        }

        private void SubmitJobBtn_Click(object sender, RoutedEventArgs e)
        {
            var jobObject = new JobObject();
            int summedPlates = 0;

            foreach(var plate in JobPlates)
            {
                summedPlates += plate.Quantity;
            }

            if (string.IsNullOrEmpty(distTxt.Text) || dateDueDtp.Value == null || string.IsNullOrEmpty(shipToTxt.Text)
                || string.IsNullOrEmpty(imageHeightTxt.Text) || string.IsNullOrEmpty(plateTotalTxt.Text) || plateTypeCbo.SelectedIndex == -1
                || string.IsNullOrEmpty(optimalWasteTxt.Text) || string.IsNullOrEmpty(imageWidthTxt.Text) || string.IsNullOrEmpty(plateSizeTxt.Text)
                || plateCaliperCbo.SelectedIndex == -1 || string.IsNullOrEmpty(chargeWidthTxt.Text) || string.IsNullOrEmpty(chargeHeightTxt.Text)
                || jobStatusCbo.SelectedIndex == -1 || JobPlates.Count == 0)
            {
                MessageBox.Show("All fields must contain a value.");
            }
            else if (Convert.ToInt32(plateTotalTxt.Text) != summedPlates)
            {
                MessageBox.Show("Inputted plate total does not match the grid's summed plate total.");
            }
            else
            {
                jobObject.DistNumber = distTxt.Text;
                jobObject.DateDue = (DateTime)dateDueDtp.Value;
                jobObject.ShipTo = shipToTxt.Text;
                jobObject.PlateTotal = Convert.ToInt32(plateTotalTxt.Text);
                jobObject.PlateType = plateTypeCbo.SelectedItem.ToString() + " " + plateCaliperCbo.SelectedItem.ToString();
                var optimalWaste = optimalWasteTxt.Text.Replace("%","");
                jobObject.OptimalWaste = Convert.ToDouble(optimalWaste);
                jobObject.ImageWidth = Convert.ToDouble(imageWidthTxt.Text);
                jobObject.ImageHeight = Convert.ToDouble(imageHeightTxt.Text);
                var sizeSplitted = plateSizeTxt.Text.Split(' ');
                jobObject.PlateHeight = Convert.ToDouble(sizeSplitted[0]);
                jobObject.PlateWidth = Convert.ToDouble(sizeSplitted[2]);
                jobObject.ChargeWidth = Convert.ToDouble(chargeWidthTxt.Text);
                jobObject.ChargeHeight = Convert.ToDouble(chargeHeightTxt.Text);
                jobObject.JobStatus = jobStatusCbo.SelectedItem.ToString();
                jobObject.JobPlates = JobPlates.ToList<JobPlateObject>();
                jobObject.JobNumber = jobNumberTxt.Text;

                if (presenter.InsertJobIntoSchedule(jobObject))
                {
                    int jobNumber = presenter.GetLastEnteredJobNumber();
                    foreach (var plate in jobObject.JobPlates)
                    {
                        if(!presenter.InsertAssociatedJobPlates(plate, jobNumber))
                        {
                            //Remove from job_color where job_number = job just entered
                            // also remove from schedule where job number = job just entered
                            MessageBox.Show("Something went wrong when adding your job to the schedule");
                        }

                        else
                        {
                            MessageBox.Show("Your job was added to the schedule successfully");
                        }
                        
                    }
                }

                else
                {
                    MessageBox.Show("Something went wrong when adding your job to the schedule.");
                }
                this.Close();
            }
        }

        private void CalculateOptimalPlate()
        {
            var plateType = plateTypeCbo.SelectedItem.ToString();
            var caliper = Convert.ToDouble(plateCaliperCbo.SelectedItem.ToString());
            var imageHeight = Convert.ToDouble(imageHeightTxt.Text);
            var imageWidth = Convert.ToDouble(imageWidthTxt.Text);
            int numPlates;

            if (plateTotalTxt.Text != "")
            {
                numPlates = Convert.ToInt32(plateTotalTxt.Text);
            }
            else
                numPlates = 1;

            List<PlateObject> plateList = presenter.LoadInitialPlateInventory().ToList();
            var filteredPlates = plateList.Where(p => (p.Type == plateType && p.Caliper == caliper && p.OnHandQuantity >= numPlates)).ToList();
            var rotatedList = presenter.RotatePlates(filteredPlates);

            var optimalPlate = presenter.FindOptimalPlate(filteredPlates, rotatedList, imageHeight, imageWidth);

            plateSizeTxt.Text = optimalPlate.Height.ToString() + " x " + optimalPlate.Width.ToString();
            optimalWasteTxt.Text = presenter.CalculateOptimalWaste(imageHeight, imageWidth, optimalPlate).ToString("P", CultureInfo.InvariantCulture);
        }

        private void CheckOptimalPlate_Click(object sender, RoutedEventArgs e)
        {
            CalculateOptimalPlate();
        }

        private void plateTypeCbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterCaliperComboBox();

            plateTypeFilled = true;
            if (plateCaliperFilled)
                plateCaliperFilled = false;

            if (plateTypeFilled && plateCaliperFilled && imageWidthFilled && imageHeightFilled)
                CalculateOptimalPlate();
        }

        private void FilterCaliperComboBox()
        {
            plateCaliperCbo.Items.Clear();
            if ((string)plateTypeCbo.SelectedItem == "EFX")
            {
                plateCaliperCbo.Items.Add(.067);
                plateCaliperCbo.IsEnabled = true;
            }

            else if((string)plateTypeCbo.SelectedItem == "DFM")
            {
                plateCaliperCbo.Items.Add(.067);
                plateCaliperCbo.Items.Add(.107);
                plateCaliperCbo.IsEnabled = true;
            }
        }

        private void PlateCaliperCbo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(plateCaliperCbo.SelectedItem != null)
                plateCaliperFilled = true;
            else
                plateCaliperFilled = false;

            if (plateTypeFilled && plateCaliperFilled && imageWidthFilled && imageHeightFilled)
                CalculateOptimalPlate();
        }

        private void ImageHeightTxt_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (imageHeightTxt.Text != "")
                imageHeightFilled = true;
            else
                imageHeightFilled = false;

            if (plateTypeFilled && plateCaliperFilled && imageWidthFilled && imageHeightFilled)
                CalculateOptimalPlate();
        }

        private void ImageWidthTxt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (imageWidthTxt.Text != "")
                imageWidthFilled = true;
            else
                imageWidthFilled = false;

            if (plateTypeFilled && plateCaliperFilled && imageWidthFilled && imageHeightFilled)
                CalculateOptimalPlate();
        }

        private void PlateTotalTxt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (plateTypeFilled && plateCaliperFilled && imageWidthFilled && imageHeightFilled)
                CalculateOptimalPlate();
        }

        private void ValidateNoLetters(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
