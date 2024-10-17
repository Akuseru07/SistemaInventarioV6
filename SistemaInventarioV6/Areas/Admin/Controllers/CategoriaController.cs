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
    [Authorize(Roles = DS.Role_Admin)]
    public class CategoriaController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public CategoriaController(IUnidadTrabajo unidadTrabajo)
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
            Categoria categoria = new Categoria();
            if (id == null)
            {
                //Crear una nueva bodega
                categoria.Estado = true;
                return View(categoria);
            }
            //Actualizamos bodega
            categoria = await _unidadTrabajo.Categoria.Obtener(id.GetValueOrDefault());
            //Por si acaso
            if(categoria == null)
            {
                return NotFound();
            }  
            return View(categoria);
        }

        [HttpPost]
        //Es para evitar las falsificaciones de solicitudes de un sitio cargado normalmente de otra pagina que puede intentar 
        //cargar datos a nuestra pagina
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                if(categoria.Id == 0)
                {
                    await _unidadTrabajo.Categoria.Agregar(categoria);
                    TempData[DS.Exitosa] = "Categoria creada exitosamente";
                }
                else
                {
                    _unidadTrabajo.Categoria.Actualizar(categoria);
                    TempData[DS.Exitosa] = "Categoria actualizada exitosamente";
                }
                await _unidadTrabajo.Guardar();
                //Aca no se puede usar View, asi que usamos esto
                return RedirectToAction(nameof(Index));
            }
            //Si en algun caso nuestro modelo no se actualiza, se hace un return a la misma vista upsert
            TempData[DS.Error] = "Error al grabar Categoria";
            return View(categoria);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Categoria.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var categoriaDb= await _unidadTrabajo.Categoria.Obtener(id);
            if(categoriaDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Categoria" });
            }
            _unidadTrabajo.Categoria.Remover(categoriaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Categoria borrada exitosamente"});
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id=0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Categoria.ObtenerTodos();
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
