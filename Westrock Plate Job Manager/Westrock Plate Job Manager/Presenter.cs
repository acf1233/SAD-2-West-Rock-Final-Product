/*
    This class is the presenter for all views in the application.
    All backend logic will be performed by this class and will act as an inbetween for SQL calls and the views.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Westrock_Plate_Job_Manager.Models;
using Westrock_Plate_Job_Manager.Views.UserControls;

namespace Westrock_Plate_Job_Manager
{
    public class Presenter
    {
        //private ScheduleManager _scheduleManager = new ScheduleManager();

        SQL SQLCaller = new SQL();

        public ObservableCollection<PlateObject> LoadInitialPlateInventory()
        {
            return SQLCaller.LoadInitialPlateInventory();
        }

        public List<string> GetListOfPlateSizesFromType(string selectedPlateType)
        {
            return SQLCaller.GetListOfPlateSizesFromType(selectedPlateType);
        }

        public List<string> GetPlateTypes()
        {
            return SQLCaller.GetPlateTypes();
        }

        public string AddOrder(PlateOrder order)
        {
            return SQLCaller.AddOrder(order);
        }

        public void UpdatePlateInventoryOnOrder(PlateOrder order)
        {
            SQLCaller.UpdatePlateInventoryOnOrder(order);
        }

        internal void UpdateJobStatus(string jobNum, string distNum, string status)
        {
            SQLCaller.UpdateJobStatus(jobNum, distNum, status);
        }

        public ObservableCollection<JobObject> LoadJobSchedule()
        {
            return SQLCaller.GetJobs();
        }

        public ObservableCollection<PlateOrder> GetPlateOrders()
        {
            return SQLCaller.GetPlateOrders();
        }

        public void UpdatePlateOrders(string orderID)
        {
            SQLCaller.UpdatePlateOrders(orderID);
        }

        public void UpdatePlateInventoryOnConfirmOrder(PlateOrder order)
        {
            SQLCaller.UpdatePlateInventoryOnConfirmOrder(order);
        }

        public ObservableCollection<JobObject> FilterJobGrid(string jobNumber, string distNumber, string plateTypeCaliper, string shipTo, string status)
        {
            return SQLCaller.FilterJobs(jobNumber, distNumber, plateTypeCaliper, shipTo, status);
        }

        public bool InsertJobIntoSchedule(JobObject job)
        {
            return SQLCaller.InsertJobIntoSchedule(job);
        }

        public bool InsertAssociatedJobPlates(JobPlateObject jobPlate, int jobNumber)
        {
            return SQLCaller.InsertAssociatedJobPlates(jobPlate, jobNumber);
        }

        public int GetLastEnteredJobNumber()
        {
            return SQLCaller.GetLastEnteredJobNumber();
        }

        public ObservableCollection<JobPlateObject> LoadPlatesForSelectedJob(int jobNumber)
        {
            return SQLCaller.LoadPlatesForSelectedJob(jobNumber);
        }

        //Logic for getting optimal plates begins here, it's the only set of functions in this entire presenter that has does something other than perform a Sql call

        
        public List<PlateObject> RotatePlates(List<PlateObject> filteredPlates)
        {
            List<PlateObject> rotatedPlates = new List<PlateObject>();

            foreach (var plate in filteredPlates)
            {
                var newPlate = new PlateObject();
                newPlate.Type = plate.Type;
                newPlate.Caliper = plate.Caliper;
                newPlate.OnHandQuantity = plate.OnHandQuantity;

                double temp;
                var tempWidth = plate.Width;
                var tempHeight = plate.Height;

                temp = tempWidth;
                tempWidth = tempHeight;
                tempHeight = temp;

                newPlate.Width = tempWidth;
                newPlate.Height = tempHeight;

                rotatedPlates.Add(newPlate);
            }

            return rotatedPlates;
        }

        public PlateObject FindOptimalPlate(List<PlateObject> filteredPlates, List<PlateObject> rotatedPlates, double imageHeight, double imageWidth)
        {
            //Get the area of the image
            var imageArea = imageWidth * imageHeight;

            //Orders the plates in ascending order
            var combinedList = filteredPlates.Concat(rotatedPlates).OrderBy(p => (p.Height * p.Width)).ToList();
            var combinedFiltered = combinedList.Where(p => (p.Width * p.Height) >= imageArea).ToList();

            PlateObject optimalPlate = new PlateObject();

            foreach (var plate in combinedFiltered)
            {              
                if (plate.Height >= imageHeight && plate.Width >= imageWidth)
                {
                    optimalPlate = plate;
                    break;
                }
            }

            return optimalPlate;
        }

        public double CalculateOptimalWaste(double imageHeight, double imageWidth, PlateObject optimalPlate)
        {
            var totalArea = optimalPlate.Height * optimalPlate.Width;
            var imageArea = imageHeight * imageWidth;
            var optimalWaste = 1 - (imageArea / totalArea);
            return optimalWaste;
        }
    }
}
