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
        public IReadOnlyCollection<BookModel> GetAllByQuery(string query) {
            var books = Book.IsIsbn(query)
                  ? bookRepository.GetAllByIsbn(query)
                  : bookRepository.GetAllByTitleOrAuthor(query);

            return books.Select(Map)
                .ToArray();
        }

        public BookModel GetById(int id)
        {
            var book=bookRepository.GetById(id);
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
