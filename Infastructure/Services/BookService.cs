// BookService.cs
using Entity.Enums;
using Entity.Models;
using Infastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infastructure.Services
{
    public static class BookService
    {
        public static AppDbContext Context { get; set; }

        public static async Task<List<Book>> GetAllBooksAsync()
        {
            return await Context.Books.ToListAsync();
        }

        public static async Task<Book?> GetBookByIdAsync(int id)
        {
            return await Context.Books.FindAsync(id);
        }

        public static async Task AddBookAsync(Book book)
        {
            Context.Books.Add(book);
            await Context.SaveChangesAsync();
        }

        public static async Task UpdateBookAsync(Book book)
        {
            Context.Books.Update(book);
            await Context.SaveChangesAsync();
        }

        public static async Task DeleteBookAsync(int id)
        {
            var book = await Context.Books.FindAsync(id);
            if (book != null)
            {
                Context.Books.Remove(book);
                await Context.SaveChangesAsync();
            }
        }

        public static async Task<List<Book>> GetBooksByGenreAsync(Genre genre)
        {
            return await Context.Books
                .Where(b => b.Genre == genre)
                .ToListAsync();
        }

        public static async Task<List<Book>> SearchBooksAsync(string searchTerm)
        {
            return await Context.Books
                .Where(b => b.Title.Contains(searchTerm) || b.Author.Name.Contains(searchTerm))
                .ToListAsync();
        }
    }
}