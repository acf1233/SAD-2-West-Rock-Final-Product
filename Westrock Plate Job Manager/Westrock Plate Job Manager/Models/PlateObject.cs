/*
    This class stores all relevent information for flexo plates. 
    The information is read in by the SQL class that runs queries on the Plate_Inventory table. 
 */
using System.ComponentModel;

namespace Westrock_Plate_Job_Manager.Models
{
    public class PlateObject
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public double Caliper { get; set; }
        public string Type { get; set; }
        public int OnHandQuantity { get; set; }
        public int OnOrderQuantity { get; set; }
        public double UnitCost { get; set; }
        public int PlatesPerBox { get; set; }
        public int ColorSet // this property allows the data grid to know which rows to highlight based upon on hand plate quantity. We reference this property in the XAML to hightlight rows
        {
            get
            {
                if (OnHandQuantity == 0)
                {
                    return 0;
                }
                else if (OnHandQuantity <= 20)
                    return 1;
                else
                    return 2;
            }
        }

    }
}
