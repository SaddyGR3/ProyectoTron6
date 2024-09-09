using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    internal class Jugador : Moto
    {
        private LinkedListNode<Item> currentItem;
        private ItemQueue itemQueue;
        public bool Destruido { get; private set; }

        public Jugador(Nodo initialPosition) : base(initialPosition)
        {
            PosActual.Data = "Jugador"; //Cambia el dato que almacena el nodo a PlayerBike
            itemQueue = new ItemQueue();
            Destruido = false;
        }
        public void MoverArriba()
        {
            Mover(PosActual.Up);
        }
        public void MoverAbajo()
        {
            Mover(PosActual.Down);
        }
        public void MoverIzquierda()
        {
            Mover(PosActual.Left);
        }
        public void MoverDerecha()
        {
            Mover(PosActual.Right);
        }
        public override void Destruir()
        {
            base.Destruir();
            Destruido = true;  // Marcar como destruido
        }
        public async Task ApplyItemsAsync()
        {
            while (true)
            {
                var item = itemQueue.Dequeue();
                if (item != null)
                {
                    item.Aplicar(this);
                    await Task.Delay(1000); // Espera 1 segundo
                }
            }
        }

        public void AñadirItem(Item item)
        {
            itemQueue.Enqueue(item);
        }
        public void UsarItem()
        {
            if (itemQueue.Contador > 0)
            {
                var item = itemQueue.Dequeue();
                item.Aplicar(this);
            }
        }
        public void CambiarItem(string direction)
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
            VerItems(currentItem.Value);
        }

        private void VerItems(Item item)
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
