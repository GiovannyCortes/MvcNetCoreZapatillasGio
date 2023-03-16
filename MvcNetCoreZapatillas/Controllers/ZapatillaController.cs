using Microsoft.AspNetCore.Mvc;
using MvcNetCoreZapatillas.Models;
using MvcNetCoreZapatillas.Repositories;

namespace MvcNetCoreZapatillas.Controllers {
    public class ZapatillaController : Controller {

        private RepositoryZapatillas repo;

        public ZapatillaController(RepositoryZapatillas repo) {
            this.repo = repo;
        }

        public async Task<IActionResult> GetZapatillas() {
            List<Zapatilla> zapatillas = await this.repo.GetZapatillas();
            return View(zapatillas);
        }

        public async Task<IActionResult> ZapatillaDetalle(int idproducto) {
            Zapatilla zapatilla = await this.repo.FindZapatilla(idproducto);
            return View(zapatilla);
        }

        public async Task<IActionResult> _DetailsZapatilla(int idproducto, int? posicion) {
            if (posicion == null) {
                posicion = 1;
            }

            int numregistros = await this.repo.GetMaxRegistros(idproducto);

            int siguiente = posicion.Value + 1;
                siguiente = (siguiente > numregistros) ? 1 : siguiente;
            
            int anterior = posicion.Value - 1;
                anterior = (anterior < 1) ? numregistros : anterior;

            ImagenZapatilla imagenZapatilla = this.repo.GetImagenZapatilla(idproducto, posicion.Value);

            ViewData["ULTIMO"] = numregistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            
            return PartialView("_DetailsZapatilla", imagenZapatilla);
        }

    }
}
