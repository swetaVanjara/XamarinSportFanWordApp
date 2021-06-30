using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExtensionClasses.AutoMapper.Mappers;
using Fanword.Business.Builders.Mobile.Base;
using Fanword.Data.Entities;
using Fanword.Data.Enums;
using Fanword.Data.Repository;
using Fanword.Poco.Models;

namespace Fanword.Business.Builders.Mobile.Feed
{
    public class FeedBuilder : BaseBuilder
    {

        MapperConfiguration MapObjects(string userId)
        {
           

            MapperConfiguration config = new MapperConfiguration(j =>
                j.CreateMap<Data.Entities.Post, FeedItem>()
                    .ForMember(m => m.Username, m => m.MapFrom(mm => mm.ContentSource == null ? mm.Team == null ? mm.School == null ? mm.Sport == null ? mm.CreatedBy.FirstName + " " + mm.CreatedBy.LastName : mm.Sport.Name : mm.School.Name : mm.Team.School.Name + "@" + mm.Team.Sport.Name : mm.ContentSource.ContentSourceName))
                    .ForMember(m => m.ImageUrl, m => m.MapFrom(mm => mm.PostImages.Select(k => k.Url).FirstOrDefault()))
                    .ForMember(m => m.VideoImageUrl, m => m.MapFrom(mm => mm.PostVideos.Select(k => k.ImageUrl).FirstOrDefault()))
                    .ForMember(m => m.VideoUrl, m => m.MapFrom(mm => mm.PostVideos.Select(k => k.Url).FirstOrDefault())).ForMember(m => m.ImageUrl, m => m.MapFrom(mm => mm.PostImages.FirstOrDefault().Url))
                    .ForMember(m => m.LinkImage, m => m.MapFrom(mm => mm.PostLinks.Select(k => k.ImageUrl).FirstOrDefault()))
                    .ForMember(m => m.LinkUrl, m => m.MapFrom(mm => mm.PostLinks.Select(k => k.LinkUrl).FirstOrDefault()))
                    .ForMember(m => m.LinkTitle, m => m.MapFrom(mm => mm.PostLinks.Select(k => k.Title).FirstOrDefault()))
                    .ForMember(m => m.ProfileUrl, m => m.MapFrom(mm => mm.ContentSource == null ? mm.Team == null ? mm.School == null ? mm.Sport == null ? mm.CreatedBy.ProfileUrl : mm.Sport.IconPublicUrl : mm.School.ProfilePublicUrl : mm.Team.ProfilePublicUrl : mm.ContentSource.LogoUrl))
                    .ForMember(m => m.TagCount, m => m.MapFrom(mm => mm.Teams.Count + mm.Schools.Count + mm.Sports.Count + mm.Events.Count))
                    .ForMember(m => m.ShareCount, m => m.MapFrom(mm => mm.Shares.Count))
                    .ForMember(m => m.ImageAspectRatio, m => m.MapFrom(mm => !mm.PostImages.Any() ? !mm.PostLinks.Any() ? !mm.PostVideos.Any() ? 0 : mm.PostVideos.Select(k => k.ImageAspectRatio).FirstOrDefault() : mm.PostLinks.Select(k =>k.ImageAspectRatio).FirstOrDefault() : mm.PostImages.Select(k => k.ImageAspectRatio).FirstOrDefault()))
                    .ForMember(m => m.LikeCount, m => m.MapFrom(mm => mm.Likes.Count))
                    .ForMember(m => m.IsLiked, m => m.MapFrom(mm => mm.Likes.Any(k => k.CreatedById == userId)))
                    .ForMember(m => m.FacebookUrl, m => m.MapFrom(mm => mm.Team == null ? mm.School == null ? null : mm.School.FacebookUrl : mm.Team.FacebookUrl))
                    .ForMember(m => m.TwitterUrl, m => m.MapFrom(mm => mm.Team == null ? mm.School == null ? null : mm.School.TwitterUrl : mm.Team.TwitterUrl))
                    .ForMember(m => m.InstagramUrl, m => m.MapFrom(mm => mm.Team == null ? mm.School == null ? null : mm.School.InstagramUrl : mm.Team.InstagramUrl))
                    .ForMember(m => m.IsCommented, m => m.MapFrom(mm => mm.Comments.Any(k => k.CreatedById == userId && k.DateDeletedUtc == null)))
                    .ForMember(m => m.IsSharePost, m => m.MapFrom(mm => !string.IsNullOrEmpty(mm.SharedFromPostId)))
                    .ForMember(m => m.CommentCount, m => m.MapFrom(mm => mm.Comments.Count(i=>i.DateDeletedUtc==null)))
                    .ForMember(m => m.SharedUsername, m => m.MapFrom(mm => !string.IsNullOrEmpty(mm.SharedFromPostId) == true ?  (mm.SharedFromPost.ContentSource == null ? mm.SharedFromPost.Team == null ? mm.SharedFromPost.School == null ? mm.SharedFromPost.Sport == null ? mm.SharedFromPost.CreatedBy.FirstName + " " + mm.SharedFromPost.CreatedBy.LastName : mm.SharedFromPost.Sport.Name : mm.SharedFromPost.School.Name : mm.SharedFromPost.Team.School.Name + " - " + mm.SharedFromPost.Team.Sport.Name : mm.SharedFromPost.ContentSource.ContentSourceName) : null)) 
                    );

            return config;
        }
        public FeedBuilder(ApplicationRepository repo)
        {
            _repo = repo ?? ApplicationRepository.CreateWithoutOwin();
        }

