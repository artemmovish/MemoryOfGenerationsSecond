using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.Music
{
    public class UserPlayListMusic
    {
        public int Id { get; set; }
        public int MusicId { get;set; }
        public int UserId { get;set; }
    }
}
