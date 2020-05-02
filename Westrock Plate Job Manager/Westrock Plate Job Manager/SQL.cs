/*
    This class will be responsible for handling all SQL queries needed for the application.
    Data will be fed through the Presenter class so that the views will be updated with the correct information.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Text;
using Westrock_Plate_Job_Manager.Models;

namespace Westrock_Plate_Job_Manager
{
    public class SQL
    {
        //string connectionString = "Data Source=ZACHSPC\\SQLEXPRESS01;Initial Catalog=WestRockProject;Integrated Security=True";
        string connectionString = "Data Source=DESKTOP-QRLLFQK\\SQLEXPRESS;Initial Catalog=WestRockProject;Integrated Security=True";
        //string connectionString = "Data Source = ALIASRAIN\\SQLEXPRESS;Initial Catalog = WestRockDatabase; Integrated Security = True";
        //string connectionString = "Data Source=TYLERS-PC\\SQLEXPRESS;Initial Catalog=NewWestRockDatabase;Integrated Security=True";

        public ObservableCollection<PlateObject> LoadInitialPlateInventory()
        {
            ObservableCollection<PlateObject> plateList = new ObservableCollection<PlateObject>();
            string query = "SELECT Height, Width, Caliper, Type, On_hand, On_order, Unit_cost, Plates_per_box FROM Plate_Inventory;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //using
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                //using
                SqlDataReader reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        PlateObject newPlate = new PlateObject();
                        newPlate.Height = Convert.ToDouble(reader["Height"]);
                        newPlate.Width = Convert.ToDouble(reader["Width"]);
                        newPlate.Caliper = Convert.ToDouble(reader["Caliper"]);
                        newPlate.Type = reader["Type"].ToString();
                        newPlate.OnHandQuantity = Convert.ToInt32(reader["On_hand"].ToString());
                        newPlate.OnOrderQuantity = Convert.ToInt32(reader["On_order"].ToString());
                        newPlate.UnitCost = Convert.ToDouble(reader["Unit_cost"].ToString());
                        newPlate.PlatesPerBox = Convert.ToInt32(reader["Plates_per_box"].ToString());
                        plateList.Add(newPlate);
                    }
                    return plateList;
                }

                catch(Exception ex)
                {
                    return new ObservableCollection<PlateObject>();
                }

                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
        }

        public void UpdateJobStatus(string jobNum, string distNum, string status)
        {
            string query = $"UPDATE Schedule SET Job_status = '{status}' WHERE Job_number = {jobNum} AND Dist_number = {distNum}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //using
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                //using

                try
                {
                    command.ExecuteNonQuery();
                }

                catch (Exception ex)
                {

                }

                finally
                {
                    connection.Close();
                }
            }
        }

        public List<string> GetListOfPlateSizesFromType(string plateType)
        {
            List<string> sizeList = new List<string>();
            string query = $"SELECT Height, Width FROM Plate_Inventory WHERE Type = '{plateType}';";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //using
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                //using
                SqlDataReader reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        string size = reader["Height"].ToString();
                        size += " x ";
                        size += reader["Width"].ToString();
                        sizeList.Add(size);
                    }
                    return sizeList;
                }

                catch (Exception ex)
                {
                    return new List<string>();
                }

                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
        }

        public List<string> GetPlateTypes()
        {
            List<string> plateTypes = new List<string>();
            string query = "SELECT Distinct Type FROM Plate_Inventory;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //using
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                //using
                SqlDataReader reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        string plateType = reader["Type"].ToString();
                        plateTypes.Add(plateType);
                    }
                    return plateTypes;
                }

                catch (Exception ex)
                {
                    return new List<string>();
                }

                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
        }

        public string AddOrder(PlateOrder order)
        {
            string query = $"INSERT INTO Plate_Orders(Type, Height, Width, Caliper, Quantity, OrderDate) VALUES('{order.Type}', {order.Height}, {order.Width}, {order.Caliper}, {order.Quantity}, '{order.OrderDate}');";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //using
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                //using
                

                try
                {
                    var success = command.ExecuteNonQuery();
                    if(success == 1)
                    {
                        return "Your order was added successfully.";
                    }
                    else
                    {
                        return "Something went wrong with your order submission.";
                    }
                }

                catch (Exception ex)
                {
                    return ex.Message;
                }

                finally
                {
                    connection.Close();
                }
            }
        }

        public void UpdatePlateInventoryOnOrder(PlateOrder order)
        {
            string query = $"UPDATE Plate_Inventory SET On_Order = On_Order + {order.Quantity} WHERE Type = '{order.Type}' AND Height = {order.Height} AND Width = {order.Width} AND Caliper = {order.Caliper} ;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //using
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                //using
                
                try
                {
                   command.ExecuteNonQuery();
                }

                catch (Exception ex)
                {
                    
                }

                finally
                {
                    connection.Close();
                }
            }
        }

        public ObservableCollection<JobObject> GetJobs()
        {
            ObservableCollection<JobObject> jobsList = new ObservableCollection<JobObject>();
            string query = "SELECT * FROM Schedule WHERE Job_status != 'Complete'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //using
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                //using
                SqlDataReader reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        var job = new JobObject();

                        job.JobNumber = Convert.ToString(reader["Job_number"]);
                        job.DateDue = Convert.ToDateTime(reader["Date_due"]);
                        job.DistNumber = Convert.ToString(reader["Dist_number"]);
                        job.PlateTotal = Convert.ToInt32(reader["Plate_total"]);
                        job.PlateType = Convert.ToString(reader["Plate_type"]);
                        job.OptimalWaste = Convert.ToDouble(reader["Optimal_waste"]);
                        job.ImageWidth = Convert.ToDouble(reader["Image_width"]);
                        job.ImageHeight = Convert.ToDouble(reader["Image_height"]);
                        job.ChargeWidth = Convert.ToDouble(reader["Charge_width"]);
                        job.ChargeHeight = Convert.ToDouble(reader["Charge_height"]);
                        job.PlateWidth = Convert.ToDouble(reader["Plate_width"]);
                        job.PlateHeight = Convert.ToDouble(reader["Plate_height"]);
                        job.ShipTo = Convert.ToString(reader["Ship_to"]);
                        job.JobStatus = Convert.ToString(reader["Job_status"]);

                        jobsList.Add(job);
                    }
                    return jobsList;
                }

                catch (Exception ex)
                {
                    return new ObservableCollection<JobObject>();
                }

                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
        }

        public ObservableCollection<PlateOrder> GetPlateOrders()
        {
            ObservableCollection<PlateOrder> orderList = new ObservableCollection<PlateOrder>();
            string query = "SELECT * FROM Plate_Orders WHERE Completed = 0;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //using
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                //using
                SqlDataReader reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        var order = new PlateOrder();

                        order.OrderId = Convert.ToString(reader["OrderID"]);
                        order.Type = Convert.ToString(reader["Type"]);
                        order.Height = Convert.ToDouble(reader["Height"]);
                        order.Width = Convert.ToDouble(reader["Width"]);
                        order.Caliper = Convert.ToDouble(reader["Caliper"]);
                        order.Quantity = Convert.ToInt32(reader["Quantity"]);
                        order.OrderDate = Convert.ToDateTime(reader["OrderDate"]);                        

                        orderList.Add(order);
                    }
                    return orderList;
                }

                catch (Exception ex)
                {
                    return new ObservableCollection<PlateOrder>();
                }

                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }            
        }
        public void UpdatePlateOrders(string orderID)
        {
            string query = $"UPDATE Plate_Orders SET Completed = 1 WHERE OrderID = {orderID}; ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //using
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void UpdatePlateInventoryOnConfirmOrder(PlateOrder order)
        {
            string query = $@"UPDATE Plate_Inventory 
                              SET On_Order = On_Order - {order.Quantity} , On_hand = On_hand + {order.Quantity}
                              WHERE Type = '{order.Type}' AND Height = {order.Height} AND Width = {order.Width} AND Caliper = {order.Caliper}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //using
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                //using

                try
                {
                    command.ExecuteNonQuery();
                }

                catch (Exception ex)
                {

                }

                finally
                {
                    connection.Close();
                }
            }
        }

        public ObservableCollection<JobObject> FilterJobs(string jobNumber, string distNumber, string plateTypeCaliper, string shipTo, string status)
        {
            ObservableCollection<JobObject> jobsList = new ObservableCollection<JobObject>();
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM Schedule");

            bool showCompleted = false;
            bool first = true;
            if (status == "Complete") showCompleted = true;

            if (!String.IsNullOrWhiteSpace(jobNumber))
            {
                query.AppendLine($@" WHERE Job_number = '{jobNumber}'");
                first = false;
            }
            //////////////////////////////////////////////////////////////////
            if (!String.IsNullOrWhiteSpace(distNumber) && first)
            {
                query.AppendLine($@" WHERE Dist_number = '{distNumber}'");
                first = false;
            }
            else if (!String.IsNullOrWhiteSpace(distNumber))
            {
                query.AppendLine($@" AND Dist_number = '{distNumber}'");
            }
            //////////////////////////////////////////////////////////////////
            if (!String.IsNullOrWhiteSpace(plateTypeCaliper) && first)
            {
                query.AppendLine($@" WHERE Plate_type LIKE '%{plateTypeCaliper}%'");
                first = false;
            }
            else if (!String.IsNullOrWhiteSpace(plateTypeCaliper))
            {
                query.AppendLine($@" AND Plate_type LIKE '%{plateTypeCaliper}%'");
            }
            //////////////////////////////////////////////////////////////////
            if (!String.IsNullOrWhiteSpace(shipTo) && first)
            {
                query.AppendLine($@" WHERE Ship_to LIKE '%{shipTo}%'");
                first = false;
            }
            else if (!String.IsNullOrWhiteSpace(shipTo))
            {
                query.AppendLine($@" AND Ship_to LIKE '%{shipTo}%'");
            }
            //////////////////////////////////////////////////////////////////
            if (!String.IsNullOrWhiteSpace(status) && first)
            {
                query.AppendLine($@" WHERE Job_status = '{status}'");
                first = false;
            }
            else if (!String.IsNullOrWhiteSpace(status))
            {
                query.AppendLine($@" AND Job_status = '{status}'");
            }

            if (!showCompleted && first)
            {
                query.AppendLine($@" WHERE Job_status != 'Complete'");
            }
            else if (!showCompleted)
            {
                query.AppendLine($@" AND Job_status != 'Complete'");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //using
                SqlCommand command = new SqlCommand(query.ToString(), connection);
                connection.Open();

                //using
                SqlDataReader reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        var job = new JobObject();

                        job.JobNumber = Convert.ToString(reader["Job_number"]);
                        job.DateDue = Convert.ToDateTime(reader["Date_due"]);
                        job.DistNumber = Convert.ToString(reader["Dist_number"]);
                        job.PlateTotal = Convert.ToInt32(reader["Plate_total"]);
                        job.PlateType = Convert.ToString(reader["Plate_type"]);
                        job.OptimalWaste = Convert.ToDouble(reader["Optimal_waste"]);
                        job.ImageWidth = Convert.ToDouble(reader["Image_width"]);
                        job.ImageHeight = Convert.ToDouble(reader["Image_height"]);
                        job.ChargeWidth = Convert.ToDouble(reader["Charge_width"]);
                        job.ChargeHeight = Convert.ToDouble(reader["Charge_height"]);
                        job.PlateWidth = Convert.ToDouble(reader["Plate_width"]);
                        job.PlateHeight = Convert.ToDouble(reader["Plate_height"]);
                        job.ShipTo = Convert.ToString(reader["Ship_to"]);
                        job.JobStatus = Convert.ToString(reader["Job_status"]);

                        jobsList.Add(job);
                    }
                    return jobsList;
                }

                catch (Exception ex)
                {
                    return new ObservableCollection<JobObject>();
                }

                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
        }

        public bool InsertJobIntoSchedule(JobObject job)
        {
            string query = $"INSERT INTO Schedule(Dist_number, Date_due, Plate_total, Plate_type, Optimal_waste, Image_width, Image_height, Charge_width, " +
                $"Charge_height, Plate_width, Plate_height, Ship_to, Job_status, Job_number) VALUES({job.DistNumber}, '{job.DateDue}', {job.PlateTotal}, '{job.PlateType}', " +
                $"{job.OptimalWaste}, {job.ImageWidth}, {job.ImageHeight}, {job.ChargeWidth}, {job.ChargeHeight}, {job.PlateWidth}, {job.PlateHeight}, '{job.ShipTo}'," +
                $" '{job.JobStatus}', {job.JobNumber});";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //using
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                //using


                try
                {
                    var success = command.ExecuteNonQuery();
                    if (success == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                catch (Exception ex)
                {
                    return false;
                }

                finally
                {
                    connection.Close();
                }
            }
        }

        public bool InsertAssociatedJobPlates(JobPlateObject jobPlate, int jobNumber)
        {
            string query = $"INSERT INTO Job_Color(Color, Quantity, Job_number) VALUES ('{jobPlate.Color}', {jobPlate.Quantity}, {jobNumber});";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //using
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                //using


                try
                {
                    var success = command.ExecuteNonQuery();
                    if (success == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                catch (Exception ex)
                {
                    return false;
                }

                finally
                {
                    connection.Close();
                }
            }
        }

        public int GetLastEnteredJobNumber()
        {
            string query = "SELECT MAX(Job_number) AS JobNumber FROM SCHEDULE";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //using
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                //using
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    int jobNumber=0;
                    while (reader.Read())
                    {
                        jobNumber = Convert.ToInt32(reader["JobNumber"]);
                    }
                    return jobNumber;
                }

                catch (Exception ex)
                {
                    return 0;
                }

                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
        }

        public ObservableCollection<JobPlateObject> LoadPlatesForSelectedJob(int jobNumber)
        {
            ObservableCollection<JobPlateObject> platesList = new ObservableCollection<JobPlateObject>();
            string query = $"SELECT Color, Quantity FROM Job_Color WHERE Job_number = {jobNumber} ;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //using
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                //using
                SqlDataReader reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        var plate = new JobPlateObject();

                        plate.Color = Convert.ToString(reader["Color"]);
                        plate.Quantity = Convert.ToInt32(reader["Quantity"]);
                        platesList.Add(plate);
                    }
                    return platesList;
                }

                catch (Exception ex)
                {
                    return new ObservableCollection<JobPlateObject>();
                }

                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
        }
    }
}
