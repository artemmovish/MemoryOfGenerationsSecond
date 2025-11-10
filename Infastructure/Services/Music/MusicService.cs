using Infastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models.MusicEntity;
using Microsoft.EntityFrameworkCore;
namespace Infastructure.Services.Music
{
    public static class MusicService
    {
        public static AppDbContext Context { get; set; }

        public static async Task<List<Entity.Models.MusicEntity.Music>> GetAllMusicsAsync()
        {
            return await Context.Musics
                .Include(m => m.Actor)
                .ToListAsync();
        }

        public static async Task<Entity.Models.MusicEntity.Music?> GetMusicByIdAsync(int id)
        {
            return await Context.Musics
                .Include(m => m.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public static async Task<List<Entity.Models.MusicEntity.Music>> GetMusicsByActorIdAsync(int actorId)
        {
            return await Context.Musics
                .Where(m => m.ActorId == actorId)
                .ToListAsync();
        }

        public static async Task AddMusicAsync(Entity.Models.MusicEntity.Music music)
        {
            Context.Musics.Add(music);
            await Context.SaveChangesAsync();
        }

        public static async Task UpdateMusicAsync(Entity.Models.MusicEntity.Music music)
        {
            Context.Musics.Update(music);
            await Context.SaveChangesAsync();
        }

        public static async Task DeleteMusicAsync(int id)
        {
            var music = await Context.Musics.FindAsync(id);
            if (music != null)
            {
                Context.Musics.Remove(music);
                await Context.SaveChangesAsync();
            }
        }
    }
}
