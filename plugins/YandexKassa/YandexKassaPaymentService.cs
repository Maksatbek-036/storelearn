using Microsoft.AspNetCore.Http;
using Store;
using System.Net.Http;
using Store.Contractors;
using Store.Web.Contractors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.YandexKassa
{
    public class YandexKassaPaymentService : IPaymentService, IWebContractorService
    {
        public string Name => "Yandex.Kassa";
        private readonly IHttpContextAccessor httpContextAccessor;
        public YandexKassaPaymentService(IHttpContextAccessor httpContextAccessor) 
        {
            this.httpContextAccessor = httpContextAccessor;

        }
        private HttpRequest Request => httpContextAccessor.HttpContext.Request;


        public string Title => "Яндекс";

        public Form FirstForm(Order order)
        {
            return Form.CreateFirst(Name)
                .AddParameters(nameof(order.Id),order.Id.ToString());
        }

        public OrderPayment GetPayment(Form form)
        {
           if(form.ServiceName!=Name || !form.IsFinal)
            {
                throw new InvalidOperationException("Invalid payment form.");
            }
            return new OrderPayment(Name, "Оплата картой", form.Parameters);
        }

        public Form NextForm(int step, IReadOnlyDictionary<string, string> values)
        {
            if (step != 1)
                new InvalidOperationException("Invalid Yandex.Kassa payment step");
            return Form.CreateLast(Name, step + 1, values);
        }

        public Uri StartSession(IReadOnlyDictionary<string, string> parameters,Uri returnUri)
        {
            var queryString = QueryString.Create(parameters);
            queryString += QueryString.Create("returnUri", returnUri.ToString());

            var builder = new UriBuilder(Request.Scheme, Request.Host.Host)
            {
                Path = "YandexKassa",
                Query = queryString.ToString()
            };
            if (Request.Host.Port != null)
            {
                builder.Port = Request.Host.Port.Value;
            }
            return builder.Uri;
        }
    }
}
