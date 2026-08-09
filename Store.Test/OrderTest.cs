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
    }
}
