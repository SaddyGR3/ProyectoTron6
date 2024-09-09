using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProyectoTron6
{
    internal class Nodo
    {
        public string Data { get; set; }
        public Nodo Up { get; set; }
        public Nodo Down { get; set; }
        public Nodo Left { get; set; }
        public Nodo Right { get; set; }

        public Nodo()
        {
            Data = "";
            Up = null;
            Down = null;
            Left = null;
            Right = null;
        }
    }
}
