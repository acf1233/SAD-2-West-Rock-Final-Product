using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Westrock_Plate_Job_Manager.Models
{
    public class PlateOrder
    {
        public string OrderId { get; set; }
        public string Type { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Caliper { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
