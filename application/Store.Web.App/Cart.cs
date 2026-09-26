namespace Store.Web.App
{
    public class Cart
    {
        public int OrderId { get; }
        public int TotalCount { get; set; }
        public decimal TotalPrice { get; set; }

        public Cart(int orderId,int totalCount,decimal totalPrice)
        {
            OrderId= orderId;
            TotalCount= totalCount;
            TotalPrice = totalPrice;
        }

    }
}
