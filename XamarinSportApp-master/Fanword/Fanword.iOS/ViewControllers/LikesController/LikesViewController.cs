// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using Fanword.Shared;
using Mobile.Extensions.iOS.Sources;
using Mobile.Extensions.iOS.Extensions;
using Fanword.Poco.Models;

namespace Fanword.iOS
{
	public partial class LikesViewController : UIViewController, IPostDetails
	{
		public string Name => "Likes";
		public int Count { get; set; }
		public string PostId { get; set; }
        public Action<string, int> UpdateParent;
		public LikesViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			GetData ();
		}

		void GetData ()
		{
			var apiTask = new ServiceApi ().GetLikes (PostId);
			apiTask.HandleError ();
			apiTask.OnSucess (response =>
			{
				var source = new CustomListSource<PostLike> (response.Result, GetCell, (arg1, arg2) => 55);
				source.NoContentText = "No Likes";
				tvLikes.Source = source;
				Count = response.Result.Count;
				tvLikes.ReloadData ();
				UpdateParent?.Invoke (Name, Count);
			});
		}

		public UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath, PostLike item)
		{
			var cell = tableView.DequeueReusableCell ("LikeCell", indexPath) as LikeCell;
			cell.SetData (item);
			cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			return cell;
		}

	}
}
