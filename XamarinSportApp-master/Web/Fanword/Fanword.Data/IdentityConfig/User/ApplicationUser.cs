using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Fanword.Data.Enums;
using Fanword.Data.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Fanword.Data.Entities;
using Fanword.Data.IdentityConfig.Managers;
using GenericRepository.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
namespace Fanword.Data.IdentityConfig.User {
    public class ApplicationUser : IdentityUser, ISaveableDelete, ISaveable<string>,ISaveableActive{
		public string FirstName { get; set; }
		public string LastName { get; set; }
        public bool IsActive { get; set; }
		public DateTime? LastLoginDateUtc { get; set; }
		public DateTime DateCreatedUtc { get; set; }
        public string ProfileContainer { get; set; }
        public string ProfileBlob { get; set; }
        public string ProfileUrl { get; set; }
        public DateTime? DateDeletedUtc { get; set; }
        [ForeignKey("Advertiser")]
        public string AdvertiserId { get; set; }
		[ForeignKey("ContentSource")]
		public string ContentSourceId { get; set; }


        public virtual Advertiser Advertiser { get; set; }
		public virtual ContentSource ContentSource { get; set; }
        public virtual ICollection<Athelete> Atheletes { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
        public virtual ICollection<Post> CreatedByPosts { get; set; }
        public virtual ICollection<CommentLike> CommentLikes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Sport> Sports { get; set; }
        public virtual ICollection<School> Schools { get; set; }
        /// <summary>
        /// Users that are being followed by this user
        /// </summary>
        public virtual ICollection<ApplicationUser> Users { get; set; }
        /// <summary>
        /// Users that are following this user
        /// </summary>
        public virtual ICollection<ApplicationUser> Followers { get; set; }
        public virtual ICollection<ContentSource> ContentSources { get; set; } 
        public virtual ICollection<TeamAdmin> TeamAdmins { get; set; }
        public virtual ICollection<SchoolAdmin> SchoolAdmins { get; set; }
        public ApplicationUser() {
            Id = Guid.NewGuid().ToString();
            DateCreatedUtc = DateTime.UtcNow;
        }

        public ApplicationUser(string username, string email, string first, string last, bool isActive = true) : this(username, email, isActive)
        {

            FirstName = first;
            LastName = last;
        }

        public ApplicationUser(string username, string email, bool isActive = true) : this()
        {

            UserName = username;
            Email = email;
            IsActive = isActive;
            DateCreatedUtc = DateTime.UtcNow;           
        }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
			userIdentity = await AddClaims(userIdentity);
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager, string authenticationType) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
			userIdentity = await AddClaims(userIdentity);
            return userIdentity;
        }

		private async Task<ClaimsIdentity> AddClaims(ClaimsIdentity userIdentity) {
            userIdentity.AddClaim(new Claim(AppClaimTypes.FullName, FirstName + " " + LastName));
            userIdentity.AddClaim(new Claim(AppClaimTypes.FirstName, FirstName));
            userIdentity.AddClaim(new Claim(AppClaimTypes.LastName, LastName));
		    if (!String.IsNullOrEmpty(AdvertiserId)) {
		        userIdentity.AddClaim(new Claim(AppClaimTypes.AdvertiserId, AdvertiserId));
		    }
			if (!String.IsNullOrEmpty(ContentSourceId))
			{
				userIdentity.AddClaim(new Claim(AppClaimTypes.ContentSourceId, ContentSourceId));
			}

            //get roles from database
            var ctx = new ApplicationDbContext();
            var roleManager = new ApplicationRoleManager(ctx);
            var roleIds = Roles.Select(i => i.RoleId).ToList();
            var dbRoleNames = await roleManager.Roles.Include(m => m.ApplicationRules).Where(m => roleIds.Contains(m.Id)).ToListAsync();


            //add roles as claims
            foreach (var role in dbRoleNames) {
                userIdentity.AddClaim(new Claim(AppClaimTypes.Role, role.Name));
            }

            //get associated rules
            var rules = dbRoleNames.SelectMany(m => m.ApplicationRules).ToList();
            if (dbRoleNames.Any(i => i.Name == AppRoles.SystemAdmin)) {
                rules = await ctx.ApplicationRules.ToListAsync();
            }

            foreach (var rule in rules) {
                userIdentity.AddClaim(new Claim(AppClaimTypes.Rule, AppRules.Join(rule.Qualifier, rule.Name)));
            }

            return userIdentity;
        }
    }
}
