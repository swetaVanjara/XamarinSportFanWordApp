using System;

using Android.App;
using Android.OS;
using Android.Runtime;
using Fanword.Android.Shared;
using Plugin.CurrentActivity;
using Plugin.PushNotifications;
using Xamarin.Facebook;
using Fanword.Shared.Service;
using Plugin.Settings;
using Mixpanel.Android.MpMetrics;
using Gcm;

namespace Fanword.Android
{
	//You can specify additional application information in this attribute
    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          :base(handle, transer)
        {
        }

        public const string MIXPANEL_TOKEN = ConstantsHelper.MIXPANEL_TOKEN; //"298c35129fd8b5dd8d197a4cd395fab5";
        public static bool DidClick = false;
        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            FacebookSdk.ApplicationId = ConstantsHelper.FB_ApplicationID; //"469763806538563";
            FacebookSdk.ApplicationName = ConstantsHelper.AppName; //"FanWord";
			FacebookSdk.SdkInitialize (this);

            MixpanelAPI mixpanel = MixpanelAPI.GetInstance(this, MIXPANEL_TOKEN);

            CrossPushNotifications.Current.Configure(ServiceApiBase.HubName, ServiceApiBase.AzureConnectionString, new[] { "fanword" }, Resource.Drawable.AppIcon);
            ////due to blank notification the code is commented
            //CrossPushNotifications.Current.PushNotificationClicked += (sender, e) =>
            //{
            //    Navigator.HandleNotificationTap(e.MetaData, e.Title, e.Message);
            //};

            //CrossPushNotifications.Current.PushNotificationRecieved += (sender, item) =>
            //{
            //    var lastId = CrossSettings.Current.GetValueOrDefault("LastNotificationId", "");
            //    if (lastId == item.Id)
            //        return;

            //    CrossSettings.Current.AddOrUpdateValue("LastNotificationId", item.Id);
            //    CrossPushNotifications.Current.ShowLocalNotification(item);
            //};
        }


        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}