using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Data.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<BooksGenres> BooksGenres { get; set; }

    }
}
