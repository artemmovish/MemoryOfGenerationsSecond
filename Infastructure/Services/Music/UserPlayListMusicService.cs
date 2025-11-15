// UserPlayListMusicService.cs
using Entity.Models.MusicEntity;
using Infastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infastructure.Services.Music
{
    public static class UserPlayListMusicService
    {
        public static AppDbContext Context { get; set; }

        public static async Task<List<UserPlayListMusic>> GetAllUserPlayListMusicsAsync()
        {
            return await Context.UserPlayListMusics
                .Include(upm => upm.UserPlayList)
                .Include(upm => upm.Music)
                .ToListAsync();
        }

        public static async Task<UserPlayListMusic?> GetUserPlayListMusicByIdAsync(int id)
        {
            return await Context.UserPlayListMusics
                .Include(upm => upm.UserPlayList)
                .Include(upm => upm.Music)
                .FirstOrDefaultAsync(upm => upm.Id == id);
        }

        public static async Task AddUserPlayListMusicAsync(UserPlayListMusic userPlayListMusic)
        {
            // Проверяем, не существует ли уже такая связь
            var exists = await Context.UserPlayListMusics
                .AnyAsync(upm => upm.UserPlayListId == userPlayListMusic.UserPlayListId &&
                               upm.MusicId == userPlayListMusic.MusicId);

            if (!exists)
            {
                Context.UserPlayListMusics.Add(userPlayListMusic);
                await Context.SaveChangesAsync();
            }
        }

        public static async Task UpdateUserPlayListMusicAsync(UserPlayListMusic userPlayListMusic)
        {
            Context.UserPlayListMusics.Update(userPlayListMusic);
            await Context.SaveChangesAsync();
        }

        public static async Task<bool> DeleteUserPlayListMusicAsync(int id)
        {
            var relation = await Context.UserPlayListMusics.FindAsync(id);
            if (relation != null)
            {
                Context.UserPlayListMusics.Remove(relation);
                await Context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public static async Task<bool> DeleteUserPlayListMusicByRelationAsync(int userPlayListId, int musicId)
        {
            var relation = await Context.UserPlayListMusics
                .FirstOrDefaultAsync(upm => upm.UserPlayListId == userPlayListId && upm.MusicId == musicId);

            if (relation != null)
            {
                Context.UserPlayListMusics.Remove(relation);
                await Context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public static async Task<int> GetMusicCountInUserPlayListAsync(int userPlayListId)
        {
            return await Context.UserPlayListMusics
                .CountAsync(upm => upm.UserPlayListId == userPlayListId);
        }

        public static async Task<List<UserPlayListMusic>> GetUserPlayListMusicsByPlayListAsync(int userPlayListId)
        {
            return await Context.UserPlayListMusics
                .Where(upm => upm.UserPlayListId == userPlayListId)
                .Include(upm => upm.Music)
                .ToListAsync();
        }
    }
}