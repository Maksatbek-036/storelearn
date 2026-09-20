using Store;
using Store.Contractors;
using Store.Web.Contractors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.YandexKassa
{
    public class YandexKassaPaymentService : IPaymentService, IWebContractorService
    {
        public string UniqueCode => "Yandex.Kassa";

        public string GetUri => "/YandexKassa/";

        public string Title => "Яндекс";

        public Form CreateForm(Order order)
        {
            return new Form(UniqueCode, order.Id,1,true,new Field[0]);
        }

        public OrderPayment GetPayment(Form form)
        {
            throw new NotImplementedException();
        }

        public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> values)
        {
            return new Form(UniqueCode,orderId,2,true, new Field[0]);
        }
    }
}
