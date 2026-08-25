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
                new Order(1, null);
            });
        }
        [Fact]
        public void TotalCount_WithEmptyItems_ReturnsZero()
        {
            var order = new Order(1, new OrderItem[0]);
            Assert.Equal(0, order.TotalCount);
        }
        [Fact]
        public void TotalPrice_WithEmptyItems_ReturnsZero() { 
            var order=new Order(1, new OrderItem[0]);
            Assert.Equal(0,order.TotalPrice);
        }
        [Fact]
        public void TotalCount_WithNonEmptyItems_CalculatesTotalCount()
        {
            var order = new Order(1, new OrderItem[] {
            new OrderItem(1,2,3m),
            new OrderItem(2,3,4m)

            });
            Assert.Equal(2 + 3, order.TotalCount);
        }
        [Fact]
        public void TotalPrice_WithNonEmptyItems_CalculatesTotalPrice()
        {
            var order = new Order(1, new OrderItem[] {
            new OrderItem(1,2,3m),
            new OrderItem(2,3,4m)

            });
            Assert.Equal(2*3m + 3*4m, order.TotalPrice);
        }
        [Fact]
        public void Get_WithExistingItem_ReturnItem()
        {
            var order = new Order(1, new OrderItem[] {
            new OrderItem(1,2,3m),
            new OrderItem(2,4,5m),
            });
            Assert.Throws<InvalidOperationException>(()=> 
            {
                var orderItem = order.GetItem(10);
                
            });
        }
        [Fact]
        public void AddOrUpdateItem_WithExistingItem_UpdatesCount()
        {
            var order = new Order(1, new OrderItem[] {
            new OrderItem(1,2,2m),
            });
            var book = new Book(1, null,null,null,null,1m);
            order.AddOrUpdateItem(book,10);
            Assert.Equal(12,order.GetItem(1).Count);
        }
        [Fact]
        public void AddOrUpdateItem_WithNonExistingItem_AddsCount()
        {
            var order = new Order(1, new OrderItem[] {
            new OrderItem(1,2,2)
            });
            var book = new Book(2, null, null, null, null, 1m);
            order.AddOrUpdateItem(book, 3);
            Assert.Equal(3, order.GetItem(2).Count);
        }
        [Fact]
        public void Remove_WithExistingItem_RemovesItem()
        {
            var order = new Order(1, new OrderItem[] {
            new OrderItem(1,2,3m),
            new OrderItem(2,4,5m),
            });
            order.RemoveItem(1);
            Assert.Equal(1, order.Items.Count);
        }
        [Fact]
        public void Remove_WithNonExistingItem_ThrowsInvalidOperation()
        {
            var order = new Order(1, new OrderItem[] {
            new OrderItem(1,2,3m),
            new OrderItem(2,4,5m),
            });
            Assert.Throws<InvalidOperationException>(() =>
            {
                order.RemoveItem(3);
            });
        }
    }
}
