using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    /// <summary>
    /// Representa una pila genérica.
    /// </summary>
    /// <typeparam name="T">El tipo de elementos en la pila.</typeparam>
    internal class Pila<T>
    {
        private class Nodo
        {
            public T valor;
            public Nodo siguiente;

            public Nodo(T valor)
            {
                this.valor = valor;
                this.siguiente = null;
            }
        }

        private Nodo tope;
        private int cantidad;

        /// <summary>
        /// Inicializa una nueva instancia de la clase Pila.
        /// </summary>
        public Pila()
        {
            tope = null;
            cantidad = 0;
        }

        /// <summary>
        /// Obtiene la cantidad de elementos en la pila.
        /// </summary>
        public int Count
        {
            get { return cantidad; }
        }

        /// <summary>
        /// Agrega un elemento a la parte superior de la pila.
        /// </summary>
        /// <param name="elemento">El elemento a agregar.</param>
        public void Push(T elemento)
        {
            Nodo nuevoNodo = new Nodo(elemento);
            nuevoNodo.siguiente = tope;
            tope = nuevoNodo;
            cantidad++;
        }

        /// <summary>
        /// Elimina y devuelve el elemento en la parte superior de la pila.
        /// </summary>
        /// <returns>El elemento eliminado de la pila.</returns>
        public T Pop()
        {
            if (tope == null)
            {
                throw new InvalidOperationException("La pila está vacía");
            }

            T valor = tope.valor;
            tope = tope.siguiente;
            cantidad--;
            return valor;
        }

        /// <summary>
        /// Devuelve el elemento en la parte superior de la pila sin eliminarlo.
        /// </summary>
        /// <returns>El elemento en la parte superior de la pila.</returns>
        public T Peek()
        {
            if (tope == null)
            {
                throw new InvalidOperationException("La pila está vacía");
            }

            return tope.valor;
        }

        /// <summary>
        /// Determina si la pila está vacía.
        /// </summary>
        /// <returns>true si la pila está vacía; de lo contrario, false.</returns>
        public bool IsEmpty()
        {
            return tope == null;
        }

        /// <summary>
        /// Realiza una acción en cada elemento de la pila.
        /// </summary>
        /// <param name="accion">La acción a realizar en cada elemento.</param>
        public void Recorrer(Action<T> accion)
        {
            Nodo actual = tope;
            while (actual != null)
            {
                accion(actual.valor);
                actual = actual.siguiente;
            }
        }
    }
}

