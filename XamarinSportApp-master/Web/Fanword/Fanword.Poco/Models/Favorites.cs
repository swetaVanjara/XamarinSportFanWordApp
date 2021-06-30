using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class Favorites
    {
        public List<FavoriteItem> Teams { get; set; }
        public List<FavoriteItem> Schools { get; set; }
        public List<FavoriteItem> Sports { get; set; }
        public List<FavoriteItem> ContentSources { get; set; }
        public List<FavoriteItem> Following { get; set; }
        public List<FavoriteItem> Followers { get; set; }
    }
}
