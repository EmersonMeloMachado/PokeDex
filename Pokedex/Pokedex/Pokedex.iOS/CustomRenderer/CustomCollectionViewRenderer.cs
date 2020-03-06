using Foundation;
using Pokedex.Custom;
using Pokedex.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomCollectionView), typeof(CustomCollectionViewRenderer))]

namespace Pokedex.iOS.CustomRenderer
{
    public class CustomCollectionViewRenderer : CollectionViewRenderer
    {
        #region Methods

        protected override void OnElementChanged(ElementChangedEventArgs<GroupableItemsView> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                NSArray views = Control.ValueForKey(new NSString("_subviewCache")) as NSMutableArray;
                UICollectionView collectionView = views.GetItem<UICollectionView>(0);
                collectionView.Delegate = new MyCollectionViewDelegate((CustomCollectionView)Element);
            }
        }

        #endregion Methods
    }

    public class MyCollectionViewDelegate : UICollectionViewDelegate
    {
        #region Fields

        CustomCollectionView customView;

        #endregion Fields

        #region Constructors

        public MyCollectionViewDelegate(CustomCollectionView collectionView)
        {
            this.customView = collectionView;
        }

        #endregion Constructors

        #region Methods

        public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
        {
            customView.RaiseAppeared(indexPath.Row);
        }

        #endregion Methods
    }
}