using System;
using System.Collections.Generic;
using System.Text;
using Moq;

namespace Store.Tests
{
    public class BookServiceTest
    {
        //[Fact] Реализация теста с использованим Moq
        //public void GetAllByQuery_WithIsbn_CallGetByIsbn()
        //{

        //  var bookRepository = new Mock<IBookRepository>();
        //    bookRepository.Setup(x => x.GetAllByIsbn(It.IsAny<string>()))
        //        .Returns(new Book[] { new Book(1,"","","")});

        //    bookRepository.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>()))
        //        .Returns(new Book[] { new Book(2, "", "", "") });

        //    var invalidIsbn = "ISBN 978-3-16-148410-0";
        //    var bookService = new BookService(bookRepository.Object);
        //    var actual = bookService.GetAllByQuery(invalidIsbn);

        //    Assert.Collection(actual, book=>Assert.Equal(1,book.Id));


        //}

        [Fact]
        public void GetAllByQuery_WithIsbn_CallGetByTitleOrAuthor()
        {

            var bookRepository = new Mock<IBookRepository>();
            bookRepository.Setup(x => x.GetAllByIsbn(It.IsAny<string>()))
                .Returns(new Book[] { new Book(1, "", "", "") });

            bookRepository.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>()))
                .Returns(new Book[] { new Book(2, "", "", "") });
            var invalidIsbn = "978-3-16-148410-0";
            var bookService = new BookService(bookRepository.Object);
            var actual = bookService.GetAllByQuery(invalidIsbn);

            Assert.Collection(actual, book => Assert.Equal(2, book.Id));


        }

        [Fact]
        public void GetAllByQuery_WithIsbn_CallGetByIsbn()
        {
            const int idOfIsbnSearch = 1;
            const int idOfTitleOrAuthorSearch = 2;

            var bookRepository = new StubBookRepository();
            bookRepository.ResultOfGetAllByIsbn = new Book[] 
            { 
                new Book(idOfIsbnSearch, "", "", "") 
            };

            bookRepository.ResultOfGetAllByTitleOrAuthor = new Book[] 
            {
                new Book(idOfTitleOrAuthorSearch, "", "", "") 
            };
            var bookService = new BookService(bookRepository);

            var books = bookService.GetAllByQuery("ISBn 978-3-16-148410-0");

            Assert.Collection(books, book => Assert.Equal(idOfIsbnSearch, book.Id));

        }
        [Fact]
        public void GetAllByQuery_WithAuthor_CallGetByTitleOrAuthor()
        {
            const int idOfIsbnSearch = 1;
            const int idOfTitleOrAuthorSearch = 2;

            var bookRepository = new StubBookRepository();
            bookRepository.ResultOfGetAllByIsbn = new Book[]
            {
                new Book(idOfIsbnSearch, "", "", "")
            };

            bookRepository.ResultOfGetAllByTitleOrAuthor = new Book[]
            {
                new Book(idOfTitleOrAuthorSearch, "", "", "")
            };
            var bookService = new BookService(bookRepository);

            var books = bookService.GetAllByQuery("Art");

            Assert.Collection(books, book => Assert.Equal(idOfTitleOrAuthorSearch, book.Id));

        }
    }
}
