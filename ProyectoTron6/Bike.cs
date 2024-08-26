using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    internal class Bike
    {
        private Node currentPosition; //Nodo que representa la posición actual de la moto.
        public int velocidad;
        public int combustible;
        public LinkedList<Node> estela; //Lista enlazada que representa la estela dejada por la moto.que deja la moto.
        public int tamañoestela;

        public Bike(Node initialPosition)
        {
            currentPosition = initialPosition; //Inicializa la posición actual de la moto segun la pos dada como entrada al crear una instancia moto
            currentPosition.Data = "Bike";//El nodo actual ahora "almacena" la moto. Data es un str definido en la clase nodo.
            estela = new LinkedList<Node>(); //Inicializa la lista enlazada para la estela.
            tamañoestela = 3; 
            velocidad = new Random().Next(1,11); //La velocidad de las motos al crearse,es un random,falta ajustar esta parte.
            combustible = 100;
        }

        public Node GetCurrentPosition() //Metodo que devuelve la posicion actual de la moto.
        {
            return currentPosition;
        }

        public LinkedList<Node> GetTrail()
        {
            return estela; //Metodo para acceder a la estela
        }

        public void MoveUp() //Metodos de movimiento de la moto. 
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

        private void Move(Node newPosition)
        {
            if (newPosition != null)//Verifica si la nueva posición es valida.
            {
                
                estela.AddFirst(currentPosition);//Agrega la posición actual al inicio de la estela.
                if (estela.Count > tamañoestela) // Verifica si la estela excede el tamaño permitido.
                {
                    Node lastNode = estela.Last.Value; //Obtiene el último nodo de la estela.
                    lastNode.Data = ""; //Vacia el dato del ultimo nodo de la estela,este paso es necesario para asegurarse que el nodo ya no este ocupado por la moto
                    estela.RemoveLast(); //Elimina el ultimo nodo de la estela.
                }

                //Actualiza la posicion de la moto
                currentPosition.Data = ""; //Vaciar el nodo actual de la moto
                currentPosition = newPosition; //La mueve a la nueva posicion.
                currentPosition.Data = "Bike"; //Marca la nueva posicion como ocupada por la moto.

                //Consumir combustible
               combustible -= velocidad / 5; //Consume 1 celda de combustible por cada 5 elementos recorridos HAY QUE ARREGLAR

                Console.WriteLine($"Current Position: [{currentPosition.Data}], Fuel: {combustible}"); //PASARLO A DEBUG.
            }
        }

        //Aqui podrian ir los metodos de poderes y items. Incorporado probablemente con pila.
    }
}
