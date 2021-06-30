using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fanword.Android.Activities.ContentSource;
using Fanword.Android.Activities.EditPost;
using Fanword.Android.Activities.MyProfile;
using Fanword.Android.Activities.PostDetails;
using Fanword.Android.Activities.TeamProfile;
using Fanword.Android.Activities.UserProfile;
using Fanword.Android.Adapters;
using Fanword.Android.Extensions;
using Fanword.Android.ViewHolders;
using Fanword.Poco.Models;
using Fanword.Shared;
using Fanword.Shared.Models;
using FFImageLoading;
using FFImageLoading.Transformations;
using Humanizer;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;
using Mobile.Extensions.Extensions;
using Plugin.Settings;
using Fanword.Android.Activities.SchoolProfile;
using Fanword.Android.Shared;
using static Android.Support.V7.Widget.StaggeredGridLayoutManager;

namespace Fanword.Android.CustomViews
{

    public class FeedRecyclerView : RecyclerView
    {
        BaseActivity activity;
        CustomRecyclerViewAdapter<FeedItem> adapter;
        bool LoadingData;
        LinearLayoutManager layoutManager;
        public ListPopupWindow popup;
        LinearLayout emptyFeedLayout;
        View headerView;
        string id;
        int itemsLoaded;
        FeedType type;
        public SwipeRefreshLayout SwipeContainer;
        public EventHandler<EventArgs> DataReceived;
        public EventHandler<EventArgs> RefreshRequested;
        public ImageView NoFeedItems;
        
        User user;

        public FeedRecyclerView(Context context)
            : base(context)
        {
        }

        public FeedRecyclerView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public FeedRecyclerView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }

