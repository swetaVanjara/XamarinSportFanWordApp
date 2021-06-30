using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Connectivity;
using Plugin.CurrentActivity;

namespace Fanword.Android.Shared
{
    public class Links
    {
        public static void OpenUrl(string link)
        {
            try
            {
                if (!link.ToLower().StartsWith("http"))
                {
                    link = "http://" + link;
                }
                Intent intent = new Intent(Intent.ActionView);
                intent.SetData(global::Android.Net.Uri.Parse(link));
                CrossCurrentActivity.Current.Activity.StartActivity(intent);
            }
            catch (Exception e)
            {
            }
        }
    }
}