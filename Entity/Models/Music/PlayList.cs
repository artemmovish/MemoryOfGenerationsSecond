// PlayList.cs
namespace Entity.Models.MusicEntity
{
    public class PlayList
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;

        // Navigation property for Music (many-to-many)
        public ICollection<Music> Musics { get; set; } = new List<Music>();
    }
}