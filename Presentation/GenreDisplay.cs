using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Business;
using Library.Data.Models;

namespace Library.Presentation
{
    class GenreDisplay
    {
        private int closeOperationId = 5;
        private GenreBusiness genreBusiness;

        private void ShowMenu()
        {
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 18) + "MENU" + new string(' ', 18));
            Console.WriteLine(new string('-', 40));
            Console.WriteLine("1. List all genres");
            Console.WriteLine("2. Add new genre");
            Console.WriteLine("3. Delete genre");
            Console.WriteLine("4. Check if Genre exists");
            Console.WriteLine("5. Exit");
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
                        CheckIfGenreExists();
                        break;
                    default:
                        break;
                }
            } while (operation != closeOperationId);
        }
        public GenreDisplay()
        {
            genreBusiness = new GenreBusiness();
            Input();
        }
        private void ListAll()
        {
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 16) + "Genres" + new string(' ', 16));
            Console.WriteLine(new string('-', 40));
            var genres = genreBusiness.GetAll();
            foreach (var genre in genres)
            {
                Console.WriteLine("{0} || {1}", genre.Id,genre.Name);
            }
        }
        private void Add()
        {
            Genre genre = new Genre();
            Console.WriteLine("Enter Genre name: ");
            genre.Name = Console.ReadLine();
            genreBusiness.Add(genre);
        }
        private void Delete()
        {
            Console.WriteLine("Enter Genre to delete: ");
            string genreName = Console.ReadLine();
            genreBusiness.Delete(genreName);
            Console.WriteLine("Done.");
        }
        private void CheckIfGenreExists()
        {
            Console.WriteLine("Enter Genre name: ");
            string genreName = Console.ReadLine();
            var genre = genreBusiness.Get(genreName);
            if (genre != null)
                Console.WriteLine("It exists!");
            else
                Console.WriteLine("It doesn't exist!");
        }
    }
}
