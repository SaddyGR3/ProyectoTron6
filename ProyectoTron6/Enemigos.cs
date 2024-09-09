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
            foreach (var item in itemQueue.GetItems())
            {
                Nodo freeNode = BuscarNodoLibre(grid); //Encuentra una celda libre
                freeNode.Data = item.GetType().Name; //Coloca el ítem en esa celda como una representación del tipo
            }
            itemQueue = new ItemQueue(); //Vaciar la cola después de soltar los ítems
        }

        //Encuentra una celda libre en la cuadrícula
        private Nodo BuscarNodoLibre(Matriz grid)
        {
            Random rand = new Random();
            Nodo freeNode;
            do
            {
                int x = rand.Next(0, grid.Matrix.GetLength(0)); // Filas
                int y = rand.Next(0, grid.Matrix.GetLength(1)); // Columnas
                freeNode = grid.GetNode(x, y);
            } while (!string.IsNullOrEmpty(freeNode.Data)); // Repetir si la celda no está libre
            return freeNode;
        }

        public override void Destruir()
        {
            Console.WriteLine("La moto ha sido destruida.");

            // Limpiar la estela
            foreach (var node in estela)
            {
                node.Data = "";  // Limpia los nodos de la estela
            }
            estela.Clear();

            // Marcar la posición actual de la moto como vacía
            PosActual.Data = "";

            // Marcar la moto como destruida
            Destruido = true;
        }

        public void MoveRandom()
        {
            Random random = new Random();
            List<int> possibleDirections = new List<int> { 0, 1, 2, 3 }; // 0 = Up, 1 = Down, 2 = Left, 3 = Right

            switch (DirActual)
            {
                case "up":
                    possibleDirections.Remove(1);
                    break;
                case "down":
                    possibleDirections.Remove(0);
                    break;
                case "left":
                    possibleDirections.Remove(3);
                    break;
                case "right":
                    possibleDirections.Remove(2);
                    break;
            }

            int direction = possibleDirections[random.Next(possibleDirections.Count)];

            switch (direction)
            {
                case 0:
                    Mover(PosActual.Up);
                    DirActual = "up";
                    break;
                case 1:
                    Mover(PosActual.Down);
                    DirActual = "down";
                    break;
                case 2:
                    Mover(PosActual.Left);
                    DirActual = "left";
                    break;
                case 3:
                    Mover(PosActual.Right);
                    DirActual = "right";
                    break;
            }
    }   }
}

