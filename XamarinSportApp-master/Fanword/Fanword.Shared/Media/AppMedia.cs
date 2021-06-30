using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PCLStorage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Settings;
using Polly;
using PortableFileUploader.Portable;
using Fanword.Poco.Models;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Linq;

namespace Fanword.Shared
{
	public class AppMedia
	{

		public static async Task<MediaFile> TakePhotoAsync (int customPhotoSize = 60, int compressQuality = 80)
		{
			var options = new StoreCameraMediaOptions ();
			options.Directory = "Fanword";
			options.Name = $"{Guid.NewGuid ()}.jpg";
			options.CustomPhotoSize = customPhotoSize;
			options.PhotoSize = PhotoSize.Custom;
			options.CompressionQuality = compressQuality;
			return await CrossMedia.Current.TakePhotoAsync (options);
		}

		public static async Task<MediaFile> PickPhotoAsync (int customPhotoSize = 60, int compressQuality = 80)
		{
			var options = new PickMediaOptions ();
			options.CustomPhotoSize = customPhotoSize;
			options.PhotoSize = PhotoSize.Custom;
			options.CompressionQuality = compressQuality;
			return await CrossMedia.Current.PickPhotoAsync (options);
		}

        public static async Task<MediaFile> TakeVideoAsync()
        {
            var options = new StoreVideoOptions();
            options.Directory = "Fanword";
            options.Name = $"{Guid.NewGuid()}.mp4";
            options.PhotoSize = PhotoSize.Large;
            return await CrossMedia.Current.TakeVideoAsync(options);
        }

		public static async Task<MediaFile> PickVideoAsync()
		{
			return await CrossMedia.Current.PickVideoAsync();
		}

	    public static async Task<AzureFileData> UploadMedia(string file, string rebuildUrl)
	    {
	        return await Task.Run(async () =>
	        {
	            FileUploader uploader = new FileUploader(ServiceApi.PortalURL, CrossSettings.Current.GetValueOrDefault("AccessToken", ""), true, 1024000);
	            AzureFileData azureFileData = null;
                bool done = false;

	            uploader.OnError += (sender, args) =>
	            {
	                done = true;
	            };

				uploader.OnFileComplete += (sender, args) =>
				{
					azureFileData = JsonConvert.DeserializeObject<AzureFileData>(args.Result);
					done = true;
				};

	            uploader.UploadFiles(new List<string>() {file}, "/api/Uploads/Part", rebuildUrl);

	            while (!done)
	            {
	                await Task.Delay(100);
	            }

	            return azureFileData;
	        });
	    }

        public static async Task<PostLink> GetLinkData(string link)
        {
            var postLink = new PostLink();
            try
            {
                if(!link.StartsWith("http"))
                {
                    link = "http://" + link;
                }
                var uri = new Uri(link);
            }
            catch
            {
                return null;
            }
            postLink.LinkUrl = link;
			var webGet = new HtmlWeb();
            try
            {
				var document = await webGet.LoadFromWebAsync(link);
				var metaNodes = document.DocumentNode.Descendants("meta").Where(m => m.Attributes.Contains("property")).ToList();
				var title = metaNodes.FirstOrDefault(m => m.Attributes.FirstOrDefault(j => j.Value == "og:title") != null)?.GetAttributeValue("content", "");
				var description = metaNodes.FirstOrDefault(m => m.Attributes.FirstOrDefault(j => j.Value == "og:description") != null)?.GetAttributeValue("content", "");
				var image = metaNodes.FirstOrDefault(m => m.Attributes.FirstOrDefault(j => j.Value == "og:image") != null)?.GetAttributeValue("content", "");

				var deEntitized = HtmlEntity.DeEntitize(Regex.Replace(title ?? "", "&apos;", "'"));
				title = Regex.Replace(deEntitized, @"&#39;", "'");
				postLink.Title = title;

				deEntitized = HtmlEntity.DeEntitize(Regex.Replace(description ?? "", "&apos;", "'"));
				description = Regex.Replace(deEntitized, @"&#39;", "'");
				postLink.Content = description;
				if (!string.IsNullOrEmpty(image))
				{
					if (!image.ToLower().StartsWith("http:") && !image.ToLower().StartsWith("https:"))
					{
						image = "http:" + image;
					}
				}

				postLink.ImageUrl = image;
			}
            catch
            {
                return null;
            }
            return postLink;
        }
    }
}
