using Microsoft.AspNetCore.Mvc;
using Store.Contractors;
using Store.Messages;
using Store.Web.Models;
using System.Text.RegularExpressions;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IEnumerable<IDeliveryService> deliveryServices;
        private readonly INotificationService notificationService;
        public OrderController(IBookRepository bookRepository, 
                               IOrderRepository orderRepository,
                               INotificationService notificationService,
                               IEnumerable<IDeliveryService> deliveryServices)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
            this.notificationService = notificationService;
            this.deliveryServices = deliveryServices;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                var order = orderRepository.GetById(cart.OrderId);
                OrderModel model = Map(order);
                return View(model);
            }
            return View("Empty");
        }
        private OrderModel Map(Order order)
        {
            var bookIds = order.Items.Select(item => item.BookId);
            var books = bookRepository.GetAllByIds(bookIds);
            var itemModels = from item in order.Items
                             join book in books on item.BookId equals book.Id
                             select new OrderItemModel
                             {
                                 BookId = book.Id,
                                 Author = book.Author,
                                 Count = item.Count,
                                 Price = item.Price,
                                 Title = book.Title
                             };
            return new OrderModel
            {
                Id = order.Id,
                Items = itemModels.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice
            };
        }
        public IActionResult AddBook(int id)
        {
            (Order order, Cart cart) = GetOrCreateOrderOrCart();
            order.GetItem(id).Count++;
            SaveOrderAndCart(order, cart);


            return RedirectToAction("Index", "Book", new { id });
        }
        [HttpPost]
        public IActionResult UpdateItem(int id, int count)
        {
            (Order order, Cart cart) = GetOrCreateOrderOrCart();
            order.GetItem(id).Count = count;
            SaveOrderAndCart(order, cart);
            return RedirectToAction("Index", "Book", new { id });
        }


        private void SaveOrderAndCart(Order order, Cart cart)
        {
            orderRepository.Update(order);
            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;
            HttpContext.Session.Set(cart);
        }

        public IActionResult AddItem(int bookId, int count = 1)
        {
            (Order order, Cart cart) = GetOrCreateOrderOrCart();
            var book = bookRepository.GetById(bookId);
            order.AddOrUpdateItem(book, count);
            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Book", new { id = bookId });
        }
        public IActionResult RemoveItem(int bookId)
        {
            (Order order, Cart cart) = GetOrCreateOrderOrCart();
            var book = bookRepository.GetById(bookId);
            order.RemoveItem(bookId);
            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }
        private (Order order, Cart cart) GetOrCreateOrderOrCart()
        {
            Order order;
            Cart cart;
            if (HttpContext.Session.TryGetCart(out cart))
            {
                order = orderRepository.GetById(cart.OrderId);
            }
            else
            {
                order = orderRepository.Create();
                cart = new Cart(order.Id);


            }
            return (order, cart);
        }
        [HttpPost]
        public IActionResult SendConfirmationCode(int id, string cellPhone)
        {
            var order = orderRepository.GetById(id);
            var model = Map(order);

            if (IsValidCellPhone(cellPhone))
            {
              
                    model.Errors["cellPhone"] = "Номер телефона не соответствует формату +79876543210";
                    return View("Index", model);
                


            }
            int code = 1111; // random.Next(1000, 10000)
            HttpContext.Session.SetInt32(cellPhone, code);
            notificationService.SendConfirmationCode(cellPhone, code);
            return View("Confirmation", new ConfirmationModel()
            {
                CellPhone = cellPhone,
                OrderId=id
            });
        }

        private bool IsValidCellPhone(string cellPhone)
        {
            if (cellPhone == null)
                return false;

            cellPhone = cellPhone.Replace(" ", "")
                                 .Replace("-", "");

            return Regex.IsMatch(cellPhone, @"^\+?\d{11}$");
        }
        [HttpPost]
        public IActionResult Confirmate(int id, string cellPhone, int code)
        {
            int? storeCode = HttpContext.Session.GetInt32(cellPhone);
            if (storeCode == null)
            {
                return View("Confitmation", new ConfirmationModel()
                {
                    OrderId = id,
                    CellPhone = cellPhone,
                    Errors = new Dictionary<string, string>() {
                    { "code","Пустой код" }
                }
                });
            }
            if (storeCode != code)
            {
                return View("Confirmation", new ConfirmationModel()
                {
                    OrderId = id,
                    CellPhone = cellPhone,
                    Errors = new Dictionary<string, string>() {
                    { "code","Отличается от отправленного кода" }
                }});

            }
            //todo:сохранить cellPhone
            HttpContext.Session.Remove(cellPhone);
            var model = new DeliveryModel
            {
                OrderId = id,
                Methods = deliveryServices.ToDictionary(service => service.UniqueCode,
                service => service.Title)
            };


            return View("DeliveryMethod", model);
        }
        public IActionResult StartDelivery(int id,string uniqueCode)
        {
            var deliveryService = deliveryServices.Single(service => service.UniqueCode == uniqueCode);
            var order=orderRepository.GetById(id);
            var form = deliveryService.CreateForm(order);
            return View("DeliveryStep", form);
        }
        [HttpPost]
        public IActionResult NextDelivery(int id,string uniqueCode,int step,Dictionary<string,string> values) {
            var deliveryService = deliveryServices.Single(service => service.UniqueCode == uniqueCode);
            var form = deliveryService.MoveNext(id, step, values);
            if (form.IsFinal)
            {
                return null;
            }

            return View("DeliveryStep", form);

        }
    }
}
