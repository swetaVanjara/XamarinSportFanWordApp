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
using FFImageLoading.Drawables;
using FFImageLoading.Targets;
using FFImageLoading.Views;
using FFImageLoading.Work;

namespace Fanword.Android
{
    public class ImageLoaderHelper : Java.Lang.Object
    {
        IScheduledWork _work;
        public ImageLoaderHelper(IScheduledWork work)
        {
            _work = work;
        }

        public void Cancel(string url)
        {
            var task = _work as ImageLoaderTask<SelfDisposingBitmapDrawable, ImageViewAsync>;
            if (task == null)
                return;
            if (task.Parameters.Path != url)
            {
                _work.Cancel();
                try
                {
                    ImageService.Instance.LoadCompiledResource(task.Parameters.LoadingPlaceholderPath).Into((task.Target as ImageViewTarget).Control);
                }
                catch { }
            }
        }
    }
}