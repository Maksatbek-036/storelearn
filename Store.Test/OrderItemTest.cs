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
                new OrderItem(1, count, 0m);
            });
        }
        [Fact]
        public void OrderItem_WithNegativeCount_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int count = -1;
                new OrderItem(1, count, 0m);
            });
        }

        [Fact]
        public void OrderItem_WithPositiveCount_SetsCount()
        {
            var orderItem = new OrderItem(1, 2, 3m);
            Assert.Equal(1, orderItem.BookId);
            Assert.Equal(2,orderItem.Count);
            Assert.Equal(3, orderItem.Price);
        }
        [Fact]
        public void Count_WithNegativeValue_ThrowsArgumentOutRangeOfException()
        {
            var orderItem = new OrderItem(1, 2, 3m);
            Assert.Throws<ArgumentOutOfRangeException>(() =>{
                orderItem.Count = -1;

            });
        }
        [Fact]
        public void Count_WithZeroValue_ThrowsArgumentOutRangeOfException()
        {
            var orderItem = new OrderItem(1, 2, 3m);
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                orderItem.Count = 0;

            });
        }
    }
}
