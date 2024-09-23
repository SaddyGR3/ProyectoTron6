using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    internal class ItemQueue
    {
        private ListaEnlazada<Item> items = new ListaEnlazada<Item>();

        // Propiedad para contar los ítems en la lista
        public int Contador
        {
            get { return items.Count; }
        }

        // Devuelve el primer ítem sin eliminarlo (Peek)
        public Item Peek()
        {
            if (items.Count > 0)
            {
                return items.ObtenerPrimero();  // Usa el método de ListaEnlazada para obtener el primer ítem
            }
            return null;
        }

        // Encola un ítem: Combustible al principio, otros al final
        public void Enqueue(Item item)
        {
            if (item is Combustible)
            {
                items.AgregarPrimero(item);  // Celdas de combustible al frente
            }
            else
            {
                items.AgregarUltimo(item);  // Otros ítems al final
            }
        }

        // Desencola un ítem (Dequeue): Elimina y devuelve el primer ítem
        public Item Dequeue()
        {
            if (items.Count > 0)
            {
                var item = items.ObtenerPrimero();  // Obtiene el primer ítem
                items.EliminarPrimero();  // Elimina el primer ítem después de obtenerlo
                return item;
            }
            return null;
        }

        // Método para recorrer la lista de ítems y devolver una lista con ellos
        public ListaEnlazada<Item> GetItems()
        {
            return items;  // Retorna la lista enlazada de ítems
        }
    }
}


