using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;

namespace SistemaInventarioV6.Areas.Admin.Controllers
{
    //A esto me refiero con lo del error por que no lo tenia antes
    [Area("Admin")]
    //Aca pa que si o si se tenga que logear pa ver las cosas, y para saber que roles si pueden verlo
    [Authorize(Roles = DS.Role_Admin)]
    public class BodegaController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public BodegaController(IUnidadTrabajo unidadTrabajo)
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
            Bodega bodega = new Bodega();
            if (id == null)
            {
                //Crear una nueva bodega
                bodega.Estado = true;
                return View(bodega);
            }
            //Actualizamos bodega
            bodega = await _unidadTrabajo.Bodega.Obtener(id.GetValueOrDefault());
            //Por si acaso
            if(bodega == null)
            {
                return NotFound();
            }  
            return View(bodega);
        }

        [HttpPost]
        //Es para evitar las falsificaciones de solicitudes de un sitio cargado normalmente de otra pagina que puede intentar 
        //cargar datos a nuestra pagina
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                if(bodega.Id == 0)
                {
                    await _unidadTrabajo.Bodega.Agregar(bodega);
                    TempData[DS.Exitosa] = "Bodega creada exitosamente";
                }
                else
                {
                    _unidadTrabajo.Bodega.Actualizar(bodega);
                    TempData[DS.Exitosa] = "Bodega actualizada exitosamente";
                }
                await _unidadTrabajo.Guardar();
                //Aca no se puede usar View, asi que usamos esto
                return RedirectToAction(nameof(Index));
            }
            //Si en algun caso nuestro modelo no se actualiza, se hace un return a la misma vista upsert
            TempData[DS.Error] = "Error al grabar Bodega";
            return View(bodega);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Bodega.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var bodegaDb= await _unidadTrabajo.Bodega.Obtener(id);
            if(bodegaDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Bodega" });
            }
            _unidadTrabajo.Bodega.Remover(bodegaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Bodega borrada exitosamente"});
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id=0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Bodega.ObtenerTodos();
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
