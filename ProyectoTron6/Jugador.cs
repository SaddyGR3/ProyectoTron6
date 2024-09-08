using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    internal class Jugador : Bike
    {
        private LinkedListNode<Item> currentItem;
        private ItemQueue itemQueue;

        public Jugador(Node initialPosition) : base(initialPosition)
        {
            currentPosition.Data = "Jugador"; // Cambia el dato que almacena el nodo a PlayerBike
            itemQueue = new ItemQueue();
        }
        public void MoveUp()
        {
            Move(currentPosition.Up);
        }

        public void MoveDown()
        {
            Move(currentPosition.Down);
        }

        public void MoveLeft()
        {
            Move(currentPosition.Left);
        }

        public void MoveRight()
        {
            Move(currentPosition.Right);
        }
        public async Task ApplyItemsAsync()
        {
            while (true)
            {
                var item = itemQueue.Dequeue();
                if (item != null)
                {
                    item.Apply(this);
                    await Task.Delay(1000); // Espera 1 segundo
                }
            }
        }

        public void AddItem(Item item)
        {
            itemQueue.Enqueue(item);
        }
        public void UseItem()
        {
            if (itemQueue.Count > 0)
            {
                var item = itemQueue.Dequeue();
                item.Apply(this);
            }
        }
        public void NavigateItems(string direction)
        {
            if (currentItem == null) currentItem = itemQueue.GetItems().First;

            if (direction == "left" && currentItem.Previous != null)
            {
                currentItem = currentItem.Previous;
            }
            else if (direction == "right" && currentItem.Next != null)
            {
                currentItem = currentItem.Next;
            }

            // Mostrar el ítem seleccionado al jugador
            DisplayItem(currentItem.Value);
        }

        private void DisplayItem(Item item)
        {
            Console.WriteLine("Ítem seleccionado: " + item.GetType().Name);
        }
        public void MoveLeftItem()
        {
            // Lógica para moverse al ítem a la izquierda
        }

        public void MoveRightItem()
        {
            // Lógica para moverse al ítem a la derecha
        }
    }
}
