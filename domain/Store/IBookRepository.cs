using System;
using System.Collections.Generic;
using System.Text;

namespace Store
{
    public interface IBookRepository
    {
      
        Book[] GetAllByTitleOrAuthor(string titleOrAuthor);
        Book[] GetAllByIsbn(string isbn);
        Book[] GetAllByIds(IEnumerable<int> ids);
        Book GetById(int id);
    }
}
