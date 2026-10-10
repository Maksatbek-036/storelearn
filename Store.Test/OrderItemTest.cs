using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Sdk;

namespace Store.Tests
{
    public class OrderItemTest
    {
        [Fact]
        public void OrderItem_WithZeroCount_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = 0;
                OrderItem.DtoFactory.Create(new Data.OrderDto(), 1, 2m, count);
            });
        }
        [Fact]
        public void OrderItem_WithNegativeCount_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = -1;

                OrderItem.DtoFactory.Create(new Data.OrderDto(), 1, 2m, count);
            });
        }

        [Fact]
        public void OrderItem_WithPositiveCount_SetsCount()
        {
            var order = Order.DtoFactory.Create();
            var orderItem = OrderItem.DtoFactory.Create(order, 1, 3m, 2);
          
            Assert.Equal(1, orderItem.BookId);
            Assert.Equal(2,orderItem.Count);
            Assert.Equal(3, orderItem.Price);
        }
        [Fact]
        public void Count_WithNegativeValue_ThrowsArgumentOutRangeOfException()
        {
            var order = Order.DtoFactory.Create();
            var orderItem = OrderItem.DtoFactory.Create(order, 1, 3m, 2);
            var item = OrderItem.Mapper.Map(orderItem);
            
            Assert.Throws<ArgumentOutOfRangeException>(() =>{
                item.Count = -1;

            });
        }
        [Fact]
        public void Count_WithZeroValue_ThrowsArgumentOutRangeOfException()
        {
            var orderItem = new OrderItem(new Data.OrderItemDto { Id=1,Count=2,Price=3m});
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                orderItem.Count = 0;

            });
        }
    }
}
