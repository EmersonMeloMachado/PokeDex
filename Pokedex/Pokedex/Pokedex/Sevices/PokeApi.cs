using Flurl.Http;
using Newtonsoft.Json;
using Pokedex.Model;
using Pokedex.Sevices.Interface;
using Pokedex.Uteis;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Pokedex.Sevices
{
    public class PokeApi : IPokeApi
    {
        #region Methods

        public async Task<PokemonList> ObterListaPokemons(int offset = 0, int limit = 20)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) return null;

            try
            {
                var url = $"{EndPoints.BaseUrl}?offset={offset}&limit={limit}";

                var response = await url
                    .AllowAnyHttpStatus()
                    .GetAsync()
                    .ReceiveJson<PokemonList>();

                return response;
            }
            catch (FlurlHttpException ex)
            {
                var msg = await ex.GetResponseStringAsync();
                throw new Exception(msg);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Pokemon> ObterPokemon(string endpoint)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) return null;

            try
            {
                var response = await endpoint
                    .WithTimeout(60)
                    .AllowAnyHttpStatus()
                    .GetAsync();

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pokemon = JsonConvert.DeserializeObject<Pokemon>(content);
                    return pokemon;
                }
                return null;
            }
            catch (FlurlHttpException ex)
            {
                var msg = await ex.GetResponseStringAsync();
                throw new Exception(msg);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PokemonType> ObterTiposPokemons()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) return null;

            try
            {
                var response = await EndPoints.UrlTypePokemon
                    .WithTimeout(60)
                    .AllowAnyHttpStatus()
                    .GetAsync();

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pokemon = JsonConvert.DeserializeObject<PokemonType>(content);
                    return pokemon;
                }
                return null;
            }
            catch (FlurlHttpException ex)
            {
                var msg = await ex.GetResponseStringAsync();
                throw new Exception(msg);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion Methods
    }
}