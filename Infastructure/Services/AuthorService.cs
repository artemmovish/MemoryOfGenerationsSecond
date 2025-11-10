// AuthorService.cs
using Entity.Models;
using Infastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infastructure.Services
{
    public static class AuthorService
    {
        public static AppDbContext Context { get; set; }

        public static async Task<List<Author>> GetAllAuthorsAsync()
        {
            return await Context.Authors
                .Include(a => a.Books)
                .ToListAsync();
        }

        public static async Task<Author?> GetAuthorByIdAsync(int id)
        {
            return await Context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public static async Task<List<Author>> SearchAuthorsAsync(string name)
        {
            return await Context.Authors
                .Where(a => a.Name.Contains(name))
                .ToListAsync();
        }

        public static async Task AddAuthorAsync(Author author)
        {
            Context.Authors.Add(author);
            await Context.SaveChangesAsync();
        }

        public static async Task UpdateAuthorAsync(Author author)
        {
            Context.Authors.Update(author);
            await Context.SaveChangesAsync();
        }

        public static async Task DeleteAuthorWithBooksAsync(int id)
        {
            var author = await Context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null) return;

            Context.Books.RemoveRange(author.Books);
            Context.Authors.Remove(author);
            await Context.SaveChangesAsync();
        }

        public static async Task<List<Book>> GetAuthorBooksAsync(int authorId)
        {
            return await Context.Books
                .Where(b => b.AuthorId == authorId)
                .ToListAsync();
        }
    }
}