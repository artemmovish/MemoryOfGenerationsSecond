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
    public static class ActorService
    {
        public static AppDbContext Context { get; set; }

        public static async Task<List<Actor>> GetAllActorsAsync()
        {
            return await Context.Actors
                .Include(a => a.Musics)
                .ToListAsync();
        }

        public static async Task<Actor?> GetActorByIdAsync(int id)
        {
            return await Context.Actors
                .Include(a => a.Musics)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public static async Task AddActorAsync(Actor actor)
        {
            Context.Actors.Add(actor);
            await Context.SaveChangesAsync();
        }

        public static async Task UpdateActorAsync(Actor actor)
        {
            Context.Actors.Update(actor);
            await Context.SaveChangesAsync();
        }

        public static async Task DeleteActorAsync(int id)
        {
            var actor = await Context.Actors.FindAsync(id);
            if (actor != null)
            {
                Context.Actors.Remove(actor);
                await Context.SaveChangesAsync();
            }
        }
    }

}
