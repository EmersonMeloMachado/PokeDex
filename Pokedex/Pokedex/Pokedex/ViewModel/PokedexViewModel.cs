using Pokedex.Extensions;
using Pokedex.Model;
using Pokedex.Sevices.Interface;
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

        private int _remainingItems = 0;

        public DelegateCommand<Pokemon> galeriaCommand;
        public DelegateCommand<Pokemon> navegarCommand;

        public DelegateCommand refreshCommand;

        #endregion Fields

        public DelegateCommand<Pokemon> GaleriaCommand => galeriaCommand ?? (galeriaCommand = new DelegateCommand<Pokemon>(async (pokemon) => await GaleriaCommandExecute(pokemon)));

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

        private Result _itemSelected;
        public Result ItemSelected
        {
            get { return _itemSelected; }
            set { SetProperty(ref _itemSelected, value); }
        }

        public DelegateCommand<Pokemon> NavegarCommand => navegarCommand ?? (navegarCommand = new DelegateCommand<Pokemon>(async (pokemon) => await NavegarCommandExecute(pokemon)));


        public DelegateCommand tresholdReachedCommand;

        public DelegateCommand ThresholdReachedCommand => tresholdReachedCommand ?? (tresholdReachedCommand = new DelegateCommand(async () => await ThresholdReachedCommandExecute()));

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

        public ObservableCollection<Pokemon> Pokemons { get; }

        public PokemonType PokemonType
        {
            get { return _pokemonType; }
            set { SetProperty(ref _pokemonType, value); }
        }

        public DelegateCommand RefreshCommand => refreshCommand ?? (refreshCommand = new DelegateCommand(async () => await RefreshCommandExecute()));

        public PokedexViewModel(INavigationService navigationService, IPokeApi pokeApi, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "Main Page";

            _pokeApi = pokeApi;
            _pageDialogService = pageDialogService;
            Pokemons = new ObservableCollection<Pokemon>();
        }

        private async Task GaleriaCommandExecute(Pokemon pokemon)
        {
            var parameter = new NavigationParameters();
            parameter.Add("pokemon", pokemon);

            //await NavigationService.NavigateAsync($"{nameof(GaleriaPage)}", parameter);
        }

        private async Task LoadPokemons()
        {
            try
            {
                IsBusy = true;
                Pokemons.Clear();
                var items = new List<Pokemon>();
                PokemonList = await _pokeApi.ObterListaPokemons(OffSetInicial, OffSetFinal);

                if (PokemonList != null)
                {
                    foreach (var poke in PokemonList.results)
                    {
                        var pokemon = await _pokeApi.ObterPokemon(poke.url);
                        if (pokemon != null)
                            items.Add(pokemon);
                    }

                    Pokemons.AddRange(items);
                    OffSetFinal += 20;
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                await _pageDialogService.DisplayAlertAsync("PokeApp", ex.Message, "OK");
            }
            finally
            {
                IsBusyTypes = false;
                IsRefreshing = false;
            }
        }

        private async Task NavegarCommandExecute(Pokemon pokemon)
        {
            //await PopupNavigation.Instance.PushAsync(new PokemonPopupPage(pokemon));
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
            if (PokemonType == null)
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

            if (navigationMode != NavigationMode.Back)
            {
                await LoadPokemons();
            }
        }
    }
}