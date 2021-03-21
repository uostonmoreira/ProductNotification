using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Faturamento.Agrupador.Dados.Utils
{
    public static class MongoDBCursorExtensions
    {
        public static async Task<IList<TEntidade>> FetchAllAsync<TEntidade>(this IAsyncCursor<TEntidade> cursor)
        {
            List<TEntidade> titulos = new List<TEntidade>();

            do
            {
                if (cursor.Current != null)
                    titulos.AddRange(cursor.Current.ToList());
            } while (await cursor.MoveNextAsync());

            return titulos;
        }
    }
}