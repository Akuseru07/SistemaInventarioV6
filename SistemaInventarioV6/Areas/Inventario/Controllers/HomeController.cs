using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.Especificaciones;
using SistemaInventario.Modelos.ViewModels;
using System.Diagnostics;

namespace SistemaInventarioV6.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public HomeController(ILogger<HomeController> logger, IUnidadTrabajo unidadTrabajo)
        {
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index(int pageNumber =1, string busqueda="", string busquedaActual="")
        {
            if (!String.IsNullOrEmpty(busqueda))
            {
                pageNumber = 1;
            }
            else
            {
                busqueda = busquedaActual;
            }
            ViewData["BusquedaActual"] = busqueda;


            if(pageNumber < 1) { pageNumber  = 1; }

            Parametros parametros = new Parametros()
            {
                PageNumber = pageNumber,
                PageSize = 4 //Aca le puedo poner el numero de productos que quiero que me muestre por pagina
            };

            var resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros);
            //IEnumerable<Producto> productoLista = await _unidadTrabajo.Producto.ObtenerTodos();

            if(!String.IsNullOrEmpty(busqueda))
            {
                resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros, p =>p.Descripcion.Contains(busqueda));
            }
            ViewData["TotalPaginas"] = resultado.MetaData.TotalPages;
            ViewData["TotalRegistros"] = resultado.MetaData.TotalCount;
            ViewData["PageSize"] = resultado.MetaData.PageSize;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Previo"] = "disabled"; //clase css para desactivar el boton
            ViewData["Sigiente"] = "";

            //esto de aca es para que si estoy en la pagina 2 o mas, poder regresar, pero si estoy en la 1, el boton esta desactivado
            if(pageNumber > 1) { ViewData["Previo"] = ""; }
            //Si el numero de la pagina donde estoy es igual al numero de paginas todal, entonces no puedo avanzar ya que no hay para donde ir
            if(resultado.MetaData.TotalPages <= pageNumber) { ViewData["Siguiente"] = "disabled"; }

            return View(resultado);  
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
