using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Fanword.Data.IdentityConfig.User;

namespace Fanword.Data.IdentityConfig.Roles {
    public class ApplicationRole : IdentityRole {

        public DateTime DateCreatedUtc { get; set; }
        public DateTime? DateModifiedUtc { get; set; }
        /// <summary>
        /// This field can determine the system role, if this field is null, it's a system role.
        /// </summary>
        [ForeignKey("CreatedBy")]
        public string CreatedById { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ICollection<ApplicationRule> ApplicationRules { get; set; }

        public ApplicationRole() {
            DateCreatedUtc = DateTime.UtcNow;
            DateModifiedUtc = DateTime.UtcNow;
        }
        /// <summary>
        /// Name of the role
        /// </summary>
        /// <param name="name"></param>
        public ApplicationRole(string name) : this() {
            Id = Guid.NewGuid().ToString();
            Name = name;
        }

       

    }
}
