﻿
#region imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion
namespace Books.ConsoleApp
{
    class Program
    {
        private static IBooksSource BooksSource = new BooksJsonSource();

        // we will use this as global state
        private static List<BooksByAuthor> BooksByAuthorCatalog;

        public static void Main()
        {
            IEnumerable<Book> books = BooksSource.Read();

            BooksByAuthorCatalog = new List<BooksByAuthor>();

            foreach(var book in books)
            {
                if (AuthorIsAlreadyCataloged(book.author))
                {
                    // there are some(1 or more) books by this author already found and catalogued
                    var authorAndBooks = LocateAuthorAlreadyCataloged(book.author);
                    authorAndBooks.Books.Add(book);
                }
                else
                {
                    CatalogueNewAuthor(book);
                }
            }

            // now we have an list that has all the authors catalogued
            OutputBooksByAuthor();

            Console.WriteLine("Finished cataloguing authors. (press a key to exit...)");
            Console.ReadLine();
        }

        private static bool AuthorIsAlreadyCataloged(string author)
        {
            return BooksByAuthorCatalog.Any(ba => ba.Author == author);
        }

        private static BooksByAuthor LocateAuthorAlreadyCataloged(string author)
        {
            return BooksByAuthorCatalog.First(ba => ba.Author == author);
        }

        private static void CatalogueNewAuthor(Book b)
        {
            // there are NONE books by this author already found and cataloged

            var newBooksList = new List<Book> { b };
            var authorAndBooks = new BooksByAuthor(b.author, newBooksList);

            BooksByAuthorCatalog.Add(authorAndBooks);
        }


        private static void OutputBooksByAuthor()
        {
            foreach(var ba in BooksByAuthorCatalog)
            {
                Console.Write("Author: {0,-28} Books: ", ba.Author);
                foreach (var book in ba.Books)
                {
                    Console.Write(book.title + ", ");
                }
                Console.Write(Environment.NewLine);
            }
        }
    }

    public class BooksByAuthor
    {
        public readonly string Author;
        public readonly List<Book> Books;

        public BooksByAuthor(string author, List<Book> books)
        {
            Author = author;
            Books = books;
        }
    }
}
