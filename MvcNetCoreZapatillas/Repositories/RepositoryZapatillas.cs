using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreZapatillas.Data;
using MvcNetCoreZapatillas.Models;

#region PROCEDURES SQL SERVER
/*
 
    CREATE PROCEDURE SP_GETIMG_ZAPAS(@IDPRODUCTO INT, @POSICION INT)
    AS
	    SELECT * FROM (
		    SELECT CAST(ISNULL(
			    ROW_NUMBER() OVER (ORDER BY IDIMAGEN),  0) AS INT)
			    AS INDICE,
		    IDIMAGEN, IDPRODUCTO, IMAGEN
		    FROM IMAGENESZAPASPRACTICA
		    WHERE IDPRODUCTO = @IDPRODUCTO
	    ) AS QUERY
	    WHERE INDICE >= @POSICION AND INDICE < (@POSICION + 1)
    GO

 */
#endregion

namespace MvcNetCoreZapatillas.Repositories{
    public class RepositoryZapatillas{
        
        private ZapatillasContext context;

        public RepositoryZapatillas(ZapatillasContext context) {
            this.context = context;
        }

        public async Task<List<Zapatilla>> GetZapatillas() {
            return await this.context.Zapatillas.ToListAsync();
        } 

        public async Task<Zapatilla> FindZapatilla(int idproducto) {
            return await this.context.Zapatillas.FirstOrDefaultAsync(x => x.IdProducto == idproducto);
        }

        public async Task<int> GetMaxRegistros(int idproducto) {
            var consulta = await this.context.ImagenesZapatillas.Where(x => x.IdProducto == idproducto).ToListAsync();
            return consulta.Count();
        }

        public ImagenZapatilla GetImagenZapatilla(int idproducto, int posicion) {
            string sql = "SP_GETIMG_ZAPAS @IDPRODUCTO, @POSICION";
            
            SqlParameter pamidproducto = new SqlParameter("@IDPRODUCTO", idproducto);
            SqlParameter pamposicion = new SqlParameter("@POSICION", posicion);

            var consulta = this.context.ImagenesZapatillas.FromSqlRaw(sql, pamidproducto, pamposicion);
            ImagenZapatilla imagenZapatilla = consulta.ToList().FirstOrDefault();
            return imagenZapatilla;
        }

    }
}
