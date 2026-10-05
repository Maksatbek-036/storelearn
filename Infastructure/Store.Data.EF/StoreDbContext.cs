using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;


namespace Store.Data.EF
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {

        }
        public DbSet<BookDto> Books { get; set; }
        public DbSet<OrderDto> Orders { get; set; }
        public DbSet<OrderItemDto> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildBooks(modelBuilder);
            BuildOrders(modelBuilder);
            BuildOrderItems(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void BuildOrderItems(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItemDto>(action =>
            {
                action.Property(dto => dto.Price).HasColumnType("money");

                action.HasOne(dto => dto.Order)
                .WithMany(dto => dto.Items)
                .IsRequired();
            }

                );
        }

        private void BuildOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDto>(action =>
            {
                action.Property(dto => dto.Id).IsRequired();
                action.Property(dto => dto.DeliveryServiceName).HasMaxLength(40);

                action.Property(dto => dto.DeliveryPrice).HasColumnType("money");
                action.Property(dto => dto.PaymentServiceName).HasMaxLength(40);

                action.Property(dto => dto.PaymentParameters).
                    HasConversion(
                    value => JsonConvert.SerializeObject(value),
                    value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
                    .Metadata.SetValueComparer(DictionaryComparer);

                action.Property(dto => dto.DeliveryParameters).
                HasConversion(
                value => JsonConvert.SerializeObject(value),
                value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
                .Metadata.SetValueComparer(DictionaryComparer);
            });


        }

        private static readonly ValueComparer DictionaryComparer =
            new ValueComparer<Dictionary<string, string>>
            (
                (dictionary1, dictionary2) => dictionary1.SequenceEqual(dictionary2),

                dictionary => dictionary.Aggregate(
                        0,
                    (a, p) => HashCode.Combine(HashCode
                                                .Combine(a, p.Key.GetHashCode(), p.Value.GetHashCode()))
                          )
            );




        private static void BuildBooks(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookDto>(action =>
            {
                action.Property(dto => dto.Id)
                .HasMaxLength(17)
                .IsRequired();

                action.Property(dto => dto.Title)
                .IsRequired();

                action.Property(dto => dto.Price).HasColumnType("money");

                action.HasData(
                   new BookDto
                   {
                       Id = 1,
                       Title = "Clean Code",
                       Isbn = "978-0132350884",
                       Author = "Robert C. Martin",
                       Description = "A handbook of agile software craftsmanship.",
                       Price = 45.99m
                   },
    new BookDto
    {
        Id = 2,
        Title = "The Pragmatic Programmer",
        Isbn = "978-0201616224",
        Author = "Andrew Hunt, David Thomas",
        Description = "Your journey to mastery in software development.",
        Price = 52.50m
    },
    new BookDto
    {
        Id = 3,
        Title = "Design Patterns",
        Isbn = "978-0201633610",
        Author = "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides",
        Description = "Elements of Reusable Object-Oriented Software.",
        Price = 60.00m
    },
    new BookDto
    {
        Id = 4,
        Title = "Head First Java",
        Isbn = "978-0596009205",
        Author = "Kathy Sierra, Bert Bates",
        Description = "Learning a complex subject made easier.",
        Price = 39.99m
    },
    new BookDto
    {
        Id = 5,
        Title = "Introduction to Algorithms",
        Isbn = "978-0262033848",
        Author = "Thomas H. Cormen, Charles E. Leiserson, Ronald L. Rivest, Clifford Stein",
        Description = "Comprehensive guide to algorithms and data structures.",
        Price = 89.50m
    }
                    );
            });




        }
    }
}
