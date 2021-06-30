using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
using Fanword.Android;
using Fanword.Android.Fragments;
using Fanword.Android.Shared;
using Fanword.Poco.Models;
using Fanword.Shared;
using Mobile.Extensions.Android.Extensions;
using Mobile.Extensions.Extensions;
using Plugin.Dialog;
using Plugin.PushNotifications;
using Plugin.PushNotifications.Shared;
using Plugin.Settings;
using Fanword.Shared.Service;
using Fanword.Android.Activities.Search;
using Fanword.Android.Activities.Settings;
using Fanword.Shared.Helpers;
using Fanword.Android.Activities.ContentSourceInfo;
using Gcm.Client;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Fanword.Android.CustomViews;

namespace Fanword.Android
{ 
    [Activity(Label = "MainActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : BaseActivity
    {
        public DrawerLayout dlDrawer { get; set; }
        public FrameLayout flMenuContainer { get; set; }
        MenuFragment menuFragment;
        HomeFragment homeFragment;
        public static string PostId { get; set; }
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MainLayout);
            this.PopulateViewProperties();
            SetupViewBindings();

            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);
            GcmClient.Register(this, ServiceApiBase.SenderID);  //fanword-191110
        
            //GcmService.Initialize(this);
            //GcmService.Register(this);
        }

        void SetupViewBindings()
        {
            menuFragment = new MenuFragment();
            menuFragment.MenuItemClick = MenuItemClick;
            var tx = FragmentManager.BeginTransaction();
            tx.Add(Resource.Id.flMenuContainer, menuFragment);
            tx.Commit();
            MenuItemClick("Home");

            var user = CrossSettings.Current.GetValueOrDefaultJson<User>("User");
            CrossPushNotifications.Current.Register(new string[] { user.Id});

            ShowHelpIfNecessary(TutorialHelper.Welcome,() => 
            {
               ShowHelpIfNecessary(TutorialHelper.FindFavorites);

             });            

            dlDrawer.DrawerOpened += (sender, e) => 
            {
                ShowHelpIfNecessary(TutorialHelper.Menu);
            };
        }

        void MenuItemClick(string id)
        {
            BaseFragment fragment = null;

            if (id == "Home")
            {
                bool wasNull = homeFragment == null;
                fragment = homeFragment = homeFragment ?? new HomeFragment();
                if (wasNull)
                {
					FragmentManager.BeginTransaction().Replace(Resource.Id.flContent, fragment).Commit();
                }
            }
            else if (id == "Profile")
            {
                var user = CrossSettings.Current.GetValueOrDefaultJson<User>("User");
                Navigator.GoToUserProflie(user.Id);
			}
			else if (id == "BecomeAdvertiser")
			{
				if (!CrossSettings.Current.GetValueOrDefault(TutorialHelper.BecomeAdvertiser.Id, false))
				{
					ShowHelpIfNecessary(TutorialHelper.BecomeAdvertiser);
					return;
				}
                Intent intent = new Intent(Intent.ActionView);
                intent.SetData(global::Android.Net.Uri.Parse(ServiceApiBase.MvcPortalURL + "/Registration/Advertisers"));
                StartActivity(intent);
			}
			else if (id == "CreateContentSource")
			{
                StartActivity(typeof(ContentSourceInfoActivity));
			}
			else if (id == "Teams")
			{
                Intent intent = new Intent(this, typeof(SearchActivity));
                intent.PutExtra("FeedType", (int)FeedType.Team);
                intent.PutExtra("UseType", true);
                StartActivity(intent);
			}
			else if (id == "Schools")
			{
				Intent intent = new Intent(this, typeof(SearchActivity));
				intent.PutExtra("FeedType", (int)FeedType.School);
				intent.PutExtra("UseType", true);
				StartActivity(intent);
			}
			else if (id == "Sports")
			{
				Intent intent = new Intent(this, typeof(SearchActivity));
				intent.PutExtra("FeedType", (int)FeedType.Sport);
				intent.PutExtra("UseType", true);
				StartActivity(intent);
			}
			else if (id == "ContentSources")
			{
				Intent intent = new Intent(this, typeof(SearchActivity));
				intent.PutExtra("FeedType", (int)FeedType.ContentSource);
				intent.PutExtra("UseType", true);
				StartActivity(intent);
			}
			else if (id == "Users")
			{
				Intent intent = new Intent(this, typeof(SearchActivity));
				intent.PutExtra("FeedType", (int)FeedType.User);
				intent.PutExtra("UseType", true);
				StartActivity(intent);
			}
            else if (id == "Settings")
            {
                Intent intent = new Intent(this, typeof(SettingsActivity));
                StartActivity(intent);
            }

            else if (id == "Logout") 
            {
                new AlertDialog.Builder(this)
                    .SetTitle("Logout")
                    .SetMessage("Are you sure you want to logout?")
                    .SetPositiveButton("Logout", (sender, args) => Logout())
                    .SetNegativeButton("Cancel", (sender, args) => { })
                    .Show();
                return;
            }

            if (fragment != null)
            {

            }

            dlDrawer?.CloseDrawer(flMenuContainer);
        }

        public override void OnBackPressed()
        {
            if (dlDrawer.IsDrawerOpen(flMenuContainer))
            {
                dlDrawer.CloseDrawer(flMenuContainer);
                return;
            }
            if (homeFragment.vpPager.CurrentItem != 0)
            {
                homeFragment.vpPager.SetCurrentItem(0, true);
                return;
            }
                  
            base.OnBackPressed();
        }

        protected override void OnResume()
        {
            base.OnResume();
            menuFragment.UpdateProfile();
            homeFragment?.GetNotifications();
        }

        public void Logout()
        {
            FacebookSdk.SdkInitialize(this);
            LoginManager.Instance.LogOut();
            CrossSettings.Current.Clear();
            StartActivity(typeof(LoginActivity));
            Finish();
        }

    }
}