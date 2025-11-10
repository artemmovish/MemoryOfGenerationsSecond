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
    public static class PlayListService
    {
        public static AppDbContext Context { get; set; }

        public static async Task<List<PlayList>> GetAllPlayListsAsync()
        {
            return await Context.PlayLists
                .Include(pl => pl.Musics)
                .ToListAsync();
        }

        public static async Task<PlayList?> GetPlayListByIdAsync(int id)
        {
            return await Context.PlayLists
                .Include(pl => pl.Musics)
                .FirstOrDefaultAsync(pl => pl.Id == id);
        }

        public static async Task AddPlayListAsync(PlayList playList)
        {
            Context.PlayLists.Add(playList);
            await Context.SaveChangesAsync();
        }

        public static async Task UpdatePlayListAsync(PlayList playList)
        {
            Context.PlayLists.Update(playList);
            await Context.SaveChangesAsync();
        }

        public static async Task DeletePlayListAsync(int id)
        {
            var playList = await Context.PlayLists.FindAsync(id);
            if (playList != null)
            {
                Context.PlayLists.Remove(playList);
                await Context.SaveChangesAsync();
            }
        }

        public static async Task AddMusicToPlayListAsync(int playListId, int musicId)
        {
            var playList = await Context.PlayLists
                .Include(pl => pl.Musics)
                .FirstOrDefaultAsync(pl => pl.Id == playListId);

            var music = await Context.Musics.FindAsync(musicId);

            if (playList != null && music != null)
            {
                playList.Musics.Add(music);
                await Context.SaveChangesAsync();
            }
        }

        public static async Task RemoveMusicFromPlayListAsync(int playListId, int musicId)
        {
            var playList = await Context.PlayLists
                .Include(pl => pl.Musics)
                .FirstOrDefaultAsync(pl => pl.Id == playListId);

            var music = playList?.Musics.FirstOrDefault(m => m.Id == musicId);

            if (playList != null && music != null)
            {
                playList.Musics.Remove(music);
                await Context.SaveChangesAsync();
            }
        }
    }
}
