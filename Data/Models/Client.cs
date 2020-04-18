using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Data.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Strikes { get; set; }  //if return is late will get a strike, if strikes >= 3 will delete Client 
        public virtual ICollection<Book> Books { get; set; }

    }
}
