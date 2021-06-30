using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Teams {
    public class TeamRecord {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SportName { get; set; }
        public string SchoolName { get; set; }
        public bool IsSchoolActive { get; set; }
        public bool IsSportActive { get; set; }
        public bool IsActive { get; set; }
        public string ProfilePublicUrl { get; set; }
        public string SchoolId { get; set; }
        public string SportId { get; set; }
    }
}
