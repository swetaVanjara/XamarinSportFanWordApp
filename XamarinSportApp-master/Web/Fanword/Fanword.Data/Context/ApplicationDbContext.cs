using System;
using System.Data.Entity;
using Fanword.Data.IdentityConfig.Roles;
using Fanword.Data.IdentityConfig.User;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Data.Entity.Migrations;
using Fanword.Data.Entities;
using Fanword.Data.IdentityConfig.RefreshTokens;


namespace Fanword.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, String, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {

        public DbSet<ApplicationRule> ApplicationRules { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<RssFeed> RssFeeds { get; set; }
		public DbSet<RssKeyword> RssKeywords { get; set; }
		public DbSet<RssKeywordType> RssKeywordTypes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<PostVideo> PostVideo { get; set; }
        public DbSet<PostLink> PostLinks { get; set; }
        public DbSet<NewsNotification> NewsNotifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<Ranking> Rankings { get; set; }
        public DbSet<RankingTeam> RankingTeams { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventTeam> EventTeams { get; set; }
        public DbSet<EventManagementPin> EventManagementPins { get; set; }
        //public DbSet<Facility> Facilities { get; set; }
        public DbSet<ContentSourceRegistrationRequest> ContentSourceRegistrationRequests { get; set; }
		public DbSet<ContentSource> ContentSources { get; set; }
		public DbSet<AdvertiserRegistrationRequest> AdvertiserRegistrationRequests { get; set; }
        public DbSet<Advertiser> Advertisers { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Athelete> Atheletes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
		public DbSet<TeamAdmin> TeamAdmins { get; set; }
		public DbSet<SchoolAdmin> SchoolAdmins { get; set; }
        public DbSet<PostShare> PostShares { get; set; }
        public ApplicationDbContext() : base("DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
            modelBuilder.Entity<IdentityUserRole>().ToTable("ApplicationUserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("ApplicationUserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("ApplicationUserClaims");
            //VERY IMPORTANT.
            //this allows us to OVERRIDE existing table names and FK's to our types, and not identities. in the base.onmodelcreating it uses identityrole and identity user, we are changing it here to applicationrole and applicationuser
            var config = modelBuilder.Entity<ApplicationRole>().ToTable("ApplicationRoles");
            config.HasMany(m => m.Users).WithRequired().HasForeignKey(r => r.RoleId);
            modelBuilder.Entity<ApplicationRule>()
                .HasMany(m => m.AttachedRoles)
                .WithMany(m => m.ApplicationRules)
                .Map(m =>
                {
                    m.MapLeftKey("ApplicationRuleId");
                    m.MapRightKey("ApplicationRoleId");
                    m.ToTable("ApplicationRuleApplicationRoles");
                });




            modelBuilder.Entity<Team>()
                .HasMany(m => m.Posts)
                .WithMany(m => m.Teams)
                .Map(m => m.MapLeftKey("TeamId").MapRightKey("PostId").ToTable("TeamPosts"));

            modelBuilder.Entity<Sport>()
                .HasMany(m => m.Posts)
                .WithMany(m => m.Sports)
                .Map(m => m.MapLeftKey("SportId").MapRightKey("PostId").ToTable("SportPosts"));

            modelBuilder.Entity<School>()
                .HasMany(m => m.Posts)
                .WithMany(m => m.Schools)
                .Map(m => m.MapLeftKey("SchoolId").MapRightKey("PostId").ToTable("SchoolPosts"));

            modelBuilder.Entity<Event>()
                .HasMany(m => m.Posts)
                .WithMany(m => m.Events)
                .Map(m => m.MapLeftKey("EventId").MapRightKey("PostId").ToTable("EventPosts"));



            modelBuilder.Entity<Campaign>()
                .HasMany(m => m.Schools)
                .WithMany(m => m.Campaigns)
                .Map(m => m.MapLeftKey("CampaignId").MapRightKey("SchoolId").ToTable("SchoolCampaigns"));

            modelBuilder.Entity<Campaign>()
                .HasMany(m => m.Teams)
                .WithMany(m => m.Campaigns)
                .Map(m => m.MapLeftKey("CampaignId").MapRightKey("TeamId").ToTable("TeamCampaigns"));

            modelBuilder.Entity<Campaign>()
                .HasMany(m => m.Sports)
                .WithMany(m => m.Campaigns)
                .Map(m => m.MapLeftKey("CampaignId").MapRightKey("SportId").ToTable("SportCampaigns"));

            modelBuilder.Entity<Post>()
                .HasOptional(m => m.CreatedBy)
                .WithMany(m => m.CreatedByPosts)
                .HasForeignKey(i => i.CreatedById);

            modelBuilder.Entity<Team>()
                .HasMany(m => m.Followers)
                .WithMany(m => m.Teams)
                .Map(m => m.MapLeftKey("TeamId").MapRightKey("UserId").ToTable("TeamUsers"));

            modelBuilder.Entity<Sport>()
                .HasMany(m => m.Followers)
                .WithMany(m => m.Sports)
                .Map(m => m.MapLeftKey("SportId").MapRightKey("UserId").ToTable("SportUsers"));

            modelBuilder.Entity<School>()
                .HasMany(m => m.Followers)
                .WithMany(m => m.Schools)
                .Map(m => m.MapLeftKey("SchoolId").MapRightKey("UserId").ToTable("SchoolUsers"));

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(m => m.Followers)
                .WithMany(m => m.Users)
                .Map(m => m.MapLeftKey("UserBeingFollowedId").MapRightKey("UserId").ToTable("UserFollows"));

            modelBuilder.Entity<ContentSource>()
                .HasMany(m => m.Followers)
                .WithMany(m => m.ContentSources)
                .Map(m => m.MapLeftKey("ContentSourceId").MapRightKey("UserId").ToTable("ContentSourceUsers"));

        }

        /// <summary>
        /// Adds or Updates an Application Rule using the specified Qualifier, Suffix, and Parent Rule Id
        /// </summary>
        /// <param name="qualifier"></param>
        /// <param name="ruleName"></param>
        /// <param name="parentRuleId"></param>
        /// <returns></returns>
        public ApplicationRule AddOrUpdateRole(string qualifier, string ruleName, string parentRuleId = null)
        {
            var role = ApplicationRules.FirstOrDefault(i => i.Qualifier == qualifier && i.Name == ruleName);
            if (role == null)
            {
                role = new ApplicationRule(qualifier, ruleName, parentRuleId);
            }
            else
            {
                role.ParentRuleId = parentRuleId;
            }

            ApplicationRules.AddOrUpdate(role);
            return role;
        }
    }
}
