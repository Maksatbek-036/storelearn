
using Microsoft.AspNetCore.Http;
using Store.Web.App;

using System.Data;

namespace Store.Web.App
{
    public static class SessionExtensions
    {
        private const string key = "Cart";
        public static void RemoveCart(this ISession session)
        {
            session.Remove(key);
        }
        public static void Set(this ISession session, Cart cart)
        {
            if (cart == null) {
                return; }
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, true))
            {
                writer.Write(cart.OrderId);
                writer.Write(cart.TotalCount);
                writer.Write(cart.TotalPrice);

              
                session.Set(key, stream.ToArray());
            }


        }
        public static bool TryGetCart(this ISession session, out Cart cart)
        {
            if (session.TryGetValue(key, out byte[] buffer))
            {
                using (var stream = new MemoryStream(buffer))
                using (var reader = new BinaryReader(stream, System.Text.Encoding.UTF8, true))
                {
                    var orderId=reader.ReadInt32();
                    var totalCount = reader.ReadInt32();
                    var totalPrice = reader.ReadDecimal();

                    cart = new Cart(orderId,totalCount,totalPrice)
                    {
                        TotalCount = totalCount,
                        TotalPrice = totalPrice
                    };
                   
                    return true;

                }
            }
            cart = null;
            return false;
        }

    }
}
