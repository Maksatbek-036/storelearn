using Microsoft.AspNetCore.Mvc;
using Store.Contractors;
using Store.Messages;
using Store.Web.App;
using Store.Web.Contractors;
using Store.Web.Models;
using System.Collections.Frozen;
using System.Text.RegularExpressions;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {

        private readonly IEnumerable<IDeliveryService> deliveryServices;
        private readonly IEnumerable<IPaymentService> paymentServices;
        private readonly OrderService orderService;
        private readonly INotificationService notificationService;
        private readonly IEnumerable<IWebContractorService> webContractorServices;
        public OrderController(OrderService orderService,
                               IEnumerable<IDeliveryService> deliveryServices,
                               IEnumerable<IPaymentService> paymentServices,
                               IEnumerable<IWebContractorService> webContractorServices)
        {
            this.orderService = orderService;
            this.notificationService = notificationService;
            this.deliveryServices = deliveryServices;
            this.paymentServices = paymentServices;
            this.webContractorServices = webContractorServices;
        }



        //public IActionResult Index()
        //{
            
        //  if(orderService.TryGetModel(out OrderModel model))
        //    {
        //        return View(model);
        //    }
        //    return View("Empty");
        //}
        public async Task<ActionResult> Index()
        {
            var (hasvalue, model) = await orderService.TryGetModelAsync();
            if (hasvalue)
            {
                return View(model);
            }
            return View("Empty");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateItem(int id, int count)
        {
         await orderService.UpdateBookAsync(id, count);
            return RedirectToAction("Index", "Book", new { id });
        }
        public async Task<IActionResult> AddItem(int bookId, int count = 1)
        {
           await orderService.AddBookAsync(bookId, count);
            return RedirectToAction("Index", "Book", new { id = bookId });
        }
        public async Task<IActionResult> RemoveItem(int bookId)
        {
            var model=await orderService.RemoveBookAsync(bookId);
       

            return RedirectToAction("Index", model);
        }
     
        [HttpPost]
        public async Task<IActionResult> SendConfirmationCode(int id, string cellPhone)
        {

            var model=await orderService.SendConfirmationAsync(cellPhone);
            return View("Confirmation", model);
          
        }

       
        [HttpPost]
        public async Task<IActionResult> Confirmate(string cellPhone, int confirmationCode)
        {
            var model=await orderService.ConfirmCellPhoneAsync(cellPhone, confirmationCode);
            if (model.Errors.Count > 0)
            {
                return View("Confirmation", model);
            }
            var deliveryMethods = deliveryServices.ToDictionary(service => service.Name,
                service => service.Title);

            return View("DeliveryMethod", deliveryMethods);
        }
        public async Task<IActionResult> StartDelivery(string serviceName)
        {
            var deliveryService = deliveryServices.Single(service => service.Name == serviceName);
            var order = await orderService.GetOrderAsync();
         
            var form = deliveryService.FirstForm(order);

            var webContractorService = webContractorServices.SingleOrDefault(service => service.Name == serviceName);
            if(webContractorService== null)
            {
                return View("DeliveryStep", form);
            }
            var returnUri = GetReturnUri(nameof(NextDelivery));
            var redirectUri = await webContractorService.StartSessionAsync(form.Parameters,returnUri);

            return Redirect(redirectUri.ToString());
        }

        private Uri GetReturnUri(string action)
        {
            var builder = new UriBuilder(Request.Scheme, Request.Host.Host)
            {
                Path = Url.Action(action),
                Query =null
            };
            if (builder.Port != null)
            {
                builder.Port = Request.Host.Port.Value;
            }
            return builder.Uri;
        }

        [HttpPost]
        public async Task<IActionResult> StartPayment(string serviceName)
        {
            var paymentService = paymentServices.Single(service => service.Name == serviceName);
            var order = orderService.GetOrder();
            var form = paymentService.FirstForm(order);
            var webContractorservice = webContractorServices
                .SingleOrDefault(service => service.Name == serviceName);
            if (webContractorservice == null)
                return View("PaymentStep",form);

            var returnUri = GetReturnUri(nameof(NextPayment));
            var redirectUri = await webContractorservice.StartSessionAsync(form.Parameters, returnUri);

            return Redirect(redirectUri.ToString());
        }
        public IActionResult Finish()
        {
            HttpContext.Session.RemoveCart();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NextPayment( string serviceName, int step, Dictionary<string, string> values)
        {
            var paymentService=paymentServices.Single(service=>service.Name == serviceName);
            var form=paymentService.NextForm(step, values);
            if (!form.IsFinal)
            {
                return View("PaymentStep",form);
            }
            var payment = paymentService.GetPayment(form);
            var model=await orderService.SetPaymentAsync(payment);
            return View("Finish", model);
            
        }
        [HttpPost]
        public async Task<IActionResult> NextDelivery( string serviceName, int step, Dictionary<string, string> values)
        {
            var deliveryService = deliveryServices.Single(service => service.Name == serviceName);
            var form = deliveryService.NextForm(step, values);
            if (!form.IsFinal)
            {

                return View("DeliveryStep", form);
            }
            var delivery = deliveryService.GetDelivery(form);
            await orderService.SetDeliveryAsync(delivery);

            var paymentMethods = paymentServices.ToDictionary(service => service.Name,
                service => service.Title);

            return View("PaymentMethod", paymentMethods);
        }
    }
}
