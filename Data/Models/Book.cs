using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Data.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime DateOfPublishing { get; set; }
        public string Language { get; set;}
        public DateTime DateOfBorrow { get; set; } //may not be needed
        public DateTime DateOfReturn { get; set; } //clients shouldn't be able to borrow books if return period on other books has passed 
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public int PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<BooksGenres> BooksGenres { get; set; }
    }
}
