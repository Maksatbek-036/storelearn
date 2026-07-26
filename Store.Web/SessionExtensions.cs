using Store.Web.Models;
using System.Data;

namespace Store.Web
{
    public static class SessionExtensions
    {
        private const string key = "Cart";
        public static void Set(this ISession session, Cart cart)
        {
            if (cart == null) { return; }
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, true))
            {

                writer.Write(cart.Items.Count);
                foreach (var item in cart.Items)
                {
                    writer.Write(item.Key);
                    writer.Write(item.Value);
                }
                writer.Write(cart.Amount);
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
                    cart = new Cart();
                    var length = reader.ReadInt32();
                    for (int i = 0; i < length; i++)
                    {
                        var bookId = reader.ReadInt32();
                        var count = reader.ReadInt32();

                        cart.Items.Add(bookId, count);
                    }
                    cart.Amount = reader.ReadDecimal();
                    return true;

                }
            }
            cart = null;
            return false;
        }
    }
}
