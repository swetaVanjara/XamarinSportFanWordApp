using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Data.Enums {
    public static class AppClaimTypes {
        public const string Role = ClaimTypes.Role;
        public const string FullName = "appClaim:full_name";
        public const string Rule = "appClaim:rule";
        public const string FirstName = ClaimTypes.GivenName; //this could also be family name
        public const string LastName = ClaimTypes.Surname;
        public const string AdvertiserId = "appClaim:advertiser_id";
		public const string ContentSourceId = "appClaim:contentSource_id";
	}
}
