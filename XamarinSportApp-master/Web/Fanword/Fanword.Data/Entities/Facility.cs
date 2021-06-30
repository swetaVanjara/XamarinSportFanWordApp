using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities {
    public class Facility : ISaveable<string>{
        public string Id { get; set; }
        public string Name { get; set; }


        public virtual ICollection<Event> Events { get; set; }
    }
}
