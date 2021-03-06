// This file has been autogenerated from a class added in the UI designer.

using System;
using Fanword.Poco.Models;
using UIKit;

namespace Fanword.iOS
{
    public partial class ScoreHeaderCell : UITableViewCell
    {
        public ScoreHeaderCell(IntPtr handle) : base(handle)
        {
        }

        public void SetData(ScoreModel item, int position)
        {
            vwSpacer.Hidden = position == 0;
            lblDate.Text = item.EventDate.ToString("D").ToUpper();
            lblCount.Text = item.TeamCount.ToString();
        }
    }
}
