using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.ViewModels.ContentManagement {
    public class PostImageViewModel {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Container { get; set; }
        public string Blob { get; set; }
    }
}
