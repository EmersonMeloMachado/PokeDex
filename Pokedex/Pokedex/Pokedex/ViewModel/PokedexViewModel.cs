using Pokedex.Extensions;
using Pokedex.Model;
using Pokedex.Sevices.Interface;
using Pokedex.View;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Pokedex.ViewModel
{
    public class PokedexViewModel : ViewModelBase
    {
        #region Fields

        private readonly IPageDialogService _pageDialogService;
        private readonly IPokeApi _pokeApi;
        private bool _isBusyAtualizacao;
        private bool _isBusyTypes;
        private PokemonType _pokemonType;
        bool isRefreshing;
        private int offSetInicial = 0;
        private int offSetFinal = 0;
        private PokemonList pokemonList; 
        private ObservableCollection<Pokemon> _pokemons;

        private int _remainingItems = 0;

        public DelegateCommand<Pokemon> navegarCommand;
        public DelegateCommand tresholdReachedCommand;

        public DelegateCommand refreshCommand;

        #endregion Fields

        public bool IsBusyAtualizacao
        {
            get => _isBusyAtualizacao;
            set => SetProperty(ref _isBusyAtualizacao, value);
        }

        public int RemainingItems
        {
            get { return _remainingItems; }
            set { SetProperty(ref _remainingItems, value); }
        }

        public bool IsBusyTypes
        {
            get { return _isBusyTypes; }
            set { SetProperty(ref _isBusyTypes, value); }
        }

        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        private Pokemon _itemSelected;
        public Pokemon ItemSelected
        {
            get { return _itemSelected; }
            set { SetProperty(ref _itemSelected, value); }
        }

        public DelegateCommand<Pokemon> NavegarCommand => navegarCommand ?? (navegarCommand = new DelegateCommand<Pokemon>(async (pokemon) => await NavegarCommandExecute(pokemon)));
        public DelegateCommand ThresholdReachedCommand => tresholdReachedCommand ?? (tresholdReachedCommand = new DelegateCommand(async () => await ThresholdReachedCommandExecute()));

        public DelegateCommand RefreshCommand => refreshCommand ?? (refreshCommand = new DelegateCommand(async () => await RefreshCommandExecute()));

        public int OffSetInicial
        {
            get { return offSetInicial; }
            set { SetProperty(ref offSetInicial, value); }
        }

        public int OffSetFinal
        {
            get { return offSetFinal; }
            set { SetProperty(ref offSetFinal, value); }
        }

        public PokemonList PokemonList
        {
            get { return pokemonList; }
            set { SetProperty(ref pokemonList, value); }
        }

        public ObservableCollection<Pokemon> Pokemons
        {
            get { return _pokemons; }
            set { SetProperty(ref _pokemons, value); }
        }

        public PokemonType PokemonType
        {
            get { return _pokemonType; }
            set { SetProperty(ref _pokemonType, value); }
        }

        public PokedexViewModel(INavigationService navigationService, IPokeApi pokeApi, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "Main Page";

            _pokeApi = pokeApi;
            _pageDialogService = pageDialogService;
            Pokemons = new ObservableCollection<Pokemon>();
        }

        private async Task LoadPokemons()
        {
            try
            {
                IsBusy = true;
                MockSkeleton();

                var items = new List<Pokemon>();
                PokemonList = await _pokeApi.ObterListaPokemons(OffSetInicial, OffSetFinal);

                if(PokemonList != null)
                {
                    foreach(var poke in PokemonList.results)
                    {
                        var pokemon = await _pokeApi.ObterPokemon(poke.url);
                        if(pokemon != null)
                            items.Add(pokemon);
                    }

                    Pokemons.Clear();
                    Pokemons.AddRange(items);
                    OffSetFinal += 20;
                }
                IsBusy = false;
            }
            catch(Exception ex)
            {
                await _pageDialogService.DisplayAlertAsync("PokeApp", ex.Message, "OK");
            }
            finally
            {
                IsBusyTypes = false;
                IsRefreshing = false;
            }
        }

        private void MockSkeleton()
        {
            this.Pokemons = new ObservableCollection<Pokemon>(new List<Pokemon> 
            {
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                },
                new Pokemon
                {
                    name = "",
                    sprites = new Sprites{ front_default  = ""}
                }
            });
        }

        private async Task NavegarCommandExecute(Pokemon pokemon)
        {
            var parameter = new NavigationParameters();
            parameter.Add("pokemon", pokemon);

            await NavigationService.NavigateAsync($"{nameof(DetalhePokemonPage)}", parameter);
        }

        private async Task ThresholdReachedCommandExecute()
        {

            if(IsBusy)
                return;

            try
            {
                if(OffSetFinal == 0) return;

                IsBusy = true;
                var items = new List<Pokemon>();

                OffSetInicial = OffSetFinal;

                PokemonList = await _pokeApi.ObterListaPokemons(OffSetInicial, OffSetFinal);

                if(PokemonList != null)
                {
                    foreach(var poke in PokemonList.results)
                    {
                        var pokemon = await _pokeApi.ObterPokemon(poke.url);
                        if(pokemon != null)
                            items.Add(pokemon);
                    }

                    Pokemons.AddRange(items);
                    OffSetFinal += 20;
                }
                IsBusy = false;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RefreshCommandExecute()
        {
            if(PokemonType == null)
                await LoadPokemons();

            Pokemons.Clear();
        }

        public IEnumerable<string> AutoComplete(string termo)
        {
            return PokemonType.results.Where(x => x.name.StartsWith(termo, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.name);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var navigationMode = parameters.GetNavigationMode();

            if(navigationMode != NavigationMode.Back)
            {
                await LoadPokemons();
            }
        }
    }
}