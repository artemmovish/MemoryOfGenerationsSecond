// User.cs
using Entity.Models.MusicEntity;

namespace Entity.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? AvatarPath { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        // Navigation properties for auth

        public int? QuestForAuthId { get; set; }
        public QuestForAuth? QuestForAuth { get; set; }
        public ICollection<UserPlayList> UserPlayLists { get; set; } = new List<UserPlayList>();


        // Navigation properties
        public ICollection<MyThought> MyThoughts { get; set; }
        public ICollection<FavoriteBook> FavoriteBooks { get; set; }

        // Navigation properties for music
        public ICollection<FavoriteMusic> FavoriteMusics { get; set; }
        
    }
}