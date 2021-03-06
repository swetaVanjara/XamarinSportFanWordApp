// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using Mobile.Extensions.iOS.Extensions;
using Fanword.Poco.Models;

namespace Fanword.iOS
{
	public partial class ViewPostViewController : BaseViewController
	{
        FeedViewController feedController;
        public string PostId { get; set; }
		public ViewPostViewController (IntPtr handle) : base (handle)
		{
		}

		public override UIStatusBarStyle PreferredStatusBarStyle()
		{
			return UIStatusBarStyle.LightContent;
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			btnBack.TouchUpInside += (sender, e) => NavigationController.PopViewController(true);

			feedController = Storyboard.InstantiateViewController<FeedViewController>();
			feedController.HeaderView = null;
            feedController.Id = PostId;
			feedController.Type = FeedType.SinglePost;

			AddChildViewController(feedController);
			View.Add(feedController.View);
			var f = feedController.View.Frame;
			f.X = 0;
			f.Y = 70;
			f.Width = UIScreen.MainScreen.Bounds.Width;
			f.Height = UIScreen.MainScreen.Bounds.Height;
			feedController.View.Frame = f;
            feedController.HideAddPost();
		}
	}
}
