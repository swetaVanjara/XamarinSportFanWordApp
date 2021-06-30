// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using Fanword.Poco.Models;

namespace Fanword.iOS
{
	public partial class EventTeamsCell : UITableViewCell
	{
        ImageLoaderHelper imageTask;
		public EventTeamsCell (IntPtr handle) : base (handle)
		{
		}

		public void SetData(EventTeam item)
		{
			lblSchool.Text = item.SchoolName;
            lblSport.Text = item.SportName;
            lblScore.Text = item.Score;

			imageTask?.Cancel(item.ProfileUrl);
			if (!string.IsNullOrEmpty(item.ProfileUrl))
			{
				imageTask = new ImageLoaderHelper(item.ProfileUrl, imgProfile, "DefaultProfile");
			}
		}
	}
}