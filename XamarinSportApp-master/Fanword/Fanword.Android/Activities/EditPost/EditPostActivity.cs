using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fanword.Android.Activities.TagProfiles;
using Fanword.Android.Extensions;
using Fanword.Poco.Models;
using Fanword.Shared;
using Fanword.Shared.Models;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using FFImageLoading.Work;
using Java.IO;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;
using Mobile.Extensions.Extensions;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using Plugin.Settings;
using Environment = System.Environment;
using File = System.IO.File;
using Path = System.IO.Path;
using Fanword.Android.Activities.TagEvents;
using Fanword.Android.TypeFaces;
using Fanword.Shared.Helpers;
using Console = System.Console;

namespace Fanword.Android.Activities.EditPost
{
    [Activity(Label = "EditPostActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class EditPostActivity : BaseActivity
    {
        private ImageButton btnBack { get; set; }
        private ImageViewAsync imgProfile { get; set; }
        private TextView lblName { get; set; }
        private Button btnNext { get; set; }
        private EditText txtContent { get; set; }
        private ImageViewAsync imgImage { get; set; }
        private ImageButton btnCancel { get; set; }
        private LinearLayout llLinkDetails { get; set; }
        private TextView lblLinkHost { get; set; }
        private TextView lblLinkTitle { get; set; }
        private ImageView imgCamera { get; set; }
        private TextView lblAddMedia { get; set; }
        private LinearLayout llAddMedia { get; set; }
        private LinearLayout llAddLink { get; set; }
        private ImageView imgLink { get; set; }
        private TextView lblLink { get; set; }
        private RelativeLayout rlMedia { get; set; }
        private ImageButton btnPostAs { get; set; }
        private bool hideTagProfiles = true;
        private TextView lblTitle { get; set; }
        private string PostId { get; set; }
        Post post;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditPostLayout);
            this.PopulateViewProperties();
            PostId = Intent.GetStringExtra("PostId");
            SetupViewBindings();
        }

        void SetupViewBindings()
        {
            btnBack.Click += (sender, e) => Finish();
            lblTitle.Typeface = CustomTypefaces.RobotoBold;

            ShowHelpIfNecessary(TutorialHelper.CreateContent);

            var user = CrossSettings.Current.GetValueOrDefaultJson<User>("User");
            hideTagProfiles = string.IsNullOrEmpty(user.AthleteTeamId) || !user.AthleteVerified;
            if (!string.IsNullOrEmpty(user.ProfileUrl))
            {
                ImageService.Instance.LoadUrl(user.ProfileUrl).Retry(3, 300).DownSample(200).Transform(new CircleTransformation()).Into(imgProfile);
            }
            lblName.Text = user.FirstName + " " + user.LastName;

            //rlMedia.Visibility = ViewStates.Invisible;
            var parameters = rlMedia.LayoutParameters as LinearLayout.LayoutParams;
            var height = Resources.DisplayMetrics.WidthPixels * 9 / 16f ;
            parameters.Height = (int)height;
            rlMedia.LayoutParameters = parameters;

            if (!string.IsNullOrEmpty(PostId))
            {
                ShowProgressDialog();
                var apiTask = new ServiceApi().GetPost(PostId);
                apiTask.HandleError(this);
                apiTask.OnSucess(this,response =>
                {
                    post = response.Result;
                    txtContent.Text = post.Content;
                    if (!string.IsNullOrEmpty(post.ContentSourceId))
                    {
                        lblName.Text = user.ContentSourceName;
                        ImageService.Instance.LoadUrl(user.ContentSourceUrl).Retry(3, 300).DownSample(200).Transform(new CircleTransformation()).Into(imgProfile);
                        hideTagProfiles = false;
                    }
                    if (!string.IsNullOrEmpty(post.TeamId))
                    {
                        var team = user.AdminTeams.FirstOrDefault(m => m.Id == post.TeamId);
                        if (team == null)
                            return;
                        lblName.Text = team.SchoolName;
                        ImageService.Instance.LoadUrl(team.ProfileUrl).Retry(3, 300).DownSample(200).Transform(new CircleTransformation()).Into(imgProfile);
                        hideTagProfiles = true;
                    }
                    if (!string.IsNullOrEmpty(post.SchoolId))
                    {
                        var school = user.AdminSchools.FirstOrDefault(m => m.Id == post.SchoolId);
                        if (school == null)
                            return;
                        lblName.Text = school.Name;
                        ImageService.Instance.LoadUrl(school.ProfileUrl).Retry(3, 300).DownSample(200).Transform(new CircleTransformation()).Into(imgProfile);
                        hideTagProfiles = false;
                    }
                    HideProgressDialog();
                    SetImageUI();
                });
            }
            else
            {
                post = new Post();
                post.Schools = new List<string>();
                post.Sports = new List<string>();
                post.Teams = new List<string>();
                post.Links = new List<PostLink>();
                post.Images = new List<PostImage>();
                post.Videos = new List<PostVideo>();
                post.Events = new List<string>();
                SetImageUI();
            }

            //btnNext.Text = "Post";

            btnNext.Click += (sender, e) =>
            {
                post.Content = txtContent.Text;
                
                if (!post.Images.Any() && !post.Videos.Any() && !post.Links.Any() && string.IsNullOrEmpty(post.Content))
                {
                    new AlertDialog.Builder(this)
                        .SetTitle("Error")
                        .SetMessage("You must choose some content to post")
                        .SetPositiveButton("Ok", (s, e2) => { })
                        .Show();
                    return;
                }

                if (hideTagProfiles)
                {
					Intent i = new Intent(this, typeof(TagEventsActivity));
					i.PutExtra("Post", JsonConvert.SerializeObject(post));
					StartActivity(i);
                }
                else
                {
                    Intent i = new Intent(this, typeof(TagProfileActivity));
                    i.PutExtra("Post", JsonConvert.SerializeObject(post));
                    StartActivity(i);
                }
            };

            llAddMedia.Click += (sender, args) =>
            {
                var view = LayoutInflater.Inflate(Resource.Layout.AddMediaOptionsLayout, null);
                var dialog = new AlertDialog.Builder(this).SetView(view).Create();
                view.FindViewById<Button>(Resource.Id.btnTakePhoto).Click += async (sender2, args2) =>
                {
                    dialog.Dismiss();
                    var file = await AppMedia.TakePhotoAsync();
                    ImagePicked(file);
                };
                view.FindViewById<Button>(Resource.Id.btnPickPhoto).Click += async (sender2, args2) =>
                {
                    dialog.Dismiss();
                    var file = await AppMedia.PickPhotoAsync();
                    ImagePicked(file);

                };
                view.FindViewById<Button>(Resource.Id.btnTakeVideo).Click += async (sender2, args2) =>
                {
                    dialog.Dismiss();
                    var file = await AppMedia.TakeVideoAsync();
                    HandleVideo(file);
                };
                view.FindViewById<Button>(Resource.Id.btnPickVideo).Click += async (sender2, args2) =>
                {
                    dialog.Dismiss();
                    var file = await AppMedia.PickVideoAsync();
                    HandleVideo(file);
                };

                dialog.Show();
            };

            llAddLink.Click += (sender, args) => 
            {
                var view = LayoutInflater.Inflate(Resource.Layout.AddLinkLayout, null);
                var txtLink = view.FindViewById<EditText>(Resource.Id.txtLink);
                var dialog = new AlertDialog.Builder(this).SetView(view).SetPositiveButton("Ok", async (s,e) =>
                {
                    SetNoImage();
                    SetNoLink();
                    post.Videos.Clear();
                    post.Images.Clear();
                    post.Links.Clear();
                    ShowProgressDialog();
                    var link = await AppMedia.GetLinkData(txtLink.Text);
                    if(link != null)
                    {
						link.ImageAspectRatio = await GetImageAspectRatio(link.ImageUrl);
                    }

                    HideProgressDialog();
                    if (link != null)
                    {
                        post.Links.Add(link);
                        lblLink.Text = txtLink.Text;
                    }

                    SetImageUI();
                }).SetNegativeButton("Cancel", (s, e) => { }).Create();

                dialog.Show();
            };

            btnCancel.Click += (sender, e) =>
            {
                post.Images.Clear();
                post.Videos.Clear();
                post.Links.Clear(); ;

                rlMedia.Visibility = ViewStates.Invisible;
                SetNoImage();
                SetNoLink();
            };

            btnPostAs.Click += (sender, e) => PostAsClicked();
        }

        void PostAsClicked()
        {
            var user = CrossSettings.Current.GetValueOrDefaultJson<User>("User");

            View view = LayoutInflater.Inflate(Resource.Layout.PostAsModalLayout, null);
            var lvAdminProfiles = view.FindViewById<ListView>(Resource.Id.lvAdminProfiles);
            var adminProfiles = user.AdminSchools?.Select(m => new PostAsAdminProfile() { SchoolId = m.Id, Title = m.Name, Url = m.ProfileUrl}).OrderBy(m => m.Title).ThenBy(m => m.SubTitle).ToList() ?? new List<PostAsAdminProfile>();
            adminProfiles.AddRange(user.AdminTeams?.Select(m => new PostAsAdminProfile() { TeamId = m.Id, Title = m.SchoolName, SubTitle = m.SportName, Url = m.ProfileUrl }) ?? new List<PostAsAdminProfile>().OrderBy(m => m.Title).ThenBy(m => m.SubTitle));
            var adapter = new CustomListAdapter<PostAsAdminProfile>(adminProfiles, GetAdminView);
            lvAdminProfiles.Adapter = adapter;
            adapter.NoContentEnabled = false;
            view.FindViewById<TextView>(Resource.Id.lblName).Text = user.FirstName + " " + user.LastName;
            if(!string.IsNullOrEmpty(user.ProfileUrl))
            {
				ImageService.Instance.LoadUrl(user.ProfileUrl).Retry(3, 300).DownSample(150).Transform(new CircleTransformation()).Into(view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile));  
            }

            var dialog = new AlertDialog.Builder(this).SetView(view).Create();


            if (string.IsNullOrEmpty(user.ContentSourceId) || !user.ContentSourceApproved)
            {
                view.FindViewById<LinearLayout>(Resource.Id.llContentSource).Visibility = ViewStates.Gone;
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.lblContentSource).Text = user.ContentSourceName;
                ImageService.Instance.LoadUrl(user.ContentSourceUrl).Retry(3, 300).DownSample(150).Transform(new CircleTransformation()).Into(view.FindViewById<ImageViewAsync>(Resource.Id.imgContentSource));
            }

