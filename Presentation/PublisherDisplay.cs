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

        private int closeOperationId = 5;
        private GenreBusiness genreBusiness;

        private void ShowMenu()
        {
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 18) + "MENU" + new string(' ', 18));
            Console.WriteLine(new string('-', 40));
            Console.WriteLine("1. List all publishers");
            Console.WriteLine("2. Add new publisher");
            Console.WriteLine("3. Delete pulisher");
            Console.WriteLine("4. Fetch publisher");
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
    }
}
