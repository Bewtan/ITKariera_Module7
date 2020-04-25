using Library.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Data
{
    public class LibraryContext: DbContext
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<BooksGenres> BooksGenres { get; set; }
        public virtual DbSet<Client> Clients { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) // Manages relations between tables
        {
            modelBuilder.Entity<Publisher>()
                .HasMany(p => p.Books)
                .WithOne(b => b.Publisher)
                .HasForeignKey(b => b.PublisherId);

            modelBuilder.Entity<Client>()
                .HasMany(p => p.Books)
                .WithOne(b => b.Client)
                .HasForeignKey(b => b.ClientId);

            modelBuilder.Entity<BooksGenres>().HasKey(bg => new { bg.BookId, bg.GenreId }); // Creates a composite key with BookId and GenreId for BooksGenres

            modelBuilder.Entity<BooksGenres>()
                .HasOne(bg => bg.Book)
                .WithMany(b => b.BooksGenres)
                .HasForeignKey(bg => bg.BookId);

            modelBuilder.Entity<BooksGenres>()
                .HasOne(bg => bg.Genre)
                .WithMany(g => g.BooksGenres)
                .HasForeignKey(bg => bg.GenreId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Project07;Integrated Security = true;");
            optionsBuilder.UseLazyLoadingProxies(); // doesn't load unnecessary info
        }
    }
}
