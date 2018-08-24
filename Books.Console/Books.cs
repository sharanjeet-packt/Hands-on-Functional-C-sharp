﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Books.ConsoleApp
{
    public class BooksCollection
    {
        public Book[] Books { get; set; }
    }

    public class Book
    {
        public string[] categories;
        public string author { get; set; }
        public string country { get; set; }
        public string imageLink { get; set; }
        public string language { get; set; }
        public string link { get; set; }
        public int pages { get; set; }
        public string title { get; set; }
        public int year { get; set; }
    }

    public interface IBooksSource
    {
        Book[] Read();
    }

    public class BooksJsonSource : IBooksSource
    {
        private string booksJsonFile;

        public BooksJsonSource(string booksFile = "books.json")
        {
            booksJsonFile = booksFile;
        }

        public Book[] Read()
        {
            var rawJsonBooks = File.ReadAllTextAsync(booksJsonFile)
                // this (the .Result)is blocking the "UI"/"Main" thread - done for simplicity
                // but utterly and very and fundamentaly wrong for production apps!
                .Result;

            return JsonConvert.DeserializeObject<Book[]>(rawJsonBooks);
        }
    }
}
