using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Store.Web.App
{
    public class BookService
    {
        private readonly IBookRepository bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }
        

        public async Task<IReadOnlyCollection<BookModel>> GetAllByQueryAsync(string query)
        {
            var books = Book.IsIsbn(query)
                  ? await bookRepository.GetAllByIsbnAsync(query)
                  : await bookRepository.GetAllByTitleOrAuthorAsync(query);

            return books.Select(Map)
                .ToArray();
        }

    
        public async Task<BookModel> GetByIdAsync(int id)
        {
            var book = await bookRepository.GetByIdAsync(id);
            return Map(book);
        }

        private BookModel Map(Book book)
        {
            return new BookModel
            {
                Author = book.Author,
                Title = book.Title,
                Description = book.Description,
                Id = book.Id,
                Isbn = book.Isbn,
                Price = book.Price
            };
        }
    }
}
