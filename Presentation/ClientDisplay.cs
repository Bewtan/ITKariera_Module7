using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Business;
using Library.Data.Models;

namespace Library.Presentation
{
    class ClientDisplay
    {
        private int closeOperationId = 9;
        private ClientBusiness clientBusiness;

        private void ShowMenu()
        {
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 18) + "MENU" + new string(' ', 18));
            Console.WriteLine(new string('-', 40));
            Console.WriteLine("1. Add new client");
            Console.WriteLine("2. Update client");
            Console.WriteLine("3. Fetch client by Id");
            Console.WriteLine("4. Delete entry by Id");
            Console.WriteLine("5. Borrow Books");
            Console.WriteLine("6. Return Books");
            Console.WriteLine("7. Fetch borrowed Books");
            Console.WriteLine("8. Fetch earliest Return Date of Books");
            Console.WriteLine("9. Exit");
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
                        Add();
                        break;
                    case 2:
                        Update();
                        break;
                    case 3:
                        Fetch();
                        break;
                    case 4:
                        Delete();
                        break;
                    case 5:
                        BorrowBooks();
                        break;
                    case 6:
                        ReturnBooks();
                        break;
                    case 7:
                        FetchBooksBorrowedByClient();
                        break;
                    case 8:
                        FetchEarliestReturnDateFromBorrowedBooks();
                        break;
                    default:
                        break;
                }
            } while (operation != closeOperationId);
        }
        public ClientDisplay()
        {
            clientBusiness = new ClientBusiness();
            Input();
        }
        private void Add()
        {
            Client client = new Client();
            Console.WriteLine("Enter First Name: ");
            client.FirstName = Console.ReadLine();
            Console.WriteLine("Enter Last Name: ");
            client.LastName = Console.ReadLine();
            client.Strikes = 0;
            clientBusiness.Add(client);
        }
        private void Update()
        {
            Console.WriteLine("Enter Client Id to update: ");
            int Id = int.Parse(Console.ReadLine());
            Client client = clientBusiness.Get(Id);
            if (client != null)
            {
                Console.WriteLine("Enter new First Name: ");
                client.FirstName = Console.ReadLine();
                Console.WriteLine("Enter new Last Name: ");
                client.LastName = Console.ReadLine();
                client.Strikes = 0;
                clientBusiness.Update(client);
            }
            else
            {
                Console.WriteLine("Client not found!");
            }
        }

        private void Fetch()
        {
            Console.WriteLine("Enter Client Id: ");
            int Id = int.Parse(Console.ReadLine());
            Client client = clientBusiness.Get(Id);
            if (client != null)
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
        private void Delete()
        {
            Console.WriteLine("Enter Id to delete");
            int id = int.Parse(Console.ReadLine());
            clientBusiness.Delete(id);
            Console.WriteLine("Done.");
        }
        private void BorrowBooks()
        {
            Console.WriteLine("Enter Client Id: ");
            int Id = int.Parse(Console.ReadLine());
            Client client = clientBusiness.Get(Id);
            if (client != null)
            {
                Console.WriteLine("Enter Books to Borrow: ");
                string[] books = Console.ReadLine().Split(" ");
                clientBusiness.BorrowBooks(Id, books);
                Console.WriteLine("Done.");
            }
            else
            {
                Console.WriteLine("Client not found!");
            }
        }

        private void ReturnBooks()
        {
            Console.WriteLine("Enter Client Id: ");
            int Id = int.Parse(Console.ReadLine());
            Client client = clientBusiness.Get(Id);
            if (client != null)
            {
                Console.WriteLine("Enter Book to Return: ");
                string[] books = Console.ReadLine().Split(" ");
                bool IsClientDeleted = clientBusiness.ReturnBooks(Id, books);
                if(IsClientDeleted)
                    Console.WriteLine("Books returned too late!\nClient was Deleted!");
                Console.WriteLine("Done.");
            }
            else
            {
                Console.WriteLine("Client not found!");
            }
        }
        private void FetchBooksBorrowedByClient()
        {
            Console.WriteLine("Enter Client Id: ");
            int Id = int.Parse(Console.ReadLine());
            Client client = clientBusiness.Get(Id);
            if (client != null)
            {
                List<Book> books = clientBusiness.GetBorrowedBooks(Id);
                Console.WriteLine(new string('-', 40));
                Console.WriteLine(new string(' ', 16) + "Books" + new string(' ', 16));
                Console.WriteLine(new string('-', 40));
                foreach (var book in books)
                {
                    Console.WriteLine("{0} || {1}", book.Id, book.Title);
                }
            }
            else
            {
                Console.WriteLine("Client not found!");
            }
        }

        private void FetchEarliestReturnDateFromBorrowedBooks()
        {
            Console.WriteLine("Enter Client Id: ");
            int Id = int.Parse(Console.ReadLine());
            Client client = clientBusiness.Get(Id);
            if (client != null)
            {
                Book book = clientBusiness.EarliestReturnDate(Id);
                Console.WriteLine(new string('-', 40));
                Console.WriteLine("ID: " + book.Id);
                Console.WriteLine("Title: " + book.Title);
                Console.WriteLine("Return Date: " + book.DateOfReturn);
                Console.WriteLine(new string('-', 40));
            }
            else
            {
                Console.WriteLine("Client not found!");
            }
        }

    }
}
