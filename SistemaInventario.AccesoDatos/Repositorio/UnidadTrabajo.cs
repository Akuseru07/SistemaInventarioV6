﻿using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.AccesoDatos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {

        private readonly ApplicationDbContext _db;
        public IBodegaRepositorio Bodega {  get; private set; }
        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Bodega = new BodegaRepositorio(_db);
        }
        public void Dispose()
        {
            _db.Dispose(); //Para que libere lo que se tiene en memoria y ya no se esta usando
        }

        public async Task Guardar()
        {
            await _db.SaveChangesAsync();
        }
    }
}