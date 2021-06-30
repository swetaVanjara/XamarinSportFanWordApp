using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.IdentityConfig.User;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities {
    public class PostVideo : ISaveable<string>, ISaveableCreate{
        public string Id { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        [ForeignKey("CreatedBy")]
        public string CreatedById { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string Container { get; set; }
        [Required]
        public string Blob { get; set; }

        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string ImageContainer { get; set; }
        [Required]
        public string ImageBlob { get; set; }
        public float ImageAspectRatio { get; set; }

        [Required,ForeignKey("Post")]
        public string PostId { get; set; }

        public virtual Post Post { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        
    }
}

