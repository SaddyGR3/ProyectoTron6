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
                System.Diagnostics.Debug.WriteLine($"Current Position: [{currentPosition.Data}]");
                currentPosition.Data = ""; //Nodo actual a vacio
                currentPosition = currentPosition.Up;
                currentPosition.Data = "Bike"; //Los datos del nuevo nodo pasan a ser la moto
                System.Diagnostics.Debug.WriteLine($"New Position: [{currentPosition.Data}]");
            }
        }

        public void MoveDown()
        {
            if (currentPosition.Down != null)
            {
                System.Diagnostics.Debug.WriteLine($"Current Position: [{currentPosition.Data}]");
                currentPosition.Data = ""; //Nodo actual a vacio
                currentPosition = currentPosition.Down;
                currentPosition.Data = "Bike"; //Los datos del nuevo nodo pasan a ser la moto
                System.Diagnostics.Debug.WriteLine($"New Position: [{currentPosition.Data}]");
            }
        }

        public void MoveLeft()
        {
            if (currentPosition.Left != null)
            {
                System.Diagnostics.Debug.WriteLine($"Current Position: [{currentPosition.Data}]");
                currentPosition.Data = "";
                currentPosition = currentPosition.Left;
                currentPosition.Data = "Bike";
                System.Diagnostics.Debug.WriteLine($"New Position: [{currentPosition.Data}]");
            }
        }

        public void MoveRight()
        {
            if (currentPosition.Right != null)
            {
                System.Diagnostics.Debug.WriteLine($"Current Position: [{currentPosition.Data}]");
                currentPosition.Data = "";
                currentPosition = currentPosition.Right;
                currentPosition.Data = "Bike";
                System.Diagnostics.Debug.WriteLine($"New Position: [{currentPosition.Data}]");
            }
        }
    }
}
