// This file has been autogenerated from a class added in the UI designer.

using System;
using Fanword.Shared;
using Foundation;
using Mobile.Extensions.iOS.Extensions;
using UIKit;
using Fanword.Poco.Models;
using Mobile.Extensions.iOS.Sources;

namespace Fanword.iOS
{
	public partial class PostSharesViewController : UIViewController, IPostDetails
	{
		public string Name => "Shares";
		public int Count { get; set; }
		public string PostId { get; set; }
		public Action<string, int> UpdateParent;
		public PostSharesViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			GetData();
		}

		void GetData()
		{
			var apiTask = new ServiceApi().GetShares(PostId);
			apiTask.HandleError();
			apiTask.OnSucess(response =>
		   {
			   var source = new CustomListSource<PostShare>(response.Result, GetCell, (arg1, arg2) => 55);
			   source.NoContentText = "No Shares";
			   tvShares.Source = source;
			   Count = response.Result.Count;
			   tvShares.ReloadData();
			   UpdateParent?.Invoke(Name, Count);
		   });
		}

		public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath, PostShare item)
		{
			var cell = tableView.DequeueReusableCell("PostShareCell", indexPath) as PostShareCell;
			cell.SetData(item);
			cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			return cell;
		}
	}
}