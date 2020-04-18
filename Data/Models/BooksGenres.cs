using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Data.Models
{
    public class BooksGenres
    {
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}
