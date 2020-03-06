using System;
using Xamarin.Forms;

namespace Pokedex.Custom
{
    public class CustomCollectionView : CollectionView
    {
        #region Events

        public event EventHandler Appeared;

        #endregion Events

        #region Methods

        public void RaiseAppeared(object element)
        {
            Appeared?.Invoke(element, null);
        }

        #endregion Methods
    }
}