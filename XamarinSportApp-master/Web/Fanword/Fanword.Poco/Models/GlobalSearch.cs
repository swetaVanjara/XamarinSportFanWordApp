using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class GlobalSearch
    {
        public string SearchText { get; set; }
        public List<GlobalSearchItem> Results { get; set; }
    }
}
