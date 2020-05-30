using Library.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Data
{
    public class ContextGenerator 
    {
        LibraryContext libraryContext;
        private bool IsMockContext;
        public ContextGenerator(LibraryContext context, bool IsMockContext)
        {
            this.IsMockContext = IsMockContext;
            libraryContext = context;
        }

        /// <summary>
        /// Generates a new library context with the given DBSets.
        /// </summary>
        /// <returns></returns>
        public LibraryContext Generate() 
        {
            if (IsMockContext)
            {
                return libraryContext;
            }
            libraryContext = new LibraryContext();

            return libraryContext;
        }
    }
}
