using Library.Data;
using Library.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Business
{
    class BookBusiness
    {
        private GenreBusiness genreBusiness = new GenreBusiness();
        private PublisherBusiness publisherBusiness = new PublisherBusiness();
        BooksGenresBusiness booksGenresBusiness = new BooksGenresBusiness();

        private LibraryContext libraryContext;

        /// <summary>
        /// Returns Book with corresponding title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Book Get(string title)
        {
            using (libraryContext = new LibraryContext())
            {
                return libraryContext.Books.SingleOrDefault(book => book.Title == title);
            }
        }
        /// <summary>
        /// Returns the genres of a book.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public List<Genre> GetGenres(string title)
        {
            using (libraryContext = new LibraryContext())
            {
                return booksGenresBusiness.GetGenres(title);
            }
        }

        /// <summary>
        /// Adds a book to the database.
        /// </summary>
        /// <param name="book"></param>
        /// <param name="genres"></param>
        public void Add(Book book, string[] genres)
        {
            using (libraryContext = new LibraryContext())
            {
                libraryContext.Books.Add(book);
                this.AddGenres(book, genres);
                libraryContext.SaveChanges();
            }
        }
        /// <summary>
        /// Checks if book is available. Returns true or false.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool IsAvailable(string title) //Maybe this needs a method in just the display, because Get already exists.
        {
            using (libraryContext = new LibraryContext())
            {
                return libraryContext.Books.SingleOrDefault(book => book.Title == title).IsAvailable;
            }
        }
        /// <summary>
        /// Returns the Client who is borrowing the book with corresponding title. If book is available returns null.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Client ReturnClient(string title)
        {
            using (libraryContext = new LibraryContext())
            {
                return libraryContext.Books.SingleOrDefault(book => book.Title == title).Client;
            }
        }
        /// <summary>
        /// Returns an array of books with the corresponding author.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Book> SearchByAuthor(string name)
        {
            using (libraryContext = new LibraryContext())
            {
                return libraryContext.Books.Where(book => book.AuthorName == name).ToList();
            }
        }
        /// <summary>
        /// Returns a book's return date.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public DateTime GetReturnDate(string title)
        {
            using (libraryContext = new LibraryContext())
            {
                return libraryContext.Books.SingleOrDefault(book => book.Title == title).DateOfReturn;
            }
        }
        /// <summary>
        /// Returns an array of books in the corresponding genre.
        /// </summary>
        /// <param name="genreName"></param>
        /// <returns></returns>
        public List<Book> SearchByGenre(string genreName)
        {
            using (libraryContext = new LibraryContext())
            {
                return booksGenresBusiness.GetBooks(genreName);
            }
        }

        /// <summary>
        /// Returns an array of books with the corresponding publisher.
        /// </summary>
        /// <param name="publisherName"></param>
        /// <returns></returns>
        public List<Book> SearchByPublisher(string publisherName)
        {
            using (libraryContext = new LibraryContext())
            {
                Publisher BookPublisher = publisherBusiness.Get(publisherName); 
                return libraryContext.Books.Where(book => book.Publisher == BookPublisher).ToList();
            }
        }
        /// <summary>
        /// Returns an array of books in the corresponding language.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public List<Book> SearchByLanguage(string language)
        {
            using (libraryContext = new LibraryContext())
            {
                return libraryContext.Books.Where(book => book.Language == language).ToList();
            }
        }
        /// <summary>
        /// Returns an array of books published before or after input date.
        /// </summary>
        /// <param name="dateOfPublishing"></param>
        /// <param name="beforeOrAfter"></param>
        /// <returns></returns>
        public List<Book> SearchByDateOfPublishing(DateTime dateOfPublishing,string beforeOrAfter)
        {
            using (libraryContext = new LibraryContext())
            {
                if (beforeOrAfter == "before")
                    return libraryContext.Books.Where(book => book.DateOfPublishing < dateOfPublishing).ToList();
                else if (beforeOrAfter == "after")
                    return libraryContext.Books.Where(book => book.DateOfPublishing > dateOfPublishing).ToList();
                else
                    return null;
            }
        }
        /// <summary>
        /// Deletes a book by Id.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id) // Unsure if the delete should be by id or title.
        {
            using (libraryContext = new LibraryContext())
            {
                var book = libraryContext.Books.Find(id);
                if (book != null)
                {
                    libraryContext.Books.Remove(book); //I think this cascades so I probably don't need to make a booksgenres delete manually.
                    libraryContext.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Updates the information about a book and its genres.
        /// </summary>
        /// <param name="bookInput"></param>
        /// <param name="genres"></param>
        public void Update(Book bookInput, string[] genres)
        {
            using (libraryContext = new LibraryContext())
            {
                var bookOld = libraryContext.Books.Find(bookInput.Id);
                if (bookOld != null)
                {
                    libraryContext.Entry(bookOld).CurrentValues.SetValues(bookInput);

                    var BooksGenres = libraryContext.BooksGenres.Where(booksgenre => booksgenre.BookId == bookOld.Id);
                    libraryContext.BooksGenres.RemoveRange(BooksGenres); //Removes old genres
                    AddGenres(bookOld, genres);
                    libraryContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Updates the information about a book.
        /// </summary>
        /// <param name="bookInput"></param>
        public void Update(Book bookInput)
        {
            using (libraryContext = new LibraryContext())
            {
                var bookOld = libraryContext.Books.Find(bookInput.Id);
                if (bookOld != null)
                {
                    libraryContext.Entry(bookOld).CurrentValues.SetValues(bookInput);
                    libraryContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Adds genres to book.
        /// </summary>
        /// <param name="book"></param>
        /// <param name="genres"></param>
        private void AddGenres(Book book, string[] genres) // Can potentially make this public if we wanna support such functionality.
        {
            foreach (string genre in genres)
            {
                booksGenresBusiness.Add(book.Id, genreBusiness.Get(genre).Id);
            }
        }

        /// <summary>
        /// Returns all books.
        /// </summary>
        /// <returns></returns>
        public List<Book> GetAll() //I'm not sure if we would need this, but I added it anyway.
        {
            using (libraryContext = new LibraryContext())
            {
                return libraryContext.Books.ToList();
            }
        }

    }
}
