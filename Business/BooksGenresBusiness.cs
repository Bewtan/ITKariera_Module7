using Library.Data;
using Library.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Business
{
    class BooksGenresBusiness
    {
        private LibraryContext libraryContext;

        /// <summary>
        /// Returns the books associated with a given genre.
        /// </summary>
        /// <param name="genreName"></param>
        /// <returns></returns>
        public List<Book> GetBooks(string genreName)
        {
            using (libraryContext = new LibraryContext())
            {
                GenreBusiness genreBusiness = new GenreBusiness();
                Genre genre = genreBusiness.Get(genreName);
                return libraryContext.BooksGenres.Where(booksgenre => booksgenre.Genre == genre).Select(booksgenre => booksgenre.Book).ToList();
            }
        }
        /// <summary>
        /// Returns the genres associated with a given book.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public List<Genre> GetGenres(string title)
        {
            using (libraryContext = new LibraryContext())
            {
                BookBusiness bookBusiness = new BookBusiness();
                Book book = bookBusiness.Get(title);
                return libraryContext.BooksGenres.Where(booksgenre => booksgenre.Book == book).Select(booksgenre => booksgenre.Genre).ToList();
            }
        }
        /// <summary>
        /// Adds a new book-genre relation.
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="genreId"></param>
        public void Add(int bookId, int genreId)
        {
            using (libraryContext = new LibraryContext())
            {
                BooksGenres booksGenre = new BooksGenres();
                booksGenre.BookId = bookId;
                booksGenre.GenreId = genreId;
                libraryContext.BooksGenres.Add(booksGenre);
                libraryContext.SaveChanges();
            }
        }
    }
}
