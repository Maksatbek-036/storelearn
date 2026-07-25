using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Store
{
    public class Book
    {
        public int Id { get; }
        public string Title { get;  }
        public string Isbn { get;  }

        public string Author { get; }
        public Book(int id,string isbn,string author, string title)
        {
            Id = id;
            Title = title;
            Isbn = isbn;
            Author = author;
        }
        internal static bool IsIsbn(string s)
        {
           if(s==null) return false;
           
           s = s.Replace("-", "")
                .Replace(" ", "")
                .ToUpper();

            return Regex.IsMatch(s, @"^ISBN\d{10}(\d{3})?$");

        }
    }
}
