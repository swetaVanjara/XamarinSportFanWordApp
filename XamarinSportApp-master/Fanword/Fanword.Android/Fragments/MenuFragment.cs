using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fanword.Android.Activities.FavoritesView;
using Fanword.Android.Activities.MyProfile;
using Fanword.Android.Extensions;
using Fanword.Android.TypeFaces;
using Fanword.Poco.Models;
using Fanword.Shared;
using Fanword.Shared.Helpers;
using Fanword.Shared.Models;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;
using Mobile.Extensions.Extensions;
using Plugin.Settings;
using Square.Picasso;

namespace Fanword.Android.Fragments
{
    public class MenuFragment : BaseFragment
    {
        private ListView lvMenu { get; set; }
        private TextView lblName { get; set; }
        private TextView lblAthlete { get; set; }

        private ImageViewAsync imgProfile { get; set; }
        public Action<string> MenuItemClick;
        private TextView lblPosts { get; set; }
        private TextView lblFollowers { get; set; }
        private TextView lblFollowing { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.MenuFragmentLayout, null);
            this.PopulateViewProperties(view);
            view.Click += (sender, e) => { };
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var adatper = new CustomListAdapter<MenuItem>(MenuItem.GetMenuItems(), GetView);
            adatper.SetViewTypes(GetType, 3);
            lvMenu.Adapter = adatper;

            lvMenu.ItemClick += (sender, args) =>
            {
                var item = (lvMenu.Adapter as CustomListAdapter<MenuItem>).Items[args.Position];
                if (!string.IsNullOrEmpty(item.Id))
                {
                    MenuItemClick?.Invoke(item.Id);
                }
            };

            lblPosts.Typeface = CustomTypefaces.RobotoBold;
            lblFollowers.Typeface = CustomTypefaces.RobotoBold;
            lblFollowing.Typeface = CustomTypefaces.RobotoBold;

            UpdateProfile();

            (lblFollowing.Parent as ViewGroup).Click += (sender, e) =>
            {
                Intent intent = new Intent(Activity, typeof(FavoritesActivity));
                //intent.PutExtra("Fragment", "Following");
                StartActivity(intent);
            };
            (lblFollowers.Parent as ViewGroup).Click += (sender, e) =>
            {
                Intent intent = new Intent(Activity, typeof(FavoritesActivity));
                intent.PutExtra("Fragment", "Followers");
                StartActivity(intent);
            };
            (lblPosts.Parent as ViewGroup).Click += (sender, e) =>
            {
                Intent intent = new Intent(Activity, typeof(MyProfileActivity));
                StartActivity(intent);
            };
        }

        int GetType(MenuItem item, int position)
        {
            if(position == (lvMenu.Adapter as CustomListAdapter<MenuItem>).Items.Count -1)
            {
                return 2;
            }
            else if (string.IsNullOrEmpty(item.Id))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        
        public override void OnResume()
        {
            base.OnResume();
            var apiTask = new ServiceApi().GetMyProfileDetails();
            apiTask.HandleError(ActivityProgresDialog);
            apiTask.OnSucess(ActivityProgresDialog, (response) =>
            {
                lblPosts.Text = LargeValueHelper.GetString(response.Result.Posts);
                lblFollowers.Text = LargeValueHelper.GetString(response.Result.Followers);
                lblFollowing.Text = LargeValueHelper.GetString(response.Result.Following);
            });
            
        }

        public void UpdateProfile()
        {
            var user = CrossSettings.Current.GetValueOrDefaultJson<User>("User");
            lblName.Text = user.FirstName + " " + user.LastName;
            if (!string.IsNullOrEmpty(user.ProfileUrl))
            {
                Picasso.With(Activity).Load(user.ProfileUrl).Resize(200,200).CenterCrop().OnlyScaleDown().Placeholder(Resource.Drawable.DefProfPic).Transform(new PiccasoCircleTransformation()).Into(imgProfile);
                //ImageService.Instance.LoadUrl(user.ProfileUrl).SetSettings(imgProfile, Resource.Drawable.DefProfPic, 200, new CircleTransformation());
            }

            if (user.AthleteVerified) {
                lblAthlete.Visibility = string.IsNullOrEmpty(user.AthleteTeamId) ? ViewStates.Gone : ViewStates.Visible;
                lblAthlete.Text = user.AthleteSchool + " - " + user.AthleteSport;
            } else {
                lblAthlete.Visibility = ViewStates.Gone;
                lblAthlete.Text = "";
            }

        }

        View GetView(MenuItem item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                if (GetType(item, position) == 0)
                {
                    view = Activity.LayoutInflater.Inflate(Resource.Layout.SearchHeaderItem, null);
                    view.FindViewById<TextView>(Resource.Id.lblTitle).Typeface = CustomTypefaces.RobotoBold;
                }
                else if(GetType(item, position) == 1)
                {
                    view = Activity.LayoutInflater.Inflate(Resource.Layout.MenuItem, null);
                    view.FindViewById<TextView>(Resource.Id.lblTitle).Typeface = CustomTypefaces.RobotoBold;
                }
                else 
                {
					view = Activity.LayoutInflater.Inflate(Resource.Layout.MenuFooterItem, null);
                }

                ActivityExtensions.SetViewFont(view as ViewGroup);

            }

            if (GetType(item, position) == 0)
            {
                view.FindViewById<TextView>(Resource.Id.lblTitle).Text = item.Title;
            }
            else if (GetType(item, position) == 1)
            {
                view.FindViewById<TextView>(Resource.Id.lblTitle).Text = item.Title;
                var id = Resources.GetIdentifier(item.Icon.ToLower(), "drawable", Activity.PackageName);
                view.FindViewById<ImageView>(Resource.Id.imgIcon).SetImageResource(id);
            }
            return view;
        }
    }

    public class PiccasoCircleTransformation : Java.Lang.Object, ITransformation
    {
        public string Key => "Circle";

        public Bitmap Transform(Bitmap source)
        {
            int size = Math.Min(source.Width, source.Height);

            int x = (source.Width - size) / 2;
            int y = (source.Height - size) / 2;

            Bitmap squaredBitmap = Bitmap.CreateBitmap(source, x, y, size, size);
            if (squaredBitmap != source)
            {
                source.Recycle();
            }

            Bitmap bitmap = Bitmap.CreateBitmap(size, size, source.GetConfig());

            Canvas canvas = new Canvas(bitmap);
            Paint paint = new Paint();
            BitmapShader shader = new BitmapShader(squaredBitmap, BitmapShader.TileMode.Clamp, BitmapShader.TileMode.Clamp);
            paint.SetShader(shader);
            paint.AntiAlias = true;

            float r = size / 2f;
            canvas.DrawCircle(r, r, r, paint);

            squaredBitmap.Recycle();
            return bitmap;
        }
    }
}