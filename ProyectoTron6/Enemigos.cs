using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    internal class Enemigos : Moto
    {
        public ItemQueue itemQueue = new ItemQueue();
        public bool Destruido { get; private set; }

        public Enemigos(Nodo initialPosition) : base(initialPosition)
        {
            PosActual.Data = "EnemyBike"; //Cambia el dato que almacena el nodo a EnemyBike
        }

        public void SoltarItems(Matriz grid)
        {
            // Usa el método Recorrer para iterar sobre cada ítem en la lista
            itemQueue.GetItems().Recorrer(item =>
            {
                Nodo freeNode = BuscarNodoLibre(grid); // Encuentra una celda libre
                freeNode.Data = item.GetType().Name; // Coloca el ítem en esa celda como una representación del tipo
            });

            // Vaciar la cola después de soltar los ítems
            itemQueue = new ItemQueue();
        }



        //Encuentra una celda libre en la cuadrícula
        private Nodo BuscarNodoLibre(Matriz grid)
        {
            Random rand = new Random();
            Nodo freeNode;
            do
            {
                int x = rand.Next(0, grid.Matrix.GetLength(0)); //Filas
                int y = rand.Next(0, grid.Matrix.GetLength(1)); //Columnas
                freeNode = grid.GetNode(x, y);
            } while (!string.IsNullOrEmpty(freeNode.Data)); //Repetir si la celda no está libre
            return freeNode;
        }

        public override void Destruir()
        {
            Console.WriteLine("La moto ha sido destruida.");

            // Limpia la estela utilizando el método personalizado de ListaEnlazada
            estela.Recorrer(nodo =>
            {
                nodo.Data = "";  //Limpia los nodos de la estela
            });
            estela.Limpiar();

            // Marca la posición actual de la moto como vacía
            PosActual.Data = "";

            //Marca la moto como destruida
            Destruido = true;
        }

        public void MoveRandom()
        {
            Random random = new Random();
            List<int> possibleDirections = new List<int> { 0, 1, 2, 3 }; // 0 = Up, 1 = Down, 2 = Left, 3 = Right

            // Elimina direcciones que llevarían a su propia estela
            if (PosActual.Up != null && PerteneceAEstela(PosActual.Up)) possibleDirections.Remove(0);   // Arriba
            if (PosActual.Down != null && PerteneceAEstela(PosActual.Down)) possibleDirections.Remove(1); // Abajo
            if (PosActual.Left != null && PerteneceAEstela(PosActual.Left)) possibleDirections.Remove(2); // Izquierda
            if (PosActual.Right != null && PerteneceAEstela(PosActual.Right)) possibleDirections.Remove(3); // Derecha

            //Si no hay direcciones posibles, no moverse
            if (possibleDirections.Count == 0) return;

            int direction = possibleDirections[random.Next(possibleDirections.Count)];

            switch (direction)
            {
                case 0:
                    Mover(PosActual.Up, "arriba");
                    DirActual = "up";
                    break;
                case 1:
                    Mover(PosActual.Down, "abajo");
                    DirActual = "down";
                    break;
                case 2:
                    Mover(PosActual.Left, "izquierda");
                    DirActual = "left";
                    break;
                case 3:
                    Mover(PosActual.Right, "derecha");
                    DirActual = "right";
                    break;
            }
        }

        //Nuevo método para verificar si el nodo pertenece a la estela de la propia moto
        private bool PerteneceAEstela(Nodo nodo)
        {
            return estela.Contains(nodo); //Verifica si el nodo está en la estela de la moto
        }
    }
}

