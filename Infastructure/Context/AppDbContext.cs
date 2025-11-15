using Entity.Models;
using Entity.Models.MusicEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Context
{
    public class AppDbContext : DbContext
    {
        // Существующие DbSet
        public DbSet<Book> Books { get; set; }
        public DbSet<MyThought> MyThoughts { get; set; }
        public DbSet<FavoriteBook> FavoriteBooks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }

        // Новые DbSet для музыки
        public DbSet<Music> Musics { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<PlayList> PlayLists { get; set; }
        public DbSet<FavoriteMusic> FavoriteMusics { get; set; }
        public DbSet<HelpText> HelpTexts { get; set; }
        public DbSet<UserPlayList> UserPlayLists { get; set; }
        public DbSet<UserPlayListMusic> UserPlayListMusics { get; set; }
        public DbSet<QuestForAuth> QuestForAuths { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "server=localhost;port=3306;database=MemoryOfGeneration;user=root;password=admin;";

            optionsBuilder.UseSqlite(@"Data Source=D:\Dev\Projects\проект\bebebe.db");

            //optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            //              .EnableSensitiveDataLogging() // Only in development
            //              .EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User → UserPlayLists (1 → many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserPlayLists)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserPlayList → UserPlayListMusic (1 → many)
            modelBuilder.Entity<UserPlayList>()
                .HasMany(p => p.UserPlayListMusics)
                .WithOne(x => x.UserPlayList)
                .HasForeignKey(x => x.UserPlayListId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserPlayListMusic → Music (many → 1)
            modelBuilder.Entity<UserPlayListMusic>()
                .HasOne(x => x.Music)
                .WithMany()
                .HasForeignKey(x => x.MusicId)
                .OnDelete(DeleteBehavior.Cascade);

            // Уникальность пары (UserPlayListId, MusicId)
            modelBuilder.Entity<UserPlayListMusic>()
                .HasIndex(x => new { x.UserPlayListId, x.MusicId })
                .IsUnique();


            // Конфигурация для QuestForAuth (если нужны дополнительные настройки)
            modelBuilder.Entity<QuestForAuth>(entity =>
            {
                entity.HasKey(e => e.Id);
                // Добавьте другие конфигурации при необходимости
            });

            // Конфигурация связи User - QuestForAuth
            modelBuilder.Entity<User>()
                .HasOne(u => u.QuestForAuth)
                .WithMany()
                .HasForeignKey(u => u.QuestForAuthId)
                .OnDelete(DeleteBehavior.Restrict);

            // Существующие связи
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MyThought>()
                .HasOne(mt => mt.User)
                .WithMany(u => u.MyThoughts)
                .HasForeignKey(mt => mt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MyThought>()
                .HasOne(mt => mt.Book)
                .WithMany(b => b.MyThoughts)
                .HasForeignKey(mt => mt.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoriteBook>()
                .HasOne(fb => fb.User)
                .WithMany(u => u.FavoriteBooks)
                .HasForeignKey(fb => fb.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoriteBook>()
                .HasOne(fb => fb.Book)
                .WithMany(b => b.FavoriteBooks)
                .HasForeignKey(fb => fb.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoriteBook>()
                .HasIndex(fb => new { fb.UserId, fb.BookId })
                .IsUnique();

            // Связи для музыки
            // Удалена связь Music -> Actor (заменена на многие-ко-многим через MusicActor)

            // Связь Music-PlayList (многие-ко-многим)
            modelBuilder.Entity<Music>()
                .HasMany(m => m.PlayLists)
                .WithMany(p => p.Musics)
                .UsingEntity<Dictionary<string, object>>(
                    "MusicPlayList",
                    j => j.HasOne<PlayList>().WithMany().HasForeignKey("PlayListId"),
                    j => j.HasOne<Music>().WithMany().HasForeignKey("MusicId"),
                    j => j.ToTable("MusicPlayList"));

            // Связи для FavoriteMusic
            modelBuilder.Entity<FavoriteMusic>()
                .HasOne(fm => fm.Music)
                .WithMany(m => m.FavoriteMusics)
                .HasForeignKey(fm => fm.MusicId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoriteMusic>()
                .HasOne(fm => fm.User)
                .WithMany(u => u.FavoriteMusics)
                .HasForeignKey(fm => fm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoriteMusic>()
                .HasIndex(fm => new { fm.UserId, fm.MusicId })
                .IsUnique();
        }
    }
}