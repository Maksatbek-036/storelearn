using Store.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Tests
{
    public class OrderTest
    {
        [Fact]
        public void Order_WithNullItem_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var order = new Order(
                    new OrderDto
                    {
                        Id=1,
                        Items = null
                    }
                    );


            });
           }

        [Fact]
        public void Get_WithExistingItem_ReturnItem()
        {
            var order = CreateEmptyTestOrder();
            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Get(100);

            });
        }

        private Order CreateEmptyTestOrder()
        {
            return new Order(new Data.OrderDto
            {
                Id = 1,
                Items = new OrderItemDto[0]
            });
        }

        [Fact]
        public void AddOrUpdateItem_WithExistingItem_UpdatesCount()
        {
            var order = CreateOrderTest();
            var book = new Book(new BookDto
            {
                Id = 1
            });
            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Add(book.Id, 2m, 5);

            });
        }
        [Fact]
        public void Add_WithNewItem_SetsCount()
        {
            var order = CreateOrderTest();

            order.Items.Add(4, 30m, 10);

            Assert.Equal(10, order.Items.Get(4).Count);

        }
        [Fact]
        public void Remove_WithExistingItem_RemovesItem()
        {
            var order = CreateOrderTest();
            
            order.Items.Remove(1);
            Assert.Collection(order.Items,
                             item => Assert.Equal(2, item.BookId));
        }
      
        private  Order CreateOrderTest()
        {
            return new Order(new OrderDto
            {
                Id = 1,
                Items = new List<OrderItemDto>
                {
                     new OrderItemDto{BookId=1,Price=10m,Count=3},
                      new OrderItemDto{BookId=2,Price=10m,Count=5},

                }
            });
        }
        public void Remove_WithNonExistingItem_ThrowsInvalidOperation()
        {
            var order = CreateOrderTest();
            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Remove(3);
            });
        }
        
    }
}
