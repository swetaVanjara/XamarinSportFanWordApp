using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public string ProfileUrl { get; set; }
        public string Email { get; set; }
        public string AdvertiserId { get; set; }
        public string ProfileContainer { get; set; }
        public string ProfileBlob { get; set; }
		public string ContentSourceId { get; set; }
        public string ContentSourceName { get; set; }
        public string ContentSourceUrl { get; set; }
        public string AthleteTeamId { get; set; }
        public string AthleteSport { get; set; }
        public string AthleteSchool { get; set; }
        public string AthleteProfileUrl { get; set; }
        public bool AthleteVerified { get; set; }
        public DateTime AthleteStartDateUtc { get; set; }
        public DateTime? AthleteEndDateUtc { get; set; }
        public List<TeamProfile> AdminTeams { get; set; }
        public List<SchoolProfile> AdminSchools { get; set; }
        public bool ContentSourceApproved { get; set; }

    }
}
