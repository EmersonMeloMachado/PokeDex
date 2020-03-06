using Pokedex.Model;
using System.Threading.Tasks;

namespace Pokedex.Sevices.Interface
{
    public interface IPokeApi
    {
        #region Methods

        Task<PokemonList> ObterListaPokemons(int offset = 20, int limit = 20);

        Task<Pokemon> ObterPokemon(string endpoint);

        Task<PokemonType> ObterTiposPokemons();

        #endregion Methods
    }
}