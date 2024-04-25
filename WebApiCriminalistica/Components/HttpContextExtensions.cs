using Microsoft.EntityFrameworkCore;

namespace WebApiCriminalistica.Components
{
    public static class HttpContextExtensions
    {
        public static async Task InsertParamsPaginations<T>(this HttpContext context,
            IQueryable<T> queryable, int cantidadRegistros) 
        {
            if (context == null) 
            { 
                throw new ArgumentNullException(nameof(context));
            }

            double conteo = await queryable.CountAsync();
            double TotalPaginas = Math.Ceiling(conteo / cantidadRegistros);
            context.Response.Headers.Add("TotalPaginas", TotalPaginas.ToString());
            context.Response.Headers.Add("CantidadRegistros", conteo.ToString());
        }

    }
}