            view.FindViewById<LinearLayout>(Resource.Id.llContentSource).Click += (sender, e) =>
            {
                hideTagProfiles = false;
                btnNext.Text = "Next";
                lblName.Text = user.ContentSourceName;
                post.ContentSourceId = user.ContentSourceId;
                post.TeamId = null;
                post.SchoolId = null;
                ImageService.Instance.LoadUrl(user.ContentSourceUrl).Retry(3, 300).Transform(new CircleTransformation()).Into(imgProfile);

                dialog.Dismiss();
            };
            view.FindViewById<LinearLayout>(Resource.Id.llUser).Click += (sender, e) =>
            {
                hideTagProfiles = string.IsNullOrEmpty(user.AthleteTeamId) || !user.AthleteVerified;
                //btnNext.Text = "Post";
                lblName.Text = user.FirstName + " " + user.LastName;
                post.ContentSourceId = null;
                post.TeamId = null;
                post.SchoolId = null;
                ImageService.Instance.LoadUrl(user.ProfileUrl).Retry(3, 300).Transform(new CircleTransformation()).Into(imgProfile);
                dialog.Dismiss();
            };

            lvAdminProfiles.ItemClick += (sender, args) =>
            {
                var item = adapter.Items[args.Position];
                hideTagProfiles = !string.IsNullOrEmpty(item.TeamId);
                btnNext.Text = "Next";
                lblName.Text = item.Title;
                post.ContentSourceId = null;
                post.TeamId = item.TeamId;
                post.SchoolId = item.SchoolId;
                ImageService.Instance.LoadUrl(item.Url).Retry(3, 300).Transform(new CircleTransformation()).Into(imgProfile);
                dialog.Dismiss();
            };

