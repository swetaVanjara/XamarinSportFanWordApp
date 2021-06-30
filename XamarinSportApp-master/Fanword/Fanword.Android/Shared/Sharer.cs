

using System;
using System.Linq;
using Android.Content;
using Fanword.Poco.Models;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Fanword.Shared;
using Plugin.CurrentActivity;
using Xamarin.Facebook.Share.Model;
using Xamarin.Facebook.Share.Widget;

namespace Fanword.Android.Shared
{
    public class Sharer
    {
        public static async Task ShareFacebook(Post post)
        {
            if (post.Images.Any())
            {
                string file = await DownloadFileIfNecessary(post.Images.FirstOrDefault().Url);
                SharePhoto photo = new SharePhoto.Builder().SetImageUrl(global::Android.Net.Uri.FromFile(new Java.IO.File(file))).Build() as SharePhoto;
                SharePhotoContent content = new SharePhotoContent.Builder().AddPhoto(photo).Build();
                ShareDialog.Show(CrossCurrentActivity.Current.Activity, content);
            }
            if (post.Links.Any())
            {
                ShareLinkContent content = (new ShareLinkContent.Builder().SetContentUrl(global::Android.Net.Uri.Parse(post.Links.FirstOrDefault().LinkUrl)) as ShareLinkContent.Builder).Build();
                ShareDialog.Show(CrossCurrentActivity.Current.Activity, content);
            }
            if (post.Videos.Any())
            {
                string file = await DownloadFileIfNecessary(post.Videos.FirstOrDefault().Url);
                ShareVideo video = new ShareVideo.Builder().SetLocalUrl(global::Android.Net.Uri.FromFile(new Java.IO.File(file))).Build() as ShareVideo;
                ShareVideoContent content = new ShareVideoContent.Builder().SetVideo(video).Build();
                ShareDialog.Show(CrossCurrentActivity.Current.Activity, content);
            }
        }

        public async static Task ShareFanword(Post post)
        {
            await new ServiceApi().Clone(post.Id);
        }

        public static async Task ShareOther(Post post)
        {
            Intent sendIntent = new Intent(Intent.ActionSend);
            if (!string.IsNullOrEmpty(post.Content))
            {
                sendIntent.PutExtra(Intent.ExtraText, post.Content);
                sendIntent.SetType("text/plain");
                if (post.Images.Any())
                {
                    string file = await DownloadFileIfNecessary(post.Images.FirstOrDefault().Url);
                    sendIntent.PutExtra(Intent.ExtraStream, global::Android.Net.Uri.FromFile(new Java.IO.File(file)));
                    sendIntent.SetType("*/*");
                }
                if (post.Links.Any())
                {
                    sendIntent.PutExtra(Intent.ExtraText, post.Links.FirstOrDefault().LinkUrl);
                    sendIntent.SetType("text/plain");
                }
                if (post.Videos.Any())
                {
                    string file = await DownloadFileIfNecessary(post.Videos.FirstOrDefault().Url);
                    sendIntent.PutExtra(Intent.ExtraStream, global::Android.Net.Uri.Parse(file));
                    sendIntent.SetType("*/*");
                }
            }
            else
            {
                if (post.Images.Any())
                {
                    string file = await DownloadFileIfNecessary(post.Images.FirstOrDefault().Url);
                    sendIntent.PutExtra(Intent.ExtraStream, global::Android.Net.Uri.FromFile(new Java.IO.File(file)));
                    sendIntent.SetType("image/*");
                }
                if (post.Links.Any())
                {
                    sendIntent.PutExtra(Intent.ExtraText, post.Links.FirstOrDefault().LinkUrl);
                    sendIntent.SetType("text/plain");
                }
                if (post.Videos.Any())
                {
                    string file = await DownloadFileIfNecessary(post.Videos.FirstOrDefault().Url);
                    sendIntent.PutExtra(Intent.ExtraStream, global::Android.Net.Uri.FromFile(new Java.IO.File(file)));
                    sendIntent.SetType("video/mp4");
                }
            }
            CrossCurrentActivity.Current.Activity.StartActivity(Intent.CreateChooser(sendIntent, "Share To"));
        }

        static async Task<string> DownloadFileIfNecessary(string path)
        {
            if (path.StartsWith("http"))
            {
                HttpClient client = new HttpClient();
                string newFile = Path.Combine(CrossCurrentActivity.Current.Activity.GetExternalFilesDir("FanwordShares").AbsolutePath, Guid.NewGuid().ToString() + Path.GetExtension(path));
                File.WriteAllBytes(newFile, await client.GetByteArrayAsync(path));
                return newFile;
            }
            return path;
        }
    }

}