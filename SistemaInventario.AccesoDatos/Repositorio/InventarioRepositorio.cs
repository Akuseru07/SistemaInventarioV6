using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventarioV6.AccesoDatos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class InventarioRepositorio : Repositorio<Inventario>, IInventarioRepositorio
    {
        private readonly ApplicationDbContext _db;

        public InventarioRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Actualizar(Inventario inventario)
        {
            var inventarioBD = _db.Inventarios.FirstOrDefault(b => b.Id == inventario.Id); //Se captura el registro antes de actualizarlo
            if (inventarioBD != null)
            {
                inventarioBD.BodegaId = inventario.BodegaId;
                inventarioBD.FechaFinal = inventario.FechaFinal;
                inventarioBD.Estado = inventario.Estado;
                _db.SaveChanges(); //Creo que no ocupo ponerlo, pero sirve para salvar los cambios
            }
        }

        //Pa que me muestre la lista de bodegas 
        public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
        {
            if(obj == "Bodega")
            {
                return _db.Bodegas.Where(b => b.Estado == true).Select(b => new SelectListItem
                {
                    Text = b.Nombre,
                    Value = b.Id.ToString()
                });
            }
            return null;
        }
    }
}
