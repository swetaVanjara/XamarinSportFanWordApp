using System;
using UIKit;
namespace Fanword.iOS.Shared
{
    public class ColorHelper
    {
        public static UIColor GetColor(string hex, UIColor defaultColor)
        {
			if (!string.IsNullOrEmpty(hex))
			{
				try
				{
					var colorString = hex.Replace("#", "");
					var color = UIColor.FromRGB(
						Convert.ToInt32(colorString.Substring(0, 2), 16),
						Convert.ToInt32(colorString.Substring(2, 2), 16),
						Convert.ToInt32(colorString.Substring(4, 2), 16)
					);

                    return color;
				}
				catch { }
			}

            return defaultColor;
        }
    }
}
