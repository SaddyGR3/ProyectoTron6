using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    internal class Enemigos : Bike
    {
        public ItemQueue itemQueue = new ItemQueue();
        public bool isDestroyed { get; private set; }

        public Enemigos(Node initialPosition) : base(initialPosition)
        {
            currentPosition.Data = "EnemyBike"; // Cambia el dato que almacena el nodo a EnemyBike
        }

        public void DropItems(Matriz grid)
        {
            foreach (var item in itemQueue.GetItems())
            {
                Node freeNode = FindFreeNode(grid); // Encuentra una celda libre
                freeNode.Data = item.GetType().Name; // Coloca el ítem en esa celda como una representación del tipo
            }
            itemQueue = new ItemQueue(); // Vaciar la cola después de soltar los ítems
        }

        // Encuentra una celda libre en la cuadrícula
        private Node FindFreeNode(Matriz grid)
        {
            Random rand = new Random();
            Node freeNode;
            do
            {
                int x = rand.Next(0, grid.Matrix.GetLength(0)); // Filas
                int y = rand.Next(0, grid.Matrix.GetLength(1)); // Columnas
                freeNode = grid.GetNode(x, y);
            } while (!string.IsNullOrEmpty(freeNode.Data)); // Repetir si la celda no está libre
            return freeNode;
        }

        public override void Destroy()
        {
            Console.WriteLine("La moto ha sido destruida.");

            // Limpiar la estela
            foreach (var node in estela)
            {
                node.Data = "";  // Limpia los nodos de la estela
            }
            estela.Clear();

            // Marcar la posición actual de la moto como vacía
            currentPosition.Data = "";

            // Marcar la moto como destruida
            isDestroyed = true;
        }

        public void MoveRandom()
        {
            Random random = new Random();
            List<int> possibleDirections = new List<int> { 0, 1, 2, 3 }; // 0 = Up, 1 = Down, 2 = Left, 3 = Right

            // Evitar que se mueva en dirección contraria a la actual
            switch (currentDirection)
            {
                case "up":
                    possibleDirections.Remove(1); //No permitir moverse hacia abajo
                    break;
                case "down":
                    possibleDirections.Remove(0); //No permitir moverse hacia arriba
                    break;
                case "left":
                    possibleDirections.Remove(3); //No permitir moverse hacia la derecha
                    break;
                case "right":
                    possibleDirections.Remove(2); //No permitir moverse hacia la izquierda
                    break;
            }

            // Elegir una dirección válida
            int direction = possibleDirections[random.Next(possibleDirections.Count)];

            switch (direction)
            {
                case 0:
                    Move(currentPosition.Up);
                    currentDirection = "up";
                    break;
                case 1:
                    Move(currentPosition.Down);
                    currentDirection = "down";
                    break;
                case 2:
                    Move(currentPosition.Left);
                    currentDirection = "left";
                    break;
                case 3:
                    Move(currentPosition.Right);
                    currentDirection = "right";
                    break;
            }
    }   }
}

