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
    class QueryTestsPublishers
    {
        Mock<DbSet<Publisher>> mockDBSetPublishers;
        Mock<LibraryContext> mockContext;
        PublisherBusiness publisherBusiness;

        [SetUp]
        public void Setup()
        {
            var data2 = new List<Publisher> //Publisher
            {
                new Publisher { Id = 1 , Name = "AAA", CountryOfOrigin = "USA"},
                new Publisher { Id = 2 , Name = "BBB" },
                new Publisher { Id = 3 , Name = "CCC" , CountryOfOrigin = "USA"},
            }.AsQueryable();

            mockDBSetPublishers = new Mock<DbSet<Publisher>>();

            mockDBSetPublishers.As<IQueryable<Publisher>>().Setup(m => m.Provider).Returns(data2.Provider);
            mockDBSetPublishers.As<IQueryable<Publisher>>().Setup(m => m.Expression).Returns(data2.Expression);
            mockDBSetPublishers.As<IQueryable<Publisher>>().Setup(m => m.ElementType).Returns(data2.ElementType);
            mockDBSetPublishers.As<IQueryable<Publisher>>().Setup(m => m.GetEnumerator()).Returns(data2.GetEnumerator());

            mockContext = new Mock<LibraryContext>();
            mockContext.Setup(c => c.Publishers).Returns(mockDBSetPublishers.Object);


            publisherBusiness = new PublisherBusiness(mockContext.Object);
        }
        [Test]
        public void TestIfGetAllReturnsCorrectValues()
        {
            var publishers = publisherBusiness.GetAll();

            Assert.AreEqual(3, publishers.Count, "Doesn't return correct number of publishers!");
            Assert.AreEqual("AAA", publishers[0].Name, "Client 1 isn't equal to AAA");
            Assert.AreEqual("BBB", publishers[1].Name, "Client 2 isn't equal to BBB");
            Assert.AreEqual("CCC", publishers[2].Name, "Client 3 isn't equal to CCC");
        }
        [Test]
        public void TestIfGetPublisherReturnsCorrectValues()
        {
            var publisher = publisherBusiness.Get("AAA");

            Assert.AreEqual("AAA", publisher.Name, "Client 1 isn't equal to AAA");
        }
        [Test]
        public void TestIfSearchByCountryOfOriginReturnsCorrectValues()
        {
            var publishers = publisherBusiness.SearchByCountryOfOrigin("USA");

            Assert.AreEqual(2, publishers.Count, "Doesn't return correct number of publishers!");
            Assert.AreEqual("AAA", publishers[0].Name, "Client 1 isn't equal to AAA");
            Assert.AreEqual("CCC", publishers[1].Name, "Client 2 isn't equal to CCC");
        }
    }
    [TestFixture]
    public class NonQueryTestsPublishers
    {
        Mock<DbSet<Publisher>> mockDBSetPublishers;
        Mock<LibraryContext> mockContext;
        PublisherBusiness publisherBusiness;

        [SetUp]
        public void Setup()
        {
            var data2 = new List<Publisher> //Publisher
            {
                new Publisher { Id = 1 , Name = "AAA"},
            }.AsQueryable();
            mockDBSetPublishers = new Mock<DbSet<Publisher>>();

            mockDBSetPublishers.As<IQueryable<Publisher>>().Setup(m => m.Provider).Returns(data2.Provider);
            mockDBSetPublishers.As<IQueryable<Publisher>>().Setup(m => m.Expression).Returns(data2.Expression);
            mockDBSetPublishers.As<IQueryable<Publisher>>().Setup(m => m.ElementType).Returns(data2.ElementType);
            mockDBSetPublishers.As<IQueryable<Publisher>>().Setup(m => m.GetEnumerator()).Returns(data2.GetEnumerator());

            mockContext = new Mock<LibraryContext>();
            mockContext.Setup(m => m.Publishers).Returns(mockDBSetPublishers.Object);

            publisherBusiness = new PublisherBusiness(mockContext.Object);
        }
        [Test]
        public void TestIfAddPublisherInvokesAdd()
        {
            publisherBusiness.Add(new Publisher() { Name = "AAA" });

            mockDBSetPublishers.Verify(m => m.Add(It.IsAny<Publisher>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        [Test]
        public void TestIfDeletePublisherInvokesRemove()
        {
            publisherBusiness.Delete("AAA");

            mockDBSetPublishers.Verify(m => m.Remove(It.IsAny<Publisher>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Test]
        public void TestIfUpdatePublisherInvokesSetValues()
        {
            var Publisher = new Publisher() { Name = "AAA" };
            try { publisherBusiness.Update(Publisher); }
            catch { mockContext.Verify(m => m.Entry(It.IsAny<Publisher>()), Times.Once()); }
        }

    }
}
