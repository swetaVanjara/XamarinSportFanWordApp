using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.Users {
    public class UserRecord {
        public string Id { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsStudentAthlete { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public int Followers { get; set; }
        public int Posts { get; set; }
        public bool IsDeleted { get; set; }
		public string ContentSource { get; set; }
    }
}
