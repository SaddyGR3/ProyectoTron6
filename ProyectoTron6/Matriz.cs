using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProyectoTron6
{
    internal class Matriz
    {
        public Node[,] Matrix { get; private set; }

        public Matriz(int rows, int cols)
        {
            Matrix = new Node[rows, cols];
            //Inicializa la matriz con sus  nodos.
            for (int i = 0; i < rows; i++) //Bucle que itera sobre cada fila
            {
                for (int j = 0; j < cols; j++)//En cada fila itera sobre toda la columna.
                {
                    Matrix[i, j] = new Node();//Crea un nodo en cada ubicacion.
                }
            }
            //Connecta los nodos en las 4 direcciones.
            for (int i = 0; i < rows; i++)//itera igual por toda la matriz
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i > 0) Matrix[i, j].Up = Matrix[i - 1, j]; //si el nodo no esta en la primera fila,Entonces tiene un Nodo arriba,al que se le asigna una referencia desde el actual al de arriba.
                    if (i < rows - 1) Matrix[i, j].Down = Matrix[i + 1, j];//Si el nodo no esta en la ultima fila,Significa que tiene un nodo debajo,le asigna una referencia.
                    if (j > 0) Matrix[i, j].Left = Matrix[i, j - 1];//si el nodo no esta en la primera columna,significa que tiene un nodo a la izquierda,al que le asigna una referencia desde el actual al de la izquierda
                    if (j < cols - 1) Matrix[i, j].Right = Matrix[i, j + 1];//Si el nodo no esta en la ultima columna,Significa que tiene un nodo a la derecha.le asigna referencia.
                }
            }
        }
        public Node FindFreeNode()
        {
            Random rand = new Random();
            Node freeNode;
            do
            {
                int row = rand.Next(0, Matrix.GetLength(0)); // Filas
                int col = rand.Next(0, Matrix.GetLength(1)); // Columnas
                freeNode = Matrix[row, col];
            } while (!string.IsNullOrEmpty(freeNode.Data)); // Verifica que el nodo esté libre
            return freeNode;
        }

        //Metodo para obtener un nodo especifico de la matriz
        public Node GetNode(int row, int col)
        {
            return Matrix[row, col];
        }
        //Metodo que actualiza la clase matriz para mostrarlo en pantalla.
    }
}
