using System;

using UIKit;
using Plugin.Settings;
using Mobile.Extensions.Extensions;
using Mobile.Extensions.iOS.ViewControllers;
using Mobile.Extensions.iOS.Extensions;
using Fanword.Shared.Helpers;

namespace Fanword.iOS
{
    public partial class BaseViewController : ExtensionsBaseViewController
    {
        public LoadingScreenViewController LoadingScreen;
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            LoadingScreen = Storyboard.InstantiateViewController<LoadingScreenViewController>();
            LoadingScreen.Parent = this;
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public void ShowHelpIfNecessary(TutorialHelper tutorial, Action dismissed = null)
        {
#if DEBUG
            //return;
#endif
            if (CrossSettings.Current.GetValueOrDefault(tutorial.Id, false))
			{
				dismissed?.Invoke();
				return;
			}
			CrossSettings.Current.AddOrUpdateValue(tutorial.Id, true);

            var controller = Storyboard.InstantiateViewController<TutorialViewController>();
            controller.Model = tutorial;
            controller.Dismissed = dismissed;
            controller.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
            controller.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            PresentViewController(controller, true, null);
        }


		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		public BaseViewController (IntPtr handle) : base(handle)
		{

		}

	}
}
