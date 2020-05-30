using Library.Data;
using Library.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Business
{
    public class ClientBusiness
    {
        private LibraryContext libraryContext;
        private BookBusiness bookBusiness;
        private ContextGenerator generator;

        public ClientBusiness(LibraryContext context)
        {
            libraryContext = context;
            bookBusiness = new BookBusiness(context);
            generator = new ContextGenerator(context, true);
        }
        public ClientBusiness()
        {
            bookBusiness = new BookBusiness();
            generator = new ContextGenerator(libraryContext, false);
        }

        /// <summary>
        /// Returns a client by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Client Get(int id)
        {
            using (libraryContext = generator.Generate())
            {
                return libraryContext.Clients.Find(id);
            }
        }
        /// <summary>
        /// Deletes a client by id.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            using (libraryContext = generator.Generate())
            {
                var client = libraryContext.Clients.Find(id);
                if (client != null)
                {
                    libraryContext.Clients.Remove(client); 
                    libraryContext.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Adds a client to the database.
        /// </summary>
        /// <param name="client"></param>
        public void Add(Client client)
        {
            using (libraryContext = generator.Generate())
            {
                libraryContext.Clients.Add(client);
                libraryContext.SaveChanges();
            }
        }
        /// <summary>
        /// Client borrows books from the library.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="books"></param>
        public void BorrowBooks(int clientId, string[] books) {

            var client = this.Get(clientId);
            using (libraryContext = generator.Generate())
            {
                foreach (string bookName in books)
                {
                    var book = bookBusiness.Get(bookName);
                    if (book.IsAvailable == true)
                    {
                        book.ClientId = clientId;
                        book.IsAvailable = false;
                        book.DateOfBorrow = DateTime.Today;
                        book.DateOfReturn = ((DateTime)(book.DateOfBorrow)).AddMonths(2); // 2 month return time ; Converting to Datetime because Datatime? doesn't have a definition of .AddMonths()
                        bookBusiness.Update(book, null);
                    }
                    else
                        throw new Exception("Book is unavailable right now!"); // Throws an exception if someone has already borrowed the book
                }
            }
        }
        /// <summary>
        /// Client returns the books to the library.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="books"></param>
        public bool ReturnBooks(int clientId, string[] books) {
            var client = this.Get(clientId);
            using (libraryContext = generator.Generate())
            {
                foreach (string bookName in books)
                {
                    var book = bookBusiness.Get(bookName);
                    if (book.DateOfReturn < DateTime.Today)
                    {
                        client.Strikes++;
                        this.Update(client);
                    }
                    if (book.ClientId == clientId)
                    {
                        book.ClientId = null;
                        book.IsAvailable = true;
                        book.DateOfBorrow = default;
                        book.DateOfReturn = default;
                        bookBusiness.Update(book, null);
                    }
                    else
                        throw new Exception("Book is not borrowed by this client!"); 
                }
                if (client.Strikes > 3) //Deletes the client at more than 3 strikes
                {
                    this.Delete(clientId);
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Updates the information about the client.
        /// </summary>
        /// <param name="client"></param>
        public void Update(Client client)
        {
            using (libraryContext = generator.Generate())
            {
                var clientOld = libraryContext.Clients.Find(client.Id);
                if (clientOld != null)
                {
                    libraryContext.Entry(clientOld).CurrentValues.SetValues(client);
                    libraryContext.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Returns a list of all books borrowed by the client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public List<Book> GetBorrowedBooks(int clientId) {
            using (libraryContext = generator.Generate())
            {
                return libraryContext.Books.Where(book => book.ClientId == clientId).ToList();
            }
        }
        /// <summary>
        /// Returns the book with the earliest return date from the ones the client has borrowed.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Book EarliestReturnDate(int clientId) //Maybe not that useful.
        {
            List<Book> borrowedBooks = this.GetBorrowedBooks(clientId);
            using (libraryContext = generator.Generate())
            {
                Book bookWithEarliestReturnDate = new Book();
                bookWithEarliestReturnDate.DateOfReturn = DateTime.MaxValue;
                foreach(Book book in borrowedBooks)
                {
                    if (book.DateOfReturn < bookWithEarliestReturnDate.DateOfReturn)
                        bookWithEarliestReturnDate = book;
                }
                return bookWithEarliestReturnDate;
            }
        }
        /// <summary>
        /// Returns all clients.
        /// </summary>
        /// <returns></returns>
        public List<Client> GetAll()
        {
            using (libraryContext = generator.Generate())
            {
                return libraryContext.Clients.ToList();
            }
        }


    }
}
