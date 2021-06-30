using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanword.Data.IdentityConfig.User;
using GenericRepository.Interfaces;

namespace Fanword.Data.IdentityConfig.RefreshTokens
{
    public class RefreshToken : ISaveable<string>
    {
        public string Id { get; set; }
        public System.DateTime DateCreatedUtc { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime IssuedUtc { get; set; }
        public System.DateTime ExpirationDateUtc { get; set; }
        [Required]
        public string ProtectedTicket { get; set; }
        [ForeignKey("ApplicationUser"), Required]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
