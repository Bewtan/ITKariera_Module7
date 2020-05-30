using NUnit.Framework;
using System;
using Moq;
using System.Linq;
using Library.Data.Models;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Business;
using System.Collections.Generic;

namespace Library.Tests
{
    [TestFixture]
    class QueryTestsBooksGenres
    {
        Mock<DbSet<BooksGenres>> mockDBSetBooksGenres;
        Mock<LibraryContext> mockContext;
        BooksGenresBusiness booksGenresBusiness;

        [SetUp]
        public void Setup()
        {
            var data = new List<BooksGenres> //Books
            {
               new BooksGenres {BookId = 1, GenreId = 1 },
               new BooksGenres {BookId = 1, GenreId = 2 },
               new BooksGenres {BookId = 2, GenreId = 2 },
               new BooksGenres {BookId = 3, GenreId = 1 }

            }.AsQueryable();

            mockDBSetBooksGenres = new Mock<DbSet<BooksGenres>>();

            mockDBSetBooksGenres.As<IQueryable<BooksGenres>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDBSetBooksGenres.As<IQueryable<BooksGenres>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDBSetBooksGenres.As<IQueryable<BooksGenres>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDBSetBooksGenres.As<IQueryable<BooksGenres>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var data2 = new List<Book> //Books
            {
                new Book { Title = "AAA", Id = 1},
                new Book { Title = "BBB", Id = 2},
                new Book { Title = "CCC", Id = 3},

            }.AsQueryable();

            Mock<DbSet<Book>> mockDBSetBooks = new Mock<DbSet<Book>>();

            mockDBSetBooks.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(data2.Provider);
            mockDBSetBooks.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(data2.Expression);
            mockDBSetBooks.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(data2.ElementType);
            mockDBSetBooks.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(data2.GetEnumerator());

            var data3 = new List<Genre> //Genres
            {
                new Genre {Id = 1 ,Name = "Genre 1"},
                new Genre {Id = 2 ,Name = "Genre 2"},
                new Genre {Id = 3,Name = "Genre 3" },
            }.AsQueryable();

            Mock<DbSet<Genre>> mockDBSetGenres = new Mock<DbSet<Genre>>();

            mockDBSetGenres.As<IQueryable<Genre>>().Setup(m => m.Provider).Returns(data3.Provider);
            mockDBSetGenres.As<IQueryable<Genre>>().Setup(m => m.Expression).Returns(data3.Expression);
            mockDBSetGenres.As<IQueryable<Genre>>().Setup(m => m.ElementType).Returns(data3.ElementType);
            mockDBSetGenres.As<IQueryable<Genre>>().Setup(m => m.GetEnumerator()).Returns(data3.GetEnumerator());

            mockContext = new Mock<LibraryContext>();
            mockContext.Setup(c => c.Books).Returns(mockDBSetBooks.Object);
            mockContext.Setup(c => c.Genres).Returns(mockDBSetGenres.Object);
            mockContext.Setup(c => c.BooksGenres).Returns(mockDBSetBooksGenres.Object);


            booksGenresBusiness = new BooksGenresBusiness(mockContext.Object);
        }
        [Test]
        public void TestIfGetBooksReturnsAllBooksWithCorrectGenre()
        {
            var books = booksGenresBusiness.GetBooks("Genre 1");

            Assert.AreEqual(2, books.Count, "Doesn't return correct number of books!");
            Assert.AreEqual("AAA", books[0].Title, "Book 1 isn't equal to AAA");
            Assert.AreEqual("CCC", books[1].Title, "Book 2 isn't equal to CCC");
        }
        [Test]
        public void TestIfGetGenresReturnsAllGenresWithCorrectBook()
        {
            var genres = booksGenresBusiness.GetGenres("AAA");

            Assert.AreEqual(2, genres.Count, "Doesn't return correct number of books!");
            Assert.AreEqual("Genre 1", genres[0].Name, "Genre 1's name isn't equal to Genre 1");
            Assert.AreEqual("Genre 2", genres[1].Name, "Genre 1's name isn't equal to Genre 2");
        }
    
    }
    [TestFixture]
    public class NonQueryTestsBooksGenres
    {
        Mock<DbSet<BooksGenres>> mockDBSetBooksGenres;
        Mock<LibraryContext> mockContext;
        BooksGenresBusiness booksGenresBusiness;

        [SetUp]
        public void Setup()
        {
            var data = new List<BooksGenres>().AsQueryable();
            mockDBSetBooksGenres = new Mock<DbSet<BooksGenres>>();

            mockDBSetBooksGenres.As<IQueryable<BooksGenres>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDBSetBooksGenres.As<IQueryable<BooksGenres>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDBSetBooksGenres.As<IQueryable<BooksGenres>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDBSetBooksGenres.As<IQueryable<BooksGenres>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockContext = new Mock<LibraryContext>();
            mockContext.Setup(m => m.BooksGenres).Returns(mockDBSetBooksGenres.Object);
            booksGenresBusiness = new BooksGenresBusiness(mockContext.Object);
        }

        [Test]
        public void TestIfAddBooksGenresInvokesAdd()
        {
            booksGenresBusiness.Add(1,2);

            mockDBSetBooksGenres.Verify(m => m.Add(It.IsAny<BooksGenres>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
    

    }
}
