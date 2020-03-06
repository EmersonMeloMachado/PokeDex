using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pokedex.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PokedexPage : ContentPage
    {
        #region Constructors

        public PokedexPage()
        {
            InitializeComponent();
        }

        #endregion Constructors
    }
}