using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities {
    public class PostImage  :ISaveable<string>, ISaveableCreate{
        public string Id { get; set; }
        public string Container { get; set; }
        public string Blob { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        [Required]
        public string Url { get; set; }
        [Required,ForeignKey("Post")]
        public string PostId { get; set; }
        public float ImageAspectRatio { get; set; }

        public virtual Post Post { get; set; }
    }
}
