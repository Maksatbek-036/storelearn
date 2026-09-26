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
            var order = new Order(1, new OrderItem[0]);
            Assert.Equal(0, order.TotalCount);
        }
        [Fact]
        public void TotalPrice_WithEmptyItems_ReturnsZero()
        {
            var order = new Order(1, new OrderItem[0]);
            Assert.Equal(0, order.TotalPrice);
        }
        [Fact]
        public void TotalCount_WithNonEmptyItems_CalculatesTotalCount()
        {
            var order = new Order(1, new OrderItem[] {
            new OrderItem(1,3m,2),
            new OrderItem(2,4m,3)

            });
            Assert.Equal(2 + 3, order.TotalCount);
        }
        [Fact]
        public void TotalPrice_WithNonEmptyItems_CalculatesTotalPrice()
        {
            var order = new Order(1, new OrderItem[] {
            new OrderItem(1,3m,2),
            new OrderItem(2,4m,3)

            });
            Assert.Equal(2 * 3m + 3 * 4m, order.TotalPrice);
        }
        [Fact]
        public void Remove_WithExistingItem_RemovesItem()
        {
            var order = new Order(1, new [] {
            new OrderItem(1,3m,2),
            new OrderItem(2,5m,4),
            });
            order.Items.Remove(1);
            Assert.Collection(order.Items,
                             item => Assert.Equal(2, item.BookId));
            Assert.Equal(1, order.Items.Count);
        }
    }
}
