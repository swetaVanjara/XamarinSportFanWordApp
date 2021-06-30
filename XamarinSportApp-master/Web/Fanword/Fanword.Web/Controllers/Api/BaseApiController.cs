using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fanword.Business.Builders.Advertisers;
using Fanword.Business.Builders.Campaigns;
using Fanword.Business.Builders.Comments;
using Fanword.Business.Builders.ContentManagement;
using Fanword.Business.Builders.Events;
using Fanword.Business.Builders.NewsNotifications;
using Fanword.Business.Builders.Rankings;
using Fanword.Business.Builders.RssFeeds;
using Fanword.Business.Builders.Schools;
using Fanword.Business.Builders.Sports;
using Fanword.Business.Builders.Teams;
using Fanword.Business.Builders.Users;
using Fanword.Business.Builders.RssKeywords;
using Fanword.Business.Builders.RssKeywordTypes;
using Fanword.Data.Repository;
using Microsoft.AspNet.Identity.Owin;
using Fanword.Business.Builders.ContentSources;
using Fanword.Business.Builders.Likes;
using Fanword.Business.Builders.UserAdmins;

namespace Fanword.Web.Controllers.Api
{
    public class BaseApiController : ApiController{
        protected ApplicationRepository _repo => Request.GetOwinContext().Get<ApplicationRepository>();
       protected SportsBuilder _sportsBuilder => new SportsBuilder(_repo);
        protected SchoolsBuilder _schoolsBuilder => new SchoolsBuilder(_repo);
        protected TeamsBuilder _teamsBuilder => new TeamsBuilder(_repo);
        protected UserBuilder _userBuilder => new UserBuilder(_repo);
        protected RssFeedBuilder _rssFeedBuilder => new RssFeedBuilder(_repo);
		protected RssKeywordBuilder _rssKeywordBuilder => new RssKeywordBuilder(_repo);
		protected RssKeywordTypeBuilder _rssKeywordTypeBuilder => new RssKeywordTypeBuilder(_repo);
        protected RankingsBuilder _rankingsBuilder => new RankingsBuilder(_repo);
        protected NewsNotificationBuilder _newsNotificationBuilder => new NewsNotificationBuilder(_repo);
        protected EventBuilder _eventBuilder => new EventBuilder(_repo);
        protected AdvertiserBuilder _advertiserBuilder => new AdvertiserBuilder(_repo);
        protected CampaignBuilder _campaignBuilder => new CampaignBuilder(_repo);
        protected PostBuilder _postsBuilder => new PostBuilder(_repo);
		protected ContentSourceBuilder _contentSourceBuilder => new ContentSourceBuilder(_repo);
        protected CommentBuilder _commentBuilder => new CommentBuilder(_repo);
        protected LikeBuilder _likeBuilder => new LikeBuilder(_repo);
		protected TeamAdminBuilder _teamAdminBuilder => new TeamAdminBuilder(_repo);
		protected SchoolAdminBuilder _schoolAdminBuilder => new SchoolAdminBuilder(_repo);
    }
}
