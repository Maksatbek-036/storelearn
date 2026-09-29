using Store.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Store
{
    public class Order
    {
        private readonly OrderDto dto;

        public int Id => dto.Id;
 
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

        public string CellPhone
        {
            get => dto.CellPhone;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(nameof(value));
                }
                dto.CellPhone = value;
            }
        }
        public OrderDelivery Delivery
        {
            get
            {
                if (dto.DeliveryUniqueCode == null)
                {
                    return null;
                }
                return new OrderDelivery(
                    dto.DeliveryUniqueCode,
                    dto.DeliveryDescription,
                    dto.DeliveryPrice,
                    dto.DeliveryParameters
                    );
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Delivery));
                dto.DeliveryDescription = value.Description;
                dto.DeliveryUniqueCode = value.UniqueCode;
                dto.DeliveryParameters = value.Parametres
                    .ToDictionary(item => item.Key,
                    item => item.Value);
            }
        }
        public OrderPayment Payment
        {
            get
            {

                if (dto.PaymentServiceName == null)
                {
                    return null;
                }
                return new OrderPayment(
                    dto.PaymentServiceName,
                    dto.PaymentDescription,
                    dto.PaymentParameters
                    );
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Delivery));
                dto.PaymentDescription = value.Description;
                dto.PaymentServiceName = value.ServiceName;
                dto.PaymentParameters = value.Parametres
                    .ToDictionary(item => item.Key,
                    item => item.Value);
            }
        }
        public Order(OrderDto dto)
        {
        
            this.dto= dto;
            Items = new OrderItemCollection(dto);
        }
        public static class DtoFactory{
            public static OrderDto Create() => new OrderDto();
        
        }
      public static class Mapper
        {
            public static Order Map(OrderDto dto) => new Order(dto);
            public static OrderDto Map(Order domain) => domain.dto;
        }
    }

}
