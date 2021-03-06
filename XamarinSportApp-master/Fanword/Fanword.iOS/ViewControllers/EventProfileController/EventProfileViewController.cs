// This file has been autogenerated from a class added in the UI designer.

using System;
using Fanword.Poco.Models;
using FFImageLoading.Transformations;
using Foundation;
using UIKit;
using Mobile.Extensions.iOS.Extensions;
using Plugin.Settings;
using Mobile.Extensions.Extensions;
using FFImageLoading;
using Fanword.Shared;
using Fanword.Shared.Helpers;
using Fanword.iOS.Shared;
using System.Globalization;
using TimeZoneNames;

namespace Fanword.iOS
{
	public partial class EventProfileViewController : BaseViewController
	{
        public string EventId;
		FeedViewController feedController;
        EventProfile profile;
		public EventProfileViewController (IntPtr handle) : base (handle)
		{

		}

		public override UIStatusBarStyle PreferredStatusBarStyle()
		{
			return UIStatusBarStyle.LightContent;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			btnBack.TouchUpInside += (sender, e) => NavigationController.PopViewController(true);

			feedController = Storyboard.InstantiateViewController<FeedViewController>();
			feedController.HeaderView = vwHeader;
			feedController.Type = FeedType.Event;
            feedController.Id = EventId;
			feedController.RefreshStarted += (sender, e) => GetData();
			AddChildViewController(feedController);
			View.Add(feedController.View);
			var f = feedController.View.Frame;
			f.X = 0;
			f.Y = 70;
			f.Width = UIScreen.MainScreen.Bounds.Width;
			f.Height = UIScreen.MainScreen.Bounds.Height;
			feedController.View.Frame = f;
			feedController.HideAddPost();
            vwManyTeams.Hidden = true;
			GetData();

            btnShowTeams.TouchUpInside += (sender, e) => 
            {
                if (profile != null)
                {
					var controller = Storyboard.InstantiateViewController<ShowTeamsViewController>();
                    controller.EventId = profile.Id;
					NavigationController.PushViewController(controller, true);
                }
            };

            imgRight.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                if (profile != null && !string.IsNullOrEmpty(profile.TicketUrl))
                {
                    Links.OpenUrl(profile.TicketUrl);
                }
            }));
		}

		void GetData()
		{
            DateTime eventDate = DateTime.Now;
			var apiTask = new ServiceApi().GetEventProfile(EventId);
			apiTask.HandleError();
			apiTask.OnSucess(response =>
			{
				this.profile = response.Result;
				lblTitle.Text = response.Result.Name;
				lblSport.Text = response.Result.SportName;
				lblLocation.Text = response.Result.Location;
                eventDate = ConvertToUTC(response.Result.DateOfEventUtc, response.Result.TimezoneId);
                lblDate.Text = eventDate.ToString("D");
                if(response.Result.IsTbd)
                {
                    lblTime.Text = "TBD";
                }
                else
                {
                    string lang = CultureInfo.CurrentCulture.Name;
                    var abbreviations = TZNames.GetAbbreviationsForTimeZone(response.Result.TimezoneId, lang);

                    lblTime.Text = eventDate.ToString("h:mm tt") + " " + abbreviations.Standard;
                }

				lblName.Text = response.Result.Name;
				lblEventName.Text = profile.Name;

				if (!string.IsNullOrEmpty(profile.Team1Url))
					ImageService.Instance.LoadUrl(profile.Team1Url).Retry(3, 300).Into(imgTeam1);

				if (!string.IsNullOrEmpty(profile.Team2Url))
					ImageService.Instance.LoadUrl(profile.Team2Url).Retry(3, 300).Into(imgTeam2);

				lblTeam1Name.Text = profile.Team1Name;
				lblTeam2Name.Text = profile.Team2Name;

                imgRight.UserInteractionEnabled = true;

				if (string.IsNullOrEmpty(profile.TicketUrl))
				{
                    imgRight.Hidden = true;
				}

				if (profile.DateOfEventUtc <= DateTime.UtcNow)
				{
					if (profile.TeamCount == 2)
					{
                        lcRightWidth.Constant = 0;
					}
					else
					{
						imgRight.Hidden = false;
					}

                    imgRight.Image = null;
					if (!string.IsNullOrEmpty(profile.WinningTeamUrl))
						ImageService.Instance.LoadUrl(profile.WinningTeamUrl).Retry(3, 300).Into(imgRight);

					imgRight.UserInteractionEnabled = false;

				}

				var boldAttributes = new UIStringAttributes();
				boldAttributes.Font = UIFont.BoldSystemFontOfSize(btnShowTeams.Font.PointSize);
				boldAttributes.ForegroundColor = UIColor.FromRGB(144, 144, 144);

				var regularAttributes = new UIStringAttributes();
				regularAttributes.Font = UIFont.SystemFontOfSize(btnShowTeams.Font.PointSize);
				regularAttributes.ForegroundColor = UIColor.FromRGB(144, 144, 144);

				NSMutableAttributedString attributedString = new NSMutableAttributedString("Show me all ", regularAttributes);
				attributedString.Append(new NSMutableAttributedString(profile.TeamCount.ToString() + " teams", boldAttributes));
				btnShowTeams.SetAttributedTitle(attributedString, UIControlState.Normal);

				if (profile.TeamCount == 2)
				{
                    vwTwoTeams.Hidden = false;
                    vwManyTeams.Hidden = true;
				}
				else
				{
                    vwTwoTeams.Hidden = true;
                    vwManyTeams.Hidden = false;
				}

				lblPosts.Text = LargeValueHelper.GetString(response.Result.Posts);

			    lblTeam1Score.Text = response.Result.Team1Score;
			    lblTeam2Score.Text = response.Result.Team2Score;
            });
		}

        public DateTime ConvertToUTC(DateTime dd, string timezoneId)
        {
            DateTime eventDate = dd;

            if (!string.IsNullOrEmpty(timezoneId))
            {
                TimeZoneInfo zoneInfo;
                try
                {
                    zoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                }
                catch (TimeZoneNotFoundException)
                {
                    zoneInfo = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
                }

                if (zoneInfo.StandardName == TimeZone.CurrentTimeZone.StandardName)
                {
                    eventDate = eventDate.ToLocalTime();
                }
                else if (timezoneId.Contains("Central"))
                {
                    eventDate = dd.AddHours(-6);
                }
                else
                {
                    eventDate = TimeZoneInfo.ConvertTimeFromUtc(dd, zoneInfo);
                }
            }
            return eventDate;
        }
	}
}
