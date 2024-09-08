using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    internal class Enemigos : Bike
    {
        public Enemigos(Node initialPosition) : base(initialPosition)
        {
            currentPosition.Data = "EnemyBike"; //Cambia el dato que almacena el nodo a EnemyBike
        }

        public void MoveRandom()
        {
            Random random = new Random();
            int direction = random.Next(0, 4); // 0 = Up, 1 = Down, 2 = Left, 3 = Right

            switch (direction)
            {
                case 0:
                    Move(currentPosition.Up);
                    break;
                case 1:
                    Move(currentPosition.Down);
                    break;
                case 2:
                    Move(currentPosition.Left);
                    break;
                case 3:
                    Move(currentPosition.Right);
                    break;
            }
        }
    }
}
