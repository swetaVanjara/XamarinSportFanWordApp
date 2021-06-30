using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.Interfaces;

namespace Fanword.Data.IdentityConfig.Roles {
    public class ApplicationRule : ISaveable<string> {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Qualifier { get; set; }

         [ForeignKey("ParentRule")]
        public string ParentRuleId { get; set; }
        
        public virtual ApplicationRule ParentRule { get; set; }
        public virtual ICollection<ApplicationRule> ChildRules { get; set; }
        public virtual ICollection<ApplicationRole> AttachedRoles { get; set; }

        public ApplicationRule() {
            
        }
        /// <summary>
        /// Creates a new role
        /// </summary>
        /// <param name="qualifier">The prefix used on the role. i.e. Knowledgebase</param>
        /// <param name="roleName">The name of the role, so if your quaifier is Knowledgebase, your role name might be 'read' or 'edit</param>
        /// <param name="parentRuleId">The Id of the role if this is intended to be in a heirarchy.  I.e. you must have read before you can edit</param>
        public ApplicationRule(string qualifier, string roleName, string parentRuleId =null) {
            Id = Guid.NewGuid().ToString();
            Qualifier = qualifier;
            Name = roleName;
            ParentRuleId = parentRuleId;
        }
    }
}
