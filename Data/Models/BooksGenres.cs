using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Data.Models
{
    class BooksGenres
    {
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
