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
        public void Get_WithExistingItem_ReturnItem()
        {
            var order = new Order(1, new OrderItem[] {
            new OrderItem(1,3m,2),
            new OrderItem(2,5m,4),
            });
            Assert.Throws<InvalidOperationException>(()=> 
            {
                order.Items.Get(100);
                
            });
        }
        [Fact]
        public void AddOrUpdateItem_WithExistingItem_UpdatesCount()
        {
            var order = new Order(1, new OrderItem[] {
            new OrderItem(1,2m,2),
            });
            var book = new Book(1, null,null,null,null,1m);
            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Add(book.Id, 2m, 5);

            });
        }
        [Fact]
        public void AddOrUpdateItem_WithNonExistingItem_AddsCount()
        {
            var order = new Order(1, new OrderItem[] {
            new OrderItem(1,2,2)
            });
            var book = new Book(2, null, null, null, null, 1m);
            order.Items.Add(2,30m,3);
            Assert.Equal(3, order.Items.Get(2).Count);
        }
        [Fact]
        public void Remove_WithExistingItem_RemovesItem()
        {
            var order = new Order(1, new OrderItem[] {
            new OrderItem(1,3m,2),
            new OrderItem(2,5m,4),
            });
            order.Items.Remove(1);
            Assert.Collection(order.Items,
                             item => Assert.Equal(2, item.BookId));
        }
        [Fact]
        public void Remove_WithNonExistingItem_ThrowsInvalidOperation()
        {
            var order = new Order(1, new OrderItem[] {
            new OrderItem(1,3m,2),
            new OrderItem(2,5m,4),
            });
            Assert.Throws<InvalidOperationException>(() =>
            {
                order.Items.Remove(3);
            });
        }
    }
}
