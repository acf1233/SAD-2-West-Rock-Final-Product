/*
    This class stores all relevent information for scheduled jobs. 
    The information is read in by the SQL class that runs queries on the Schedule table. 
 */

using System;
using System.Collections.Generic;

namespace Westrock_Plate_Job_Manager.Models
{
    public class JobObject
    {
        public string JobNumber { get; set; }
        public string DistNumber { get; set; }
        public DateTime DateDue { get; set; }
        public string ShipTo { get; set; }
        public int PlateTotal { get; set; }
        public string PlateType { get; set; }
        public double OptimalWaste { get; set; }
        public double ImageWidth { get; set; }
        public double ImageHeight { get; set; }
        public double PlateWidth { get; set; }
        public double PlateHeight { get; set; }
        public double ChargeWidth { get; set; }
        public double ChargeHeight { get; set; }
        public string JobStatus { get; set; }
        public List<JobPlateObject> JobPlates { get; set; }
    }
}
