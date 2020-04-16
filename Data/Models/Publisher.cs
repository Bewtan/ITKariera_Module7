using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Data.Models
{
    class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Website { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
