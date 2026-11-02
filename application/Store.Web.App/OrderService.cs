using Microsoft.AspNetCore.Http;
using PhoneNumbers;
using Store.Messages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
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
        public async Task<(bool hasValue,OrderModel model)> TryGetModelAsync()
        {
            var (hasvalue, order) = await TryGetOrderAsync();
            if (hasvalue)
            {
                var model = Map(order);
                return (true, model);
            }
            return (false, null);
           
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
        private async Task<(bool hasvalue, Order order)> TryGetOrderAsync()
        {
            if (Session.TryGetCart(out Cart cart))
            {
               var order = await orderRepository.GetByIdAsync(cart.OrderId);
                return (true,order);
            }
            
            return (false,null);
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
        public async Task<OrderModel> AddBookAsync(int bookId, int count)
        {
            if (count < 1)
                throw new InvalidOperationException("Too few books to add");
            var (hasvalue, order) = await TryGetOrderAsync();
            if (!hasvalue)
                order =await orderRepository.CreateAsync();

            await AddOrUpdateBookAsync(order, bookId, count);
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
           orderRepository.Update(order);
        }
        private async Task AddOrUpdateBookAsync(Order order, int bookId, int count)
        {
            var book = await bookRepository.GetByIdAsync(bookId);
            if (order.Items.TryGet(bookId, out OrderItem orderItems))
            {
                orderItems.Count += count;
            }
            else
            {
                order.Items.Add(book.Id, book.Price, count);
            }
           await orderRepository.UpdateAsync(order);
        }
       
        public async Task<OrderModel> UpdateBookAsync(int bookId, int count)
        {
            var order = await GetOrderAsync();
            order.Items.Get(bookId).Count = count;
            UpdateSession(order);
            return Map(order);
        }

      
       
        public async Task<OrderModel> RemoveBookAsync(int bookId)
        {
            var order = await GetOrderAsync();
            order.Items.Remove(bookId);
             await orderRepository.UpdateAsync(order);
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
        public async Task<Order> GetOrderAsync()
        {
            var (hasValue, order) = await TryGetOrderAsync();
            if(hasValue)
                return order;
            else
                throw new InvalidOperationException("Empty session.");
        }

     
        public async Task<OrderModel> SendConfirmationAsync(string cellPhone)
        {
            var order = await GetOrderAsync();
            var model = Map(order);
            if (TryFormatPhone(cellPhone, out string formattedPhone))
            {
                var confirmationCode = 1111; //TODO:random.Next(1000,1000)
                model.CellPhone = formattedPhone;
                Session.SetInt32(formattedPhone, confirmationCode);
                await notificationService.SendConfirmationCodeAsync(formattedPhone, confirmationCode);
            }
            else
            {
                model.Errors["cellPhone"] = "Номер не соотвествует формату +996774250425";
            }
            return model;

        }
    
        public async Task<OrderModel> ConfirmCellPhoneAsync(string cellPhone, int confirmationCode)
        {
            int? storeCode = Session.GetInt32(cellPhone);
            var model = new OrderModel();
            if (storeCode == null)
            {
                model.Errors["cellPhone"] = "Что-то случилось. Попробуйте получить код еще раз.";
                return model;
            }
            if (storeCode != confirmationCode)
            {
                model.Errors["confirmationCode"] = "Неверный код. Проверьте и попробуйте еще раз.";
                return model;
            }
            var order = await GetOrderAsync();
            order.CellPhone = cellPhone;
            await orderRepository.UpdateAsync(order);
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

    

       
        public async Task<OrderModel> SetPaymentAsync(OrderPayment payment)
        {
            var order = await GetOrderAsync();
            order.Payment = payment;
            Session.RemoveCart();

            await notificationService.StartProcessAsync(order);
            return Map(order);
        }

        public async Task<OrderModel> SetDeliveryAsync(OrderDelivery delivery)
        {
            var order = await GetOrderAsync();
            order.Delivery = delivery;
            await orderRepository.UpdateAsync(order);
            return Map(order);
        }


    }
}
