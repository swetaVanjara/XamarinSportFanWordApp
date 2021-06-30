using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities {
    public class PostLink :ISaveable<string>, ISaveableCreate{
        public string Id { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        [Required]
        public string LinkUrl { get; set; }
        public string ImageUrl { get; set; }
        
        public string Title { get; set; }
        public string Content { get; set; }
        public float ImageAspectRatio { get; set; }

        [Required,ForeignKey("Post")]
        public string PostId { get; set; }

        public virtual Post Post { get; set; }
    }
}
