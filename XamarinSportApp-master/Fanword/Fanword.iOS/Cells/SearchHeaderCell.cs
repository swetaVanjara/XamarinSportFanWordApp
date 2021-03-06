// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using Fanword.Poco.Models;
using System.Linq;

namespace Fanword.iOS
{
	public partial class SearchHeaderCell : UITableViewCell
	{
		public SearchHeaderCell (IntPtr handle) : base (handle)
		{
		}

		public void SetData(GlobalSearchItem item)
		{
            if(!string.IsNullOrEmpty(item.Title))
            {
                lblTitle.Text = item.Title;
            }
            else
            {
				var str = string.Concat(item.Type.ToString().Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' '); // Add spaces
				lblTitle.Text = str + "s";
            }
		}
	}
}
