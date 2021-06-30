using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Views;
using FFImageLoading.Work;

namespace Fanword.Android.Extensions
{
    public static class IScheduledWorkExtensions
    {
        public static IScheduledWork SetSettings(this TaskParameter task, ImageViewAsync imageview, int placeholder, int? downsampleSize = null, ITransformation transformation = null)
        {
            if (downsampleSize != null)
            {
                task = task.DownSampleInDip(downsampleSize ?? 0);
            }

            if (transformation != null)
            {
                task = task.Transform(transformation);
            }

            IScheduledWork work = null;
            task.Success((size, loadingResult) =>
            {
                System.Diagnostics.Debug.WriteLine(loadingResult);
            });

            work = task.Retry(5, 300)
                           .ErrorPlaceholder(placeholder.ToString(), ImageSource.CompiledResource)
                           .LoadingPlaceholder(placeholder.ToString(), ImageSource.CompiledResource)
                           .Error(exception =>
                           {
                               System.Diagnostics.Debug.WriteLine("------------------------------------------------------------------------------------------------------");
                               System.Diagnostics.Debug.WriteLine($"Error: {exception.ToString()}");
                               System.Diagnostics.Debug.WriteLine("------------------------------------------------------------------------------------------------------");
                           }).Into(imageview);
            return work;
        }
    }
}