            dialog.Show();
        }

        View GetAdminView(PostAsAdminProfile item, int position, View convertView,ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = LayoutInflater.Inflate(Resource.Layout.PostAsAdminItem, null);
            }
            view.FindViewById<TextView>(Resource.Id.lblName).Text = item.Title;
            view.FindViewById<TextView>(Resource.Id.lblSubtitle).Text = item.SubTitle;

            var profileImageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
            profileImageView.Tag?.CancelPendingTask(item.Url);
            var task = ImageService.Instance.LoadUrl(item.Url)
                .Retry(3, 300)
                .DownSample(200)
                .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                .Into(profileImageView);

            profileImageView.Tag = new ImageLoaderHelper(task);

            return view;
        }

        void SetNoLink()
        {
            imgLink.SetImageResource(Resource.Drawable.Link);
            lblLink.SetTextColor(new Color(21, 21, 21));
            lblLink.Text = "Add a Link";
        }

        void SetNoImage()
        {
            imgCamera.SetImageResource(Resource.Drawable.CameraBlack);
            lblAddMedia.SetTextColor(new Color(21, 21, 21));
        }

        void SetImageUI()
        {
            if (post.Videos.Any() || post.Images.Any() || post.Links.Any())
            {
                rlMedia.Visibility = ViewStates.Visible;
                if (post.Images.Any())
                {
                    var image = post.Images.FirstOrDefault();
                    if (string.IsNullOrEmpty(image.Id))
                    {
                        var exist = File.Exists(image.Url);
                        //var bitmap = BitmapFactory.DecodeFile(image.Url);
                        ImageService.Instance.LoadFile(image.Url).Into(imgImage);
                        //imgImage.SetImageBitmap(bitmap);
                    }
                    else
                    {
                        ImageService.Instance.LoadUrl(image.Url).Into(imgImage);
                    }

                    var parameters = rlMedia.LayoutParameters as LinearLayout.LayoutParams;
                    var height = Resources.DisplayMetrics.WidthPixels * image.ImageAspectRatio;
                    parameters.Height = (int)height;
                    rlMedia.LayoutParameters = parameters;

                    imgCamera.SetImageResource(Resource.Drawable.CameraOrange);
                    lblAddMedia.SetTextColor(new Color(249, 95, 6));
                    llLinkDetails.Visibility = ViewStates.Gone;
                    SetNoLink();
                }
                else if (post.Videos.Any())
                {
                    var video = post.Videos.FirstOrDefault();
                    if (string.IsNullOrEmpty(video.Id))
                    {
                        ImageService.Instance.LoadFile(video.ImageUrl).Into(imgImage);
                    }
                    else
                    {
                        ImageService.Instance.LoadUrl(video.ImageUrl).Into(imgImage);
                    }

                    var parameters = rlMedia.LayoutParameters as LinearLayout.LayoutParams;
                    var height = Resources.DisplayMetrics.WidthPixels * video.ImageAspectRatio;
                    parameters.Height = (int)height;
                    rlMedia.LayoutParameters = parameters;

                    imgCamera.SetImageResource(Resource.Drawable.CameraOrange);
                    lblAddMedia.SetTextColor(new Color(249, 95, 6));
                    llLinkDetails.Visibility = ViewStates.Gone;
                    SetNoLink();
                }
                else if (post.Links.Any())
                {
                    var link = post.Links.FirstOrDefault();
                    if (string.IsNullOrEmpty(link.ImageUrl))
                    {
                        imgImage.SetImageResource(Resource.Drawable.NoImageGraphic);
                    }
                    else
                    {
                        ImageService.Instance.LoadUrl(link.ImageUrl).Retry(3, 300).Into(imgImage);
                    }

                    var parameters = rlMedia.LayoutParameters as LinearLayout.LayoutParams;
                    var height = Resources.DisplayMetrics.WidthPixels * link.ImageAspectRatio;
                    parameters.Height = (int)height;
                    rlMedia.LayoutParameters = parameters;

                    lblLink.SetTextColor(new Color(249, 95, 6));
                    imgLink.SetImageResource(Resource.Drawable.LinkOrange);
                    var uri = new Uri(link.LinkUrl);
                    lblLinkHost.Text = uri.Host;
                    llLinkDetails.Visibility = ViewStates.Visible;
                    lblLinkTitle.Text = link.Title;
                }
            }
            else
            {
                rlMedia.Visibility = ViewStates.Invisible;
            }

        }

        void HandleVideo(MediaFile file)
        {
            if (file == null)
                return;

            MediaMetadataRetriever m = new MediaMetadataRetriever();
            m.SetDataSource(file.Path);
            var bitmap = m.GetFrameAtTime(1000);
            var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var imageFile = Path.Combine(documentsDirectory, Guid.NewGuid().ToString() + ".jpg");

            var outputStream = new FileStream(imageFile, FileMode.CreateNew);
            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 80, outputStream);
            outputStream.Close();
            bitmap.Recycle();
            VideoPicked(file.Path, imageFile);
        }

        async void ImagePicked(MediaFile file)
        {
            if (file == null)
                return;
            
            post.Images.Clear();
            post.Images.Add(new PostImage() { Url = file.Path, ImageAspectRatio = await GetImageAspectRatio (file.Path)});
            post.Videos.Clear();
            post.Links.Clear();
            SetImageUI();
        }

        async void VideoPicked(string path, string imageFile)
        {
            post.Videos.Clear();
            post.Videos.Add(new PostVideo() { Url = path, ImageUrl = imageFile, ImageAspectRatio = await GetImageAspectRatio(imageFile) });
            post.Images.Clear();
            post.Links.Clear();
            SetImageUI();
        }

        async Task<float> GetImageAspectRatio(string file)
        {
            float ratio = 0.5625f;
            try
            {
                BitmapFactory.Options options = new BitmapFactory.Options();
                options.InJustDecodeBounds = true;
                if (file.StartsWith("http"))
                {
                    HttpClient client = new HttpClient();
                    var bytes = await client.GetByteArrayAsync(file);
                    await BitmapFactory.DecodeByteArrayAsync(bytes, 0, bytes.Length, options);
                }
                else
                {
                    BitmapFactory.DecodeFile(file, options);
                }

                int width = options.OutWidth;
                int height = options.OutHeight;
                ratio = (float)height / (float)width;
                ratio = ratio <= 0 ? .5625f : ratio;
            }
            catch (Exception e)
            {
            }

            return ratio;
        }
    }
}