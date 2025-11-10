using Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string CoverImagePath { get; set; } = "";
        public string Title { get; set; } = "";

        // Заменяем строку Author на связь с моделью Author
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        public string Description { get; set; } = "";
        public Genre Genre { get; set; } = Genre.Другое;
        public string BookFilePath { get; set; } = "";
        public string? AudioBookPath { get; set; }

        // Навигационные свойства
        public ICollection<MyThought> MyThoughts { get; set; } = new List<MyThought>();
        public ICollection<FavoriteBook> FavoriteBooks { get; set; } = new List<FavoriteBook>();
    }
}
