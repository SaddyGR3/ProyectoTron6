using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTron6
{
    internal abstract class Item //Clase abstracta para los ítems
    {
        public abstract void Apply(Bike bike);

    }
    internal class FuelCell : Item
    {
        public override void Apply(Bike bike)
        {
            if (bike.combustible < 100)
            {
                bike.combustible = Math.Min(bike.combustible + 10, 100);
            }
            else
            {
                // Reenfilar el combustible si está lleno
                bike.itemQueue.Enqueue(this);
            }
        }
    }
    internal class TrailGrowth : Item
    {
        public override void Apply(Bike bike)
        {
            Random random = new Random();
            int increase = random.Next(2, 6); // Incremento aleatorio entre 2 y 5
            bike.tamañoestela += increase;
        }
    }
    internal class Bomb : Item
    {
        public override void Apply(Bike bike)
        {
            //Verificar si la moto es invulnerable antes de destruirla
            if (!bike.IsInvulnerable()) //revisar esta parte si verifica True o False
            {
                bike.Destroy();
            }
            else
            {
                Console.WriteLine("La moto es invulnerable, no puede ser destruida.");
            }
        }
    }
    internal abstract class Power
    {
        public abstract void Activate(Bike bike);
    }

    internal class Shield : Power
    {
        public override void Activate(Bike bike)
        {
            bike.SetInvulnerable(5); // Invulnerabilidad durante 5 segundos
            Task.Run(async () =>
            {
                await Task.Delay(5000); // Esperar 5 segundos
                bike.SetInvulnerable(0); // Desactivar invulnerabilidad
            });
        }
    }

    internal class HyperSpeed : Power
    {
        public override void Activate(Bike bike)
        {
            Random random = new Random();
            int increase = random.Next(1, 11);
            bike.velocidad += increase;
            Task.Run(async () =>
            {
                int duration = random.Next(2000, 10000); // Duración aleatoria entre 2 y 10 segundos
                await Task.Delay(duration);
                bike.velocidad -= increase; // Restaurar velocidad original
            });
        }
    }

}

