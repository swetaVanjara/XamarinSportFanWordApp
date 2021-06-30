using System;
using Plugin.Settings;
using Mobile.Extensions.Extensions;
using Fanword.Poco.Models;
namespace Fanword.Shared.Helpers
{
    public class TutorialHelper
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public string AndroidIcon { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string ButtonText { get; set; }

        public TutorialHelper()
        {
            ButtonText = "Got it, thanks!";
        }

        public static TutorialHelper Welcome => new TutorialHelper() { AndroidIcon = "icon_loading_image", Icon = "FanwordLoading", Id = "Welcome", Title = "Welcome " + CrossSettings.Current.GetValueOrDefaultJson<User>("User").FirstName + " " + CrossSettings.Current.GetValueOrDefaultJson<User>("User").LastName, Subtitle = "Thank you for downloading our app and joining us on our path to support and celebrate College Sports. Are you ready to get started?", ButtonText = "Let's Go!"};
		public static TutorialHelper FindFavorites => new TutorialHelper() { AndroidIcon = "SearchOrange", Icon = "IconSearchOrange", Id = "Find Favorites", Title = "Find Favorites", Subtitle = "To begin with, you should pick some profiles that you would like to follow and add to your feed. Use the \"search\" or click the menu to add new profiles.", ButtonText = "Follow Profiles" };
		public static TutorialHelper SchedulesScores => new TutorialHelper() { AndroidIcon = "ScoreOrange", Icon = "IconScoreOrange", Id = "Schedules and Scores", Title = "Schedules and Scores", Subtitle = "All the schedules and scores of all the profiles you are following appear here. At this point, some schedules might be incomplete but we will do our best to complete them as soon as possible." };
        public static TutorialHelper Profiles => new TutorialHelper() { AndroidIcon = "ProfileOrange", Icon = "IconProfileOrange", Id = "Profile", Title = "Profiles", Subtitle = "You can click on every single profile and explore its feed and other relevant screens such as scores or rankings. Follow a proflie to add it to your feed." };
		public static TutorialHelper Menu => new TutorialHelper() { AndroidIcon = "MenuOrange", Icon = "IconMenuOrange", Id = "Menu", Title = "Menu", Subtitle = "Use the menu to explore all of our different profile types, quickly access your profile, change your settings, become an advertiser, or create a content source." };
		public static TutorialHelper TagEvents => new TutorialHelper() { AndroidIcon = "EventsOrange", Icon = "IconEventsOrange", Id = "Tag Events", Title = "Tag Events", Subtitle = "Did you take a great picture at last night's Basketball event or want to discuss the result with other fans? Go ahead and tag an event for everyone to see." };
		public static TutorialHelper ContentSource => new TutorialHelper() { AndroidIcon = "CSOrange", Icon = "IconCSOrange", Id = "Content Sources", Title = "Content Sources", Subtitle = "Content Sources provide college sports-related content. Perfect for blogs, websites, podcasts, or magazines. Create your own content source on FanWord through accessing our \"menu\"." };
		public static TutorialHelper ContentSourceCreation => new TutorialHelper() { AndroidIcon = "CSOrange", Icon = "IconCSOrange", Id = "Content Source Creation", Title = "Content Source Creation", Subtitle = "Content Sources are blogs, websites, podcasts, newspapers, or magazines that provide college sports-related content. If you want to create a Content Source on FanWord, follow the next steps." };
		public static TutorialHelper CreateContent => new TutorialHelper() { AndroidIcon = "PencilOrange", Icon = "IconPencilOrange", Id = "Create Content", Title = "Create Content", Subtitle = "Upload and create your own content and share it with your friends. Please be aware that all content on FanWord is public. Therefore, please be considerate and respectful to others when posting." };
        public static TutorialHelper BecomeAdmin => new TutorialHelper() { AndroidIcon = "TeamsOrange", Icon = "IconTeamsOrange", Id = "Become an Admin", Title = "Become an Admin", Subtitle = "If you are an athlete, coach, or administrative person of this team/school, you can become an admin of this profile, and unlock helpful premium features." };
		public static TutorialHelper BecomeAdvertiser => new TutorialHelper() { AndroidIcon = "AdvertiserOrange", Icon = "IconAdvertiserOrange", Id = "Become an Advertiser", Title = "Become an Advertiser", Subtitle = "Ready to advertise on FanWord? Precisely determine specific audiences and the level of exposure to match your ad/ad campaign objectives." };
    }
}
