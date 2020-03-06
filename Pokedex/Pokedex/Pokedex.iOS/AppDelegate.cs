using FFImageLoading.Forms.Platform;
using Foundation;
using Prism;
using Prism.Ioc;
using UIKit;

namespace Pokedex.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        #region Classes

        public class iOSInitializer : IPlatformInitializer
        {
            #region Methods

            public void RegisterTypes(IContainerRegistry container)
            {
            }

            #endregion Methods
        }

        #endregion Classes

        #region Methods

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            CachedImageRenderer.Init();
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        #endregion Methods
    }
}