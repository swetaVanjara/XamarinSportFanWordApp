using System.Net.Http.Headers;
using Fanword.Poco.Models;
using Mobile.Extensions.Extensions;
using Plugin.Dialog;
using PortableFileUploader.Portable;
using System.Diagnostics;
using Notifications.Mobile.Models;
using Notifications.Mobile.Service;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Fanword.Shared.Service;
using ModernHttpClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.Settings;
using Polly;
using Fanword.Shared.Helpers;
using Fanword.Shared.Models;

namespace Fanword.Shared
{
    public class ServiceApi : ServiceApiBase
    {
        #region Account Login/Regiser
        public async Task<AccessTokenResponse> Login(string username, string password)
        {
            AccessTokenResponse results = null;
            HttpClient c = new HttpClient(new NativeMessageHandler()) { BaseAddress = new Uri(PortalURL) };

            var vals = new List<KeyValuePair<string, string>>();
            vals.Add(new KeyValuePair<string, string>("username", username));
            vals.Add(new KeyValuePair<string, string>("password", password));
            vals.Add(new KeyValuePair<string, string>("grant_type", "password"));
            if (CrossConnectivity.Current.IsConnected)
            {
                var t = c.PostAsync(PortalURL + "/token", new FormUrlEncodedContent(vals));
                var message = await Policy
                .Handle<WebException>().Or<IOException>()
                .WaitAndRetryAsync(retryCount: 2, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () => await t);

                string content = await message.Content.ReadAsStringAsync();
                if (!message.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<JObject>(content);
                    throw new WebException(error["error_description"].ToString());
                }
                results = JsonConvert.DeserializeObject<AccessTokenResponse>(content);
                LocalStorage.SaveLogin(results);
            }
            else
            {
                throw new Exception("No internet connection.");
            }

            if (results == null)
            {
                throw new NullReferenceException("Results should not be null");
            }

            return results;
        }
        public async Task<AccessTokenResponse> RefreshToken(string refreshToken)
        {
            AccessTokenResponse results = null;
            HttpClient c = new HttpClient(new NativeMessageHandler()) { BaseAddress = new Uri(PortalURL) };

            var vals = new List<KeyValuePair<string, string>>();
            vals.Add(new KeyValuePair<string, string>("refresh_token", refreshToken));
            vals.Add(new KeyValuePair<string, string>("client_id", "self"));
            vals.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));
            if (CrossConnectivity.Current.IsConnected)
            {
                var t = c.PostAsync(PortalURL + "/token", new FormUrlEncodedContent(vals));
                var message = await Policy
                .Handle<WebException>().Or<IOException>()
                .WaitAndRetryAsync(retryCount: 2, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () => await t);

                string content = await message.Content.ReadAsStringAsync();
                if (!message.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<JObject>(content);
                    throw new WebException(error["error"].ToString());
                }
                results = JsonConvert.DeserializeObject<AccessTokenResponse>(content);
                LocalStorage.SaveLogin(results);
            }
            else
            {
                throw new Exception("No internet connection.");
            }

            if (results == null)
            {
                throw new NullReferenceException("Results should not be null");
            }

