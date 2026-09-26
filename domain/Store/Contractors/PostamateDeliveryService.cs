using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Contractors
{
    public class PostamateDeliveryService : IDeliveryService
    {
        public string Name => "Postamate";

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



        public Form FirstForm(Order order)
        {
            return Form.CreateFirst(Name)
                 .AddParameters(nameof(order.Id), order.Id.ToString())
                 .AddField(new SelectionField("Город","city","1",cities));
        }


        public Form NextForm( int step, IReadOnlyDictionary<string, string> values)
        {
            if (step == 1)
            {
                if (values["city"] == "1")
                {
                    return Form.CreateNext(Name, 2, values)
                        .AddField(new SelectionField("Постамат", "postamate", "1", postamates["1"]));
                }
                else if (values["city"] == "2")
                {
                    return Form.CreateNext(Name, 2, values)
                      .AddField(new SelectionField("Постамат", "postamate", "4", postamates["2"]));
                }
                else
                {
                    throw new InvalidOperationException("Invalid postamate city");
                }
            }
            else if (step == 2)
            {
                return Form.CreateLast(Name, 3, values);
            }
            else throw new InvalidOperationException("Invalid postamate step");
        }

        public OrderDelivery GetDelivery(Form form)
        {
           if(form.ServiceName!=Name || !form.IsFinal)
            {
                throw new InvalidOperationException("Invalid form.");
            }
            var cityId = form.Parameters["city"];
            var cityName = cities[cityId];
            var postamateId = form.Parameters["postamate"];
            var postamateName = postamates[cityId][postamateId];
            var parameters = new Dictionary<string, string>
           {
                {nameof(cityId), cityId },
                {nameof(cityName),cityName },
                {nameof(postamateId),postamateId },
                {nameof(postamateName),postamateName }
           };
            var description = $"Город: {cityName},\n Постамат: {postamateName}";
            return new OrderDelivery(Name, description,150m, parameters);
            
        }
    }
}

