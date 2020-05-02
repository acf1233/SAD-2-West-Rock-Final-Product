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
using Westrock_Plate_Job_Manager.Models;


namespace Westrock_Plate_Job_Manager.Views
{
    /// <summary>
    /// Interaction logic for ChangeJobStatus.xaml
    /// </summary>
    public partial class ChangeJobStatus : Window
    {
        Presenter presenter = new Presenter();
        private JobObject _selectedJob = new JobObject();


        public ChangeJobStatus()
        {
            InitializeComponent();
        }

        public ChangeJobStatus(JobObject selectedJob)
        {
            InitializeComponent();
            _selectedJob = selectedJob;
            jobNumTxt.Text = selectedJob.JobNumber;
            distNumTxt.Text = selectedJob.DistNumber;
            jobStatusCbo.SelectedValue = selectedJob.JobStatus;
        }

        private void submitStatusChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedJob.JobStatus == (string)jobStatusCbo.SelectedValue) return;

            if(jobStatusCbo.SelectedValue.ToString() == "Complete")
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to complete Job Number: {_selectedJob.JobNumber}?  It will be removed from the active schedule.", "Complete this job?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    presenter.UpdateJobStatus(_selectedJob.JobNumber, _selectedJob.DistNumber, jobStatusCbo.SelectedValue.ToString());
                    this.Close();
                }
                else
                {
                    this.Close();
                    return;
                }
            }
            presenter.UpdateJobStatus(_selectedJob.JobNumber, _selectedJob.DistNumber, jobStatusCbo.SelectedValue.ToString());
            this.Close();
        }
    }
}
