using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Data.Models
{
    class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; } //will become AuthorId if the table "Author" is implented
        public bool IsAvailable { get; set; }
        public DateTime DateOfPublishing { get; set; }
        public string Language { get; set;}
        public DateTime DateOfBorrow { get; set; }
        public DateTime DateOfReturn { get; set; } //clients shouldn't be able to borrow books if return period on other books has passed 
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }
        public ICollection<BooksGenres> BooksGenres { get; set; }
    }
}
