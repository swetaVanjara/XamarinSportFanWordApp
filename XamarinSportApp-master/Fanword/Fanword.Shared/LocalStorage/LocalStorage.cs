using System;
using System.Threading.Tasks;
using Fanword.Poco.Models;
using Mobile.Extensions.Extensions;
using Plugin.Settings;
using System.Diagnostics;

namespace Fanword.Shared
{
	public class LocalStorage
	{
		public static void SaveLogin (AccessTokenResponse Result)
		{
			CrossSettings.Current.AddOrUpdateValue ("AccessToken", Result.access_token);
			CrossSettings.Current.AddOrUpdateValue ("RefreshToken", Result.refresh_token);
			CrossSettings.Current.AddOrUpdateValue ("AccessTokenIssueDate", DateTime.UtcNow.ToString ());
			CrossSettings.Current.AddOrUpdateValue ("RefreshTokenTokenIssueDate", DateTime.UtcNow.ToString ());
			CrossSettings.Current.AddOrUpdateValue ("PortalUrl", ServiceApi.PortalURL);
			CrossSettings.Current.AddOrUpdateValue ("User", Result.user);
		}

	    public static void UpdateUser(User user)
	    {
	        CrossSettings.Current.AddOrUpdateJson("User", user);
	    }

        public static bool IsLoggedIn()
	    {
	        var token = CrossSettings.Current.GetValueOrDefault("AccessToken", "");
            var portalUrl = CrossSettings.Current.GetValueOrDefault("PortalUrl", "");
            var refreshTokenIssueDateString = CrossSettings.Current.GetValueOrDefault("RefreshTokenTokenIssueDate", new DateTime().ToString());
            var refreshTokenIssueDate = DateTime.Parse(refreshTokenIssueDateString);

            if (string.IsNullOrEmpty(token) || portalUrl != ServiceApi.PortalURL)
            {
                return false;
            }

            if (DateTime.UtcNow > refreshTokenIssueDate.AddYears(1))
            {
                return false;
            }
	        var oldRefreshToken = CrossSettings.Current.GetValueOrDefault("RefreshToken", "");
	        CrossSettings.Current.AddOrUpdateValue("OldRefreshToken", oldRefreshToken);
            // Ask for new token in the background
            var apiTask = new ServiceApi().RefreshToken(CrossSettings.Current.GetValueOrDefault("RefreshToken", ""));
            apiTask.ContinueWith(response =>
            {
                var tokenResponse = response.Result;
                SaveLogin(response.Result);
                new ServiceApi().GetMyUser();
                var apiTask2 = new ServiceApi().RefreshTokenSuccess(tokenResponse.access_token, CrossSettings.Current.GetValueOrDefault("OldRefreshToken", ""));
                apiTask2.ContinueWith((t) =>
                {
                    var exc = t.Exception;
                    Debugger.Break();
                }, TaskContinuationOptions.NotOnRanToCompletion);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
           
            return true;
	    }
	}
}
