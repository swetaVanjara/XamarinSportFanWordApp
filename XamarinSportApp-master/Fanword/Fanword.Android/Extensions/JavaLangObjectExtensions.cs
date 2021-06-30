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

namespace Fanword.Android.Extensions
{
    public static class JavaLangObjectExtensions
    {
        public static void CancelPendingTask(this Java.Lang.Object javaObject, string url)
        {
           (javaObject as ImageLoaderHelper)?.Cancel(url);
        }
    }
}