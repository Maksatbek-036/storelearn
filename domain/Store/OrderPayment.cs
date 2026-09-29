namespace Store
{
    public class OrderPayment
    {
        public OrderPayment(string serviceName, string description, IReadOnlyDictionary<string, string> parametres)
        {
            ServiceName = serviceName;
            Description = description;
            Parametres = parametres;
            if (string.IsNullOrWhiteSpace(serviceName)) throw new ArgumentException(nameof(serviceName));
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException(nameof(description));

            if (parametres == null) throw new ArgumentNullException(nameof(parametres));
        }

        public string ServiceName { get; }
        public string Description { get; }
        public IReadOnlyDictionary<string, string> Parametres { get; }
    }
}