using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.Entities
{
    public enum OrderStatusEnum
    {
        Pending = 1,
        Cancelled,
        Approved,
        Shipped,
        Delivered
    }
    public enum PaymentMethodEnum 
    {
        Cash = 1,
        Visa
    }
    public class Order
    {
        public int Id { get; set; }
        public OrderStatusEnum OrderStatus { get; set; } = OrderStatusEnum.Pending; 
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime ShippedDate { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public decimal AmountPaid { get; set; }
        public string? SessionId { get; set; }
        public string? PaymnetId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
