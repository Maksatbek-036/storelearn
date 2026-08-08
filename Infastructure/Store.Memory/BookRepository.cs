namespace Store.Memory
{
    public class BookRepository : IBookRepository
    {
        public readonly Book[] books = new Book[] {
       new Book(1, "ISBN 978-3-16-148410-0",  "F. Scott Fitzgerald","The Great Gatsby", "Lorem ipsum dolor sit amet", 9.99m),
      new Book(2, "ISBN 978-0-14-118263-6", "Harper Lee", "To Kill a Mockingbird", "Lorem ipsum dolor sit amet", 14.99m),
      new Book(3, "ISBN 978-0-452-28423-4", "George Orwell", "1984", "Lorem ipsum dolor sit amet", 19.99m),
      new Book(4, "ISBN 978-0-7432-7356-5", "J.D. Salinger", "The Catcher in the Rye", "Lorem ipsum dolor sit amet", 24.99m)
        };

        public Book[] GetAllByIds(IEnumerable<int> bookIds)
        {
            var foundBooks=from book in books
                           join bookId in bookIds on book.Id equals bookId
                           select book;
            return foundBooks.ToArray();
        }

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

        public Book GetById(int id)
        {
            return books.Single(book=>book.Id == id);
        }
    }
}
