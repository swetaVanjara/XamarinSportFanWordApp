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
    public static class StringExtensions
    {
        public static bool DisplayErrorMessage(this string message, Activity activity)
        {
            var hasError = !string.IsNullOrEmpty(message);
            if (hasError)
            {
                new AlertDialog.Builder(activity).SetTitle("Error")
                    .SetMessage(message)
                    .SetPositiveButton("Ok", (o, eventArgs) => { })
                    .Show();
            }
            return hasError;
        }

        public static bool DisplayErrorMessage(this string message, Fragment fragment)
        {
            var hasError = !string.IsNullOrEmpty(message);
            if (hasError)
            {
                new AlertDialog.Builder(fragment.Activity).SetTitle("Error")
                    .SetMessage(message)
                    .SetPositiveButton("Ok", (o, eventArgs) => { })
                    .Show();
            }
            return hasError;
        }
    }
}