using Library.Data;
using Library.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Business
{
    public class BooksGenresBusiness
    {
        private LibraryContext libraryContext;
        public BooksGenresBusiness(LibraryContext context)
        {
            libraryContext = context;
        }
        public BooksGenresBusiness()
        {
            libraryContext = new LibraryContext();
        }

        /// <summary>
        /// Returns the books associated with a given genre.
        /// </summary>
        /// <param name="genreName"></param>
        /// <returns></returns>
        public List<Book> GetBooks(string genreName)
        {
            using (libraryContext)
            {
                Genre genre = libraryContext.Genres.SingleOrDefault(genre => genre.Name == genreName);
                List<int> bookId = libraryContext.BooksGenres.Where(booksgenre => booksgenre.Genre == genre).Select(booksgenre => booksgenre.Book.Id).ToList();
                return libraryContext.Books.Where(book => bookId.Contains(book.Id)).ToList();
            }
        }
        /// <summary>
        /// Returns the genres associated with a given book.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public List<Genre> GetGenres(string title)
        {
            using (libraryContext)
            {
                Book book = libraryContext.Books.SingleOrDefault(book => book.Title == title);
                List<int> genreId = libraryContext.BooksGenres.Where(booksgenre => booksgenre.Book == book).Select(booksgenre => booksgenre.GenreId).ToList();
                return libraryContext.Genres.Where(genre => genreId.Contains(genre.Id)).ToList();
            }
        }
        /// <summary>
        /// Adds a new book-genre relation.
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="genreId"></param>
        public void Add(int bookId, int genreId)
        {
            using (libraryContext)
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
