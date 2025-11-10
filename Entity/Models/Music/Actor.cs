

namespace Entity.Models.MusicEntity
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string InterestingFacts { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Music> Musics { get; set; } = new List<Music>();
    }
}