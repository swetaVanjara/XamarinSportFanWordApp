using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.Enums;

namespace Fanword.Business.ViewModels.Campaigns {
    public class CampaignRecord {
        public string Id { get; set; }
        public CampaignStatus CampaignStatus { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public int Weight { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public List<string> TeamList { get; set; }
        public List<string> SchoolList { get; set; }
        public List<string> SportList { get; set; }

        public CampaignRecord() {
            TeamList = new List<string>();
            SchoolList = new List<string>();
            SportList = new List<string>();
        }
        public string Profiles {
            get {
                var fullList = TeamList;
                fullList.AddRange(SchoolList);
                fullList.AddRange(SportList);
                var combined =String.Join(", ", fullList);
                if (combined.Length > 150) {
                    return combined.Substring(0, 150) + "...";
                }
                return combined;
            }
        }

    }
}
