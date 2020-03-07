using Pokedex.Model;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.ViewModel
{
    public class GaleriaPopupViewModel : ViewModelBase
    {
        public ObservableCollection<string> Images { get; } = new ObservableCollection<string>();

        public GaleriaPopupViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var pokemon = parameters.GetValue<Pokemon>("pokemon");

            if(pokemon != null)
                LoadGalery(pokemon).ConfigureAwait(false);

        }

        async Task LoadGalery(Pokemon pokemon)
        {
            Images.Add(pokemon.sprites.front_default);
            Images.Add(pokemon.sprites.back_default);
            Images.Add(pokemon.sprites.front_shiny);
            Images.Add(pokemon.sprites.back_shiny);
        }
    }
}
