using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    /// <summary>
    /// Clase que representa una lista enlazada.
    /// </summary>
    /// <typeparam name="T">Tipo de datos almacenados en la lista.</typeparam>
    internal class ListaEnlazada<T>
    {
        /// <summary>
        /// Obtiene el primer nodo de la lista.
        /// </summary>
        public NodoLista<T> Primero { get; private set; }

        /// <summary>
        /// Obtiene el último nodo de la lista.
        /// </summary>
        public NodoLista<T> Ultimo { get; private set; }

        /// <summary>
        /// Obtiene el número de elementos en la lista.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Crea una nueva instancia de la clase ListaEnlazada.
        /// </summary>
        public ListaEnlazada()
        {
            Primero = null;
            Ultimo = null;
            Count = 0;
        }

        /// <summary>
        /// Agrega un elemento al principio de la lista.
        /// </summary>
        /// <param name="data">Elemento a agregar.</param>
        public void AgregarPrimero(T data)
        {
            NodoLista<T> nuevoNodo = new NodoLista<T>(data);

            if (Primero == null)
            {
                Primero = nuevoNodo;
                Ultimo = nuevoNodo;
            }
            else
            {
                nuevoNodo.Siguiente = Primero;
                Primero = nuevoNodo;
            }
            Count++;
        }

        /// <summary>
        /// Agrega un elemento al final de la lista.
        /// </summary>
        /// <param name="data">Elemento a agregar.</param>
        public void AgregarUltimo(T data)
        {
            NodoLista<T> nuevoNodo = new NodoLista<T>(data);
            if (Ultimo == null)
            {
                Primero = nuevoNodo;
                Ultimo = nuevoNodo;
            }
            else
            {
                Ultimo.Siguiente = nuevoNodo;
                Ultimo = nuevoNodo;
            }
            Count++;
        }

        /// <summary>
        /// Obtiene el último elemento de la lista.
        /// </summary>
        /// <returns>Último elemento de la lista.</returns>
        public T ObtenerUltimo()
        {
            return Ultimo != null ? Ultimo.Data : default(T);
        }

        /// <summary>
        /// Obtiene el primer elemento de la lista.
        /// </summary>
        /// <returns>Primer elemento de la lista.</returns>
        public T ObtenerPrimero()
        {
            return Primero != null ? Primero.Data : default(T);
        }

        /// <summary>
        /// Elimina el primer elemento de la lista.
        /// </summary>
        public void EliminarPrimero()
        {
            if (Primero != null)
            {
                Primero = Primero.Siguiente;
                if (Primero == null)
                {
                    Ultimo = null; // Si la lista queda vacía, actualiza último a null
                }
                Count--;
            }
        }

        /// <summary>
        /// Elimina el último elemento de la lista.
        /// </summary>
        public void EliminarUltimo()
        {
            if (Primero == null)
                return;

            if (Primero == Ultimo)
            {
                Primero = null;
                Ultimo = null;
            }
            else
            {
                NodoLista<T> actual = Primero;
                while (actual.Siguiente != Ultimo)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = null;
                Ultimo = actual;
            }
            Count--;
        }

        /// <summary>
        /// Verifica si la lista contiene un elemento específico.
        /// </summary>
        /// <param name="data">Elemento a buscar.</param>
        /// <returns>true si el elemento está en la lista; de lo contrario, false.</returns>
        public bool Contains(T data)
        {
            NodoLista<T> actual = Primero;
            while (actual != null)
            {
                if (actual.Data.Equals(data))
                {
                    return true;
                }
                actual = actual.Siguiente;
            }
            return false;
        }

        /// <summary>
        /// Limpia la lista, eliminando todos los elementos.
        /// </summary>
        public void Limpiar()
        {
            Primero = null;
            Ultimo = null;
            Count = 0;
        }

        /// <summary>
        /// Recorre la lista y ejecuta una acción en cada elemento.
        /// </summary>
        /// <param name="accion">Acción a ejecutar en cada elemento.</param>
        public void Recorrer(Action<T> accion)
        {
            NodoLista<T> actual = Primero;
            while (actual != null)
            {
                accion(actual.Data);  // Ejecuta la acción en cada nodo
                actual = actual.Siguiente;
            }
        }
    }



    /// <summary>
    /// Clase que representa un nodo de la lista enlazada.
    /// </summary>
    /// <typeparam name="T">Tipo de datos almacenado en el nodo.</typeparam>
    internal class NodoLista<T>
    {
        /// <summary>
        /// Obtiene o establece el dato almacenado en el nodo.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Obtiene o establece el siguiente nodo en la lista.
        /// </summary>
        public NodoLista<T> Siguiente { get; set; }

        /// <summary>
        /// Crea una nueva instancia de la clase NodoLista.
        /// </summary>
        /// <param name="data">Dato a almacenar en el nodo.</param>
        public NodoLista(T data)
        {
            Data = data;
            Siguiente = null;
        }
    }
}

