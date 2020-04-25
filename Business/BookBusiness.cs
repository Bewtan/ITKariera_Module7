using Library.Data;
using Library.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Business
{
    public class BookBusiness
    {
        private BooksGenresBusiness booksGenresBusiness;
        private LibraryContext libraryContext = new LibraryContext();
        private IQueryable<Book> books;
        private IQueryable<Client> clients;
        private IQueryable<Publisher> publishers;
        private IQueryable<BooksGenres> booksGenres;
        private IQueryable<Genre> genres;


        public BookBusiness(LibraryContext context)
        {
            libraryContext = context;
            books = context.Books;
            clients = context.Clients;
            publishers = context.Publishers;
            booksGenres = context.BooksGenres;
            genres = context.Genres;

            booksGenresBusiness = new BooksGenresBusiness(context);
        }
        public BookBusiness()
        {
            libraryContext = new LibraryContext();
            books = libraryContext.Books;
            clients = libraryContext.Clients;
            publishers = libraryContext.Publishers;
            booksGenres = libraryContext.BooksGenres;
            genres = libraryContext.Genres;
            booksGenresBusiness = new BooksGenresBusiness();
        }
        /// <summary>
        /// Returns Book with corresponding title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Book Get(string title)
        {
            using (libraryContext = ContextGenerator())
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
            using (libraryContext = ContextGenerator())
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
            using (libraryContext = ContextGenerator())
            {
                libraryContext.Books.Add(book);
                if(genres != null)
                    this.AddGenres(book, genres);
                libraryContext.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a book by Id.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id) // Unsure if the delete should be by id or title.
        {
            using (libraryContext = ContextGenerator())
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
            using (libraryContext = ContextGenerator())
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
            
            using (libraryContext = ContextGenerator())
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
            using (libraryContext = ContextGenerator())
            {
                Genre genre;
                foreach (string genreName in genres)
                {
                    genre = libraryContext.Genres.SingleOrDefault(g => g.Name == genreName);
                    booksGenresBusiness.Add(book.Id, genre.Id);
                }
            }
        }

        /// <summary>
        /// Returns all books.
        /// </summary>
        /// <returns></returns>
        public List<Book> GetAll() //I'm not sure if we would need this, but I added it anyway.
        {
            using (libraryContext= ContextGenerator())
            {
                return libraryContext.Books.ToList();
            }
        }


        /// <summary>
        /// Checks if book is available. Returns true or false.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool IsAvailable(string title) //This may be rather obsolete, because Get() exists
        {
            using (libraryContext = ContextGenerator())
            {
                return libraryContext.Books.SingleOrDefault(book => book.Title == title).IsAvailable;
            }
        }

        /// <summary>
        /// Returns the Client who is borrowing the book with corresponding title. If book is available returns null.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Client GetClient(string title) 
        {
            using (libraryContext = ContextGenerator())
            {
                var Book = this.Get(title);
                return libraryContext.Clients.Find(Book.ClientId);
            }
        }
        /// <summary>
        /// Returns the Publisher of the corresponding book. If unknown returns null.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Publisher GetPublisher(string title)
        {
            using (libraryContext= ContextGenerator())
            {
                var Book = this.Get(title);
                return libraryContext.Publishers.Find(Book.PublisherId);
            }
        }

        /// <summary>
        /// Returns a book's return date.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public DateTime? GetReturnDate(string title)  //This may be rather obsolete, because Get() exists
        {
            using (libraryContext = ContextGenerator())
            {
                return libraryContext.Books.SingleOrDefault(book => book.Title == title).DateOfReturn;
            }
        }

        /// <summary>
        /// Returns an array of books with the corresponding author.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Book> SearchByAuthor(string name)
        {
            using (libraryContext = ContextGenerator())
            {
                return libraryContext.Books.Where(book => book.AuthorName == name).ToList();
            }
        }

        /// <summary>
        /// Returns an array of books in the corresponding genre.
        /// </summary>
        /// <param name="genreName"></param>
        /// <returns></returns>
        public List<Book> SearchByGenre(string genreName)
        {
            using (libraryContext = ContextGenerator())
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
            using (libraryContext = ContextGenerator())
            {
                Publisher BookPublisher = libraryContext.Publishers.SingleOrDefault(publisher => publisher.Name == publisherName);
                return libraryContext.Books.Where(book => book.Publisher.Id == BookPublisher.Id).ToList();
            }
        }
        /// <summary>
        /// Returns an array of books in the corresponding language.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public List<Book> SearchByLanguage(string language)
        {
            using (libraryContext = ContextGenerator())
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
            using (libraryContext = ContextGenerator())
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
        /// Generates a new library context.
        /// </summary>
        /// <returns></returns>
        private LibraryContext ContextGenerator() // I needed to do this because this is the only solution I thought in order to implent the unit tests together with using statements
        {
            libraryContext = new LibraryContext();
            libraryContext.Books = (DbSet<Book>)books;
            libraryContext.Clients = (DbSet<Client>)clients;
            libraryContext.Publishers = (DbSet<Publisher>)publishers;
            libraryContext.BooksGenres = (DbSet<BooksGenres>)booksGenres;
            libraryContext.Genres = (DbSet<Genre>)genres;

            return libraryContext;
        }
    }
}
