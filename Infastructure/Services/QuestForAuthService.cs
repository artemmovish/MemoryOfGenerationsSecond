// QuestForAuthService.cs
using Entity.Models;
using Infastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infastructure.Services
{
    public static class QuestForAuthService
    {
        public static AppDbContext Context { get; set; }

        public static async Task<List<QuestForAuth>> GetAllQuestsAsync()
        {
            return await Context.QuestForAuths.ToListAsync();
        }

        public static async Task<QuestForAuth?> GetQuestByIdAsync(int id)
        {
            return await Context.QuestForAuths.FindAsync(id);
        }

        public static async Task<QuestForAuth?> GetQuestByTitleAsync(string title)
        {
            return await Context.QuestForAuths
                .FirstOrDefaultAsync(q => q.Title == title);
        }

        public static async Task AddQuestAsync(QuestForAuth quest)
        {
            Context.QuestForAuths.Add(quest);
            await Context.SaveChangesAsync();
        }

        public static async Task UpdateQuestAsync(QuestForAuth quest)
        {
            Context.QuestForAuths.Update(quest);
            await Context.SaveChangesAsync();
        }

        public static async Task<bool> DeleteQuestAsync(int id)
        {
            var quest = await Context.QuestForAuths.FindAsync(id);
            if (quest != null)
            {
                Context.QuestForAuths.Remove(quest);
                await Context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public static async Task<List<QuestForAuth>> GetRandomQuestsAsync(int count)
        {
            return await Context.QuestForAuths
                .OrderBy(q => Guid.NewGuid()) // Случайный порядок
                .Take(count)
                .ToListAsync();
        }

        public static async Task<bool> VerifyAnswerAsync(int questId, string answer)
        {
            var quest = await Context.QuestForAuths.FindAsync(questId);
            return quest?.Answer?.ToLower() == answer?.ToLower();
        }

        public static async Task<bool> VerifyAnswerByTitleAsync(string title, string answer)
        {
            var quest = await Context.QuestForAuths
                .FirstOrDefaultAsync(q => q.Title == title);
            return quest?.Answer?.ToLower() == answer?.ToLower();
        }
    }
}