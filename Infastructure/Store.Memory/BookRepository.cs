namespace Store.Memory
{
    public class BookRepository : IBookRepository
    {
        public readonly Book[] books = new Book[] { 
        new Book(1, "The Great Gatsby"),
        new Book(2, "To Kill a Mockingbird"),
        new Book(3, "1984"),
        new Book(4, "Art of programming"),
        };

        public Book[] GetAllByTitle(string titlePartial)
        {
            return books.Where(book => book.Title.Contains(titlePartial))
                .ToArray();
        }
    }
}
