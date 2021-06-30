using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Schools {
    public class SchoolRecord {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int NumberOfTeams { get; set; }
        public int NumberOfAthletes { get; set; }
        public int NumberOfPosts { get; set; }
        public int NumberOfFollowers { get; set; }
        public string ProfilePublicUrl { get; set; }
    }
}
