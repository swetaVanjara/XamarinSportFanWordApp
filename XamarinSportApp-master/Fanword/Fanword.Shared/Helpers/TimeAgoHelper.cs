using System;
namespace Fanword.Shared
{
	public class TimeAgoHelper
	{
		public static string GetTimeAgo (DateTime dateTime)
		{
			var difference = DateTime.UtcNow - dateTime;
			if (difference.TotalMinutes < 1)
			{
				return "Just Now";
			}

			if (difference.TotalMinutes < 60)
			{
				return ((int)difference.TotalMinutes) +"m";
			}

			if (difference.TotalDays < 1)
			{
				return ((int)difference.TotalMinutes / 60) +"h";
			}

			if (difference.TotalDays < 7)
			{
				return ((int)difference.TotalDays) +"d";
			}

			if (difference.TotalDays < 365)
			{
				return ((int)difference.TotalDays / 7) + "w";
			}
			else
			{
				return "Over a year";
			}
		}
	}
}
