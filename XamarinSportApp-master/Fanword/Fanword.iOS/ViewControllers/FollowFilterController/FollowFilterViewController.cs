// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;

namespace Fanword.iOS
{
	public partial class FollowFilterViewController : UIViewController
	{

		public bool MySchools;
		public bool MySports;
		public bool MyTeams;
		public Action SetValues;

		public FollowFilterViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			View.BackgroundColor = UIColor.Clear;

			btnTeams.SetImage (UIImage.FromBundle (MyTeams ? "IconChecked" : "IconUnchecked"), UIControlState.Normal);
			btnSports.SetImage (UIImage.FromBundle (MySports ? "IconChecked" : "IconUnchecked"), UIControlState.Normal);
			btnSchools.SetImage (UIImage.FromBundle (MySchools ? "IconChecked" : "IconUnchecked"), UIControlState.Normal);

			btnTeams.TouchUpInside += (sender, e) =>
			{
				MyTeams = !MyTeams;
				btnTeams.SetImage (UIImage.FromBundle (MyTeams ? "IconChecked" : "IconUnchecked"), UIControlState.Normal);
			};

			btnSports.TouchUpInside += (sender, e) =>
			{
				MySports = !MySports;
				btnSports.SetImage (UIImage.FromBundle (MySports ? "IconChecked" : "IconUnchecked"), UIControlState.Normal);
			};

			btnSchools.TouchUpInside += (sender, e) =>
			{
				MySchools = !MySchools;
				btnSchools.SetImage (UIImage.FromBundle (MySchools ? "IconChecked" : "IconUnchecked"), UIControlState.Normal);
			};

			btnSearch.TouchUpInside += (sender, e) =>
			{
				SetValues?.Invoke ();
				DismissViewController (true, null);
			};
		}
	}
}
