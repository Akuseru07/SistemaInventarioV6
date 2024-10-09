using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;

namespace SistemaInventarioV6.Areas.Admin.Controllers
{
    //A esto me refiero con lo del error por que no lo tenia antes
    [Area("Admin")]
    public class MarcaController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public MarcaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        //se pone en ? para que pueda recibir el id vacio o nulo
        public async Task<IActionResult> Upsert(int? id)
        {
            Marca marca = new Marca();
            if (id == null)
            {
                //Crear una nueva bodega
                marca.Estado = true;
                return View(marca);
            }
            //Actualizamos bodega
            marca = await _unidadTrabajo.Marca.Obtener(id.GetValueOrDefault());
            //Por si acaso
            if(marca == null)
            {
                return NotFound();
            }  
            return View(marca);
        }

        [HttpPost]
        //Es para evitar las falsificaciones de solicitudes de un sitio cargado normalmente de otra pagina que puede intentar 
        //cargar datos a nuestra pagina
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Marca marca)
        {
            if (ModelState.IsValid)
            {
                if(marca.Id == 0)
                {
                    await _unidadTrabajo.Marca.Agregar(marca);
                    TempData[DS.Exitosa] = "Marca creada exitosamente";
                }
                else
                {
                    _unidadTrabajo.Marca.Actualizar(marca);
                    TempData[DS.Exitosa] = "Marca actualizada exitosamente";
                }
                await _unidadTrabajo.Guardar();
                //Aca no se puede usar View, asi que usamos esto
                return RedirectToAction(nameof(Index));
            }
            //Si en algun caso nuestro modelo no se actualiza, se hace un return a la misma vista upsert
            TempData[DS.Error] = "Error al grabar Marca";
            return View(marca);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Marca.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var marcaDb= await _unidadTrabajo.Marca.Obtener(id);
            if(marcaDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Marca" });
            }
            _unidadTrabajo.Marca.Remover(marcaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Marca borrada exitosamente"});
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id=0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Marca.ObtenerTodos();
            if(id== 0)
            {
                //Trim para que me retorne un true o un false dependiendo de la comparacion
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && b.Id != id);
            }
            if(valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }

        #endregion
    }
}
