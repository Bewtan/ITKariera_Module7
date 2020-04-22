using Library.Data;
using Library.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Business
{
    class PublisherBusiness
    {
        private LibraryContext libraryContext;
        /// <summary>
        /// Returns a publisher by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Publisher Get(string name)
        {
            using (libraryContext = new LibraryContext())
            {
                return libraryContext.Publishers.SingleOrDefault(publisher => publisher.Name == name);
            }
        }
        /// <summary>
        /// Adds a publisher to database.
        /// </summary>
        /// <param name="publisher"></param>
        public void Add(Publisher publisher)
        {
            using (libraryContext = new LibraryContext())
            {
                libraryContext.Publishers.Add(publisher);
                libraryContext.SaveChanges();
            }
        }
        /// <summary>
        /// Deletes a publisher by name.
        /// </summary>
        /// <param name="name"></param>
        public void Delete(string name)
        {
            using (libraryContext = new LibraryContext())
            {
                var publisher = this.Get(name);
                if (publisher != null)
                {
                    libraryContext.Publishers.Remove(publisher);
                    libraryContext.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Updates publisher info.
        /// </summary>
        /// <param name="publisher"></param>
        public void Update(Publisher publisher)
        {
            using (libraryContext = new LibraryContext())
            {
                var publisherOld = libraryContext.Publishers.Find(publisher.Id);
                if (publisherOld != null)
                {
                    libraryContext.Entry(publisherOld).CurrentValues.SetValues(publisher);
                    libraryContext.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Returns all publishers from a certain country.
        /// </summary>
        /// <param name="countryOfOrigin"></param>
        /// <returns></returns>
        public List<Publisher> SearchByCountryOfOrigin(string countryOfOrigin)
        {
            using (libraryContext = new LibraryContext())
            {
                return libraryContext.Publishers.Where(publisher => publisher.CountryOfOrigin == countryOfOrigin).ToList();
            }
        }
    }
}
