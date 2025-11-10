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

        // Navigation properties
        public ICollection<MyThought> MyThoughts { get; set; }
        public ICollection<FavoriteBook> FavoriteBooks { get; set; }

        // Navigation properties for music
        public ICollection<FavoriteMusic> FavoriteMusics { get; set; } = new List<FavoriteMusic>();
    }
}