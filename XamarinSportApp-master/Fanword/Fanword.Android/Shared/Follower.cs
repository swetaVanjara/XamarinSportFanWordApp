using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fanword.Poco.Interfaces;
using Fanword.Poco.Models;
using Fanword.Shared;
using Mobile.Extensions.Android.Extensions;
using Mobile.Extensions.Android.Interfaces;

namespace Fanword.Android.Shared
{
    public class Follower
    {
        public static void FollowToggle(IActivityProgresDialog activity, Button btnFollow, IFollowing model, string id, FeedType type, Action<bool> succeeded = null)
        {
            var oldText = btnFollow.Text;
            btnFollow.Text = "Loading";
            if (model.IsFollowing)
            {
                Task apiTask = null;
                if(type == FeedType.Team)
                    apiTask = new ServiceApi().UnfollowTeam(id);
                else if (type == FeedType.User)
                    apiTask = new ServiceApi().UnfollowUser(id);
                else if (type == FeedType.Sport)
                    apiTask = new ServiceApi().UnfollowSport(id);
                else if (type == FeedType.ContentSource)
                    apiTask = new ServiceApi().UnfollowContentSource(id);
                else if (type == FeedType.School)
                    apiTask = new ServiceApi().UnfollowSchool(id);

                apiTask.HandleError(activity, true, () =>
                {
                    btnFollow.Text = oldText;
                });
                apiTask.OnSucess(activity, response =>
                {
                    model.IsFollowing = false;
                    Views.SetFollowed(btnFollow, model.IsFollowing);
                    succeeded?.Invoke(false);
                });
            }
            else
            {
                Task apiTask = null;
                if (type == FeedType.Team)
                    apiTask = new ServiceApi().FollowTeam(id);
                else if (type == FeedType.User)
                    apiTask = new ServiceApi().FollowUser(id);
                else if (type == FeedType.Sport)
                    apiTask = new ServiceApi().FollowSport(id);
                else if (type == FeedType.ContentSource)
                    apiTask = new ServiceApi().FollowContentSource(id);
                else if (type == FeedType.School)
                    apiTask = new ServiceApi().FollowSchool(id);
                apiTask.HandleError(activity, true, () =>
                {
                    btnFollow.Text = oldText;
                });
                apiTask.OnSucess(activity, response =>
                {
                    model.IsFollowing = true;
                    Views.SetFollowed(btnFollow, model.IsFollowing);
                    succeeded?.Invoke(true);
                });
            }
        }
    }
}