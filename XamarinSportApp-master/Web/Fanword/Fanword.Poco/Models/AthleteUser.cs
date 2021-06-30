using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Poco.Models
{
    public class AthleteUser
    {
        public User User { get; set; }
        public Athlete Athlete { get; set; }
    }
}
