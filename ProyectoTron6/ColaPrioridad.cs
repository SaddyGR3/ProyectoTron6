using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    /// <summary>
    /// Clase que representa una cola de prioridad genérica.
    /// </summary>
    /// <typeparam name="T">El tipo de elementos en la cola de prioridad.</typeparam>
    internal class ColaPrioridad<T>
    {
        private ListaEnlazada<T> items = new ListaEnlazada<T>();

        /// <summary>
        /// Obtiene el número de elementos en la cola de prioridad.
        /// </summary>
        public int Contador
        {
            get { return items.Count; }
        }

        /// <summary>
        /// Agrega un elemento a la cola de prioridad.
        /// </summary>
        /// <param name="item">El elemento a agregar.</param>
        public void Enqueue(T item)
        {
            if (item is Combustible)
            {
                items.AgregarPrimero(item); //Da prioridad al combustible
            }
            else
            {
                items.AgregarUltimo(item);  //Agrega al final si no es combustible
            }
        }

        /// <summary>
        /// Obtiene el elemento de mayor prioridad en la cola de prioridad sin eliminarlo.
        /// </summary>
        /// <returns>El elemento de mayor prioridad.</returns>
        public T Peek()
        {
            return items.ObtenerPrimero();
        }

        /// <summary>
        /// Elimina y devuelve el elemento de mayor prioridad en la cola de prioridad.
        /// </summary>
        /// <returns>El elemento de mayor prioridad.</returns>
        public T Dequeue()
        {
            T item = items.ObtenerPrimero();
            items.EliminarPrimero();
            return item;
        }

        /// <summary>
        /// Obtiene el número de elementos en la cola de prioridad.
        /// </summary>
        /// <returns>El número de elementos en la cola de prioridad.</returns>
        public int Count()
        {
            return items.Count;
        }
    }
}

