namespace ProyectoTron6
{
    public partial class Form1 : Form
    {
        private Matriz linkedList;
        private Bike bike;
        private const int GridSize = 30; // Tamaño de la matriz 30x30

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true; // Activar el doble buffering para evitar parpadeos

            linkedList = new Matriz(GridSize, GridSize);
            bike = new Bike(linkedList.GetNode(GridSize / 2, GridSize / 2)); // Moto inicia en el centro de la matriz

            this.Paint += new PaintEventHandler(Form1_Paint);
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int cellWidth = this.ClientSize.Width / GridSize;
            int cellHeight = this.ClientSize.Height / GridSize;

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    Node node = linkedList.GetNode(i, j);
                    Rectangle rect = new Rectangle(j * cellWidth, i * cellHeight, cellWidth, cellHeight);

                    // Dibuja la estela en amarillo
                    if (bike.GetTrail().Contains(node))
                    {
                        g.FillRectangle(Brushes.Yellow, rect); // Dibuja la estela en amarillo
                    }
                    else if (node == bike.GetCurrentPosition())
                    {
                        g.FillRectangle(Brushes.Red, rect); // Dibuja la moto como un cuadro rojo
                    }
                    else
                    {
                        g.FillRectangle(Brushes.Black, rect); // Dibuja los nodos como cuadros negros
                    }

                    g.DrawRectangle(Pens.Gray, rect); // Opcional: dibujar bordes grises entre los cuadros
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    bike.MoveUp();
                    break;
                case Keys.S:
                    bike.MoveDown();
                    break;
                case Keys.A:
                    bike.MoveLeft();
                    break;
                case Keys.D:
                    bike.MoveRight();
                    break;
            }

            this.Refresh(); // Redraw the panel after the bike moves
        }
    }
}
