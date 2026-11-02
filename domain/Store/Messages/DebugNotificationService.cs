using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;

namespace Store.Messages
{
    public class DebugNotificationService : INotificationService
    {
        public void SendConfirmationCode(string cellPhone, int code)
        {
            Debug.WriteLine("Cell phone: {0}, code: {1:0000}.", cellPhone, code);
        }

        public Task SendConfirmationCodeAsync(string cellPhone, int code)
        {
            Debug.WriteLine("Cell phone: {0}, code: {1:0000}.", cellPhone, code);
            return Task.CompletedTask;
        }

        public void StartProcess(Order order)
        {
            //using (var client = new SmtpClient())
            //{
            //    var message = new MailMessage("from@at.my.domain", "to@at.my.domain");
            //    message.Subject = "Заказ #" + order.Id;

            //    var builder = new StringBuilder();
            //    foreach (var item in order.Items)
            //    {
            //        builder.Append("{0}, {1}", item.BookId, item.Count);
            //        builder.AppendLine();
            //    }

            //    message.Body = builder.ToString();
            //    client.Send(message);
            //}
            Debug.WriteLine("Order Id: " + order.Id.ToString());
            Debug.WriteLine("Delivery: " + order.Delivery.Description);
            Debug.WriteLine("Payment: " + order.Payment.Description);
        }

        public Task StartProcessAsync(Order order)
        {
            StartProcess(order);
            return Task.CompletedTask;
        }
    }
}
