using Library.Data;
using Library.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Library.Business
{
    public class GenreBusiness
    {
        private LibraryContext libraryContext;

        public GenreBusiness(LibraryContext context)
        {
            libraryContext = context;
        }
        public GenreBusiness()
        {
            libraryContext = new LibraryContext();
        }
        /// <summary>
        /// Returns a genre by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Genre Get(string name)
        {
            using (libraryContext)
            {
                return libraryContext.Genres.SingleOrDefault(genre => genre.Name == name);
            }
        }
        /// <summary>
        /// Adds a genre to database.
        /// </summary>
        /// <param name="genre"></param>
        public void Add(Genre genre)
        {
            using (libraryContext)
            {
                libraryContext.Genres.Add(genre);
                libraryContext.SaveChanges();
            }
        }
        /// <summary>
        /// Deletes a genre by name.
        /// </summary>
        /// <param name="name"></param>
        public void Delete(string name)
        {
            using (libraryContext)
            {
                var genre = this.Get(name);
                if (genre != null)
                {
                    libraryContext.Genres.Remove(genre); // Once again I think this cascades.
                    libraryContext.SaveChanges();
                }
            }
        }

    }
}