        public async Task<List<FeedItem>> Build(DateTime lastPostCreatedAt, string userId, string id, FeedType type)
        {

            var config = MapObjects(userId);

            var followedTeams = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Teams.Select(j => j.Id));
            var followedSchools = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Schools.Select(j => j.Id));
            var followedSports = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Sports.Select(j => j.Id));
            var followedUsers = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.Users.Select(j => j.Id));
            var followedContentSources = _repo.Users.Where(m => m.Id == userId).SelectMany(m => m.ContentSources.Select(j => j.Id));

            List<FeedItem> posts = new List<FeedItem>();
            if (type == FeedType.Main)
            {
                posts = await _repo.Posts.Where(m => m.DateCreatedUtc < lastPostCreatedAt && m.DateDeletedUtc == null && (
                        /*(!string.IsNullOrEmpty(m.ContentSourceId) || !string.IsNullOrEmpty(m.TeamId) || !string.IsNullOrEmpty(m.SchoolId)) && */ // Making sure that I only see posts that are tagged that are not from a user
                        (m.Teams.Any(j => followedTeams.Contains(j.Id)) ||                       // Tagged with a Team I follow
                        m.Schools.Any(j => followedSchools.Contains(j.Id)) ||                   // Tagged with a School I follow
                        m.Sports.Any(j => followedSports.Contains(j.Id)) ||                     // Tagged with a Sport I follow
                        followedContentSources.Contains(m.ContentSourceId)) ||                  // Tagged with a Content source I follow
                        followedTeams.Contains(m.TeamId) ||                                     // Posted by a team I follow
                        followedSchools.Contains(m.SchoolId) ||                                 // Posted by a school I follow
                        followedSports.Contains(m.SportId) ||                                   // Posted by a sport I follow
                        followedContentSources.Contains(m.ContentSourceId) ||                   // Posted by a content source I follow
                        (string.IsNullOrEmpty(m.ContentSourceId) && m.CreatedById == userId) || // From me
                        (string.IsNullOrEmpty(m.ContentSourceId) && followedUsers.Contains(m.CreatedById)) && 
                        (string.IsNullOrEmpty(m.TeamId) && followedUsers.Contains(m.CreatedById)) &&
                        (string.IsNullOrEmpty(m.SchoolId) && followedUsers.Contains(m.CreatedById))) // From a user I follow
                        ).ProjectTo<FeedItem>(config).OrderByDescending(m => m.DateCreatedUtc).Take(20).ToListAsync();
            }
            else if (type == FeedType.MyProfile)
            {
                posts = await _repo.Posts.Where(m => m.CreatedById == userId && string.IsNullOrEmpty(m.ContentSourceId) && string.IsNullOrEmpty(m.TeamId) && string.IsNullOrEmpty(m.SchoolId) && m.DateCreatedUtc < lastPostCreatedAt && m.DateDeletedUtc == null).ProjectTo<FeedItem>(config).OrderByDescending(m => m.DateCreatedUtc).Take(20).ToListAsync();
            }
            else if (type == FeedType.User)
            {
                posts = await _repo.Posts.Where(m => m.CreatedById == id && string.IsNullOrEmpty(m.ContentSourceId) && string.IsNullOrEmpty(m.TeamId) && string.IsNullOrEmpty(m.SchoolId) && m.DateCreatedUtc < lastPostCreatedAt && m.DateDeletedUtc == null).ProjectTo<FeedItem>(config).OrderByDescending(m => m.DateCreatedUtc).Take(20).ToListAsync();
            }
            else if (type == FeedType.ContentSource)
            {
                posts = await _repo.Posts.Where(m => m.ContentSourceId == id && m.DateCreatedUtc < lastPostCreatedAt && m.DateDeletedUtc == null).ProjectTo<FeedItem>(config).OrderByDescending(m => m.DateCreatedUtc).Take(20).ToListAsync();
            }
            else if (type == FeedType.Team)
            {
                posts = await _repo.Posts.Where(m => (m.Teams.Any(j => j.Id == id) || m.TeamId == id) && m.DateCreatedUtc < lastPostCreatedAt && m.DateDeletedUtc == null).ProjectTo<FeedItem>(config).OrderByDescending(m => m.DateCreatedUtc).Take(20).ToListAsync();
            }
            else if (type == FeedType.Sport)
            {
                //posts = await _repo.Posts.Where(m => m.Sports.Any(j => j.Id == id) && m.DateCreatedUtc < lastPostCreatedAt && m.DateDeletedUtc == null).ProjectTo<FeedItem>(config).OrderByDescending(m => m.DateCreatedUtc).Take(20).ToListAsync();
                posts = await _repo.Posts.Where(m => (m.Teams.Any(j => j.SportId == id) || m.Team.SportId == id || m.Sports.Any(j => j.Id == id) || m.SportId == id && m.DateDeletedUtc == null) && m.DateCreatedUtc < lastPostCreatedAt).ProjectTo<FeedItem>(config).OrderByDescending(m => m.DateCreatedUtc).Take(20).ToListAsync();
            }
            else if (type == FeedType.School)
            {
                posts = await _repo.Posts.Where(m => (m.Teams.Any(j => j.SchoolId == id) || m.Team.SchoolId == id || m.Schools.Any(j => j.Id == id) || m.SchoolId == id && m.DateDeletedUtc == null) && m.DateCreatedUtc < lastPostCreatedAt).ProjectTo<FeedItem>(config).OrderByDescending(m => m.DateCreatedUtc).Take(20).ToListAsync();
            }
            else if (type == FeedType.Event)
            {
                posts = await _repo.Posts.Where(m => m.Events.Any(j => j.Id == id) && m.DateCreatedUtc < lastPostCreatedAt && m.DateDeletedUtc == null).ProjectTo<FeedItem>(config).OrderByDescending(m => m.DateCreatedUtc).Take(20).ToListAsync();
            }
            else if (type == FeedType.SinglePost)
            {
                posts = await _repo.Posts.Where(m => m.Id == id && m.DateDeletedUtc == null).ProjectTo<FeedItem>(config).OrderByDescending(m => m.DateCreatedUtc).Take(1).ToListAsync();
            }

            //if (posts.Count > 0 && type == FeedType.Main)
            //{
            //    var utcNow = DateTime.UtcNow;
            //    var campaigns = await _repo.Campaigns.Where(m => m.StartUtc <= utcNow &&
            //                                                     m.EndUtc >= utcNow &&
            //                                                     m.CampaignStatus == CampaignStatus.Approved &&
            //                                                     (m.Teams.Any(j => j.DateDeletedUtc == null && followedTeams.Contains(j.Id)) ||
            //                                                      m.Schools.Any(j => j.DateDeletedUtc == null && followedSchools.Contains(j.Id)) ||
            //                                                      m.Sports.Any(j => j.DateDeletedUtc == null && followedSports.Contains(j.Id)))
            //    ).Select(m => new { m.Advertiser.CompanyName, m.Advertiser.LogoUrl, m.Description, m.Url, m.ImageUrl, m.Weight, m.ImageAspectRatio }).ToListAsync();

                //if (campaigns.Count > 0)
                //{
                //    // This block of code attempts take campaign weight into account.
                //    List<int> cumulativeWights = new List<int>();

                //    // The basic idea is we take all the campaigns, take their weights, and construct a new list where that should allow picking a campaign based on its weight.
                //    /* 
                //     * Quick Example:
                //     * Suppose three campaigns, campaignA has a weight 2 and campaignB has a weight of 3, campaignC has a weight of 4, the cumulativeWeights list will have values of { 2, 5, 9 },
                //     * Now you generate a random number between 1 - 9 inclusive.  if the number is 1-2 you would pick the first campaign (2 possible numbers) if the number is 3-5 you pick campaign
                //     * B (3 possible numbers) if the number is 6-9 you pick campaignC (4 possible numbers) thus its more likely the campaign with weight 4 will get choosen to be shown here.
                //     * */
                //    for (int i = 0; i < campaigns.Count; i++)
                //    {
                //        cumulativeWights.Add((i == 0 ? 0 : cumulativeWights[i - 1]) + campaigns[i].Weight);
                //    }
                    
                //    int random = new Random((int)DateTime.UtcNow.Ticks).Next(0, cumulativeWights[cumulativeWights.Count - 1] + 1);  // Adding one so we include the ending value
                //    int campaignIndex = cumulativeWights.FindIndex(m => m >= random);
                    
                //    var campaign = campaigns[campaignIndex];
                //    posts.Add(new FeedItem() { ImageUrl = campaign.ImageUrl, AdvertisementUrl = campaign.Url, Username = campaign.CompanyName, ProfileUrl = campaign.LogoUrl, Content = campaign.Description, ImageAspectRatio = campaign.ImageAspectRatio});
                //}
            //}
            return posts;
        }

        public async Task<FeedItem> BuildSingle(string postId, string userId)
        {
            var config = MapObjects(userId);
            return await _repo.Posts.Where(m => m.Id == postId).ProjectTo<FeedItem>(config).FirstOrDefaultAsync();
        }

        public async Task DeletePostAsync(string postId)
        {
            await _repo.Posts.DeleteAndSaveAsync(postId);
        }
    }
}
