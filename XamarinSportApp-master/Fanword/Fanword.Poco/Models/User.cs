using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public string ProfileUrl { get; set; }
        public string Email { get; set; }
        public string AdvertiserId { get; set; }

    }
}
