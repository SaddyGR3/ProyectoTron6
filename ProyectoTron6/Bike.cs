using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    internal class Bike
    {
        private Node currentPosition;

        public Bike(Node initialPosition)
        {
            currentPosition = initialPosition;
            currentPosition.Data = "Bike";
        }

        public Node GetCurrentPosition()
        {
            return currentPosition;
        }

        public void MoveUp()
        {
            if (currentPosition.Up != null)
            {
                currentPosition.Data = ""; //Nodo actual a vacio
                currentPosition = currentPosition.Up;
                currentPosition.Data = "Bike"; //Los datos del nuevo nodo pasan a ser la moto
            }
        }

        public void MoveDown()
        {
            if (currentPosition.Down != null)
            {
                currentPosition.Data = ""; //Nodo actual a vacio
                currentPosition = currentPosition.Down;
                currentPosition.Data = "Bike"; //Los datos del nuevo nodo pasan a ser la moto
            }
        }

        public void MoveLeft()
        {
            if (currentPosition.Left != null)
            {
                currentPosition.Data = "";
                currentPosition = currentPosition.Left;
                currentPosition.Data = "Bike";
            }
        }

        public void MoveRight()
        {
            if (currentPosition.Right != null)
            {
                currentPosition.Data = "";
                currentPosition = currentPosition.Right;
                currentPosition.Data = "Bike";
            }
        }
    }
}
