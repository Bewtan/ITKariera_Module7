using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Business;
using Library.Data.Models;


namespace Library.Presentation
{
    class BookDisplay
    {
        private int closeOperationId = 15;
        private BookBusiness bookBusiness;

        private void ShowMenu()
        {
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 18) + "MENU" + new string(' ', 18));
            Console.WriteLine(new string('-', 40));
            Console.WriteLine("1. List all books");
            Console.WriteLine("2. Add new book");
            Console.WriteLine("3. Update book");
            Console.WriteLine("4. Fetch book by Title");
            Console.WriteLine("5. Delete entry by Id");
            Console.WriteLine("6. Add Genres to Book");
            Console.WriteLine("7. Check if Book is Available");
            Console.WriteLine("8. Get Book Client");
            Console.WriteLine("9. Get Book Return Date");
            Console.WriteLine("10. Search Books by Author");
            Console.WriteLine("11. Search Books by Genre");
            Console.WriteLine("12. Search Books by Publisher");
            Console.WriteLine("13. Search Books by Language");
            Console.WriteLine("14. Search Books by Date of Publishing");
            Console.WriteLine("15. Exit");
        }
        private void Input()
        {
            var operation = -1;
            do
            {
                ShowMenu();
                operation = int.Parse(Console.ReadLine());
                switch (operation)
                {
                    case 1:
                        ListAll();
                        break;
                    case 2:
                        Add();
                        break;
                    case 3:
                        Update();
                        break;
                    case 4:
                        Fetch();
                        break;
                    case 5:
                        Delete();
                        break;
                    case 6:
                        AddGenres();
                        break;
                    case 7:
                        CheckIfBookIsAvailable();
                        break;
                    case 8:
                        GetBookClient();
                        break;
                    case 9:
                        GetReturnDate();
                        break;
                    case 10:
                        SearchBooksByAuthor();
                        break;
                    case 11:
                        SearchBooksByGenre();
                        break;
                    case 12:
                        SearchBooksByPublisher();
                        break;
                    case 13:
                        SearchBooksByLanguage();
                        break;
                    case 14:
                        SearchBooksByDateOfPublishing();
                        break;
                    default:
                        break;
                }
            } while (operation != closeOperationId);
        }
        public BookDisplay()
        {
            bookBusiness = new BookBusiness();
            Input();
        }
        private void Add()
        {
            Book book = new Book();
            Console.WriteLine("Enter Title: ");
            book.Title = Console.ReadLine();
            Console.WriteLine("Enter Author name: ");
            book.AuthorName = Console.ReadLine();
            Console.WriteLine("Enter Language: ");
            book.Language = Console.ReadLine();
            Console.WriteLine("Enter Publisher ID: ");
            book.PublisherId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Date of publishing: ");
            book.DateOfPublishing = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter Genres: ");
            string[] genres = Console.ReadLine().Split(" ");
            book.IsAvailable = true;
            bookBusiness.Add(book, genres);
        }
        private void ListAll()
        {
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 16) + "Books" + new string(' ', 16));
            Console.WriteLine(new string('-', 40));
            var books = bookBusiness.GetAll();
            foreach (var book in books)
            {
                var BookPublisher = bookBusiness.GetPublisher(book.Title);
                Console.WriteLine("{0} || {1} || {2} || {3} || {4} || {5}", book.Id, book.Title, book.Language, book.AuthorName,book.DateOfPublishing.Year, BookPublisher.Name);
            }
        }
        private void Update()
        {
            Console.WriteLine("Enter Title to update: ");
            string title = Console.ReadLine();
            Book book = bookBusiness.Get(title);
            if (book != null)
            {
                Console.WriteLine("Enter new Title: ");
                book.Title = Console.ReadLine();
                Console.WriteLine("Enter Author name: ");
                book.AuthorName = Console.ReadLine();
                Console.WriteLine("Enter Language: ");
                book.Language = Console.ReadLine();
                Console.WriteLine("Enter Publisher ID: ");
                book.PublisherId = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter Date of publishing: ");
                book.DateOfPublishing = DateTime.Parse(Console.ReadLine());
                Console.WriteLine("Enter Genres: ");
                string[] genres = Console.ReadLine().Split(" ");
                bookBusiness.Update(book, genres);
            }
            else
            {
                Console.WriteLine("Book not found!");
            }
        }
        private void Fetch()
        {
            Console.WriteLine("Enter Title to fetch: ");
            string title = Console.ReadLine();
            var book = bookBusiness.Get(title);
            if (book != null)
            {
                var BookPublisher = bookBusiness.GetPublisher(book.Title);
                Console.WriteLine(new string('-', 40));
                Console.WriteLine("ID: " + book.Id);
                Console.WriteLine("Title: " + book.Title);
                Console.WriteLine("AuthorName: " + book.AuthorName);
                Console.WriteLine("Language: " + book.Language);
                Console.WriteLine("Date of Publishing: " + book.DateOfPublishing.Year);
                Console.WriteLine("Publisher: " + BookPublisher.Name);
                var genres = bookBusiness.GetGenres(book.Title).Select(genre => genre.Name);
                Console.WriteLine("Genres: " + String.Join(" ",genres));
                Console.WriteLine(new string('-', 40));
            }
            else
            {
                Console.WriteLine("Book not found!");
            }
        }
        private void Delete()
        {
            Console.WriteLine("Enter Id to delete");
            int id = int.Parse(Console.ReadLine());
            bookBusiness.Delete(id);
            Console.WriteLine("Done.");
        }
        private void AddGenres()
        {
            Console.WriteLine("Enter book Title: ");
            string title = Console.ReadLine();
            Book book = bookBusiness.Get(title);
            if (book != null)
            {
                Console.WriteLine("Enter Genres: ");
                string[] genres = Console.ReadLine().Split(" ");
                bookBusiness.AddGenres(book, genres);
            }
            else
            {
                Console.WriteLine("Book not found!");
            }
        }

        private void CheckIfBookIsAvailable()
        {
            Console.WriteLine("Enter book Title: ");
            string title = Console.ReadLine();
            Book book = bookBusiness.Get(title);
            if (book != null)
            {
                Console.WriteLine(bookBusiness.IsAvailable(title));
            }
            else
            {
                Console.WriteLine("Book not found!");
            }
        }
        private void GetBookClient()
        {
            Console.WriteLine("Enter book Title: ");
            string title = Console.ReadLine();
            Client client = bookBusiness.GetClient(title);
            if(client != null)
            {
                Console.WriteLine(new string('-', 40));
                Console.WriteLine("ID: " + client.Id);
                Console.WriteLine("First Name: " + client.FirstName);
                Console.WriteLine("Last Name: " + client.LastName);
                Console.WriteLine("Strikes: " + client.Strikes);
                Console.WriteLine(new string('-', 40));

            }
            else
            {
                Console.WriteLine("Client not found!");
            }
        }
        private void GetReturnDate()
        {
            Console.WriteLine("Enter book Title: ");
            string title = Console.ReadLine();
            Book book = bookBusiness.Get(title);
            if (book != null)
            {
                Console.WriteLine(bookBusiness.GetReturnDate(title));
            }
            else
            {
                Console.WriteLine("Book not found!");
            }
        }
        private void SearchBooksByAuthor()
        {
            Console.WriteLine("Enter Author name: ");
            string authorName = Console.ReadLine();
            List<Book> books = bookBusiness.SearchByAuthor(authorName);
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 16) + "Books" + new string(' ', 16));
            Console.WriteLine(new string('-', 40));
            foreach (var book in books)
            {
                Console.WriteLine("{0} || {1}", book.Id, book.Title);
            }
        }

        private void SearchBooksByGenre()
        {
            Console.WriteLine("Enter Genre: ");
            string genre = Console.ReadLine();
            List<Book> books = bookBusiness.SearchByGenre(genre);
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 16) + "Books" + new string(' ', 16));
            Console.WriteLine(new string('-', 40));
            foreach (var book in books)
            {
                Console.WriteLine("{0} || {1}", book.Id, book.Title);
            }
        }

        private void SearchBooksByPublisher()
        {
            Console.WriteLine("Enter Publisher name: ");
            string publisherName = Console.ReadLine();
            List<Book> books = bookBusiness.SearchByPublisher(publisherName);
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 16) + "Books" + new string(' ', 16));
            Console.WriteLine(new string('-', 40));
            foreach (var book in books)
            {
                Console.WriteLine("{0} || {1}", book.Id, book.Title);
            }
        }

        private void SearchBooksByLanguage()
        {
            Console.WriteLine("Enter Language: ");
            string language = Console.ReadLine();
            List<Book> books = bookBusiness.SearchByLanguage(language);
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 16) + "Books" + new string(' ', 16));
            Console.WriteLine(new string('-', 40));
            foreach (var book in books)
            {
                Console.WriteLine("{0} || {1}", book.Id, book.Title);
            }
        }

        private void SearchBooksByDateOfPublishing()
        {
            Console.WriteLine("Enter Date: ");
            DateTime date = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Enter Before or After: ");
            string beforeOrAfter = Console.ReadLine();
            List<Book> books = bookBusiness.SearchByDateOfPublishing(date, beforeOrAfter);
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 16) + "Books" + new string(' ', 16));
            Console.WriteLine(new string('-', 40));
            foreach (var book in books)
            {
                Console.WriteLine("{0} || {1}", book.Id, book.Title);
            }
        }
    }
}
