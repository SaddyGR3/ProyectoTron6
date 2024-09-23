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
        public ListaEnlazada<Nodo> estela;
        public int tamañoestela;
        public double Tiempointervalo;
        public ItemQueue itemQueue2 = new ItemQueue();
        private bool invulnerabilidad = false;
        protected string DirActual; //Para el movimiento continuado del jugador
        private string direccionActual = "derecha";
        private int nodosRecorridos = 0;

        // Cola para items que se activan automáticamente
        public ColaPrioridad<Item> itemQueue = new ColaPrioridad<Item>();

        // Pila para poderes activados manualmente
        public Pila<Poder> poderStack = new Pila<Poder>();



        public Moto(Nodo PosInicial)
        {
            PosActual = PosInicial; //Inicializa la posición actual de la moto.
            PosActual.Data = "Bike"; //El nodo actual almacena la moto.
            estela = new ListaEnlazada<Nodo>(); //Inicializa la lista enlazada para la estela.
            tamañoestela = 3;
            combustible = 100;
            velocidad = new Random().Next(1, 11); //la velocidad es aleatoria entre 1 y 10 al abrir el juego.
            Tiempointervalo = ConversionVelocidad(velocidad);//metodo que convierte estos intervalos entre 1 y 10 a nodos por segundo.
            DirActual = "right"; // Dirección inicial predeterminada para evitar errores
        }

        //Metodo para manejar la velocidad de la moto
        private double ConversionVelocidad(int velocidad)
        {
            velocidad = Math.Clamp(velocidad, 1, 10);//Limita la velocidad a un rango entre 1 y 10.
            return 250 + (10 - velocidad) * 25;//Intervalo base: velocidad máxima (10) = 250 ms por nodo// Cuanto menor sea la velocidad, mayor será el intervalo (más lento).
        }

        //Metodo que devuelve la posición actual de la moto
        public Nodo RPosActual()
        {
            return PosActual;
        }
        public string RDirActual()
        {
            return DirActual;
        }
        //Metodo para establecer la dirección
        public void EstablecerDireccion(string Direccion)
        {
            if ((DirActual == "arriba" && Direccion == "abajo") ||
                (DirActual == "abajo" && Direccion == "arriba") ||
                (DirActual == "izquierda" && Direccion == "derecha") ||
                (DirActual == "derecha" && Direccion == "izquierda"))
            {
                return; //No permite giros de 180 grados
            }
            DirActual = Direccion;
        }


        //Devuelve la estela de la moto
        public ListaEnlazada<Nodo>  GetTrail()
        {
            return estela;
        }

        //Metodo que se mueve en la direccion actual
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
        //Método para verificar si hay colisión en la nueva posición
        protected bool Colision(Nodo posNueva)
        {
            return Colisioncontramoto(posNueva) || ColisionContraEstela(posNueva);
        }

        //Método para verificar si hay colisión con otra moto
        protected bool Colisioncontramoto(Nodo posNueva)
        {
            return posNueva.Data == "EnemyBike" || posNueva.Data == "Jugador";
        }

        //Método para verificar si hay colisión con la estela
        protected bool ColisionContraEstela(Nodo posNueva)
        {
            return posNueva.Data == "Trail";
        }
        //Método para manejar la colisión
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
                // Evitar giros bruscos hacia la dirección opuesta.
                if ((direccionActual == "arriba" && direccionNueva == "abajo") ||
                    (direccionActual == "abajo" && direccionNueva == "arriba") ||
                    (direccionActual == "izquierda" && direccionNueva == "derecha") ||
                    (direccionActual == "derecha" && direccionNueva == "izquierda"))
                {
                    return; //No permite cambiar bruscamente hacia la dirección opuesta.
                }

                //Verifica colisiones
                if (Colision(posNueva))
                {
                    ManejoColision(posNueva);
                    return;
                }

                //Verifica si el nodo contiene un objeto de tipo Item o Poder basado en el string
                switch (posNueva.Data)
                {
                    case "Combustible":
                        var combustible = new Combustible();  
                        itemQueue.Enqueue(combustible);      
                        break;

                    case "Incremento":
                        var incrementar = new Incrementar();  
                        itemQueue.Enqueue(incrementar);       
                        break;

                    case "Bomba":
                        var bomba = new Bomba();  
                        itemQueue.Enqueue(bomba); 
                        break;

                    case "Escudo":
                        var escudo = new Escudo();  
                        poderStack.Push(escudo);   
                        break;

                    case "Acelerar":
                        var hiperVelocidad = new HiperVelocidad();  
                        poderStack.Push(hiperVelocidad);           
                        break;

                    default:
                        //No hace nada si no es un item o poder conocido
                        break;
                }

                //Continua el movimiento si no hay colisión
                estela.AgregarPrimero(PosActual); // Agregar la posición actual al inicio de la estela
                //Si la estela supera el tamaño, eliminar el último nodo
                if (estela.Count > tamañoestela)
                {
                    Nodo ultimoNodo = estela.ObtenerUltimo(); // Usar el método para obtener el último nodo
                    if (ultimoNodo != null)
                    {
                        ultimoNodo.Data = ""; //Limpia el dato del último nodo
                    }
                    estela.EliminarUltimo(); //Elimina el último nodo
                }

                PosActual.Data = "Trail"; //Marca la posición anterior como estela
                PosActual = posNueva; //Mueve a la nueva posición
                PosActual.Data = DatadeMoto(); //Marca la nueva posición como la moto
                                               //Incrementa el contador de nodos recorridos
                nodosRecorridos++;

                //Cada 5 nodos, reducir 10 de combustible
                if (nodosRecorridos >= 5)
                {
                    combustible -= 10;
                    nodosRecorridos = 0; //Reinicia el contador
                }

                if (combustible <= 0)
                {
                    this.Destruir();  //Combustible agotado, se destruye.
                }

                //Actualiza la dirección actual si el movimiento fue exitoso.
                direccionActual = direccionNueva;
            }
        }



        public void HacerInvulnerable(int seconds)
        {
            invulnerabilidad = seconds > 0;
            Console.WriteLine(invulnerabilidad ? "Moto invulnerable" : "Moto vulnerable");
        }

        public virtual void Destruir()
        {
            //logica para destruir la moto del jugador
            Console.WriteLine("La moto ha sido destruida.");
            PosActual.Data = "";  //Libera la posición actual
            estela.Limpiar(); //Vacia la estela

        }

        //Método para verificar si la moto es invulnerable
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
