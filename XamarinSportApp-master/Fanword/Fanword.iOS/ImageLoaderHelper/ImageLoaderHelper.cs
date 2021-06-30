using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Work;
using UIKit;

namespace Fanword.iOS
{
	public class ImageLoaderHelper
	{
		private string Url { get; set; }
		private IScheduledWork ImageTask { get; set; }
		private UIImageView ImageView { get; set; }
		private string Placeholder { get; set; }
		private int? DownsampleSize { get; set; }
		private ITransformation Transformation { get; set; }
		private Action<ImageInformation, LoadingResult> Success { get; set; }

		public ImageLoaderHelper (string url, UIImageView imageView, string placeHolder, int? downsampleSize = null, ITransformation tranformation = null, Action<ImageInformation, LoadingResult> success = null)
		{
			Url = url;
			ImageView = imageView;
			Placeholder = placeHolder;
			DownsampleSize = downsampleSize;
			Transformation = tranformation;
			Success = success;

			Load ();
		}

		void Load ()
		{
            var imageTask = ImageService.Instance.LoadUrl (Url).ErrorPlaceholder(Placeholder, ImageSource.CompiledResource).LoadingPlaceholder(Placeholder, ImageSource.CompiledResource).WithCustomLoadingPlaceholderDataResolver(new CustomDataResolver()).WithCustomErrorPlaceholderDataResolver(new CustomDataResolver());

            if (DownsampleSize != null)
			{
				imageTask = imageTask.DownSampleInDip (DownsampleSize ?? 0);
			}
			if (Transformation != null)
			{
				imageTask = imageTask.Transform (Transformation);
			}

			if (Success != null)
			{
                
				imageTask = imageTask.Success ((a,b) =>
                {
                    Success?.Invoke(a, b);
                });
			}
			ImageTask = imageTask.Into (ImageView);
		}

		public void Cancel (string url)
		{
			if (Url != url)
			{
				ImageTask?.Cancel ();
				ImageView.Image = UIImage.FromBundle (Placeholder);
			}
		}
	}

    public class CustomDataResolver : IDataResolver
    {
        public Task<Tuple<Stream, LoadingResult, ImageInformation>> Resolve(string identifier, TaskParameter parameters, CancellationToken token)
        {
            var t = Task.Run(() =>
            {
				Stream stream = null;
				LoadingResult result = LoadingResult.Disk;
				try
				{
					stream = UIImage.FromBundle(identifier).AsPNG().AsStream();

				}
				catch
				{
					result = LoadingResult.Failed;
				}

				var tuple = new Tuple<Stream, LoadingResult, ImageInformation>(stream, result, new ImageInformation());
                return tuple;
            });

            return t;
        }
    }
}
