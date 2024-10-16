﻿using SistemaInventario.Modelos.Especificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IRepositorio<T> where T : class
    {
        //Para hacerlo asincrono solo se pone el Task, los 2 ultimos no se pueden ya que son de remover algo
        Task<T> Obtener(int id);
        
        Task<IEnumerable<T>> ObtenerTodos(
            Expression<Func<T,bool>> filtro = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
            string incluirPropiedades = null,
            bool isTracking = true 
            );

        //Declaracion de el metodo, es casi igual que obtener todos
        PagedList<T> ObtenerTodosPaginado(Parametros parametros,
            Expression<Func<T, bool>> filtro = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
            string incluirPropiedades = null,
            bool isTracking = true
            );

        Task<T> ObtenerPrimero(
            Expression<Func<T, bool>> filtro = null,
            string incluirPropiedades = null,
            bool isTracking = true
            );

        Task Agregar(T entidad);

        void Remover(T entidad);
        
        void RemoverRango(IEnumerable<T> entidad);
    }
}
