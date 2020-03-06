using Pokedex.Sevices;
using Pokedex.Sevices.Interface;
using Pokedex.View;
using Pokedex.ViewModel;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Pokedex
{
    public partial class App : PrismApplication
    {
        #region Constructors

        public App() : this(null)
        {
        }

        public App(IPlatformInitializer initializer)
             : this(initializer, true)
        {
        }

        public App(IPlatformInitializer initializer, bool setFormsDependencyResolver)
            : base(initializer, setFormsDependencyResolver)
        {
        }

        #endregion Constructors

        #region Methods

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
                App.Current.MainPage.DisplayAlert("Aviso", "No momento você está sem conexão com a internet!", "OK");
            else if (e.NetworkAccess == NetworkAccess.Internet)
                App.Current.MainPage.DisplayAlert("Aviso", "Conectado a internet!", "OK");
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync("NavigationPage/PokedexPage");
        }

        protected override void OnResume()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnStart()
        {
            base.OnStart();

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>();
            containerRegistry.RegisterForNavigation<PokedexPage, PokedexViewModel>();

            containerRegistry.Register<IPokeApi, PokeApi>();
        }

        #endregion Methods
    }
}