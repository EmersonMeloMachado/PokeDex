using Android.Content;
using Android.Support.V7.Widget;
using Pokedex.Custom;
using Pokedex.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomCollectionView), typeof(CustomCollectionViewRenderer))]

namespace Pokedex.Droid.CustomRenderer
{
    public class CustomCollectionViewRenderer : CollectionViewRenderer
    {
        #region Fields

        private readonly Context _context;

        private CustomCollectionView _enhancedCollectionView;

        #endregion Fields

        #region Constructors

        public CustomCollectionViewRenderer(Context context) : base(context)
        {
            _context = context;
        }

        #endregion Constructors

        #region Methods

        private void EnhancedCollectionViewRenderer_ChildViewAdded(object sender, ChildViewAddedEventArgs e)
        {
            _enhancedCollectionView.RaiseAppeared(((RecyclerView)e.Parent).GetChildViewHolder(e.Child).AdapterPosition);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ItemsView> elementChangedEvent)
        {
            base.OnElementChanged(elementChangedEvent);

            if (elementChangedEvent.NewElement is CustomCollectionView enhancedCollectionView)
            {
                _enhancedCollectionView = enhancedCollectionView;
                ChildViewAdded += EnhancedCollectionViewRenderer_ChildViewAdded;
            }
        }

        #endregion Methods
    }
}