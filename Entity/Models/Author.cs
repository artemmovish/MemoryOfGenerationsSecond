using System.ComponentModel.DataAnnotations;

namespace Entity.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Biography { get; set; }
        public string? PhotoPath { get; set; }
        public string? InterestingFacts { get; set; }
        public string LifeDate { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }

        // Navigation properties
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}