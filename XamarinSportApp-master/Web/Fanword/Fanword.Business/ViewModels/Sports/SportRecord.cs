using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.Enums;

namespace Fanword.Business.ViewModels.Sports {
    public class SportRecord {
        public string Id { get; set; }
        public string IconPublicUrl { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
