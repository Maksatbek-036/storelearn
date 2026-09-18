namespace Store
{
    public class OrderDelivery
    {
        public OrderDelivery(string uniqueCode,
            string description,
            decimal amount,
            IReadOnlyDictionary<string, string> parametres
            )
        {
            UniqueCode = uniqueCode;
            Description = description;
            Parametres = parametres;

            if (string.IsNullOrWhiteSpace(uniqueCode)) throw new ArgumentException(nameof(uniqueCode));
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException(nameof(description));

            if (parametres == null) throw new ArgumentNullException(nameof(parametres));
            Amount = amount;
        }

        public string UniqueCode { get; }
        public string Description { get; }
        public IReadOnlyDictionary<string, string> Parametres { get; }
        public decimal Amount { get; }


    }
}