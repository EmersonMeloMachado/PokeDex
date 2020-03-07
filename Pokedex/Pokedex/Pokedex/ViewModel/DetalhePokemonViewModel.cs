using Pokedex.Model;
using Pokedex.Sevices.Interface;
using Pokedex.View.Popups;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;

namespace Pokedex.ViewModel
{
    public class DetalhePokemonViewModel : ViewModelBase
    {
        private Pokemon _pokemon = new Pokemon();
        public Pokemon Pokemon
        {
            get { return _pokemon; }
            set { SetProperty(ref _pokemon, value); }
        }

        public DetalhePokemonViewModel(INavigationService navigationService, IPokeApi pokeApi, IPageDialogService pageDialogService) : base(navigationService)
        {

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var pokemon = parameters.GetValue<Pokemon>("pokemon");

            if(pokemon != null)
            {
                DefinirTipo(pokemon);
                Pokemon = pokemon;
            }
        }

        private void DefinirTipo(Pokemon pokemon)
        {
            int Type = pokemon.types.Length;
            for(int i = 0; i < Type; i++)
            {
                switch(pokemon.types[i].type.name)
                {
                    case "grass":
                        pokemon.types[i].type.TipoColor = "#9bcc50";
                        break;
                    case "poison":
                        pokemon.types[i].type.TipoColor = "#b97fc9";
                        break;
                    case "water":
                        pokemon.types[i].type.TipoColor = "#4592c4";
                        break;
                    case "fire":
                        pokemon.types[i].type.TipoColor = "#fd7d24";
                        break;
                    case "flying":
                        pokemon.types[i].type.TipoColor = "#3dc7ef";
                        break;
                    case "electric":
                        pokemon.types[i].type.TipoColor = "#eed535";
                        break;
                    case "steel":
                        pokemon.types[i].type.TipoColor = "#9eb7b8";
                        break;
                    case "ground":
                        pokemon.types[i].type.TipoColor = "#ab9842";
                        break;
                    case "rock":
                        pokemon.types[i].type.TipoColor = "#a38c21";
                        break;
                    case "ice":
                        pokemon.types[i].type.TipoColor = "#51c4e7";
                        break;
                    case "ghost":
                        pokemon.types[i].type.TipoColor = "#7b62a3";
                        break;
                    case "fighting":
                        pokemon.types[i].type.TipoColor = "#d56723";
                        break;
                    case "psychic":
                        pokemon.types[i].type.TipoColor = "#f366b9";
                        break;
                    case "dark":
                        pokemon.types[i].type.TipoColor = "#707070";
                        break;
                    case "normal":
                        pokemon.types[i].type.TipoColor = "#a4acaf";
                        break;
                    case "fairy":
                        pokemon.types[i].type.TipoColor = "#fdb9e9";
                        break;
                    case "bug":
                        pokemon.types[i].type.TipoColor = "#729f3f";
                        break;
                    case "dragon":
                        pokemon.types[i].type.TipoColor = "#f16e57";
                        break;
                }
            }
        }

        public DelegateCommand galeriaCommand;
        public DelegateCommand GaleriaCommand => galeriaCommand ?? (galeriaCommand = new DelegateCommand(async () => await GaleriaCommandExecute()));

        private async Task GaleriaCommandExecute()
        {
            var parameter = new NavigationParameters();
            parameter.Add("pokemon", Pokemon);

            await NavigationService.NavigateAsync($"{nameof(GaleriaPopupPage)}", parameter);
        }
    }
}
