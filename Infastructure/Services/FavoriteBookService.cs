// FavoriteBookService.cs
using Entity.Models;
using Infastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infastructure.Services
{
    public static class FavoriteBookService
    {
        public static AppDbContext Context { get; set; }

        public static async Task<List<FavoriteBook>> GetUserFavoritesAsync(int userId)
        {
            return await Context.FavoriteBooks
                .Where(fb => fb.UserId == userId)
                .Include(fb => fb.Book)
                .ToListAsync();
        }

        public static async Task<bool> IsBookFavoriteAsync(int userId, int bookId)
        {
            return await Context.FavoriteBooks
                .AnyAsync(fb => fb.UserId == userId && fb.BookId == bookId);
        }

        public static async Task AddFavoriteAsync(FavoriteBook favorite)
        {
            if (!await IsBookFavoriteAsync(favorite.UserId, favorite.BookId))
            {
                Context.FavoriteBooks.Add(favorite);
                await Context.SaveChangesAsync();
            }
        }

        public static async Task RemoveFavoriteAsync(int userId, int bookId)
        {
            var list = Context.FavoriteBooks.ToList();

            var favorite = await Context.FavoriteBooks
                .FirstOrDefaultAsync(fb => fb.UserId == userId && fb.BookId == bookId);

            if (favorite != null)
            {
                Context.FavoriteBooks.Remove(favorite);
                await Context.SaveChangesAsync();
            }
        }

        public static async Task<int> GetFavoriteCountAsync(int userId)
        {
            return await Context.FavoriteBooks
                .CountAsync(fb => fb.UserId == userId);
        }
    }
}