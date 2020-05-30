using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Business;
using Library.Data.Models;

namespace Library.Presentation
{
    class PublisherDisplay
    {

        private int closeOperationId = 7;
        private PublisherBusiness publisherBusiness;

        private void ShowMenu()
        {
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 18) + "MENU" + new string(' ', 18));
            Console.WriteLine(new string('-', 40));
            Console.WriteLine("1. List all publishers");
            Console.WriteLine("2. Add new publisher");
            Console.WriteLine("3. Delete publisher");
            Console.WriteLine("4. Fetch publisher");
            Console.WriteLine("5. Update publisher");
            Console.WriteLine("6. Filter publishers by country of origin");
            Console.WriteLine("7. Exit");
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
                        Delete();
                        break;
                    case 4:
                        Fetch();
                        break;
                    case 5:
                        Update();
                        break;
                    case 6:
                        FilterPublishersByCountryOfOrigin();
                        break;
                    default:
                        break;
                }
            } while (operation != closeOperationId);
        }
        public PublisherDisplay()
        {
            publisherBusiness = new PublisherBusiness();
            Input();
        }
        private void Add()
        {
            Publisher publisher = new Publisher();
            Console.WriteLine("Enter Publisher name: ");
            publisher.Name = Console.ReadLine();
            Console.WriteLine("Enter Publisher country: ");
            publisher.CountryOfOrigin = Console.ReadLine();
            Console.WriteLine("Enter Publisher website: ");
            publisher.Website = Console.ReadLine();
            publisherBusiness.Add(publisher);
        }
        private void Delete()
        {
            Console.WriteLine("Enter Publisher to delete: ");
            string publisherName = Console.ReadLine();
            publisherBusiness.Delete(publisherName);
            Console.WriteLine("Done.");
        }
        private void Fetch()
        {
            Console.WriteLine("Enter Publisher to fetch: ");
            string publisherName = Console.ReadLine();
            var publisher = publisherBusiness.Get(publisherName);
            if (publisher != null)
            {
                Console.WriteLine(new string('-', 40));
                Console.WriteLine("ID: " + publisher.Id);
                Console.WriteLine("Name: " + publisher.Name);
                Console.WriteLine("Country of Origin: " + publisher.CountryOfOrigin);
                Console.WriteLine("Website: " + publisher.Website);
                Console.WriteLine(new string('-', 40));
            }
            else
            {
                Console.WriteLine("Publisher not found!");
            }
        }
        private void Update()
        {
            Console.WriteLine("Enter Publisher to update: ");
            string publisherName = Console.ReadLine();
            Publisher publisher = publisherBusiness.Get(publisherName);
            if (publisher != null)
            {
                Console.WriteLine("Enter new Publisher name: ");
                publisher.Name = Console.ReadLine();
                Console.WriteLine("Enter new Publisher country: ");
                publisher.CountryOfOrigin = Console.ReadLine();
                Console.WriteLine("Enter new Publisher website: ");
                publisher.Website = Console.ReadLine();
                publisherBusiness.Update(publisher);
            }
            else
            {
                Console.WriteLine("Publisher not found!");
            }
        }

        private void FilterPublishersByCountryOfOrigin()
        {
            Console.WriteLine("Enter Country: ");
            string country = Console.ReadLine();
            List<Publisher> publishers = publisherBusiness.SearchByCountryOfOrigin(country);
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 16) + "Publishers" + new string(' ', 16));
            Console.WriteLine(new string('-', 40));
            foreach (var publisher in publishers)
            {
                Console.WriteLine("{0} || {1}", publisher.Id, publisher.Name);
            }
        }
        private void ListAll()
        {
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 16) + "Publishers" + new string(' ', 16));
            Console.WriteLine(new string('-', 40));
            var publishers = publisherBusiness.GetAll();
            foreach (var publisher in publishers)
            {
                Console.WriteLine("{0} || {1} || {2} ", publisher.Id, publisher.Name, publisher.CountryOfOrigin);
            }
        }
    }
}
