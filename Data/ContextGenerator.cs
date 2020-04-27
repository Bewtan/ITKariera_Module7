using Library.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Data
{
    public class ContextGenerator // I needed to do this because this is the only solution I thought in order to implent the unit tests together with using statements
    {
        private DbSet<Book> books;
        private DbSet<Client> clients;
        private DbSet<Publisher> publishers;
        private DbSet<BooksGenres> booksGenres;
        private DbSet<Genre> genres;
        private bool IsMockContext;
        public ContextGenerator(LibraryContext context)
        {
            books = context.Books;
            clients = context.Clients;
            publishers = context.Publishers;
            booksGenres = context.BooksGenres;
            genres = context.Genres;
            LibraryContext testContext = new LibraryContext();
            if(books.Count() != testContext.Books.Count())
            {
                IsMockContext = true;
            }
            else
                IsMockContext = false;
        }

        /// <summary>
        /// Generates a new library context with the given DBSets.
        /// </summary>
        /// <returns></returns>
        public LibraryContext Generate() 
        {
            LibraryContext libraryContext = new LibraryContext();

            if (IsMockContext)
            {
                libraryContext.Books = books;
                libraryContext.Clients = clients;
                libraryContext.Publishers = publishers;
                libraryContext.BooksGenres = booksGenres;
                libraryContext.Genres = genres;
            }

            return libraryContext;
        }
    }
}
