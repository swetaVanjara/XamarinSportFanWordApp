using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.TypeFaces
{
    public class CustomTypefaces
    {
        private static Typeface robotoBold;
        public static Typeface RobotoBold
        {
            get
            {
                return robotoBold ?? Typeface.CreateFromAsset(Application.Context.Assets, "Roboto-Bold.ttf");
            }
        }
    }
}