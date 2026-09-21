using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Store
{
    public class Order
    {
        public int Id { get; }
 
        public OrderItemCollection Items { get; }
     
        public int TotalCount
        {
            get
            {
                return Items.Sum(item => item.Count);
            }
        }
        public decimal TotalPrice
        {
            get { return Items.Sum(item => item.Price * item.Count)+(Delivery?.Amount??0m); }
        }

        public string CellPhone { get; set; }
        public OrderDelivery Delivery { get; set; }
        public OrderPayment Payment { get; set; }
        public Order(int id, IEnumerable<OrderItem> items)
        {
        
            Id = id;
            Items = new OrderItemCollection(items);
        }
      
    }

}
