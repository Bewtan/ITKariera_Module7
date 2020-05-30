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
    class QueryTestsClients
    {
        Mock<DbSet<Client>> mockDBSetClients;
        Mock<LibraryContext> mockContext;
        ClientBusiness clientBusiness;

        [SetUp]
        public void Setup()
        {
            var data = new List<Book> //Books
            {
                new Book { Title = "AAA",ClientId = 2, DateOfReturn = DateTime.Today.AddDays(15)},
                new Book { Title = "BBB",DateOfReturn = DateTime.Today, IsAvailable = true},
                new Book { Title = "CCC",ClientId = 2,DateOfReturn = DateTime.Today},
            }.AsQueryable();

            Mock<DbSet<Book>> mockDBSetBooks = new Mock<DbSet<Book>>();

            mockDBSetBooks.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDBSetBooks.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDBSetBooks.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDBSetBooks.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockDBSetBooks.Setup(m => m.Find(2)).Returns(new Book() { Id = 2 }); //Sets up Find method for Case 2


            var data2 = new List<Client> //Clients
            {
                new Client { Id = 1 , FirstName = "Alan"},
                new Client { Id = 2 , FirstName = "Bob" },
                new Client { Id = 3 , FirstName = "Jefrey" },
            }.AsQueryable();

             mockDBSetClients = new Mock<DbSet<Client>>();

            mockDBSetClients.As<IQueryable<Client>>().Setup(m => m.Provider).Returns(data2.Provider);
            mockDBSetClients.As<IQueryable<Client>>().Setup(m => m.Expression).Returns(data2.Expression);
            mockDBSetClients.As<IQueryable<Client>>().Setup(m => m.ElementType).Returns(data2.ElementType);
            mockDBSetClients.As<IQueryable<Client>>().Setup(m => m.GetEnumerator()).Returns(data2.GetEnumerator());

            mockDBSetClients.Setup(m => m.Find(2)).Returns(new Client() { Id = 2 }); //Sets up Find method for Case 2

            mockContext = new Mock<LibraryContext>();
            mockContext.Setup(c => c.Books).Returns(mockDBSetBooks.Object);
            mockContext.Setup(c => c.Clients).Returns(mockDBSetClients.Object);


            clientBusiness = new ClientBusiness(mockContext.Object);
        }

        [Test]
        public void TestIfGetAllReturnsCorrectValues()
        {
            var clients = clientBusiness.GetAll();

            Assert.AreEqual(3, clients.Count, "Doesn't return correct number of clients!");
            Assert.AreEqual("Alan", clients[0].FirstName, "Client 1 isn't equal to Alan");
            Assert.AreEqual("Bob", clients[1].FirstName, "Client 2 isn't equal to Bob");
            Assert.AreEqual("Jefrey", clients[2].FirstName, "Client 3 isn't equal to Jefrey");
        }
        [Test]
        public void TestIfGetBorrowedBooksReturnsCorrectValues()
        {
            var books = clientBusiness.GetBorrowedBooks(2);

            Assert.AreEqual(2, books.Count, "Doesn't return correct number of clients!");
            Assert.AreEqual("AAA", books[0].Title, "Book 1 isn't equal to AAA");
            Assert.AreEqual("CCC", books[1].Title, "Book 2 isn't equal to CCC");
        }
        [Test]
        public void TestIfEarliestReturnDateReturnsCorrectValues()
        {
            var earliestReturnDate = clientBusiness.EarliestReturnDate(2);

            Assert.AreEqual("CCC", earliestReturnDate.Title, "Book with earliest return date is not CCC!");
        }
        [Test]
        public void TestIfBorrowBooksUpdatesBook()
        {
            
            try { clientBusiness.BorrowBooks(2, new string[] { "BBB" }); }
            catch { mockContext.Verify(m => m.Entry(It.IsAny<Book>()), Times.Once()); }
        }
        [Test]
        public void TestIfBorrowBooksThrowsException()
        {
            try { clientBusiness.BorrowBooks(2, new string[] { "CCC" }); }
            catch(Exception e)
            {
                Assert.AreEqual(e.Message, "Book is unavailable right now!");
            }
        }
        [Test]
        public void TestIfReturnBooksUpdatesBook()
        {
            try { clientBusiness.ReturnBooks(2, new string[] { "AAA" }); }
            catch { mockContext.Verify(m => m.Entry(It.IsAny<Book>()), Times.Once()); }
        }
        [Test]
        public void TestIfReturnBooksThrowsException()
        {
            try { clientBusiness.ReturnBooks(2, new string[] { "BBB" }); }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "Book is not borrowed by this client!");
            }
        }
    }

    [TestFixture]
    public class NonQueryTestsClients
    {
        Mock<DbSet<Client>> mockDBSetClients;
        Mock<LibraryContext> mockContext;
        ClientBusiness clientBusiness;

        [SetUp]
        public void Setup()
        {
            var data2 = new List<Client>().AsQueryable();
            mockDBSetClients = new Mock<DbSet<Client>>();

            mockDBSetClients.As<IQueryable<Client>>().Setup(m => m.Provider).Returns(data2.Provider);
            mockDBSetClients.As<IQueryable<Client>>().Setup(m => m.Expression).Returns(data2.Expression);
            mockDBSetClients.As<IQueryable<Client>>().Setup(m => m.ElementType).Returns(data2.ElementType);
            mockDBSetClients.As<IQueryable<Client>>().Setup(m => m.GetEnumerator()).Returns(data2.GetEnumerator());

            mockDBSetClients.Setup(m => m.Find(2)).Returns(new Client() { Id = 2 }); //Sets up Find method for Case 2

            mockContext = new Mock<LibraryContext>();
            mockContext.Setup(m => m.Clients).Returns(mockDBSetClients.Object);

            clientBusiness = new ClientBusiness(mockContext.Object);
        }
        [Test]
        public void TestIfAddClientInvokesAdd()
        {
            clientBusiness.Add(new Client() { FirstName = "AAA" });

            mockDBSetClients.Verify(m => m.Add(It.IsAny<Client>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        [Test]
        public void TestIfDeleteClientInvokesRemove()
        {
            clientBusiness.Delete(2);

            mockDBSetClients.Verify(m => m.Remove(It.IsAny<Client>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        public void TestIfUpdateClientInvokesSetValues()
        {
            var Client = new Client() { Id = 2 };
            try { clientBusiness.Update(Client); }
            catch { mockContext.Verify(m => m.Entry(It.IsAny<Client>()), Times.Once()); }
        }

    }
}