        protected FeedRecyclerView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }


        public void Initialize(BaseActivity activity)
        {
            this.activity = activity;
            SharedInitialize();
        }

        public void Initialize(BaseActivity activity, View view, string id, FeedType type)
        {
            this.headerView = view;
            this.activity = activity;
            this.activity = activity;
            this.id = id;
            this.type = type;
            SharedInitialize();
        }

        void SharedInitialize()
        {
            user = CrossSettings.Current.GetValueOrDefaultJson<User>("User");
            layoutManager = new LinearLayoutManager(activity);
            SetLayoutManager(layoutManager);
            // emptyFeedLayout = FindViewById<LinearLayout>(Resource.Id.emptyFeedLayout);
            var list = new List<FeedItem>();
            if (headerView != null)
            {
                list.Add(new FeedItem());
            }
            adapter = new CustomRecyclerViewAdapter<FeedItem>(list, GetView, CreateViewHolder, GetViewType);
            SetAdapter(adapter);

           


            // adapter.NoContentText = "No content yet. Try to follow some profiles.";

            AddOnScrollListener(new RecyclerViewScrollListener(() =>
            {
                var visibleItemCount = ChildCount;
                var totalItemCount = adapter.Items.Count(/*m => m.PostType != NewsFeedPostTypes.Advertisement*/);
                var pastVisiblesItems = layoutManager.FindFirstVisibleItemPosition();

                if ((visibleItemCount + pastVisiblesItems) >= totalItemCount && !LoadingData && totalItemCount > 9 && totalItemCount != 0 && itemsLoaded >= 20)
                {
                    GetNewsFeedItems(false);
                }
            }));

          GetNewsFeedItems(true);
        }
        RecyclerView.ViewHolder CreateViewHolder(ViewGroup parent, int viewType, Action<int> onClick)
        {
            if (viewType == (int)FeedViewType.NormalItem)
            {
                var view = activity.LayoutInflater.Inflate(Resource.Layout.FeedItem, null);
                var cvh = new FeedItemViewHolder(view, onClick);
                cvh.lblLikes.Click += (sender, args) => PostDetailsClicked(adapter.Items[(int)cvh.ItemView.Tag].Id, "Likes");
                cvh.btnLike.Click += (sender, args) => LikeClicked((int)cvh.ItemView.Tag, cvh.btnLike, cvh.lblLikes);
                cvh.lblComments.Click += (sender, args) => PostDetailsClicked(adapter.Items[(int)cvh.ItemView.Tag].Id, "Comments");
                cvh.btnComment.Click += (sender, args) => PostDetailsClicked(adapter.Items[(int)cvh.ItemView.Tag].Id, "Comments");
                cvh.lblShares.Click += (sender, args) => PostDetailsClicked(adapter.Items[(int)cvh.ItemView.Tag].Id, "Shares");
                cvh.btnTag.Click += (sender, args) => PostDetailsClicked(adapter.Items[(int)cvh.ItemView.Tag].Id, "Tags");
                cvh.btnShare.Click += (sender, args) => ShareClicked(adapter.Items[(int) cvh.ItemView.Tag]);
                cvh.btnOptions.Click += (sender, args) => OptionsClicked(adapter.Items[(int) cvh.ItemView.Tag]);
                cvh.btnPlay.Click += (sender, args) => PlayVideoClicked(adapter.Items[(int) cvh.ItemView.Tag]);
                cvh.lblName.Click += (sender, args) => UserClicked(adapter.Items[(int) cvh.ItemView.Tag]);
                cvh.imgImage.Click += (sender, e) => ImageClicked(adapter.Items[(int)cvh.ItemView.Tag]);
				cvh.imgProfile.Click += (sender, args) => UserClicked(adapter.Items[(int)cvh.ItemView.Tag]);
               // cvh.btnFacebook.Click += (sender, args) => Links.OpenUrl(adapter.Items[(int)cvh.ItemView.Tag].FacebookUrl);
                //cvh.btnTwitter.Click += (sender, args) => Links.OpenUrl(adapter.Items[(int)cvh.ItemView.Tag].TwitterUrl);
                //cvh.btnInstagram.Click += (sender, args) => Links.OpenUrl(adapter.Items[(int)cvh.ItemView.Tag].InstagramUrl);
                return cvh;
            }
            else if (viewType == (int)FeedViewType.HeaderItem)
            {
                var phvh = new ProfileHeaderViewHolder(headerView, (pos) => { });
                return phvh;
            }
            else
            {
                var view = activity.LayoutInflater.Inflate(Resource.Layout.AdvertisementItem, null);

                var avh = new AdvertisementViewHolder(view, onClick);
                avh.ItemView.Click += (sender, e) => AdvertisementClicked((int)avh.ItemView.Tag);
                return avh;
            }
            
        }

        public void UpdateFeedItem(string postId)
        {
            if (!string.IsNullOrEmpty(postId))
            {
                if (postId == "Refresh")
                {
                    GetNewsFeedItems(true);
                }
                else
                {
                    var apiTask = new ServiceApi().GetFeedItem(postId);
                    apiTask.HandleError(activity);
                    apiTask.OnSucess(activity, (response) =>
                    {
                        var index = adapter.Items.FindIndex(m => m.Id == postId);
                        if(index  >= 0)
                        {
							adapter.Items[index] = response.Result;
							adapter.NotifyDataSetChanged();
                        }
                    });
                }

            }
        }

        public int GetNewsFeedItems(bool refresh)
        {
            if (!adapter.Items.Any())
            {
                activity.ShowProgressDialog();
            }

            LoadingData = true;
            DateTime queryDate = DateTime.UtcNow.AddMinutes(2);
            string strQueryDate = string.Format("{0:MM/dd/yyyy hh:mm tt}", queryDate);

            if (!refresh)
            {
                queryDate = DateTime.SpecifyKind(adapter.Items.LastOrDefault(m => !string.IsNullOrEmpty(m.Id)).DateCreatedUtc, DateTimeKind.Utc).ToLocalTime();
                strQueryDate = string.Format("{0:MM/dd/yyyy hh:mm tt}", queryDate);
            }
            else
            {
                RefreshRequested?.Invoke(this, EventArgs.Empty);
            }

            var apiTask = new ServiceApi().GetFeed(strQueryDate, id, type);
            apiTask.HandleError(activity);
            apiTask.OnSucess(activity, (response) =>
            {
                /*if (response.Result.Count == 0)
                {
                 
                }
                else
                {
                    //NoFeedItems.Visibility = ViewStates.Gone;
                }*/
               // }
                if (SwipeContainer != null)
                {
                    SwipeContainer.Refreshing = false;
                }
                activity.HideProgressDialog();
                itemsLoaded = response.Result.Count;


                if (refresh)
                {
                    adapter.Items = response.Result;
                    if (headerView != null)
                    {
                        adapter.Items.Insert(0, new FeedItem());
                    }
                }
                else
                {
                    adapter.Items.AddRange(response.Result);
                }
                if (refresh)
                {
                    adapter.NotifyDataSetChanged(); // Needs to be scrolled to the top
                    layoutManager.ScrollToPosition(0);
                }
                else
                {
                    adapter.NotifyDataSetChanged();
                }

                DataReceived?.Invoke(this, EventArgs.Empty);
                LoadingData = false;
            });
            return itemsLoaded;
        }
        /*
        public void UpdateComments(int position, int count)
        {
            adapter.Items[position].CommentCount = count;
            adapter.NotifyItemChanged(position);
        }
        */
        int GetViewType(FeedItem item, int position)
        {
            if (!string.IsNullOrEmpty(item.AdvertisementUrl))
            {
                return (int)FeedViewType.AdvertisementItem;
            }
            
            if (string.IsNullOrEmpty(item.Id))
            {
                return (int)FeedViewType.HeaderItem;
            }
            
            return (int)FeedViewType.NormalItem;
        }

        void GetView(RecyclerView.ViewHolder holder, FeedItem item, int position, int viewType)
        {
            IFeedCell cell = holder as IFeedCell;

            if(viewType == (int)FeedViewType.AdvertisementItem)
            {
                var advertisment = holder as AdvertisementViewHolder;
                advertisment.ItemView.Tag = position;
                advertisment.lblName.Text = item.Username;
                advertisment.lblContent.Text = item.Content;
                if (string.IsNullOrEmpty(advertisment.lblContent.Text))
                {
                    advertisment.lblContent.Visibility = ViewStates.Gone;
                }
                else
                {
                    advertisment.lblContent.Visibility = ViewStates.Visible;
                }

                var parameters = advertisment.imgImage.LayoutParameters as LinearLayout.LayoutParams;
                var height = Application.Context.Resources.DisplayMetrics.WidthPixels * item.ImageAspectRatio;
                parameters.Height = (int)height;
                advertisment.imgImage.LayoutParameters = parameters;

                advertisment.ProfileTask?.Cancel(item.ProfileUrl);
                if (!string.IsNullOrEmpty(item.ProfileUrl))
                {
                    advertisment.ProfileTask = new ImageLoaderHelper(ImageService.Instance.LoadUrl(item.ProfileUrl).SetSettings(advertisment.imgProfile, Resource.Drawable.DefProfPic, 60, new CircleTransformation()));
                }
                else
                {
                    advertisment.imgProfile.SetImageResource(Resource.Drawable.DefProfPic);
                }

                advertisment.ImageTask?.Cancel(item.ImageUrl);
                if (!string.IsNullOrEmpty(item.ImageUrl))
                {
                    advertisment.ImageTask = new ImageLoaderHelper(ImageService.Instance.LoadUrl(item.ImageUrl).SetSettings(advertisment.imgImage, Resource.Drawable.NoImageGraphic, 300));
                }
                else
                {
                    advertisment.imgImage.SetImageResource(Resource.Drawable.NoImageGraphic);
                }

                return;
            }
            else if (viewType == (int) FeedViewType.HeaderItem)
            {
                return;
            }

            if ((item.IsSharePost) && (!string.IsNullOrWhiteSpace(item.SharedUsername)))
            {
                var spanFromName = new SpannableString("Shared from" +" "+ item.SharedUsername);
                spanFromName.SetSpan(new ForegroundColorSpan(Color.LightGray), 0, 11, 0);
                spanFromName.SetSpan(new RelativeSizeSpan(1.0f), 0, 11, 0);
                spanFromName.SetSpan(new StyleSpan(TypefaceStyle.Normal), 0, 11, 0);
               
                spanFromName.SetSpan(new ForegroundColorSpan(Color.Black), 12, item.SharedUsername.Length + 12, 0);
                spanFromName.SetSpan(new RelativeSizeSpan(1.0f), 12, item.SharedUsername.Length + 12, 0);
                spanFromName.SetSpan(new StyleSpan(TypefaceStyle.Bold), 12, item.SharedUsername.Length + 12, 0);



                cell.llSharePost.Visibility = ViewStates.Visible;
                cell.lblSharedFrom.SetText(spanFromName, TextView.BufferType.Spannable);
            }
            else
            {
                cell.llSharePost.Visibility = ViewStates.Gone;
            }


            cell.lblContent.Text = item.Content;
            //cell.lblContent.MovementMethod = LinkMovementMethod.Instance;

            DateTime date = DateTime.SpecifyKind(item.DateCreatedUtc, DateTimeKind.Utc);
            if (date > DateTime.UtcNow)
            {
                date = DateTime.UtcNow;
            }

			cell.lblTimeAgo.Text = TimeAgoHelper.GetTimeAgo(date);


            string[] separators = { "@" };
            string value = item.Username;
            string[] words = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string schoolName = string.Empty;
            string sportsName = string.Empty;

            if (words.Count() > 1)
            {
                for (int i = 0; i < words.Length; i++)
                {
                    schoolName = words[0];
                    sportsName = words[1];
                }
            }
            else
            {
                schoolName = words[0];
            }

            var span = new SpannableString(schoolName + "\n"+ sportsName);
            span.SetSpan(new ForegroundColorSpan(Color.Black), 0, schoolName.Length, 0);
            span.SetSpan(new RelativeSizeSpan(1.0f), 0, schoolName.Length, 0);
            span.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, schoolName.Length, 0);
            if(!string.IsNullOrEmpty(sportsName)){
                span.SetSpan(new ForegroundColorSpan(Color.LightGray), schoolName.Length + 1, schoolName.Length + sportsName.Length + 1, 0);
                span.SetSpan(new RelativeSizeSpan(1.0f), schoolName.Length + 1, schoolName.Length + sportsName.Length + 1, 0);
                span.SetSpan(new StyleSpan(TypefaceStyle.Normal), schoolName.Length + 1, schoolName.Length + sportsName.Length + 1, 0);
            }
            cell.lblName.SetText(span, TextView.BufferType.Spannable);

            cell.ProfileTask?.Cancel(item.ProfileUrl);
            if (!string.IsNullOrEmpty(item.ProfileUrl))
            {
                cell.ProfileTask = new ImageLoaderHelper(ImageService.Instance.LoadUrl(item.ProfileUrl).SetSettings(cell.imgProfile, Resource.Drawable.DefProfPic, 60, new CircleTransformation()));
            }
            else
            {
                cell.imgProfile.SetImageResource(Resource.Drawable.DefProfPic);
            }
           
            cell.lblLikes.Text = item.LikeCount.ToString();
            cell.lblComments.Text = item.CommentCount.ToString();
            cell.lblTags.Text = item.TagCount.ToString();
            cell.lblShares.Text = item.ShareCount.ToString();

            cell.lblLikes.SetTextColor(item.IsLiked ? new Color(244, 62, 110) : new Color(136, 136, 136));
            cell.btnLike.SetImageResource(item.IsLiked ? Resource.Drawable.Liked : Resource.Drawable.Like);
            
            cell.lblComments.SetTextColor(item.IsCommented ? new Color(244, 62, 110) : new Color(136, 136, 136));
            cell.btnComment.SetImageResource(item.IsCommented ? Resource.Drawable.Commented : Resource.Drawable.Comment);

            cell.btnOptions.Visibility = user.Id == item.CreatedById ? ViewStates.Visible : ViewStates.Invisible;

            //cell.btnFacebook.Visibility = string.IsNullOrEmpty(item.FacebookUrl) ? ViewStates.Gone : ViewStates.Visible;
            //cell.btnTwitter.Visibility = string.IsNullOrEmpty(item.TwitterUrl) ? ViewStates.Gone : ViewStates.Visible;
            //cell.btnInstagram.Visibility = string.IsNullOrEmpty(item.InstagramUrl) ? ViewStates.Gone : ViewStates.Visible;
            
            if (string.IsNullOrEmpty(cell.lblContent.Text))
            {
                (cell.lblContent.Parent as ViewGroup).Visibility = ViewStates.Gone;
            }
            else
            {
                (cell.lblContent.Parent as ViewGroup).Visibility = ViewStates.Visible;
            }

            if (!string.IsNullOrEmpty(item.ImageUrl))
            {
                SetImageViewHeight(cell, item);

                cell.rlMedia.Visibility = ViewStates.Visible;
                cell.ImageTask?.Cancel(item.ImageUrl);
                if (!string.IsNullOrEmpty(item.ImageUrl))
                {
                    cell.ImageTask = new ImageLoaderHelper(ImageService.Instance.LoadUrl(item.ImageUrl).SetSettings(cell.imgImage, Resource.Drawable.NoImageGraphic, 300));
                }
                else
                {
                    cell.imgImage.SetImageResource(Resource.Drawable.NoImageGraphic);
                }
                cell.llLinkDetails.Visibility = ViewStates.Gone;
                cell.btnPlay.Visibility = ViewStates.Gone;
            }
            else if (!string.IsNullOrEmpty(item.VideoUrl))
            {
                SetImageViewHeight(cell, item);
                cell.rlMedia.Visibility = ViewStates.Visible;
                cell.ImageTask?.Cancel(item.VideoImageUrl);
                if (!string.IsNullOrEmpty(item.VideoImageUrl))
                {
                    cell.ImageTask = new ImageLoaderHelper(ImageService.Instance.LoadUrl(item.VideoImageUrl).SetSettings(cell.imgImage, Resource.Drawable.NoImageGraphic, 300));
                }
                else
                {
                    cell.imgImage.SetImageResource(Resource.Drawable.NoImageGraphic);
                }
                cell.llLinkDetails.Visibility = ViewStates.Gone;
                cell.btnPlay.Visibility = ViewStates.Visible;
            }
            else if (!string.IsNullOrEmpty(item.LinkUrl))
            {
                SetImageViewHeight(cell, item);
                cell.rlMedia.Visibility = ViewStates.Visible;
                cell.ImageTask?.Cancel(item.LinkImage);
                if (!string.IsNullOrEmpty(item.LinkImage))
                {
                    cell.ImageTask = new ImageLoaderHelper(ImageService.Instance.LoadUrl(item.LinkImage).SetSettings(cell.imgImage, Resource.Drawable.NoImageGraphic, 300));
                }
                else
                {
                    cell.imgImage.SetImageResource(Resource.Drawable.NoImageGraphic);
                }

                cell.lblLinkTitle.Text = item.LinkTitle;
                try
                {
                    var uri = new Uri(item.LinkUrl);
                    cell.lblLinkHost.Text = uri.Host;
                }
                catch
                {
                }

                cell.llLinkDetails.Visibility = ViewStates.Visible;
                cell.btnPlay.Visibility = ViewStates.Gone;
            }
            else
            {
                cell.rlMedia.Visibility = ViewStates.Gone;
            }
            
            cell.ItemView.Tag = position;
        }

        void SetImageViewHeight(IFeedCell cell, FeedItem item)
        {
            var parameters = cell.rlMedia.LayoutParameters as LinearLayout.LayoutParams;
            var height = Application.Context.Resources.DisplayMetrics.WidthPixels * item.ImageAspectRatio;
            parameters.Height = (int)height;
            cell.rlMedia.LayoutParameters = parameters;
        }

        void ImageClicked(FeedItem item)
        {
            if(!string.IsNullOrEmpty(item.LinkUrl))
            {
                Links.OpenUrl(item.LinkUrl);
            }
        }

        void UserClicked(FeedItem item)
        {
            var me = CrossSettings.Current.GetValueOrDefaultJson<User>("User");
            if (!string.IsNullOrEmpty(item.ContentSourceId))
            {
                Intent intent = new Intent(activity, typeof(ContentSourceActivity));
                intent.PutExtra("ContentSourceId", item.ContentSourceId);
                activity.StartActivity(intent);
            }
            else if (!string.IsNullOrEmpty(item.TeamId))
            {
                Navigator.GoToTeamProflie(item.TeamId);
            }
            else if (!string.IsNullOrEmpty(item.SchoolId))
            {
				Intent intent = new Intent(activity, typeof(SchoolProfileActivity));
				intent.PutExtra("SchoolId", item.SchoolId);
				activity.StartActivity(intent);
            }
            else if (!string.IsNullOrEmpty(item.SportId))
            {
                Navigator.GoToSportProflie(item.SportId);
            }
            else if (item.CreatedById == me.Id)
            {
                activity.StartActivity(typeof(MyProfileActivity));
            }
            else if(!string.IsNullOrEmpty(item.CreatedById))
            {
                Navigator.GoToUserProflie(item.CreatedById);
            }
        }

        void ShareClicked(FeedItem item)
        {

            var view = activity.LayoutInflater.Inflate(Resource.Layout.ShareOptionsLayout, null);
            var btnFacebook = view.FindViewById<Button>(Resource.Id.btnFacebook);
            var btnFanword = view.FindViewById<Button>(Resource.Id.btnFanword);
            var btnOther = view.FindViewById<Button>(Resource.Id.btnOther);

            btnFacebook.Visibility = ViewStates.Gone;

            var builder = new AlertDialog.Builder(activity);
            builder.SetView(view);

            ApplicationInfo info = null;
            try
            {
                info = activity.PackageManager.GetApplicationInfo("com.facebook.katana", 0);
            }
            catch (PackageManager.NameNotFoundException ex)
            {
            }

            activity.ShowProgressDialog();
            var apiTask = new ServiceApi().GetPost(item.Id);
            apiTask.HandleError(activity);
            apiTask.OnSucess(activity, (response) =>
            {
                activity.HideProgressDialog();
                var Post = response.Result;
                var dialog = builder.Create();
                if ((Post.Images.Any() || Post.Links.Any() || Post.Videos.Any()) && info != null)
                {
                    btnFacebook.Visibility = ViewStates.Visible;
                    btnFacebook.Click += async (s2, e2) =>
                    {
                        dialog.Dismiss();

                        activity.ShowProgressDialog();
                        await Sharer.ShareFacebook(Post);
                        activity.HideProgressDialog();
                        var saveShareTask = new ServiceApi().SaveShare(Post.Id);
                        saveShareTask.HandleError(activity);
                    };
                }

                btnFanword.Click += async (sender, args) =>
                {
                    dialog.Dismiss();
                    activity.ShowProgressDialog();
                    await Sharer.ShareFanword(Post);
                    GetNewsFeedItems(true);
                    activity.HideProgressDialog();
                };

                btnOther.Click += async (s2, e2) =>
                {
                    dialog.Dismiss();
                    activity.ShowProgressDialog();
                    await Sharer.ShareOther(Post);
                    activity.HideProgressDialog();
                    var saveShareTask = new ServiceApi().SaveShare(Post.Id);
                    saveShareTask.HandleError(activity);

                };

                dialog.Show();
            });
        }

        void PostDetailsClicked(string postId, string fragment)
        {
            Intent intent = new Intent(activity, typeof(PostDetailsActivity));
            intent.PutExtra("PostId", postId);
            intent.PutExtra("Fragment", fragment);
            activity.StartActivity(intent);
        }

        void LikeClicked(int position, ImageButton btnLike, TextView lblLikes)
        {
            var item = adapter.Items[position];
            btnLike.Enabled = false;

            if (item.IsLiked)
            {
                var apiTask = new ServiceApi().UnlikePost(item.Id);
                apiTask.HandleError(activity, true, () =>
                {
                    btnLike.Enabled = true;
                });
                apiTask.OnSucess(activity, (response) =>
                {
                    HandleLikeResult(false, position, btnLike, lblLikes);
                });
            }
            else
            {
                var apiTask = new ServiceApi().LikePost(item.Id);
                apiTask.HandleError(activity);
                apiTask.HandleError(activity, true, () =>
                {
                    btnLike.Enabled = true;
                });
                apiTask.OnSucess(activity, (response) =>
                {
                    HandleLikeResult(true, position, btnLike, lblLikes);
                });
            }
        }

        void PlayVideoClicked(FeedItem model)
        {
            if (Build.VERSION.SdkInt > BuildVersionCodes.Lollipop)
            {
                model.VideoUrl = model.VideoUrl.Replace("https", "http");
            }
            var uri = global::Android.Net.Uri.Parse(model.VideoUrl);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, "video/mp4");
            activity.StartActivity(intent);
        }

        void HandleLikeResult(bool isLIked, int position, ImageButton btnLike, TextView lblLikes)
        {
            adapter.Items[position].LikeCount = adapter.Items[position].LikeCount + (isLIked ? 1 : -1);
            adapter.Items[position].IsLiked = isLIked;
            lblLikes.Text = adapter.Items[position].LikeCount.ToString();
            btnLike.Enabled = true;
            adapter.NotifyDataSetChanged();
        }

        void OptionsClicked(FeedItem item)
        {
            if (item.CreatedById == CrossSettings.Current.GetValueOrDefaultJson<User>("User").Id)
            {
                var view = activity.LayoutInflater.Inflate(Resource.Layout.PostOptionsLayout, null);
                var dialog = new AlertDialog.Builder(activity).SetView(view).Create();
                view.FindViewById<Button>(Resource.Id.btnEditPost).Click += (sender, args) =>
                {
                    dialog.Dismiss();
                    Intent intent = new Intent(activity, typeof(EditPostActivity));
                    intent.PutExtra("PostId", item.Id);
                    activity.StartActivity(intent);
                };

                view.FindViewById<Button>(Resource.Id.btnDeletePost).Click += (sender, args) =>
                {
                    dialog.Dismiss();
                    new AlertDialog.Builder(activity).SetTitle("Confirm")
                        .SetMessage("Are you sure you want to delete this post")
                        .SetPositiveButton("Delete", (sender2, e2) =>
                        {
                            activity.ShowProgressDialog();
                            var apiTask = new ServiceApi().DeletePost(item.Id);
                            apiTask.HandleError(activity);
                            apiTask.OnSucess(activity, response =>
                            {
                                GetNewsFeedItems(true);
                            });
                        })
                        .SetNegativeButton("Cancel", (sender3, e3) => { })
                        .Show();
                };

                dialog.Show();
            }
        }

        void AdvertisementClicked(int position)
        {
            var item = adapter.Items[position];
            if (!string.IsNullOrEmpty(item.AdvertisementUrl))
            {
                Links.OpenUrl(item.AdvertisementUrl);
            }
        }

        public class ListPopupWindow
        {
        }
    }

    public class RecyclerViewScrollListener : RecyclerView.OnScrollListener
    {
        Action OnScroll;
        public RecyclerViewScrollListener(Action onScroll)
        {
            OnScroll = onScroll;
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);
            OnScroll();
        }
    }

    public enum FeedViewType
    {
        NormalItem,
        HeaderItem,
        AdvertisementItem,
        EmptyFeedItem
        
    }
}