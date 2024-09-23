using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    internal class Jugador : Moto
    {
        private ListaEnlazada<Item> itemLista;  // ListaEnlazada de ítems
        private NodoLista<Item> currentItem;    // Nodo actual de la lista enlazada
        public bool Destruido { get; private set; }

        public Jugador(Nodo initialPosition) : base(initialPosition)
        {
            PosActual.Data = "Jugador"; // Cambia el dato que almacena el nodo a PlayerBike
            itemLista = new ListaEnlazada<Item>();
            Destruido = false;
        }

        public void MoverArriba()
        {
            Mover(PosActual.Up, "arriba"); // Agrega la dirección "arriba"
        }

        public void MoverAbajo()
        {
            Mover(PosActual.Down, "abajo"); // Agrega la dirección "abajo"
        }

        public void MoverIzquierda()
        {
            Mover(PosActual.Left, "izquierda"); // Agrega la dirección "izquierda"
        }

        public void MoverDerecha()
        {
            Mover(PosActual.Right, "derecha"); // Agrega la dirección "derecha"
        }

        public override void Destruir()
        {
            base.Destruir();
            Destruido = true; // Marca como destruido
        }

        public async Task ApplyItemsAsync()
        {
            while (true)
            {
                if (itemLista.Count > 0)
                {
                    var item = itemLista.ObtenerPrimero();  // Obtiene el primer ítem de la lista
                    item.Aplicar(this);
                    itemLista.EliminarPrimero();  // Elimina el primer ítem después de aplicarlo
                    await Task.Delay(1000); // Espera 1 segundo
                }
            }
        }

        public void AñadirItem(Item item)
        {
            itemLista.AgregarUltimo(item);  // Añade el ítem al final de la lista enlazada
        }

        public void UsarItem()
        {
            if (itemLista.Count > 0)
            {
                var item = itemLista.ObtenerPrimero();  // Obtiene el primer ítem de la lista
                item.Aplicar(this);
                itemLista.EliminarPrimero();  // Elimina el ítem usado
            }
        }

        public void CambiarItem(string direction)
        {
            if (currentItem == null) currentItem = itemLista.Primero;  // Inicializa el ítem actual al primero de la lista

            if (direction == "left" && currentItem.Siguiente != null)
            {
                currentItem = currentItem.Siguiente;  // Avanza al siguiente ítem
            }
            else if (direction == "right" && currentItem.Siguiente != null)
            {
                currentItem = currentItem.Siguiente;  // Retrocede al ítem anterior
            }

            // Muestra el ítem seleccionado al jugador
            VerItems(currentItem.Data);
        }

        private void VerItems(Item item)
        {
            Console.WriteLine("Ítem seleccionado: " + item.GetType().Name);
        }
    

        public void MoveLeftItem()
        {
            //Lógica para mover al ítem a la izquierda
        }

        public void MoveRightItem()
        {
            //Lógica para mover al ítem a la derecha
        }
    }
}
