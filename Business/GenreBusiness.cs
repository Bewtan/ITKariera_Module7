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
        private ContextGenerator generator;

        public GenreBusiness(LibraryContext context)
        {
            libraryContext = context;
            generator = new ContextGenerator(context);

        }
        public GenreBusiness()
        {
            libraryContext = new LibraryContext();
            generator = new ContextGenerator(libraryContext);

        }
        /// <summary>
        /// Returns a genre by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Genre Get(string name)
        {
            using (libraryContext = generator.Generate())
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
            using (libraryContext = generator.Generate())
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
            var genre = this.Get(name);
            using (libraryContext = generator.Generate())
            {
                if (genre != null)
                {
                    libraryContext.Genres.Remove(genre); // Once again I think this cascades.
                    libraryContext.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Returns all genres.
        /// </summary>
        /// <returns></returns>
        public List<Genre> GetAll() 
        {
            using (libraryContext = generator.Generate())
            {
                return libraryContext.Genres.ToList();
            }
        }

    }
}
