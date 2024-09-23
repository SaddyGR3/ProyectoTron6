using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    internal abstract class Item
    {
        /// <summary>
        /// Método abstracto para aplicar el ítem a una moto.
        /// </summary>
        /// <param name="moto">La moto a la que se aplica el ítem.</param>
        public abstract void Aplicar(Moto moto);
    }
    /// <summary>
    /// Clase que representa un ítem de combustible.
    /// </summary>
    internal class Combustible : Item
    {
        /// <summary>
        /// Aplica el ítem de combustible a una moto.
        /// </summary>
        /// <param name="moto">La moto a la que se aplica el ítem.</param>
        public override void Aplicar(Moto moto)
        {
            if (moto.combustible <= 90) //Solo aplica si el combustible es <= 90
            {
                moto.combustible = Math.Min(moto.combustible + 10, 100);
            }
            else
            {
                //vuelve a la cola el combustible si está lleno
                moto.itemQueue.Enqueue(this);
            }
        }
    }
    /// <summary>
    /// Clase que representa un ítem de incremento.
    /// </summary>
    internal class Incrementar : Item
    {
        /// <summary>
        /// Aplica el ítem de incremento a una moto.
        /// </summary>
        /// <param name="moto">La moto a la que se aplica el ítem.</param>
        public override void Aplicar(Moto moto)
        {
            Random random = new Random();
            int aumentar = random.Next(2, 6); //Incremento aleatorio entre 2 y 5
            moto.tamañoestela += aumentar;
        }
    }
    /// <summary>
    /// Clase que representa un ítem de bomba.
    /// </summary>
    internal class Bomba : Item
    {
        /// <summary>
        /// Aplica el ítem de bomba a una moto.
        /// </summary>
        /// <param name="moto">La moto a la que se aplica el ítem.</param>
        public override void Aplicar(Moto moto)
        {
            //Verifica si la moto es invulnerable antes de destruirla
            if (!moto.Invulnerable()) //revisa esta parte si verifica True o False
            {
                moto.Destruir();
            }
            else
            {
                Console.WriteLine("La moto es invulnerable, no puede ser destruida.");
            }
        }
    }
    /// <summary>
    /// Clase abstracta para los poderes.
    /// </summary>
    internal abstract class Poder
    {
        /// <summary>
        /// Método abstracto para activar el poder en una moto.
        /// </summary>
        /// <param name="moto">La moto en la que se activa el poder.</param>
        public abstract void Activar(Moto moto);
    }

    /// <summary>
    /// Clase que representa un poder de escudo.
    /// </summary>
    internal class Escudo : Poder
    {
        /// <summary>
        /// Activa el poder de escudo en una moto.
        /// </summary>
        /// <param name="moto">La moto en la que se activa el poder.</param>
        public override void Activar(Moto moto)
        {
            moto.HacerInvulnerable(5); //Invulnerabilidad durante 5 segundos
            Task.Run(async () =>
            {
                await Task.Delay(5000); //Espera 5 segundos
                moto.HacerInvulnerable(0); //Desactiva la invulnerabilidad
            });
        }
    }

    /// <summary>
    /// Clase que representa el poder de hipervelocidad.
    /// </summary>
    internal class HiperVelocidad : Poder
    {
        /// <summary>
        /// Activa el poder de hipervelocidad en una moto.
        /// </summary>
        /// <param name="moto">La moto en la que se activa el poder.</param>
        public override void Activar(Moto moto)
        {
            Random random = new Random();
            int incrementoVelocidad = random.Next(1, 11);
            moto.velocidad += incrementoVelocidad;
            Task.Run(async () =>
            {
                int duracion = random.Next(2000, 10000); //Duración aleatoria entre 2 y 10 segundos
                await Task.Delay(duracion);
                moto.velocidad -= incrementoVelocidad; //Restaura la  velocidad a la original
            });
        }
    }

}

