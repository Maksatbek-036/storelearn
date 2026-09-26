using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Contractors
{
    public class CashPaymentService : IPaymentService
    {
        public string Name => "Cash";

        public string Title => "Оплата наличными";

        public Form FirstForm(Order order)
        {
            return Form.CreateFirst(Name)
                .AddParameters("orderId",order.Id.ToString());
        }

        public OrderPayment GetPayment(Form form)
        {
            if(form.ServiceName!=Name|| !form.IsFinal)
            {
                throw new ArgumentException("Invalid payment form.");
            }
            return new OrderPayment(Name, "Оплата наличными", new Dictionary<string, string>());

        }

        public Form NextForm(int step, IReadOnlyDictionary<string, string> values)
        {
            if (step != 1)
            {
                throw new InvalidOperationException("Invalid cash step.");
            }
            return Form.CreateLast(Name, step+1, values);
        }
    }
}
