using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Data.EF
{
    internal class BookRepository : IBookRepository
    {
        public Book[] GetAllByIds(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public Book[] GetAllByIsbn(string isbn)
        {
            throw new NotImplementedException();
        }

        public Book[] GetAllByTitleOrAuthor(string titleOrAuthor)
        {
            throw new NotImplementedException();
        }

        public Book GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
