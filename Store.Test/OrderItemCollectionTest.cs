using Store.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Tests
{
    public class OrderItemCollectionTest
    {


        [Fact]
        public void TotalCount_WithEmptyItems_ReturnsZero()
        {

            var order = new Order(new Data.OrderDto
            {
                Id = 1,
                Items = new OrderItemDto[0]
            });
            Assert.Equal(0, order.TotalCount);
        }
        [Fact]
        public void TotalPrice_WithEmptyItems_ReturnsZero()
        {
            var order = new Order(new Data.OrderDto
            {
                Id = 1,
                Items = new OrderItemDto[0]
            });
            Assert.Equal(0, order.TotalPrice);
        }
        [Fact]
        public void TotalCount_WithNonEmptyItems_CalculatesTotalCount()
        {
            var order = new Order(new OrderDto
            {
                Id = 1,
                Items = new OrderItemDto[] {
                new OrderItemDto{Id=1,Price=3m,Count=2},
                new OrderItemDto{Id=2,Price=3m,Count=3}
                }
            });
            Assert.Equal(2 + 3, order.TotalCount);
        }
        [Fact]
        public void TotalPrice_WithNonEmptyItems_CalculatesTotalPrice()
        {
         
            var order = new Order(new OrderDto
            {
                Id = 1,
                Items = new OrderItemDto[] {
                new OrderItemDto{Id=1,Price=3m,Count=2},
                new OrderItemDto{Id=2,Price=4m,Count=3}
                }
            });

            Assert.Equal(2 * 3m + 3 * 4m, order.TotalPrice);
        }
        [Fact]
        public void Remove_WithExistingItem_RemovesItem()
        {
            var order = CreateTestOrder();
          
            order.Items.Remove(1);
            Assert.Collection(order.Items,
                             item => Assert.Equal(2, item.BookId));

        }

        private static Order CreateTestOrder()
        {
            return new Order(new OrderDto
            {

                Id = 1,
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto { BookId = 1, Price = 10m, Count = 3},
                    new OrderItemDto { BookId = 2, Price = 100m, Count = 5},
                }
            });
        }
    }
}
