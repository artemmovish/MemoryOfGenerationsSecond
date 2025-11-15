using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.MusicEntity
{
    public class UserPlayList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string? PathPhoto { get; set; }

        public User User { get; set; }

        // Правильная навигация
        public ICollection<UserPlayListMusic> UserPlayListMusics { get; set; } = new List<UserPlayListMusic>();
    }

}
