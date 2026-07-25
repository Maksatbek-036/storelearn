namespace Store.Memory
{
    public class BookRepository : IBookRepository
    {
        public readonly Book[] books = new Book[] { 
        new Book(1,"ISBN 122542-1225","Dddd", "The Great Gatsby"),
       new Book(2,"ISBN 122542-1235","Ella", "The Great Gatsby 2"),
       new Book(3,"ISBN 122542-1241","Maks", "The Great Gatsby 3"),
       new Book(4,"IsBN 12-25-42-1245","dsaf", "Dream day")
        };

        public Book[] GetAllByIsbn(string isbn)
        {
            return books.Where(book => book.Isbn == isbn)
                .ToArray();
        }

        public Book[] GetAllByTitle(string titlePartial)
        {
            return books.Where(book => book.Title.Contains(titlePartial))
                .ToArray();
        }

        public Book[] GetAllByTitleOrAuthor(string titleOrAuthor)
        {
            return books.Where(book => book.Title.Contains(titleOrAuthor) 
                                    || book.Author.Contains(titleOrAuthor))
                .ToArray();
        }
    }
}
