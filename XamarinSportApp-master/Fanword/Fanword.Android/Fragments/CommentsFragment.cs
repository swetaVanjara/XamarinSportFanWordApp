using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fanword.Android.Activities.PostDetails;
using Fanword.Android.Extensions;
using Fanword.Android.Interfaces;
using Fanword.Poco.Models;
using Fanword.Shared;
using Fanword.Shared.Helpers;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using FFImageLoading.Work;
using Humanizer;
using Mobile.Extensions.Android.Adapters;
using Mobile.Extensions.Android.Extensions;

namespace Fanword.Android.Fragments
{
    public class CommentsFragment : BaseFragment, IPostDetails
    {
        public string Name => "Comments";
        public int Count { get; set; }
        private ListView lvComments { get; set; }
        private EditText txtComment { get; set; }
        private Button btnSend { get; set; }
        CustomListAdapter<Comment> adapter;
        private string expandedCommentId = null;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.CommentsFragment, null);
            this.PopulateViewProperties(view);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            GetData(true);

            btnSend.Click += (sender, args) =>
            {
                if (string.IsNullOrEmpty(txtComment.Text))
                    return;

                ActivityProgresDialog.ShowProgressDialog();
                var apiTask = new ServiceApi().SaveComment(txtComment.Text, (Activity as PostDetailsActivity).PostId, expandedCommentId);
                apiTask.HandleError(ActivityProgresDialog);
                apiTask.OnSucess(ActivityProgresDialog, response =>
                {
                    txtComment.Text = "";
                    GetData(true);
                    ActivityProgresDialog.HideProgressDialog();
                });
            };
        }

        void GetData(bool updateParent = false)
        {
            var apiTask = new ServiceApi().GetComments((Activity as PostDetailsActivity).PostId);
            apiTask.HandleError(ActivityProgresDialog);
            apiTask.OnSucess(ActivityProgresDialog, response =>
            {
                adapter = new CustomListAdapter<Comment>(response.Result, GetView);
                adapter.Items = CommentsHelper.GetCommentsWithReply(adapter.AllItems, expandedCommentId);
                adapter.NoContentText = "No Comments";
                lvComments.Adapter = adapter;
                Count = response.Result.Count;
                ActivityProgresDialog.HideProgressDialog();
                if (updateParent)
                {
                    (Activity as PostDetailsActivity).SetHeaderInfo(Name, Count);
                }

                lvComments.Post(() =>
                {
                    lvComments.SetSelection(adapter.Items.Count - 1);
                });
            });
        }

        View GetView(Comment item, int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = Activity.LayoutInflater.Inflate(Resource.Layout.CommentItem, null);
                view.FindViewById<ImageButton>(Resource.Id.btnLike).Click += (sender, args) => LikeClicked((int)view.Tag, sender as ImageButton, view.FindViewById<TextView>(Resource.Id.lblLikes));
                view.FindViewById<Button>(Resource.Id.btnReply).Click += (sender, args) =>
                {
                    var adapter = lvComments.Adapter as CustomListAdapter<Comment>;
                    var model = adapter.Items[(int) view.Tag];
                    
                    var newExpandedCommentId = string.IsNullOrEmpty(model.ParentCommentId) ? model.Id : model.ParentCommentId;
                    if (expandedCommentId == newExpandedCommentId)
                    {
                        SetExpandedCommentId(null);
                        return;
                    }
                    SetExpandedCommentId(newExpandedCommentId);
                    adapter.Items = CommentsHelper.GetCommentsWithReply(adapter.AllItems, expandedCommentId);
                    adapter.NotifyDataSetChanged();
                };
            }
            view.Tag = position;

            view.FindViewById<TextView>(Resource.Id.lblName).Text = item.Username;
            view.FindViewById<TextView>(Resource.Id.lblContent).Text = item.Content;
			view.FindViewById<TextView>(Resource.Id.lblTimeAgo).Text = TimeAgoHelper.GetTimeAgo(item.DateCreatedUtc);
            view.FindViewById<TextView>(Resource.Id.lblLikes).Text = item.LikeCount.ToString();
            view.FindViewById<TextView>(Resource.Id.lblReplies).Text = item.ReplyCount.ToString();
            view.FindViewById<TextView>(Resource.Id.lblResponseTo).Text = "Response to " + item.RepliedToUsername;
            view.FindViewById<ImageButton>(Resource.Id.btnLike).SetImageResource(item.IsLiked ? Resource.Drawable.Liked: Resource.Drawable.Like);

            var profileImageView = view.FindViewById<ImageViewAsync>(Resource.Id.imgProfile);
            profileImageView.Tag?.CancelPendingTask(item.ProfileUrl);
            var task = ImageService.Instance.LoadUrl(item.ProfileUrl)
                .Retry(3, 300)
                .LoadingPlaceholder(Resource.Drawable.DefProfPic.ToString(), ImageSource.CompiledResource)
                .Transform(new CircleTransformation())
                .DownSample(100)
                .Into(profileImageView);

            profileImageView.Tag = new ImageLoaderHelper(task);

            if (string.IsNullOrEmpty(item.ParentCommentId))
            {
                view.FindViewById<TextView>(Resource.Id.lblResponseTo).Visibility = ViewStates.Gone;
                view.SetBackgroundColor(Color.White);
				view.FindViewById<TextView> (Resource.Id.lblReplies).Visibility = ViewStates.Visible;
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.lblResponseTo).Visibility = ViewStates.Visible;
                view.SetBackgroundColor(new Color(244,244,244));
				view.FindViewById<TextView> (Resource.Id.lblReplies).Visibility = ViewStates.Gone;
            }
            return view;
        }

        void SetExpandedCommentId(string commentId)
        {
            if (string.IsNullOrEmpty(commentId))
            {
                txtComment.Hint = "Comment...";
            }
            else
            {
                var adapter = lvComments.Adapter as CustomListAdapter<Comment>;
                var model = adapter.AllItems.FirstOrDefault(m => m.Id == commentId);
                txtComment.Hint = "Reply to " + model.Username;
            }
            expandedCommentId = commentId;
        }

        void LikeClicked(int position, ImageButton btnLike, TextView lblLikes)
        {
            var item = adapter.Items[position];
            btnLike.Enabled = false;

            if (item.IsLiked)
            {
                var apiTask = new ServiceApi().UnlikeComment(item.Id);
                apiTask.HandleError(ActivityProgresDialog, true, () =>
                {
                    btnLike.Enabled = true;
                });
                apiTask.OnSucess(ActivityProgresDialog, (response) =>
                {
                    HandleLikeResult(false, position, btnLike, lblLikes);
                });
            }
            else
            {
                var apiTask = new ServiceApi().LikeComment(item.Id);
                apiTask.HandleError(ActivityProgresDialog);
                apiTask.HandleError(ActivityProgresDialog, true, () =>
                {
                    btnLike.Enabled = true;
                });
                apiTask.OnSucess(ActivityProgresDialog, (response) =>
                {
                    HandleLikeResult(true, position, btnLike, lblLikes);
                });
            }
        }

        void HandleLikeResult(bool isLIked, int position, ImageButton btnLike, TextView lblLikes)
        {
            adapter.Items[position].LikeCount = adapter.Items[position].LikeCount + (isLIked ? 1 : -1);
            adapter.Items[position].IsLiked = isLIked;
            lblLikes.Text = adapter.Items[position].LikeCount.ToString();
            btnLike.Enabled = true;
            adapter.NotifyDataSetChanged();
        }
    }
}