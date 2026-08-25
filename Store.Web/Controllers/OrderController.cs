using Microsoft.AspNetCore.Mvc;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    public class OrderController: Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        public OrderController(IBookRepository bookRepository, IOrderRepository orderRepository)
        {
            this.bookRepository = bookRepository;
            this.orderRepository = orderRepository;
        }
        public IActionResult Index()
        {   if(HttpContext.Session.TryGetCart(out Cart cart))
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
                                 BookId=book.Id,
                                 Author=book.Author,
                                 Count=item.Count,
                                 Price=item.Price,
                                 Title=book.Title
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
            order.GetItem(id).Count += 1;
            SaveOrderAndCart(order, cart);
            

            return RedirectToAction("Index", "Book", new { id });
        }
        [HttpPost]
        public IActionResult UpdateItem(int id,int count)
        {
            (Order order,Cart cart) = GetOrCreateOrderOrCart();
            order.GetItem(id).Count = count;
            SaveOrderAndCart(order,cart);
            return RedirectToAction("Index", "Book", new { id });
        }
   

        private void SaveOrderAndCart(Order order, Cart cart)
        {
            orderRepository.Update(order);
            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;
            HttpContext.Session.Set(cart);
        }

        public IActionResult AddItem(int id,int count)
        {
            (Order order, Cart cart) = GetOrCreateOrderOrCart();
            Book book = bookRepository.GetById(id);
            order.AddOrUpdateItem(book, count);

            return RedirectToAction("Index", "Book", new { id });
        }
        public IActionResult RemoveItem(int id)
        {
            (Order order, Cart cart) = GetOrCreateOrderOrCart();
            var book = bookRepository.GetById(id);
            order.RemoveItem(id);
            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Book", new { id });
        }
        private (Order order,Cart cart) GetOrCreateOrderOrCart()
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

    }
}
