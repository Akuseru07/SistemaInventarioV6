using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.AccesoDatos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        

        public Repositorio(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task Agregar(T entidad)
        {
            await dbSet.AddAsync(entidad); //Es equivalente a un insert into Table
        }

        public async Task<T> Obtener(int id)
        {
            return await dbSet.FindAsync(id); //Select * from (Solo por ID por que ya tenemos otro metodo que lo hace de otra manera)
        }
        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if(filtro != null)
            {
                query = query.Where(filtro); //select * from where ....
            }

            if(incluirPropiedades != null)
            {
                foreach(var incluirProp in incluirPropiedades.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp); // ejemplo "Categoria, Marca"
                }
            }
            if(orderby != null)
            {
                query = orderby(query); // va a estar ordenada por el parametro que yo le envie en el orderby
            }
            if(!isTracking)
            {
                query = query.AsNoTracking(); //Que no muestra que el registro en el caso de que yo lo este utilizando y al mismo tiempo
                                              //lo quiera actualizar
            }
            return await query.ToListAsync();
        }

        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro); //select * from where ....
            }

            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp); // ejemplo "Categoria, Marca"
                }
            }
            if (!isTracking)
            {
                query = query.AsNoTracking(); //Que no muestra que el registro en el caso de que yo lo este utilizando y al mismo tiempo
                                              //lo quiera actualizar
            }
            return await query.FirstOrDefaultAsync();
        }


        public void Remover(T entidad)
        {
            dbSet.Remove(entidad); // DELETE from * where
        }

        public void RemoverRango(IEnumerable<T> entidad)
        {
            dbSet.RemoveRange(entidad); 
        }
    }
}
