// UserService.cs
using Entity.Models;
using Entity.Models.MusicEntity;
using Infastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Infastructure.Services
{
    public static class UserService
    {
        public static AppDbContext Context { get; set; }

        public static async Task<List<User>> GetAllUsersAsync()
        {
            return await Context.Users.ToListAsync();
        }

        public static async Task<User?> GetUserByIdAsync(int id)
        {
            return await Context.Users.FindAsync(id);
        }

        public static async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await Context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public static async Task AddUserAsync(User user)
        {
            user.Password = HashPassword(user.Password);
            Context.Users.Add(user);
            await Context.SaveChangesAsync();
        }

        public static async Task UpdateUserAsync(User user)
        {
            Context.Users.Update(user);
            await Context.SaveChangesAsync();
        }

        public static async Task UpdatePasswordUserAsync(User user)
        {
            user.Password = HashPassword(user.Password);
            Context.Users.Update(user);
            await Context.SaveChangesAsync();
        }

        public static async Task DeleteUserAsync(int id)
        {
            var user = await Context.Users.FindAsync(id);
            if (user != null)
            {
                Context.Users.Remove(user);
                await Context.SaveChangesAsync();
            }
        }

        public static async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await Context.Users
                .Include(u => u.UserPlayLists)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return null;

            if (VerifyPassword(password, user.Password))
                return user;

            return null;
        }


        public static async Task<List<UserPlayList>> GetUserPlayListsAsync(int userId)
        {
            return await Context.UserPlayLists
                .Where(upl => upl.UserId == userId)
                .Include(upl => upl.UserPlayListMusics)
                    .ThenInclude(upm => upm.Music)
                .ToListAsync();
        }

        public static async Task<User?> GetUserWithDetailsAsync(int id)
        {
            return await Context.Users
                .Include(u => u.QuestForAuth)
                .Include(u => u.MyThoughts)
                .Include(u => u.FavoriteBooks)
                .Include(u => u.FavoriteMusics)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public static async Task<bool> UpdateUserQuestAsync(int userId, int questForAuthId)
        {
            var user = await Context.Users.FindAsync(userId);
            if (user != null)
            {
                user.QuestForAuthId = questForAuthId;
                Context.Users.Update(user);
                await Context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        // Упрощенное хеширование пароля с солью
        private static string HashPassword(string password)
        {
            // Генерируем соль
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            // Создаем хеш с солью
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Комбинируем соль и хеш
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        // Проверка пароля
        public static bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            // Извлекаем соль из хранимого пароля
            byte[] hashBytes = Convert.FromBase64String(storedPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Хешируем введенный пароль с той же солью
            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Сравниваем хеши
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}