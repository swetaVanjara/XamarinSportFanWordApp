using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities {
    public class AdvertiserRegistrationRequest :ISaveable<string>, ISaveableCreate{
        public string Id { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public string Email { get; set; }
    }
}
