using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.MusicEntity
{
    public class UserPlayListMusic
    {
        public int Id { get; set; }
        public int UserPlayListId { get; set; }
        public int MusicId { get; set; }

        public UserPlayList UserPlayList { get; set; }
        public Music Music { get; set; }
    }

}
