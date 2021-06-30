using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Fanword.Android.Activities.ContentSource;
using Fanword.Android.Activities.EventsProfile;
using Fanword.Android.Activities.TeamProfile;
using Plugin.CurrentActivity;
using Fanword.Android.Activities.SportProfile;
using Plugin.Settings;
using Mobile.Extensions.Extensions;
using Fanword.Poco.Models;
using Fanword.Android.Activities.UserProfile;
using Fanword.Android.Activities.MyProfile;
using Fanword.Android.Activities.PostDetails;
using Fanword.Android.Activities.SchoolProfile;
using Fanword.Android.Activities.ViewPost;
using Fanword.Shared.Models;
using Fanword.Android.Activities.ViewNotification;
using System.Diagnostics;

namespace Fanword.Android.Shared
{
    public class Navigator
    {
        public static void GoToTeamProflie(string teamId, bool toRankings = false)
        {
            var activity = CrossCurrentActivity.Current.Activity;
			Intent intent = new Intent(activity, typeof(TeamProfileActivity));
			intent.PutExtra("TeamId", teamId);
			intent.PutExtra("GoToRankings", toRankings);
			activity.StartActivity(intent);
        }

		public static void GoToSportProflie(string sportId, bool toRankings = false)
		{
			var activity = CrossCurrentActivity.Current.Activity;
			Intent intent = new Intent(activity, typeof(SportProfileActivity));
			intent.PutExtra("SportId", sportId);
            intent.PutExtra("GoToRankings", toRankings);
			activity.StartActivity(intent);
		}

        public static void GoToSchoolProflie(string schoolId)
        {
            var activity = CrossCurrentActivity.Current.Activity;
            Intent intent = new Intent(activity, typeof(SchoolProfileActivity));
            intent.PutExtra("SchoolId", schoolId);
            //intent.PutExtra("GoToRankings", toRankings);
            activity.StartActivity(intent);
        }

        public static void GoToContentSource(string contentSourceId)
        {
            var activity = CrossCurrentActivity.Current.Activity;
            Intent intent = new Intent(activity, typeof(ContentSourceActivity));
            intent.PutExtra("ContentSourceId", contentSourceId);
            activity.StartActivity(intent);
        }

        public static void GoToEventProfile(string eventId)
        {
            var activity = CrossCurrentActivity.Current.Activity ?? Application.Context;
            Intent intent = new Intent(activity, typeof(EventProfileActivity));
            intent.PutExtra("EventId", eventId);
            activity.StartActivity(intent);
        }

        public static void GoToUserProflie(string userId)
		{
			Context activity = CrossCurrentActivity.Current.Activity;
			bool newTask = false;
			if (activity == null)
			{
				activity = Application.Context;
				newTask = true;
			}
            var user = CrossSettings.Current.GetValueOrDefaultJson<User>("User");
            if(userId == user.Id)
            {
				Intent intent = new Intent(activity, typeof(MyProfileActivity));
				if (newTask)
				{
					intent.SetFlags(ActivityFlags.NewTask);
				}
				activity.StartActivity(intent);
            }
            else
            {
				Intent intent = new Intent(activity, typeof(UserProfileActivity));
				intent.PutExtra("UserId", userId);
				if (newTask)
				{
					intent.SetFlags(ActivityFlags.NewTask);
				}
				activity.StartActivity(intent);
            }

		}

        public static void HandleNotificationTap(Dictionary<string, string> metaData, string title, string message)
        {
            Context activity = CrossCurrentActivity.Current.Activity;
            bool newTask = false;
            if (activity == null)
            {
                activity = Application.Context;
                newTask = true;
            }
            if(metaData != null){
            if (metaData[MetaDataKeys.UserNotificationType] == UserNotificationType.Like.ToString())
            {
                Intent intent = new Intent(activity, typeof(ViewPostActivity));
                intent.PutExtra("PostId", metaData[MetaDataKeys.PostId]);
                if (newTask)
                {
                    intent.SetFlags(ActivityFlags.NewTask);
                }
                activity.StartActivity(intent);
            }
            else if (metaData[MetaDataKeys.UserNotificationType] == UserNotificationType.Comment.ToString())
            {
                Intent intent = new Intent(activity, typeof(PostDetailsActivity));
                intent.PutExtra("PostId", metaData[MetaDataKeys.PostId]);
                intent.PutExtra("Fragment", "Comments");
                if (newTask)
                {
                    intent.SetFlags(ActivityFlags.NewTask);
                }
                activity.StartActivity(intent);
            }
            else if (metaData[MetaDataKeys.UserNotificationType] == UserNotificationType.Follow.ToString())
            {
                GoToUserProflie(metaData[MetaDataKeys.FromId]);
            }
            else
            {
                try
                {
                    Intent intent = new Intent(activity, typeof(ViewNotificationActivity));
                    intent.PutExtra("Title", title);
                    var content = metaData[MetaDataKeys.NewsNotificationMessage];
                    intent.PutExtra("Message", content);
                    if (newTask)
                    {
                        intent.SetFlags(ActivityFlags.NewTask);
                    }
                    activity.StartActivity(intent);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
        }
    }

    public class Logger 
    {
        public static void Log(string text)
        {
            var existing = CrossSettings.Current.GetValueOrDefault("Log", "");
            existing += "\n\n" + text;
            CrossSettings.Current.AddOrUpdateValue("Log", existing);
        }
    }
}


