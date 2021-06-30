using Foundation;
using UIKit;
using FFImageLoading;
using HockeyApp.iOS;
using Facebook.CoreKit;
using Plugin.Media;
using Plugin.PushNotifications;
using Fanword.Shared.Service;
using Newtonsoft.Json;
using System.Collections.Generic;
using Plugin.PushNotifications.Shared;
using System;
using Fanword.iOS.Shared;
using MixpanelLib;

namespace Fanword.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		public static string DefaultProfileString = "DefaultProfile";
        public const string MIXPANEL_TOKEN = ConstantsHelper.MIXPANEL_TOKEN; //"298c35129fd8b5dd8d197a4cd395fab5";

		public override UIWindow Window
		{
			get;
			set;
		}
        public static PushNotificationItem ClickedNotification;
		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			// Override point for customization after application launch.
			// If not required for your application you can safely delete this method			
            Xamarin.IQKeyboardManager.SharedManager.ShouldResignOnTouchOutside = true;
			Xamarin.IQKeyboardManager.SharedManager.EnableAutoToolbar = true;
			ImageService.Instance.Config.SchedulerMaxParallelTasks = 6;

			var manager = BITHockeyManager.SharedHockeyManager;
			manager.Configure ("8a31b849e71547c7b6137a87d58fdb09");
			manager.StartManager ();
            Settings.AppID = ConstantsHelper.FB_ApplicationID; //"469763806538563";
            Settings.DisplayName = ConstantsHelper.AppName; //"Fanword";

            MixpanelTweaks.Register(typeof(AppTweaks));
            Mixpanel.SharedInstanceWithToken(MIXPANEL_TOKEN);
            Mixpanel.SharedInstance.Track("Launch");


			CrossPushNotifications.Current.Configure(ServiceApiBase.HubName, ServiceApiBase.AzureConnectionString, new[] { "fanword" }, 0);
            CrossPushNotifications.Current.PushNotificationClicked += (sender, e) => 
            {
                if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Inactive && MainViewController.Instance?.NavigationController != null)
                {
                    // clicked on notification from background
                    var notification = (e as PushNotificationItem);
                    Navigator.HandleNotificationTap(MainViewController.Instance.NavigationController, notification.MetaData,notification.Title, notification.Message);
                }
                else if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active)
                {
                    // Update badge
                    MainViewController.Instance?.GetNotifications();
                }
                else
                {
					// app closed
                    ClickedNotification = (e as PushNotificationItem);					
                }

            };

			return true;
		}

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            (CrossPushNotifications.Current as CrossPushNotificationsImplementation).RegisteredForRemoteNotifications(deviceToken);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, System.Action<UIBackgroundFetchResult> completionHandler)
        {
            try
            {
                var aps = userInfo["aps"] as NSDictionary;
                var metaData = JsonConvert.DeserializeObject<Dictionary<string, string>>(aps["MetaData"]?.ToString() ?? "");
                var id = aps["Id"]?.ToString();
                var message = aps["Message"]?.ToString();
                var title = aps["Title"]?.ToString();

                CrossPushNotifications.Current.PushNotificationClicked?.Invoke(this, new PushNotificationItem() { Id = id, Title = title, Message = message, MetaData = metaData });
            }
            catch (Exception ex)
            {
                
            }

        }



		public override void OnResignActivation (UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground (UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground (UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated (UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate (UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}

		public override bool OpenUrl (UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			// We need to handle URLs by passing them to their own OpenUrl in order to make the SSO authentication works.
			return ApplicationDelegate.SharedInstance.OpenUrl (application, url, sourceApplication, annotation);
		}
	}
}

