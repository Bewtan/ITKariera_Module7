using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Presentation
{
    class Display
    {
        private int closeOperationId = 5;
        private void ShowMenu()
        {
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(new string(' ', 18) + "MENU" + new string(' ', 18));
            Console.WriteLine(new string('-', 40));
            Console.WriteLine("1. Book Menu");
            Console.WriteLine("2. Client Menu");
            Console.WriteLine("3. Genre Menu");
            Console.WriteLine("4. Publisher Menu");
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
                        CreateBookDisplay();
                        break;
                    case 2:
                        CreateClientDisplay();
                        break;
                    case 3:
                        CreateGenreDisplay();
                        break;
                    case 4:
                        CreatePublisherDisplay();
                        break;
                    default:
                        break;
                }
            } while (operation != closeOperationId);
        }
        public Display()
        {
            Input();
        }
        private void CreateBookDisplay()
        {
            BookDisplay display = new BookDisplay();
        }
        private void CreateClientDisplay()
        {
            ClientDisplay display = new ClientDisplay();
        }
        private void CreateGenreDisplay()
        {
            GenreDisplay display = new GenreDisplay();
        }
        private void CreatePublisherDisplay()
        {
            PublisherDisplay display = new PublisherDisplay();
        }
    }
}
