namespace Store
{
    public class OrderPayment
    {
        public OrderPayment(string uniqueCode, string description, IReadOnlyDictionary<string, string> parametres)
        {
            UniqueCode = uniqueCode;
            Description = description;
            Parametres = parametres;
            if (string.IsNullOrWhiteSpace(uniqueCode)) throw new ArgumentException(nameof(uniqueCode));
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException(nameof(description));

            if (parametres == null) throw new ArgumentNullException(nameof(parametres));
        }

        public string UniqueCode { get; }
        public string Description { get; }
        public IReadOnlyDictionary<string, string> Parametres { get; }
    }
}