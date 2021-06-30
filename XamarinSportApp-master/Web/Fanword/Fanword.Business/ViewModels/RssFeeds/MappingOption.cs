using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.RssFeeds {
    public class MappingOption {
        public string Name { get; set; }
        public string Sample { get; set; }
        public MappingOption(string name, string sample) {

            Name = name;
            Sample = name + " - " + (sample.Length > 80 ? sample.Substring(0, 80) + "..." : sample);
        }
    }
}
