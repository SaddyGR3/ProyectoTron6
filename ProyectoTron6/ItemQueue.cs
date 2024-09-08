using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    internal class ItemQueue
    {
        private LinkedList<Item> items = new LinkedList<Item>();

        // Este método solo gestiona la cola. No tiene referencia a la moto.
        public int Count
        {
            get { return items.Count; }
        }

        public void Enqueue(Item item)
        {
            if (item is FuelCell)
            {
                items.AddFirst(item); // Celdas de combustible al frente
            }
            else
            {
                items.AddLast(item); // Otros ítems al final
            }
        }

        public Item Dequeue()
        {
            if (items.Count > 0)
            {
                var item = items.First.Value;
                items.RemoveFirst();
                return item;
            }
            return null;
        }

        // Para obtener la lista de ítems
        public LinkedList<Item> GetItems()
        {
            return items;
        }
    }
}

