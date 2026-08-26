using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Store
{
    public class Order
    {
        public int Id { get; }
        private List<OrderItem> items;
        public IReadOnlyCollection<OrderItem> Items
        {
            get { return items; }
        }
        public int TotalCount
        {
            get
            {
                return items.Sum(item => item.Count);
            }
        }
        public decimal TotalPrice
        {
            get { return items.Sum(item => item.Price * item.Count); }
        }

        public Order(int id, IEnumerable<OrderItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            Id = id;
            this.items = new List<OrderItem>(items);
        }

        public void AddItem(Book book, int count)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            var item = items.SingleOrDefault(x => x.BookId == book.Id);
            if (item == null)
            {
                items.Add(new OrderItem(book.Id, count, book.Price));
            }
            else
            {
                items.Remove(item);
                items.Add(new OrderItem(book.Id, item.Count + count, book.Price));
            }

        }
        public void RemoveItem(int bookId)
        {
            
            int index = items.FindIndex(item => item.BookId == bookId);

            if (index == -1)
                ThrowItemException("Order does not contain item.",bookId);

            items.RemoveAt(index);
        }
        public OrderItem GetItem(int bookId)
        {
            int index = items.FindIndex(item=>item.BookId== bookId);
            if (index == -1)
            {

                throw new InvalidOperationException("Book not found.");
            }
            return items[index];
        }
        public void AddOrUpdateItem(Book book, int count)
        {
            if (book == null)
               ThrowItemException("Book not found",book.Id);

            int index = items.FindIndex(item => item.BookId == book.Id);
            if (index == -1)
            {
                items.Add(new OrderItem(book.Id, count, book.Price));
            }
            else
            {
                items[index].Count += count;
            }
           
        }
        private void ThrowItemException(string message, int bookId)
        {
            var exception = new InvalidOperationException();
            exception.Data["bookId"] = bookId;
            throw exception;
        }
        public bool IsContainsItem(int bookId)
        {
            return items.Any(item=>item.BookId == bookId);
        }
    }

}
