using Store.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Store
{
    public class Book
    {
        private readonly BookDto dto;
        public int Id
        {
            get { return dto.Id; }
            set { dto.Id = value; }
        }
        public string Title
        {
            get { return dto.Title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(nameof(value));
                }
                dto.Title = value;
            }
        }
        public string Isbn
        {
            get { return dto.Isbn; }
            set { dto.Isbn = value; }

        }
        public string Author
        {
            get { return dto.Author; }
            set { dto.Author = value; }
        }
        public string Description
        {
            get { return dto.Description; }
            set { dto.Description = value; }
        }
        public decimal Price
        {
            get { return dto.Price; }
            set { dto.Price = value; }
        }
        internal Book(BookDto dto)
        {
            this.dto = dto;

        }
        public static bool TryFormatIsbn(string isbn, out string formattedIsbn)
        {
            if (isbn == null)
            {
                formattedIsbn = null;
                return false;
            }

            formattedIsbn = isbn.Replace("-", "")
                 .Replace(" ", "")
                 .ToUpper();

            return Regex.IsMatch(formattedIsbn, @"^ISBN\d{10}(\d{3})?$");

        }
        public static bool IsIsbn(string s)
        {
            return TryFormatIsbn(s, out _);
        }
        public static class DtoFactory
        {
            public static BookDto Create(string isbn, string author, string title, string description, decimal price)
            {
                if(TryFormatIsbn(isbn, out string formattedIsbn))
                {
                    isbn = formattedIsbn;
                }
                else
                {
                    throw new ArgumentException(nameof(isbn));
                }
                if (string.IsNullOrWhiteSpace(title))
                {
                    throw new ArgumentException(nameof(title));
                }
                return new BookDto
                {
                    Isbn = isbn,
                    Author = author?.Trim(),
                    Title = title?.Trim(),
                    Description = description?.Trim(),
                    Price = price

                };
            }
        }
        public static class Mapper
        {
            public static Book Map(BookDto dto) => new Book(dto);
            public static BookDto Map(Book domain) => domain.dto;
        }
    }
}
