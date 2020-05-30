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
    class QueryTestsGenres
    {
        Mock<DbSet<Genre>> mockDBSetGenres;
        Mock<LibraryContext> mockContext;
        GenreBusiness genreBusiness;

        [SetUp]
        public void Setup()
        {
            var data2 = new List<Genre> //Genres
            {
                new Genre { Id = 1 , Name = "Genre 1"},
                new Genre { Id = 2 , Name = "Genre 2" },
                new Genre { Id = 3 , Name = "Genre 3" },
            }.AsQueryable();

            mockDBSetGenres = new Mock<DbSet<Genre>>();

            mockDBSetGenres.As<IQueryable<Genre>>().Setup(m => m.Provider).Returns(data2.Provider);
            mockDBSetGenres.As<IQueryable<Genre>>().Setup(m => m.Expression).Returns(data2.Expression);
            mockDBSetGenres.As<IQueryable<Genre>>().Setup(m => m.ElementType).Returns(data2.ElementType);
            mockDBSetGenres.As<IQueryable<Genre>>().Setup(m => m.GetEnumerator()).Returns(data2.GetEnumerator());

            mockContext = new Mock<LibraryContext>();
            mockContext.Setup(c => c.Genres).Returns(mockDBSetGenres.Object);


            genreBusiness = new GenreBusiness(mockContext.Object);
        }

        [Test]
        public void TestIfGetAllReturnsCorrectValues()
        {
            var genres = genreBusiness.GetAll();

            Assert.AreEqual(3, genres.Count, "Doesn't return correct number of books!");
            Assert.AreEqual("Genre 1", genres[0].Name, "Genre 1's Name isn't equal to Genre 1");
            Assert.AreEqual("Genre 2", genres[1].Name, "Genre 2's Name isn't equal to Genre 2");
            Assert.AreEqual("Genre 3", genres[2].Name, "Genre 3's Name isn't equal to Genre 3");
        }
        [Test]
        public void TestIfGetBookReturnsCorrectValues()
        {
            var genre = genreBusiness.Get("Genre 2");

            Assert.AreEqual("Genre 2", genre.Name, "Returned genre isn't equal to Genre 2");
        }
    }
    [TestFixture]
    public class NonQueryTestsGenres
    {
        Mock<DbSet<Genre>> mockDBSetGenres;
        Mock<LibraryContext> mockContext;
        GenreBusiness genreBusiness;

        [SetUp]
        public void Setup()
        {
            var data2 = new List<Genre> //Clients
            {
                new Genre {Name = "Genre 1"},
            }.AsQueryable();
            mockDBSetGenres = new Mock<DbSet<Genre>>();

            mockDBSetGenres.As<IQueryable<Genre>>().Setup(m => m.Provider).Returns(data2.Provider);
            mockDBSetGenres.As<IQueryable<Genre>>().Setup(m => m.Expression).Returns(data2.Expression);
            mockDBSetGenres.As<IQueryable<Genre>>().Setup(m => m.ElementType).Returns(data2.ElementType);
            mockDBSetGenres.As<IQueryable<Genre>>().Setup(m => m.GetEnumerator()).Returns(data2.GetEnumerator());

            mockContext = new Mock<LibraryContext>();
            mockContext.Setup(m => m.Genres).Returns(mockDBSetGenres.Object);
            genreBusiness = new GenreBusiness(mockContext.Object);
        }

        [Test]
        public void TestIfAddGenreInvokesAdd()
        {
            genreBusiness.Add(new Genre());

            mockDBSetGenres.Verify(m => m.Add(It.IsAny<Genre>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }


        [Test]
        public void TestIfDeleteGenreInvokesRemove()
        {
            genreBusiness.Delete("Genre 1");

            mockDBSetGenres.Verify(m => m.Remove(It.IsAny<Genre>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
    }
}
