using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Westrock_Plate_Job_Manager.Models;

namespace Westrock_Plate_Job_Manager.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ScheduleManager.xaml
    /// </summary>
    public partial class ScheduleManager : UserControl
    {
        private Presenter _presenter = new Presenter();

        public ScheduleManager()
        {
            InitializeComponent();
            JobGrid.ItemsSource = _presenter.LoadJobSchedule();
            jobNumberTxt.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, StopPaste));
            distNumberTxt.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, StopPaste));
            JobGrid.SelectedIndex = 0;
            
        }
        
        private void StopPaste(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void AddJobBtn_Click(object sender, RoutedEventArgs e)
        {
            var addJob = new AddJob();
            addJob.ShowDialog();

            var jobNumber = jobNumberTxt.Text;
            var distNumber = distNumberTxt.Text;

            string plateType = "";
            if (plateTypeCbo.SelectedIndex != -1)
                plateType = plateTypeCbo.SelectedValue.ToString();

            string caliper = "";
            if (caliperCbo.SelectedIndex != -1)
                caliper = caliperCbo.SelectedItem.ToString();

            var plateTypeCaliper = plateType + " " + caliper;

            var shipTo = shipToTxt.Text;

            string status = "";
            if (jobStatusCbo.SelectedIndex != -1)
                status = jobStatusCbo.SelectedValue.ToString();

            JobGrid.ItemsSource = _presenter.FilterJobGrid(jobNumber, distNumber, plateTypeCaliper, shipTo, status);
        }

        private void filterBtn_Click(object sender, RoutedEventArgs e)
        {
            string jobNumber = "";
            string distNumber = "";
            string plateType = "";
            string caliper = "";
            string plateTypeCaliper = "";
            string shipTo = "";
            string status = "";

            jobNumber = jobNumberTxt.Text;
            distNumber = distNumberTxt.Text;

            if (plateTypeCbo.SelectedIndex != -1) plateType = plateTypeCbo.SelectedValue.ToString();
            if (caliperCbo.SelectedIndex != -1) caliper = "0" + caliperCbo.SelectedValue.ToString();

            plateTypeCaliper = plateType + " " + caliper;

            shipTo = shipToTxt.Text;

            if (jobStatusCbo.SelectedIndex != -1) status = jobStatusCbo.SelectedValue.ToString();
            
            JobGrid.ItemsSource = _presenter.FilterJobGrid(jobNumber, distNumber, plateTypeCaliper, shipTo, status);
            JobGrid.SelectedIndex = 0;
        }

        private void ClearFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            jobNumberTxt.Clear();
            distNumberTxt.Clear();
            plateTypeCbo.SelectedIndex = -1;
            caliperCbo.SelectedIndex = -1;
            shipToTxt.Clear();
            jobStatusCbo.SelectedIndex = -1;

            JobGrid.ItemsSource = _presenter.LoadJobSchedule();
            JobPlatesGrid.ItemsSource = "";
        }

        private void changeStatusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (JobGrid.SelectedItem == null) return;

            ChangeJobStatus changeStatus = new ChangeJobStatus((JobObject)JobGrid.SelectedItem);
            changeStatus.ShowDialog();

            var jobNumber = jobNumberTxt.Text;
            var distNumber = distNumberTxt.Text;

            string plateType = "";
            if (plateTypeCbo.SelectedIndex != -1)
                plateType = plateTypeCbo.SelectedValue.ToString();

            string caliper = "";
            if (caliperCbo.SelectedIndex != -1)
                caliper = caliperCbo.SelectedItem.ToString();

            var plateTypeCaliper = plateType + " " + caliper;

            var shipTo = shipToTxt.Text;

            string status = "";
            if (jobStatusCbo.SelectedIndex != -1)
                status = jobStatusCbo.SelectedValue.ToString();

            JobGrid.ItemsSource = _presenter.FilterJobGrid(jobNumber, distNumber, plateTypeCaliper, shipTo, status);
        }

        private void ValidateNoLetters(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void JobGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "JobPlates")
            {
                e.Cancel = true;
            }
        }

        private void JobPlatesGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if(e.Column.Header.ToString() == "Color" || e.Column.Header.ToString() == "Quantity")
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void JobGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedJob = (JobObject)JobGrid.SelectedItem;
            if(selectedJob != null)
            {
                JobPlatesGrid.ItemsSource = _presenter.LoadPlatesForSelectedJob(Convert.ToInt32(selectedJob.JobNumber));
                JobPlatesGrid.Items.Refresh();
            }
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            var jobNumber = jobNumberTxt.Text;
            var distNumber = distNumberTxt.Text;

            string plateType = "";
            if (plateTypeCbo.SelectedIndex != -1)
                plateType = plateTypeCbo.SelectedValue.ToString();

            string caliper = "";
            if (caliperCbo.SelectedIndex != -1)
                caliper = caliperCbo.SelectedItem.ToString();

            var plateTypeCaliper = plateType + " " + caliper;

            var shipTo = shipToTxt.Text;

            string status = "";
            if (jobStatusCbo.SelectedIndex != -1)
                status = jobStatusCbo.SelectedValue.ToString();

            JobGrid.ItemsSource = _presenter.FilterJobGrid(jobNumber, distNumber, plateTypeCaliper, shipTo, status);
            JobGrid.SelectedIndex = 0;
            var selectedJob = (JobObject)JobGrid.SelectedItem;
            if (selectedJob != null)
            {
                JobPlatesGrid.ItemsSource = _presenter.LoadPlatesForSelectedJob(Convert.ToInt32(selectedJob.JobNumber));
                JobPlatesGrid.Items.Refresh();
            }
        }

    }
}
