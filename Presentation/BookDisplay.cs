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
        private int closeOperationId = 6;
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
            Console.WriteLine("5. Delete entry by Title");
            Console.WriteLine("6. Exit");
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
                bookBusiness.Update(book);
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
        }
        private void Delete()
        {
            Console.WriteLine("Enter Id to delete");
            int id = int.Parse(Console.ReadLine());
            bookBusiness.Delete(id);
            Console.WriteLine("Done.");
        }

    }
}
