using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Contractors
{
    public class PostamateDeliveryService : IDeliveryService
    {
        public string UniqueCode => "Postamate";

        public string Title => "Доставка через постаматы в Бишкек";
        private static IReadOnlyDictionary<string, string> cities = new Dictionary<string, string>
        {
            { "1", "Бишкек" },
            { "2", "Ош" },
        };

        private static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> postamates = new Dictionary<string, IReadOnlyDictionary<string, string>>
        {
            {
                "1",
                new Dictionary<string, string>
                {
                    { "1", "Бишкек-1 жд вокзал" },
                    { "2", "Аламедин рынок" },
                    { "3", "Какое-то место" },
                }
            },
            {
                "2",
                new Dictionary<string, string>
                {
                    { "4", "Какой-то вокзал" },
                    { "5", "Чей-то двор" },
                    { "6", "Мой дом" },
                }
            }
        };



        public Form CreateForm(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return new Form(UniqueCode, order.Id, 1, false, new[]
            {
                new SelectionField("Город", "city", "1", cities),
            });
        }


        public Form MoveNext(int orderId, int step, IReadOnlyDictionary<string, string> values)
        {
            if (step == 1)
            {
                if (values["city"] == "1")
                {
                    return new Form(UniqueCode, orderId, 2, false, new Field[]
                    {
                        new HiddenField("Город", "city", "1"),
                        new SelectionField("Постамат", "postamate", "1", postamates["1"]),
                    });
                }
                else if (values["city"] == "2")
                {
                    return new Form(UniqueCode, orderId, 2, false, new Field[]
                    {
                        new HiddenField("Город", "city", "2"),
                        new SelectionField("Постамат", "postamate", "4", postamates["2"]),
                    });
                }
                else
                    throw new InvalidOperationException("Invalid postamate city.");
            }
            else if (step == 2)
            {
                return new Form(UniqueCode, orderId, 3, true, new Field[]
                {
                    new HiddenField("Город", "city", values["city"]),
                    new HiddenField("Постамат", "postamate", values["postamate"]),
                });
            }
            else
                throw new InvalidOperationException("Invalid postamate step.");
        }
    }
}

