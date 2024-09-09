using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    internal class Bike
    {
        protected Node currentPosition; //Posicion Actual de la moto
        public int velocidad;
        public int combustible;
        public LinkedList<Node> estela; //La estela dejada por la moto
        public int tamañoestela;
        public double intervalTime;
        public ItemQueue itemQueue = new ItemQueue();
        private bool isInvulnerable = false;


        //variable para el movimiento continuado del jugador
        protected string currentDirection;


        public Bike(Node initialPosition)
        {
            currentPosition = initialPosition; //Inicializa la posición actual de la moto.
            currentPosition.Data = "Bike"; //El nodo actual almacena la moto.
            estela = new LinkedList<Node>(); //Inicializa la lista enlazada para la estela.
            tamañoestela = 3;
            combustible = 100;

            // Asignar velocidad aleatoria entre 1 y 10
            velocidad = new Random().Next(1, 11);
            intervalTime = ConvertSpeedToInterval(velocidad);

            currentDirection = "right"; // Dirección inicial predeterminada
        }

        //Metodo para manejar la velocidad de la moto
        private double ConvertSpeedToInterval(int speed)
        {
            switch (speed)
            {
                case 10: return 250; //4 nodos/segundo = 250 ms por nodo
                case 9: return 263;
                case 8: return 278;
                case 7: return 294;
                case 6: return 313;
                case 5: return 333;
                case 4: return 357;
                case 3: return 385;
                case 2: return 455; //2.2 nodos/segundo = 455 ms por nodo
                default: return 500; //Si no coincide, velocidad más lenta por defecto
            }
        }

        //Método que devuelve la posición actual de la moto
        public Node GetCurrentPosition()
        {
            return currentPosition;
        }

        //Método para establecer la dirección
        public void SetDirection(string direction)
        {
            currentDirection = direction;
        }

        //Devuelve la estela de la moto
        public LinkedList<Node> GetTrail()
        {
            return estela;
        }

        //Método que se mueve en la dirección actual
        public void MoveInCurrentDirection()
        {
            switch (currentDirection)
            {
                case "up":
                    Move(currentPosition.Up);
                    break;
                case "down":
                    Move(currentPosition.Down);
                    break;
                case "left":
                    Move(currentPosition.Left);
                    break;
                case "right":
                    Move(currentPosition.Right);
                    break;
            }
        }
        protected bool HasCollision(Node newPosition)
        {
            return IsCollidingWithBike(newPosition) || IsCollidingWithTrail(newPosition);
        }

        protected bool IsCollidingWithBike(Node newPosition)
        {
            return newPosition.Data == "EnemyBike" || newPosition.Data == "Jugador";
        }

        protected bool IsCollidingWithTrail(Node newPosition)
        {
            return newPosition.Data == "Trail";
        }
        protected void HandleCollision(Node newPosition)
        {
            if (IsCollidingWithBike(newPosition))
            {
                Console.WriteLine("Colisión entre motos: ambas destruidas");
                this.Destroy();
            }
            else if (IsCollidingWithTrail(newPosition))
            {
                Console.WriteLine("Colisión con una estela: la moto se destruye");
                this.Destroy();
            }
        }
        //Método para mover la moto
        protected void Move(Node newPosition)
        {
            if (newPosition != null)
            {
                // Verificar colisiones
                if (HasCollision(newPosition))
                {
                    HandleCollision(newPosition);
                    return;
                }

                // Si no hay colisión, continuar moviendo la moto
                estela.AddFirst(currentPosition); // Agrega la posición actual al inicio de la estela
                if (estela.Count > tamañoestela)
                {
                    Node lastNode = estela.Last.Value; // Obtiene el último nodo de la estela
                    lastNode.Data = ""; // Limpia el dato del último nodo de la estela
                    estela.RemoveLast(); // Elimina el último nodo de la estela
                }

                currentPosition.Data = "Trail"; // Marcar la posición anterior como estela
                currentPosition = newPosition; // Mover a la nueva posición
                currentPosition.Data = GetBikeData(); // Marcar la nueva posición como la moto

                // Consumir combustible
                combustible -= velocidad / 5;

                if (combustible <= 0)
                {
                    Console.WriteLine("Combustible agotado: la moto se destruye.");
                    this.Destroy();  // Llama al método de destrucción
                }
            }
        }

        public async Task ApplyItems()
        {
            while (itemQueue.GetItems().Count > 0)
            {
                var item = itemQueue.Dequeue();
                item.Apply(this);  // Aplica el ítem a la moto
                await Task.Delay(1000); // Delay de 1 segundo entre ítems
            }
        }

        public void SetInvulnerable(int seconds)
        {
            isInvulnerable = seconds > 0;
            Console.WriteLine(isInvulnerable ? "Moto invulnerable" : "Moto vulnerable");
        }

        public virtual void Destroy()
        {
            // Lógica para destruir la moto del jugador
            Console.WriteLine("La moto ha sido destruida.");
            currentPosition.Data = "";  // Liberar la posición actual
            estela.Clear();             // Vaciar la estela

        }

        // Método para verificar si la moto es invulnerable
        public bool IsInvulnerable()
        {
            return isInvulnerable;
        }

        //Método sobrescribible para obtener los datos de la moto
        protected virtual string GetBikeData()
        {
            return "Bike"; //Valor por defecto
        }


    }
}
