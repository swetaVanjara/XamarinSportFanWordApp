using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.Repository;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Fanword.Data.IdentityConfig.Managers;
using Fanword.Data.Context;
using Fanword.Data.Entities;
using Fanword.Data.IdentityConfig.RefreshTokens;
using Fanword.Data.IdentityConfig.User;
using GenericRepository.Interfaces;

namespace Fanword.Data.Repository {
    public class ApplicationRepository : ApplicationRepositoryBase<ApplicationDbContext> {

        public ApplicationUserManager UserManager { get; set; }
        public ApplicationRoleManager RoleManager { get; set; }
        public ApplicationSignInManager SignInManager { get; set; }

        public IRepository<string,Sport,string> Sports { get; set; }
        public IRepository<string,School,string> Schools { get; set; }
        public IRepository<string,Team,string> Teams { get; set; }
        public IRepository<string,RssFeed,string> RssFeeds { get; set; }
		public IRepository<string,RssKeyword,string> RssKeywords { get; set; }
		public IRepository<string,RssKeywordType,string> RssKeywordTypes { get; set; }
        public IRepository<string,Post,string> Posts { get; set; }
        public IRepository<string,PostImage,string> PostImages { get; set; }
        public IRepository<string,PostVideo,string> PostVideos { get; set; }
        public IRepository<string, PostLink,string> PostLinks { get; set; }
        public IRepository<string,NewsNotification,string> NewsNotifications { get; set; }
        public IRepository<string,UserNotification,string> UserNotifications { get; set; }
        public IRepository<string,Ranking,string> Rankings { get; set; }
        public IRepository<string,RankingTeam,string> RankingTeams { get; set; }

        public IRepository<string,Event,string> Events { get; set; }
        public IRepository<string,EventTeam,string> EventTeams { get; set; }
        public IRepository<string, EventManagementPin, string> EventManagementPins { get; set; }
        //public IRepository<string,Facility,string> Facilities { get; set; }
        public IRepository<string,AdvertiserRegistrationRequest,string> AdvertiserRegistrationRequests { get; set; }

        public IRepository<string,Advertiser,string> Advertisers { get; set; }
		public IRepository<string,ContentSourceRegistrationRequest, string> ContentSourceRegistrationRequests { get; set; }
		public IRepository<string,ContentSource,string> ContentSources { get; set; }
        public IRepository<string,Campaign,string> Campaigns { get; set; }
        public IRepository<string, RefreshToken, string> RefreshTokens { get; set; }
        public IRepository<string,Athelete,string> Atheletes { get; set; }
        public IRepository<string, PostLike, string> PostLikes { get; set; }
        public IRepository<string, CommentLike, string> CommentLikes { get; set; }
        public IRepository<string, Comment, string> Comments { get; set; }
        public IRepository<string, ApplicationUser, string> Users { get; set; }
		public IRepository<string, TeamAdmin, string> TeamAdmins { get; set; }
		public IRepository<string, SchoolAdmin, string> SchoolAdmins { get; set; }
        public IRepository<string, PostShare, string> PostShares { get; set; }
        public ApplicationRepository(ApplicationDbContext ctx) : base(ctx) {
            //PROPERTIES OF IREPOS HERE
            Sports = new Repository<string, Sport, string>(ctx);
            Schools = new Repository<string, School, string>(ctx);
            Teams = new Repository<string, Team, string>(ctx);
            Users = new Repository<string, ApplicationUser, string>(ctx);
            RssFeeds = new Repository<string, RssFeed, string>(ctx);
			RssKeywords = new Repository<string, RssKeyword, string>(ctx);
			RssKeywordTypes = new Repository<string, RssKeywordType, string>(ctx);
            Posts = new Repository<string, Post, string>(ctx);
            PostImages = new Repository<string, PostImage, string>(ctx);
            PostVideos = new Repository<string, PostVideo, string>(ctx);
            PostLinks = new Repository<string, PostLink, string>(ctx);
            NewsNotifications = new Repository<string, NewsNotification, string>(ctx);
            UserNotifications = new Repository<string, UserNotification, string>(ctx);
            Rankings = new Repository<string, Ranking, string>(ctx);
            RankingTeams = new Repository<string, RankingTeam, string>(ctx);
            Events = new Repository<string, Event, string>(ctx);
            EventManagementPins = new Repository<string, EventManagementPin, string>(ctx);
            EventTeams = new Repository<string, EventTeam, string>(ctx);
            //Facilities = new Repository<string, Facility, string>(ctx);
            AdvertiserRegistrationRequests = new Repository<string, AdvertiserRegistrationRequest, string>(ctx);
            Advertisers = new Repository<string, Advertiser, string>(ctx);
			ContentSourceRegistrationRequests = new Repository<string, ContentSourceRegistrationRequest, string>(ctx);
			ContentSources = new Repository<string, ContentSource, string>(ctx);
            Campaigns = new Repository<string, Campaign, string>(ctx);
            RefreshTokens = new Repository<string, RefreshToken, string>(ctx);
            Atheletes = new Repository<string, Athelete, string>(ctx);
            PostLikes = new Repository<string, PostLike, string>(ctx);
            Comments = new Repository<string, Comment, string>(ctx);
            CommentLikes = new Repository<string, CommentLike, string>(ctx);
			TeamAdmins = new Repository<string, TeamAdmin, string>(ctx);
			SchoolAdmins = new Repository<string, SchoolAdmin, string>(ctx);
            PostShares = new Repository<string, PostShare, string>(ctx);
        }

        public ApplicationRepository(ApplicationDbContext ctx, ApplicationUserManager uManager,
            ApplicationRoleManager rManager, ApplicationSignInManager sManager) : this(ctx) {
            UserManager = uManager;
            RoleManager = rManager;
            SignInManager = sManager;
        }

        public static ApplicationRepository Create(IdentityFactoryOptions<ApplicationRepository> options, IOwinContext ctx) {
            return new ApplicationRepository(ctx.Get<ApplicationDbContext>(), ctx.GetUserManager<ApplicationUserManager>(), ctx.Get<ApplicationRoleManager>(), ctx.Get<ApplicationSignInManager>());
        }

        public static ApplicationRepository CreateWithoutOwin() {
            var ctx = new ApplicationDbContext();
            var uManager = new ApplicationUserManager(ctx);
            uManager.CreateIt(); // sets default settings
            var roleManager = new ApplicationRoleManager(ctx);
            return new ApplicationRepository(ctx, uManager, roleManager, null);
        }
    }
}
