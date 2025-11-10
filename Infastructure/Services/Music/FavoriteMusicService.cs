using Entity.Models.MusicEntity;
using Infastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Services.Music
{
    public static class FavoriteMusicService
    {
        public static AppDbContext Context { get; set; }

        public static async Task<List<FavoriteMusic>> GetUserFavoritesAsync(int userId)
        {
            return await Context.FavoriteMusics
                .Where(fm => fm.UserId == userId)
                .Include(fm => fm.Music)
                .ToListAsync();
        }

        public static async Task<bool> IsMusicFavoriteAsync(int userId, int musicId)
        {
            return await Context.FavoriteMusics
                .AnyAsync(fm => fm.UserId == userId && fm.MusicId == musicId);
        }

        public static async Task AddFavoriteAsync(FavoriteMusic favorite)
        {
            if (!await IsMusicFavoriteAsync(favorite.UserId, favorite.MusicId))
            {
                Context.FavoriteMusics.Add(favorite);
                await Context.SaveChangesAsync();
            }
        }

        public static async Task RemoveFavoriteAsync(int userId, int musicId)
        {
            var favorite = await Context.FavoriteMusics
                .FirstOrDefaultAsync(fm => fm.UserId == userId && fm.MusicId == musicId);

            if (favorite != null)
            {
                Context.FavoriteMusics.Remove(favorite);
                await Context.SaveChangesAsync();
            }
        }

        public static async Task<int> GetFavoriteCountAsync(int userId)
        {
            return await Context.FavoriteMusics
                .CountAsync(fm => fm.UserId == userId);
        }
    }
}
