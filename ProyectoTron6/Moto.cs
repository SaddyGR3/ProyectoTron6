using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    internal class Moto
    {
        public Nodo PosActual; 
        public int velocidad;
        public int combustible;
        public LinkedList<Nodo> estela;
        public int tamañoestela;
        public double Tiempointervalo;
        public ItemQueue itemQueue = new ItemQueue();
        private bool invulnerabilidad = false;
        protected string DirActual; //Para el movimiento continuado del jugador
        private string direccionActual = "derecha";


        public Moto(Nodo PosInicial)
        {
            PosActual = PosInicial; //Inicializa la posición actual de la moto.
            PosActual.Data = "Bike"; //El nodo actual almacena la moto.
            estela = new LinkedList<Nodo>(); //Inicializa la lista enlazada para la estela.
            tamañoestela = 3;
            combustible = 100;
            velocidad = new Random().Next(1, 11); //la velocidad es aleatoria entre 1 y 10 al abrir el juego.
            Tiempointervalo = ConversionVelocidad(velocidad);//metodo que convierte estos intervalos entre 1 y 10 a nodos por segundo.
            DirActual = "right"; // Dirección inicial predeterminada para evitar errores
        }

        //Metodo para manejar la velocidad de la moto
        private double ConversionVelocidad(int velocidad)
        {
            velocidad = Math.Clamp(velocidad, 1, 10);// Limita la velocidad a un rango entre 1 y 10.
            return 250 + (10 - velocidad) * 25;//Intervalo base: velocidad máxima (10) = 250 ms por nodo// Cuanto menor sea la velocidad, mayor será el intervalo (más lento).
        }

        //Método que devuelve la posición actual de la moto
        public Nodo RPosActual()
        {
            return PosActual;
        }
        public string RDirActual()
        {
            return DirActual;
        }
        //Método para establecer la dirección
        public void EstablecerDireccion(string Direccion)
        {
            if ((DirActual == "arriba" && Direccion == "abajo") ||
                (DirActual == "abajo" && Direccion == "arriba") ||
                (DirActual == "izquierda" && Direccion == "derecha") ||
                (DirActual == "derecha" && Direccion == "izquierda"))
            {
                return; // No permitir giros de 180 grados
            }
            DirActual = Direccion;
        }


        //Devuelve la estela de la moto
        public LinkedList<Nodo> GetTrail()
        {
            return estela;
        }

        //Método que se mueve en la dirección actual
        public void MoverDireccionActual()
        {
            switch (DirActual)
            {
                case "arriba":
                    Mover(PosActual.Up, "arriba");
                    break;
                case "abajo":
                    Mover(PosActual.Down, "abajo");
                    break;
                case "Izquierda":
                    Mover(PosActual.Left, "izquierda");
                    break;
                case "Derecha":
                    Mover(PosActual.Right, "derecha");
                    break;
            }
        }
        protected bool Colision(Nodo posNueva)
        {
            return Colisioncontramoto(posNueva) || ColisionContraEstela(posNueva);
        }

        protected bool Colisioncontramoto(Nodo posNueva)
        {
            return posNueva.Data == "EnemyBike" || posNueva.Data == "Jugador";
        }

        protected bool ColisionContraEstela(Nodo posNueva)
        {
            return posNueva.Data == "Trail";
        }
        protected void ManejoColision(Nodo posNueva)
        {
            if (Colisioncontramoto(posNueva))
            {
                Console.WriteLine("Colisión entre motos: ambas destruidas");
                this.Destruir();
            }
            else if (ColisionContraEstela(posNueva))
            {
                Console.WriteLine("Colisión con una estela: la moto se destruye");
                this.Destruir();
            }
        }
        //Método para mover la moto
        protected void Mover(Nodo posNueva, string direccionNueva)
        {
            if (posNueva != null)
            {
                // Evitamos giros bruscos hacia la dirección opuesta.
                if ((direccionActual == "arriba" && direccionNueva == "abajo") ||
                    (direccionActual == "abajo" && direccionNueva == "arriba") ||
                    (direccionActual == "izquierda" && direccionNueva == "derecha") ||
                    (direccionActual == "derecha" && direccionNueva == "izquierda"))
                {
                    return; // No permitimos cambiar bruscamente hacia la dirección opuesta.
                }

                // Verificar colisiones
                if (Colision(posNueva))
                {
                    ManejoColision(posNueva);
                    return;
                }

                //Si no hay colisión, continuar moviendo la moto
                estela.AddFirst(PosActual); // Agrega la posicion actual al inicio de la estela
                if (estela.Count > tamañoestela)
                {
                    Nodo lastNode = estela.Last.Value; //Obtiene el último nodo de la estela
                    lastNode.Data = ""; //Limpia el dato del último nodo de la estela
                    estela.RemoveLast(); //Elimina el último nodo de la estela
                }

                PosActual.Data = "Trail"; // Marcar la posición anterior como estela
                PosActual = posNueva; // Mover a la nueva posición
                PosActual.Data = DatadeMoto(); // Marcar la nueva posición como la moto

                // Consumir combustible
                combustible -= velocidad / 5;

                if (combustible <= 0)
                {
                    this.Destruir();  //Combustible agotado,se destruye.
                }

                // Actualizamos la dirección actual si el movimiento fue exitoso.
                direccionActual = direccionNueva;
            }
        }

        public async Task AplicarItem()
        {
            while (itemQueue.GetItems().Count > 0)
            {
                var item = itemQueue.Dequeue();
                item.Aplicar(this);  // Aplica el ítem a la moto
                await Task.Delay(1000); // Delay de 1 segundo entre ítems
            }
        }

        public void HacerInvulnerable(int seconds)
        {
            invulnerabilidad = seconds > 0;
            Console.WriteLine(invulnerabilidad ? "Moto invulnerable" : "Moto vulnerable");
        }

        public virtual void Destruir()
        {
            // Lógica para destruir la moto del jugador
            Console.WriteLine("La moto ha sido destruida.");
            PosActual.Data = "";  // Liberar la posición actual
            estela.Clear();             // Vaciar la estela

        }

        // Método para verificar si la moto es invulnerable
        public bool Invulnerable()
        {
            return invulnerabilidad;
        }

        //Método sobrescribible para obtener los datos de la moto
        protected virtual string DatadeMoto()
        {
            return "Bike"; //Valor por defecto
        }


    }
}
