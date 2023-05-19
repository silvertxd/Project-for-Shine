using Project.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class SupplyDg
    {
        public int Id { get; set; }
        public DateTime SupplyDate { get; set; }
        public string SupplyDateStr => SupplyDate.ToShortDateString();
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string SellerName { get; set; }
        public double TotalPrice { get; set; }
        public bool IsSelected { get; set; } = false; 
    }
}
