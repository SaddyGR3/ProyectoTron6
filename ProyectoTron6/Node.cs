using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProyectoTron6
{
    internal class Node
    {
        public string Data { get; set; }
        public Node Up { get; set; }
        public Node Down { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node()
        {
            Data = "";
            Up = null;
            Down = null;
            Left = null;
            Right = null;
        }
    }
}
