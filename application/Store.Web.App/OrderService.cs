using Microsoft.AspNetCore.Http;
using PhoneNumbers;
using Store.Messages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Store.Web.App
{
    public class OrderService
    {
        private IBookRepository bookRepository;
        private IOrderRepository orderRepository;
        private INotificationService notificationService;
        private IHttpContextAccessor httpContextAccessor;
        protected ISession Session => httpContextAccessor.HttpContext.Session;

  

        public OrderService(IBookRepository bookRepository,
                            IOrderRepository orderRepository,
                            INotificationService notificationService, 
                            IHttpContextAccessor httpContextAccessor)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.notificationService = notificationService;
            this.httpContextAccessor = httpContextAccessor;
        }
        public bool TryGetModel(out OrderModel model)
        {
            if(TryGetOrder(out Order order))
            {
                model = Map(order);
                return true;
            }
            model = null;
            return false;
        }

        private OrderModel Map(Order order)
        {
            var books = GetBooks(order);
            var items = from item in order.Items
                        join book in books on item.BookId equals book.Id
                        select new OrderItemModel 
                        { 
                        BookId= book.Id,
                        Title= book.Title,
                        Author=book.Author,
                        Count=item.Count,
                        Price=book.Price,
                        };
            return new OrderModel
            {
                Id = order.Id,
                Items = items.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice=order.TotalPrice,
                CellPhone = order.CellPhone,
                DeliveryDescription = order.Delivery?.Description,
                PaymentDescription=order.Payment?.Description,
                

            };
        }

        internal IEnumerable<Book> GetBooks(Order order)
        {
            var bookIds = order.Items.Select(item => item.BookId);
            return bookRepository.GetAllByIds(bookIds);
        }

        internal bool TryGetOrder(out Order order)
        {
            if(Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId);
                return true;
            }
            order = null;
            return false;
        }
        public OrderModel AddBook(int bookId,int count)
        {
            if (count < 1)
                throw new InvalidOperationException("Too few books to add");
            if (!TryGetOrder(out Order order))
                order = orderRepository.Create();
            
            AddOrUpdateBook(order, bookId, count);
            UpdateSession(order);
            
            
           
            return Map(order);
        }

        private void UpdateSession(Order order)
        {
            Cart cart = new Cart(order.Id, order.TotalCount, order.TotalPrice);
            Session.Set(cart);
        }

        private void AddOrUpdateBook(Order order, int bookId, int count)
        {
            var book = bookRepository.GetById(bookId);
            if(order.Items.TryGet(bookId, out OrderItem orderItems))
            {
                orderItems.Count += count;
            }
            else
            {
                order.Items.Add(book.Id, book.Price, count);
            }
        }
        public OrderModel UpdateBook(int bookId,int count)
        {
            var order = GetOrder();
            order.Items.Get(bookId).Count = count;
            UpdateSession(order);
            return Map(order);
        }
        public OrderModel RemoveBook(int bookId)
        {
            var order = GetOrder();
            order.Items.Remove(bookId);
            orderRepository.Update(order);
            UpdateSession(order);
            return Map(order);
        }

        public Order GetOrder()
        {
            if (TryGetOrder(out Order order))
                return order;
            else
                throw new InvalidOperationException("Empty session.");
        }
        public OrderModel SendConfirmation(string cellPhone)
        {
            var order = GetOrder();
            var model = Map(order);
            if(TryFormatPhone(cellPhone,out string formattedPhone))
            {
                var confirmationCode = 1111; //TODO:random.Next(1000,1000)
                model.CellPhone = formattedPhone;
                Session.SetInt32(formattedPhone, confirmationCode);
                notificationService.SendConfirmationCode(formattedPhone, confirmationCode);
            }
            else
            {
                model.Errors["cellPhone"] = "Номер не соотвествует формату +996774250425";
            }
            return model;
        }
        public OrderModel ConfirmCellPhone(string cellPhone,int confirmationCode)
        {
            int? storeCode = Session.GetInt32(cellPhone);
            var model = new OrderModel();
            if (storeCode == null)
            {
                model.Errors["cellPhone"] = "Что-то случилось. Попробуйте получить код еще раз.";
                return model;
            }
            if (storeCode != confirmationCode) {
                model.Errors["confirmationCode"] = "Неверный код. Проверьте и попробуйте еще раз.";
                return model;
            }
            var order=GetOrder();
            order.CellPhone=cellPhone;
            orderRepository.Update(order);
            Session.Remove(cellPhone);
            return Map(order);
            

        }
        private readonly PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
        private bool TryFormatPhone(string cellPhone, out string formattedPhone)
        {
            try
            {
                var phoneNumber = phoneNumberUtil.Parse(cellPhone, "KG");
                formattedPhone = phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);
                return true;
            }
            catch (NumberParseException)
            {
                formattedPhone = null;
                return false;
            }
        }

        public object GetOrderBooks()
        {
            throw new NotImplementedException();
        }

        public void SetDelivery(OrderDelivery delivery)
        {
            GetOrder().Delivery = delivery;
        }

        public OrderModel SetPayment(OrderPayment payment)
        {
            var order = GetOrder();
            order.Payment=payment;
            return Map(order);
        }
    }
}
