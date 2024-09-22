using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    internal abstract class Item //Clase abstracta para los ítems
    {
        public abstract void Aplicar(Moto moto);

    }
    internal class Combustible : Item
    {
        public override void Aplicar(Moto moto)
        {
            if (moto.combustible <= 90) // Solo aplica si el combustible es <= 90
            {
                moto.combustible = Math.Min(moto.combustible + 10, 100);
            }
            else
            {
                // Reenfilar el combustible si está lleno
                moto.itemQueue.Enqueue(this);
            }
        }
    }
    internal class Incrementar : Item
    {
        public override void Aplicar(Moto moto)
        {
            Random random = new Random();
            int aumentar = random.Next(2, 6); // Incremento aleatorio entre 2 y 5
            moto.tamañoestela += aumentar;
        }
    }
    internal class Bomba : Item
    {
        public override void Aplicar(Moto moto)
        {
            //Verificar si la moto es invulnerable antes de destruirla
            if (!moto.Invulnerable()) //revisar esta parte si verifica True o False
            {
                moto.Destruir();
            }
            else
            {
                Console.WriteLine("La moto es invulnerable, no puede ser destruida.");
            }
        }
    }
    internal abstract class Poder
    {
        public abstract void Activar(Moto moto);
    }

    internal class Escudo : Poder
    {
        public override void Activar(Moto moto)
        {
            moto.HacerInvulnerable(5); // Invulnerabilidad durante 5 segundos
            Task.Run(async () =>
            {
                await Task.Delay(5000); // Esperar 5 segundos
                moto.HacerInvulnerable(0); // Desactivar invulnerabilidad
            });
        }
    }

    internal class HiperVelocidad : Poder
    {
        public override void Activar(Moto moto)
        {
            Random random = new Random();
            int incrementoVelocidad = random.Next(1, 11);
            moto.velocidad += incrementoVelocidad;
            Task.Run(async () =>
            {
                int duracion = random.Next(2000, 10000); // Duración aleatoria entre 2 y 10 segundos
                await Task.Delay(duracion);
                moto.velocidad -= incrementoVelocidad; // Restaurar velocidad original
            });
        }
    }

}

