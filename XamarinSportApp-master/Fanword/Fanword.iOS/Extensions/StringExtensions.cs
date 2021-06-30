using System;
using UIKit;
namespace Fanword.iOS
{
	public static class StringExtensions
	{
		public static bool DisplayErrorMessage (this string message)
		{
			var hasError = !string.IsNullOrEmpty (message);
			if (hasError)
			{
				new UIAlertView ("Error", message, null, "Ok", null).Show ();
			}
			return hasError;
		}
	}
}
