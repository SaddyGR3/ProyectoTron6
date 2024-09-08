using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    internal class Jugador : Bike
    {
        public Jugador(Node initialPosition) : base(initialPosition)
        {
            currentPosition.Data = "Jugador"; // Cambia el dato que almacena el nodo a PlayerBike
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
    }
}
