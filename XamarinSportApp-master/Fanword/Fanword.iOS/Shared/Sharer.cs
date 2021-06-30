using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AssetsLibrary;
using Facebook.ShareKit;
using Fanword.Poco.Models;
using Fanword.Shared;
using Foundation;
using UIKit;

namespace Fanword.iOS.Shared
{
    public class Sharer
    {
        public static async Task ShareFacebook(UIViewController controller,Post post)
		{
			var dialog = new ShareDialog();
			dialog.FromViewController = controller;
			dialog.Mode = ShareDialogMode.Automatic;

			if (post.Links.Any())
			{
				var content = new ShareLinkContent();

				content.SetContentUrl(new NSUrl(post.Links.FirstOrDefault().LinkUrl));
				dialog.SetShareContent(content);
			}
			if (post.Images.Any())
			{
				var content = new SharePhotoContent();
				var path = await DownloadFileIfNecessary(post.Images.FirstOrDefault().Url);
				content.Photos = new SharePhoto[1] { SharePhoto.From(UIImage.FromFile(path), true) };
				dialog.SetShareContent(content);
			}

			if (post.Videos.Any())
			{
				var path = await DownloadFileIfNecessary(post.Videos.FirstOrDefault().Url);
				ALAssetsLibrary library = new ALAssetsLibrary();
				library.WriteVideoToSavedPhotosAlbum(NSUrl.FromFilename(path), (arg1, arg2) =>
				 {
					 controller.InvokeOnMainThread(() =>
					 {
						 var content = new ShareVideoContent();
						 content.Video = ShareVideo.From(arg1.AbsoluteUrl);
						 dialog.SetShareContent(content);
						 dialog.Show();
					 });
				 });
			}
			NSError error = new NSError();
			dialog.ValidateWithError(out error);

			var result = dialog.Show();
		}

		public async static Task ShareFanword(Post post)
		{
			await new ServiceApi().Clone(post.Id);
		}
		
        public static async Task ShareOther(UIViewController parent, Post post, Action completedAction)
        {
			List<NSObject> shareItems = new List<NSObject>();
			if (!string.IsNullOrEmpty(post.Content))
			{
				shareItems.Add(new NSString(post.Content));
			}
			if (post.Links.Any())
			{
				shareItems.Add(new NSUrl(post.Links.FirstOrDefault().LinkUrl));
			}
			if (post.Images.Any())
			{
                var path = await DownloadFileIfNecessary(post.Images.FirstOrDefault().Url);
				shareItems.Add(UIImage.FromFile(path));
			}
			if (post.Videos.Any())
			{
				var path = await DownloadFileIfNecessary(post.Videos.FirstOrDefault().Url);
				shareItems.Add(NSUrl.FromFilename(path));
			}

			UIActivityViewController controller = new UIActivityViewController(shareItems.ToArray(), null);
			controller.ExcludedActivityTypes = new NSString[1] { new NSString("com.apple.UIKit.activity.PostToFacebook") };
			controller.CompletionWithItemsHandler = (activityType, completed, returnedItems, error) =>
			{
				if (completed)
				{
                    completedAction?.Invoke();
				}
			};
			parent.PresentViewController(controller, true, null);
        }

		static async Task<string> DownloadFileIfNecessary(string path)
		{
			if (path.StartsWith("http"))
			{
				HttpClient client = new HttpClient();
                string newFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Guid.NewGuid().ToString() + Path.GetExtension(path));
				File.WriteAllBytes(newFile, await client.GetByteArrayAsync(path));
				return newFile;
			}
			return path;
		}
    }
}
