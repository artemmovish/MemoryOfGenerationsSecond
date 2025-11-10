// MyThoughtService.cs
using Entity.Models;
using Infastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infastructure.Services
{
    public static class MyThoughtService
    {
        public static AppDbContext Context { get; set; }

        public static async Task<List<MyThought>> GetAllThoughtsAsync()
        {
            return await Context.MyThoughts
                .Include(mt => mt.User)
                .Include(mt => mt.Book)
                .ToListAsync();
        }

        public static async Task<MyThought?> GetThoughtByIdAsync(int id)
        {
            return await Context.MyThoughts
                .Include(mt => mt.User)
                .Include(mt => mt.Book)
                .FirstOrDefaultAsync(mt => mt.Id == id);
        }

        public static async Task<List<MyThought>> GetUserThoughtsAsync(int userId)
        {
            return await Context.MyThoughts
                .Where(mt => mt.UserId == userId)
                .Include(mt => mt.Book)
                .ToListAsync();
        }

        public static async Task<List<MyThought>> GetBookThoughtsAsync(int bookId)
        {
            return await Context.MyThoughts
                .Where(mt => mt.BookId == bookId)
                .Include(mt => mt.User)
                .ToListAsync();
        }

        public static async Task AddThoughtAsync(MyThought thought)
        {
            Context.MyThoughts.Add(thought);
            await Context.SaveChangesAsync();
        }

        public static async Task UpdateThoughtAsync(MyThought thought)
        {
            Context.MyThoughts.Update(thought);
            await Context.SaveChangesAsync();
        }

        public static async Task DeleteThoughtAsync(int id)
        {
            var thought = await Context.MyThoughts.FindAsync(id);
            if (thought != null)
            {
                Context.MyThoughts.Remove(thought);
                await Context.SaveChangesAsync();
            }
        }
    }
}