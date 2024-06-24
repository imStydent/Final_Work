using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragrantWorld.Classes
{
    internal class Order
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public DateTime DeliveryDate { get; set; }
        public int PickupPoint { get; set; }
        public int UserId { get; set; }
        public int ReceiptCode { get; set; } = (new Random()).Next(100, 1000);
        public double TotalDiscount { get => DataAccessLayer.GetOrderTotalDiscount(Id); set { } }
        public double TotalCost { get => DataAccessLayer.GetOrderTotalCost(Id); set { } }
        public string UserFullname { get => DataAccessLayer.GetUserFullName(Id); set { } }
    }
}
