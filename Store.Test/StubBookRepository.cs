using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Tests
{
    internal class StubBookRepository : IBookRepository
    {
        public Book[] ResultOfGetAllByIsbn { get; set; } 
        public Book[] ResultOfGetAllByTitleOrAuthor { get; set; }
        public Book[] GetAllByIsbn(string isbn)
        {
            return ResultOfGetAllByIsbn;
        }

        public Book[] GetAllByTitleOrAuthor(string titleOrAuthor)
        {
            return ResultOfGetAllByTitleOrAuthor;
        }

        public Book GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
