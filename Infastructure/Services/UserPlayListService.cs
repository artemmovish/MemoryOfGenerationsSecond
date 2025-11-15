// UserPlayListService.cs
using Entity.Models.MusicEntity;
using Infastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infastructure.Services.Music
{
    public static class UserPlayListService
    {
        public static AppDbContext Context { get; set; }

        public static async Task<List<UserPlayList>> GetAllUserPlayListsAsync()
        {
            return await Context.UserPlayLists
                .Include(upl => upl.User)
                .Include(upl => upl.UserPlayListMusics)
                    .ThenInclude(upm => upm.Music)
                .ToListAsync();
        }

        public static async Task<List<UserPlayList>> GetUserPlayListsByUserIdAsync(int userId)
        {
            return await Context.UserPlayLists
                .Where(upl => upl.UserId == userId)
                .Include(upl => upl.UserPlayListMusics)
                    .ThenInclude(upm => upm.Music)
                .ToListAsync();
        }

        public static async Task<UserPlayList?> GetUserPlayListByIdAsync(int id)
        {
            return await Context.UserPlayLists
                .Include(upl => upl.User)
                .Include(upl => upl.UserPlayListMusics)
                    .ThenInclude(upm => upm.Music)
                .FirstOrDefaultAsync(upl => upl.Id == id);
        }

        public static async Task<UserPlayList?> GetUserPlayListByNameAsync(int userId, string name)
        {
            return await Context.UserPlayLists
                .FirstOrDefaultAsync(upl => upl.UserId == userId && upl.Name == name);
        }

        public static async Task<UserPlayList> CreateUserPlayListAsync(int userId, string name, string pathPhoto = null)
        {
            var playList = new UserPlayList
            {
                UserId = userId,
                Name = name,
                PathPhoto = pathPhoto,
                UserPlayListMusics = new List<UserPlayListMusic>()
            };

            Context.UserPlayLists.Add(playList);
            await Context.SaveChangesAsync();
            return playList;
        }

        public static async Task AddUserPlayListAsync(UserPlayList userPlayList)
        {
            Context.UserPlayLists.Add(userPlayList);
            await Context.SaveChangesAsync();
        }

        public static async Task UpdateUserPlayListAsync(UserPlayList userPlayList)
        {
            Context.UserPlayLists.Update(userPlayList);
            await Context.SaveChangesAsync();
        }

        public static async Task<bool> DeleteUserPlayListAsync(int id)
        {
            var playList = await Context.UserPlayLists.FindAsync(id);
            if (playList != null)
            {
                Context.UserPlayLists.Remove(playList);
                await Context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public static async Task<bool> AddMusicToUserPlayListAsync(int userPlayListId, int musicId)
        {
            var existingRelation = await Context.UserPlayListMusics
                .FirstOrDefaultAsync(upm => upm.UserPlayListId == userPlayListId && upm.MusicId == musicId);

            if (existingRelation != null)
                return false; // Музыка уже в плейлисте

            var relation = new UserPlayListMusic
            {
                UserPlayListId = userPlayListId,
                MusicId = musicId
            };

            Context.UserPlayListMusics.Add(relation);
            await Context.SaveChangesAsync();
            return true;
        }

        public static async Task<bool> RemoveMusicFromUserPlayListAsync(int userPlayListId, int musicId)
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

        public static async Task<List<Entity.Models.MusicEntity.Music>> GetMusicsFromUserPlayListAsync(int userPlayListId)
        {
            return await Context.UserPlayListMusics
                .Where(upm => upm.UserPlayListId == userPlayListId)
                .Include(upm => upm.Music)
                .Select(upm => upm.Music)
                .ToListAsync();
        }

        public static async Task<bool> IsMusicInUserPlayListAsync(int userPlayListId, int musicId)
        {
            return await Context.UserPlayListMusics
                .AnyAsync(upm => upm.UserPlayListId == userPlayListId && upm.MusicId == musicId);
        }
    }
}