            return results;
        }

        public async Task ForgotPassword(string email)
        {
            HttpClient c = new HttpClient(new NativeMessageHandler()) { BaseAddress = new Uri(MvcPortalURL) };

            var vals = new List<KeyValuePair<string, string>>();
            vals.Add(new KeyValuePair<string, string>("Email", email));
            if (CrossConnectivity.Current.IsConnected)
            {
                var t = c.PostAsync(MvcPortalURL + "/account/forgotpassword", new FormUrlEncodedContent(vals));
                var message = await Policy
                .Handle<WebException>().Or<IOException>()
                .WaitAndRetryAsync(retryCount: 2, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () => await t);

                string content = await message.Content.ReadAsStringAsync();
                if (!message.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<JObject>(content);
                    throw new WebException(error["error"].ToString());
                }
            }
            else
            {
                throw new Exception("No internet connection.");
            }
        }

        public async Task RefreshTokenSuccess(string accessToken, string oldToken)
        {
            HttpClient c = new HttpClient(new NativeMessageHandler()) { BaseAddress = new Uri(PortalURL) };
            c.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            if (CrossConnectivity.Current.IsConnected)
            {
                var t = c.GetAsync(PortalURL + $"/api/account/TokenRefreshSuccess?oldToken={oldToken}");
                var message = await Policy
                .Handle<WebException>().Or<IOException>()
                .WaitAndRetryAsync(retryCount: 2, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () => await t);

                string content = await message.Content.ReadAsStringAsync();
                if (!message.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<JObject>(content);
                    throw new WebException(error["error"].ToString());
                }
            }
            else
            {
                throw new Exception("No internet connection.");
            }
        }

        public async Task<AccessTokenResponse> Register(string firstName, string lastName, string username, string password, string confirmPassword)
        {
            AccessTokenResponse results = null;
            HttpClient c = new HttpClient(new NativeMessageHandler()) { BaseAddress = new Uri(PortalURL) };

            var vals = new List<KeyValuePair<string, string>>();
            vals.Add(new KeyValuePair<string, string>("Email", username));
            vals.Add(new KeyValuePair<string, string>("Password", password));
            vals.Add(new KeyValuePair<string, string>("ConfirmPassword", confirmPassword));
            vals.Add(new KeyValuePair<string, string>("FirstName", firstName));
            vals.Add(new KeyValuePair<string, string>("LastName", lastName));

            if (CrossConnectivity.Current.IsConnected)
            {
                var t = c.PostAsync(PortalURL + "/api/account/register", new FormUrlEncodedContent(vals));
                var message = await Policy
                .Handle<WebException>().Or<IOException>()
                .WaitAndRetryAsync(retryCount: 2, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () => await t);

                string content = await message.Content.ReadAsStringAsync();
                if (!message.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<JObject>(content);

                    string errors = "";
                    foreach (var err in error["ModelState"])
                    {
                        errors += err.Values().FirstOrDefault() + "\n";
                    }

                    errors = errors.Trim();

                    throw new WebException(errors);
                }
                results = JsonConvert.DeserializeObject<AccessTokenResponse>(content);
                LocalStorage.SaveLogin(results);
            }
            else
            {
                throw new Exception("No internet connection.");
            }

            if (results == null)
            {
                throw new NullReferenceException("Results should not be null");
            }

            return results;
        }

        public async Task<AccessTokenResponse> RegisterFacebook(string facebookToken)
        {
            var response = await Get<AccessTokenResponse>("/api/account/RegisterFacebook", new Dictionary<string, object>() { { "facebookToken", facebookToken } });
            LocalStorage.SaveLogin(response);
            return response;
        }

        public async Task<User> GetMyUser()
        {
            var response = await Get<User>("/api/users/MyUser", new Dictionary<string, object>() { });
            LocalStorage.UpdateUser(response);
            return response;
        }
        public async Task<User> GetUser(string userId) {
            var response = await Get<User>("/api/users/GetUser", new Dictionary<string, object>() { { "userId", userId } });
            return response;
        }

        #endregion

        #region Athlete

        public async Task<AthleteTeamSearch> GetAthleteTeams(string search)
        {
            var dictionary = new Dictionary<string, object>() { { "search", search } };
            return await Get<AthleteTeamSearch>("/api/teams/AthleteTeams", dictionary);
        }

        public async Task<User> SaveAthlete(User user)
        {
            user.ProfileContainer = string.IsNullOrEmpty(user.ProfileUrl) ? null : "userprofiles";
            return await Post<User>("/api/Athletes/SaveUser", user);
        }
        public async Task<AthleteUser> SaveAthleteUser(AthleteUser athleteUser)
        {
            athleteUser.User.ProfileContainer = string.IsNullOrEmpty(athleteUser.User.ProfileUrl) ? null : "userprofiles";
            return await Post<AthleteUser>("/api/Users/SaveAthleteUser", athleteUser);
        }
        public async Task DeleteAthlete()
        {
            await Delete("/api/Users/DeleteAthlete", new Dictionary<string, object>());
            var user = CrossSettings.Current.GetValueOrDefaultJson<User>("User");
            user.AthleteEndDateUtc = null;
            user.AthleteProfileUrl = null;
            user.AthleteSchool = null;
            user.AthleteSport = null;
            user.AthleteTeamId = null;
            user.AthleteStartDateUtc = new DateTime();
            CrossSettings.Current.AddOrUpdateJson("User", user);
        }

        public async Task<string> SaveAthleteUser(string filePath, DateTime fromDate, DateTime? untilDate, string teamId)
        {
            AthleteUser athleteUser = new AthleteUser();
            athleteUser.User = CrossSettings.Current.GetValueOrDefaultJson<User>("User");
            if (!string.IsNullOrEmpty(filePath))
            {
                var azureFileData = await AppMedia.UploadMedia(filePath, "/api/Uploads/UserProfilePhoto");
                if (azureFileData == null)
                {
                    return "There was a problem while uploading the file";
                }
                athleteUser.User.ProfileUrl = azureFileData.Url;
                athleteUser.User.ProfileBlob = azureFileData.BlobName;

            }

            athleteUser.Athlete = new Athlete();
            athleteUser.Athlete.StartUtc = fromDate;
            athleteUser.Athlete.EndUtc = untilDate;
            athleteUser.Athlete.TeamId = teamId;

            // Save profile and athlete
            try
            {
                var result = await new ServiceApi().SaveAthleteUser(athleteUser);
                CrossSettings.Current.AddOrUpdateJson("User", result.User);
                CrossSettings.Current.AddOrUpdateJson("Athlete", result.Athlete);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return null;
        }
        #endregion

        #region User
        public async Task<User> SaveUser(User user)
        {
            user.ProfileContainer = string.IsNullOrEmpty(user.ProfileUrl) ? null : "userprofiles";
            return await Post<User>("/api/Users/SaveUser", user);
        }

        public async Task<string> SaveUser(string filePath)
        {
            var user = CrossSettings.Current.GetValueOrDefaultJson<User>("User");

            if (!string.IsNullOrEmpty(filePath))
            {
                var azureFileData = await AppMedia.UploadMedia(filePath, "/api/Uploads/UserProfilePhoto");
                if (azureFileData == null)
                {
                    return "There was a problem while uploading the file";
                }

                user.ProfileUrl = azureFileData.Url;
                user.ProfileBlob = azureFileData.BlobName;
            }

            // Save profile
            var result = await new ServiceApi().SaveUser(user);
            CrossSettings.Current.AddOrUpdateJson("User", result);

            return null;
        }

        #endregion

        #region Feed
        public async Task<List<FeedItem>> GetFeed(string lastPostCreateDate, string id, FeedType type)
        {
            return await Get<List<FeedItem>>("/api/Feed/", new Dictionary<string, object>() { { "lastPostCreatedAt", lastPostCreateDate.ToString() }, { "id", id ?? "" }, { "type", type } });
        }

        public async Task DeletePost(string postId)
        {
            await Delete("/api/Feed/", new Dictionary<string, object>() { { "postId", postId } });
        }

        public async Task<FeedItem> GetFeedItem(string postId)
        {
            return await Get<FeedItem>("/api/Feed/", new Dictionary<string, object>() { { "postId", postId } });
        }

        #endregion

        #region Post Likes
        public async Task<List<PostLike>> GetLikes(string postId)
        {
            return await Get<List<PostLike>>("/api/PostLikes/", new Dictionary<string, object>() { { "postId", postId } });
        }

        public async Task LikePost(string postId)
        {
            await Get("/api/PostLikes/Like", new Dictionary<string, object>() { { "postId", postId } });
        }

        public async Task UnlikePost(string postId)
        {
            await Get("/api/PostLikes/Unlike", new Dictionary<string, object>() { { "postId", postId } });
        }
        #endregion

        #region Post Tags
        public async Task<List<PostTag>> GetTags(string postId)
        {
            return await Get<List<PostTag>>("/api/PostTags/", new Dictionary<string, object>() { { "postId", postId } });
        }
        #endregion

        #region Post Shares
        public async Task<List<PostShare>> GetShares(string postId)
        {
            return await Get<List<PostShare>>("/api/PostShares/", new Dictionary<string, object>() { { "postId", postId } });
        }
        public async Task SaveShare(string postId)
        {
            await Get("/api/PostShares/SaveShare", new Dictionary<string, object>() { { "postId", postId } });
        }
        #endregion

        #region Comment Likes
        public async Task LikeComment(string commentId)
        {
            await Get("/api/CommentLikes/Like", new Dictionary<string, object>() { { "commentId", commentId } });
        }

        public async Task UnlikeComment(string commentId)
        {
            await Get("/api/CommentLikes/Unlike", new Dictionary<string, object>() { { "commentId", commentId } });
        }
        #endregion

        #region Post Comments
        public async Task<List<Comment>> GetComments(string postId)
        {
            return await Get<List<Comment>>("/api/Comments/", new Dictionary<string, object>() { { "postId", postId } });
        }

        public async Task SaveComment(string content, string postId, string parentCommentId)
        {
            var comment = new Comment();
            comment.Content = content;
            comment.PostId = postId;
            comment.ParentCommentId = parentCommentId;
            await Post("/api/Comments/", comment);
        }
        #endregion

        #region Rankings

        public async Task<List<Ranking>> GetRankings(FollowingFilterModel filter)
        {
            var data = await Post<List<Ranking>>("/api/Rankings/", filter);
         
            return data;
            
        }

        #endregion

        #region Follows
        public async Task FollowTeam(string teamId)
        {
            await Get("/api/Follows/FollowTeam/", new Dictionary<string, object>() { { "teamId", teamId } });
        }

        public async Task UnfollowTeam(string teamId)
        {
            await Get("/api/Follows/UnfollowTeam/", new Dictionary<string, object>() { { "teamId", teamId } });
        }

        public async Task FollowUser(string userId)
        {
            await Get("/api/Follows/FollowUser/", new Dictionary<string, object>() { { "userId", userId } });
        }

        public async Task UnfollowUser(string userId)
        {
            await Get("/api/Follows/UnfollowUser/", new Dictionary<string, object>() { { "userId", userId } });
        }

        public async Task FollowContentSource(string contentSourceId)
        {
            await Get("/api/Follows/FollowContentSource/", new Dictionary<string, object>() { { "contentSourceId", contentSourceId } });
        }

        public async Task UnfollowContentSource(string contentSourceId)
        {
            await Get("/api/Follows/UnfollowContentSource/", new Dictionary<string, object>() { { "contentSourceId", contentSourceId } });
        }

        public async Task FollowSport(string sportId)
        {
            await Get("/api/Follows/FollowSport/", new Dictionary<string, object>() { { "sportId", sportId } });
        }

        public async Task UnfollowSport(string sportId)
        {
            await Get("/api/Follows/UnfollowSport/", new Dictionary<string, object>() { { "sportId", sportId } });
        }

        public async Task FollowSchool(string schoolId)
        {
            await Get("/api/Follows/FollowSchool/", new Dictionary<string, object>() { { "schoolId", schoolId } });
        }

        public async Task UnfollowSchool(string schoolId)
        {
            await Get("/api/Follows/UnfollowSchool/", new Dictionary<string, object>() { { "schoolId", schoolId } });
        }

        #endregion

        #region Scores
        public async Task<List<ScoreModel>> GetScores(ScoresFilterModel filter)
        {
            var scores = await Post<List<ScoreModel>>("/api/Scores/", filter);

            //var grouped = scores.GroupBy(m => m.EventDate).ToDictionary(t => t.Key, t => t.ToList());
            //var items = new List<ScoreModel>();
            //foreach (var group in grouped)
            //{
            //    items.Add(new ScoreModel()
            //    {
            //        EventDate = group.Key,
            //        TimezoneId = group.Value.FirstOrDefault().TimezoneId,
            //        TeamCount = group.Value.Count()
            //    });
            //    items.AddRange(group.Value);
            //}

            return scores;
        }
        #endregion

        #region CreatePostProfileSearch
        public async Task<ProfileSearch> SearchProfiles(string text, string id, FeedType type)
        {
            return await Get<ProfileSearch>("/api/ProfileSearch/Search", new Dictionary<string, object>() { { "text", text }, { "id", id ?? "" }, { "type", type } });
        }

        public async Task<EventSearch> SearchEvents(DateTime utcMidnight)
        {
            return await Get<EventSearch>("/api/EventSearch/Search", new Dictionary<string, object>() { { "time", utcMidnight } });
        }
        #endregion

        #region Posts
        public async Task<Post> GetPost(string postId)
        {
            return await Get<Post>("/api/Posts", new Dictionary<string, object>() { { "postId", postId } });
        }

        public async Task SavePost(Post post)
        {
            // upload extras
            if (post.Images.Any(m => string.IsNullOrEmpty(m.Id)))
            {
                foreach (var image in post.Images)
                {
                    var azureFileData = await AppMedia.UploadMedia(image.Url, "/api/Uploads/PostPhoto");
                    if (azureFileData == null)
                    {
                        throw new Exception("Error uploading file");
                    }

                    image.Url = azureFileData.Url;
                    image.Blob = azureFileData.BlobName;
                    image.Container = "posts";

                }
                await Post("/api/Posts", post);
            }
            else if (post.Videos.Any(m => string.IsNullOrEmpty(m.Id)))
            {
                Task.Run(async () =>
                {
                    foreach (var video in post.Videos)
                    {
                        var imageAzureFileData = await AppMedia.UploadMedia(video.ImageUrl, "/api/Uploads/PostPhoto");
                        if (imageAzureFileData == null)
                        {
                            throw new Exception("Error uploading image");
                        }

                        var azureFileData = await AppMedia.UploadMedia(video.Url, "/api/Uploads/PostPhoto");
                        if (azureFileData == null)
                        {
                            throw new Exception("Error uploading video");
                        }

                        video.Url = azureFileData.Url;
                        video.Blob = azureFileData.BlobName;
                        video.Container = "posts";
                        video.ImageUrl = imageAzureFileData.Url;
                        video.ImageBlob = imageAzureFileData.BlobName;
                        video.ImageContainer = "posts";
                    }

                    await Post("/api/Posts", post);
                });

            }
            else
            {
                await Post("/api/Posts", post);
            }

        }

        public async Task Clone(string postId)
        {
            await Get("/api/Posts/Clone", new Dictionary<string, object>() { { "postId", postId } });
        }
        #endregion

        #region MyProfileDetails
        public async Task<MyProfileDetails> GetMyProfileDetails()
        {
            return await Get<MyProfileDetails>("/api/MyProfileDetails/", new Dictionary<string, object>());
        }
        #endregion

        #region UserProfile
        public async Task<UserProfile> GetUserProfile(string userId)
        {
            return await Get<UserProfile>("/api/UserProfile/", new Dictionary<string, object>() { { "userId", userId } });
        }
        #endregion

        #region ContentSourceProfile
        public async Task<ContentSourceProfile> GetContentSourceProfile(string contentSourceId)
        {
            return await Get<ContentSourceProfile>("/api/ContentSourceProfile/", new Dictionary<string, object>() { { "contentSourceId", contentSourceId } });
        }
        #endregion

        #region TeamProfile
        public async Task<TeamProfile> GetTeamProfile(string teamId)
        {
            return await Get<TeamProfile>("/api/TeamProfile/", new Dictionary<string, object>() { { "teamId", teamId } });
        }

        public async Task RequestTeamAdmin(string teamId)
        {
            await Get("/api/TeamProfile/RequestAdmin", new Dictionary<string, object>() { { "teamId", teamId } });
        }

        public async Task<List<TeamRanking>> GetTeamRankings(string teamId)
        {
            return await Get<List<TeamRanking>>("/api/TeamRankings/", new Dictionary<string, object>() { { "teamId", teamId } });
        }

        public async Task<List<AthleteItem>> GetAthletesForTeam(string teamId)
        {
            return await Get<List<AthleteItem>>("/api/AthleteItems/GetByTeam", new Dictionary<string, object>() { { "teamId", teamId } });
        }

        #endregion

        #region SportProfile
        public async Task<SportProfile> GetSportProfile(string sportId)
        {
            return await Get<SportProfile>("/api/SportProfile/", new Dictionary<string, object>() { { "sportId", sportId } });
        }

        public async Task<List<Ranking>> GetSportRankings(string sportId)
        {
            return await GetRankings(new FollowingFilterModel() { SportId = sportId });
        }


        public async Task<List<TeamProfile>> GetTeamsForSport(string sportId)
        {
            return await Get<List<TeamProfile>>("/api/TeamProfile/GetBySport", new Dictionary<string, object>() { { "sportId", sportId } });
        }

        public async Task<List<AthleteItem>> GetAthletesForSport(string sportId)
        {
            return await Get<List<AthleteItem>>("/api/AthleteItems/GetBySport", new Dictionary<string, object>() { { "sportId", sportId } });
        }


        #endregion

        #region SchoolProfile
        public async Task<SchoolProfile> GetSchoolProfile(string schoolId)
        {
            return await Get<SchoolProfile>("/api/SchoolProfile/", new Dictionary<string, object>() { { "schoolId", schoolId } });
        }

        public async Task<List<TeamProfile>> GetTeamsForSchool(string schoolId)
        {
            return await Get<List<TeamProfile>>("/api/TeamProfile/GetBySchool", new Dictionary<string, object>() { { "schoolId", schoolId } });
        }

        public async Task<List<AthleteItem>> GetAthletesForSchool(string schoolId)
        {
            return await Get<List<AthleteItem>>("/api/AthleteItems/GetBySchool", new Dictionary<string, object>() { { "schoolId", schoolId } });
        }

        public async Task<List<Ranking>> GetSchoolRankings(string schoolId)
        {
            return await GetRankings(new FollowingFilterModel() { SchoolId = schoolId });
        }

        public async Task RequestSchoolAdmin(string schoolId)
        {
            await Get("/api/SchoolProfile/RequestAdmin", new Dictionary<string, object>() { { "schoolId", schoolId } });
        }
        #endregion

        #region Notifications
        public async Task<List<UserNotification>> GetNotifcations()
        {
            var accessToken = CrossSettings.Current.GetValueOrDefault("AccessToken", "");
            return (await new NotificationService(accessToken).GetAllAsync()).OrderByDescending(m => m.DateCreatedUtc).Select(m => new UserNotification() { IsRead = m.DateReadUtc != null, DateCreatedUtc = m.DateCreatedUtc, Id = m.Id, Title = m.Title, Message = NotificationContentHelper.GetContentFromNotification(m), ProfileUrl = m.UserMetaData["ProfileUrl"], MetaData = m.MetaData, UserMetaData = m.UserMetaData }).ToList();
        }

        public async Task MarkNotificationsAsRead()
        {
            var accessToken = CrossSettings.Current.GetValueOrDefault("AccessToken", "");
            await new NotificationService(accessToken).MarkAllAsReadAsync();
        }
        #endregion

        #region Search
        public async Task<GlobalSearch> Search(string filter)
        {
            var data = await Get<GlobalSearch>("/api/Search/", new Dictionary<string, object>() { { "filter", filter ?? "" } });
            if (string.IsNullOrEmpty(filter))
            {
                data.Results = data.Results.OrderByDescending(m => m.Followers).ThenBy(m => m.Title).ThenBy(m => m.Subtitle).ToList();
                data.Results.Insert(0, new GlobalSearchItem() { Title = "Popular" });
            }
            else
            {
                var grouped = data.Results.GroupBy(m => m.Type).OrderBy(m => m.Key).ToList();
                List<GlobalSearchItem> newData = new List<GlobalSearchItem>();
                foreach (var group in grouped)
                {
                    newData.Add(new GlobalSearchItem() { Type = group.Key });
                    newData.AddRange(group.OrderBy(m => m.Title).ThenBy(m => m.Subtitle));
                }
                data.Results = newData;
            }
            return data;
        }

        public async Task<GlobalSearch> SearchByType(string filter, FeedType type)
        {
            return await Get<GlobalSearch>("/api/Search/", new Dictionary<string, object>() { { "filter", filter }, { "type", (int)type } });
        }
        #endregion

        #region Favorites
        public async Task<Favorites> Favorites()
        {
            return await Get<Favorites>("/api/favorites/", new Dictionary<string, object>());
        }
        #endregion

        #region EventProfile
        public async Task<EventProfile> GetEventProfile(string eventId)
        {
            return await Get<EventProfile>("/api/EventProfile/", new Dictionary<string, object>() { { "eventId", eventId } });
        }
        #endregion

        #region EventTeams
        public async Task<List<EventTeam>> GetEventTeams(string eventId)
        {
            var teams = await Get<List<EventTeam>>("/api/EventTeams/", new Dictionary<string, object>() { { "eventId", eventId } });
            bool orderByNumber = teams.FirstOrDefault()?.Score?.ToCharArray()?.All(char.IsNumber) ?? false;

            if (orderByNumber)
            {
                teams = teams.OrderByDescending(m =>
                {
                    try
                    {
                        return int.Parse(new string(m.Score.ToCharArray().Where(char.IsNumber).ToArray()));
                    }
                    catch
                    {
                        return 0;
                    }
                }).ToList();
            }
            else
            {
                teams = teams.OrderBy(m =>
                {
                    try
                    {
                        return int.Parse(new string(m.Score.ToCharArray().Where(char.IsNumber).ToArray()));
                    }
                    catch
                    {
                        return 0;
                    }
                }).ToList();
            }

            return teams;
        }
        #endregion

        #region Followers
        public async Task<List<Follower>> Followers(string id, FeedType type)
        {
            if (type == FeedType.User)
            {
                return await Get<List<Follower>>("/api/Followers/User", new Dictionary<string, object>() { { "userId", id } });
            }
            if (type == FeedType.Team)
            {
                return await Get<List<Follower>>("/api/Followers/Team", new Dictionary<string, object>() { { "teamId", id } });
            }
            if (type == FeedType.School)
            {
                return await Get<List<Follower>>("/api/Followers/School", new Dictionary<string, object>() { { "schoolId", id } });
            }
            if (type == FeedType.Sport)
            {
                return await Get<List<Follower>>("/api/Followers/Sport", new Dictionary<string, object>() { { "sportId", id } });
            }
            if (type == FeedType.ContentSource)
            {
                return await Get<List<Follower>>("/api/Followers/ContentSource", new Dictionary<string, object>() { { "contentSourceId", id } });
            }
            return null;
        }

        #endregion

    }
}
