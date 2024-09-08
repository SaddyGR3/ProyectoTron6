namespace ProyectoTron6
{
    public partial class Form1 : Form
    {
        private Matriz linkedList;
        private Jugador jugador;
        private List<Enemigos> enemigos;
        private const int GridSize = 30; // Tamaño de la matriz 30x30
        private System.Windows.Forms.Timer MovimientoJugador;
        private Keys lastDirection;
        private List<System.Windows.Forms.Timer> timersEnemigos;

        //Variables para mostrar la información del jugador y enemigos
        private Label lblVelocidadJugador;
        private Label lblCombustibleJugador;
        private Label lblVelocidadEnemigos;

        public Form1()
        {
            InitializeComponent();

            // Altura reservada para el área de visualización de datos
            int panelHeight = 80; // Altura del área superior de datos
            int matrizSize = 600; // Tamaño fijo de la matriz cuadrada (30x30)

            // Establecer el tamaño total de la ventana: altura = matriz + panelHeight
            this.ClientSize = new Size(matrizSize, matrizSize + panelHeight);
            this.MinimumSize = this.ClientSize; // Evitar que la ventana se haga más pequeña que el tamaño necesario

            // Configurar fondo negro para el formulario completo
            // Configurar fondo negro para el formulario completo
            this.BackColor = Color.Black;

            // Crear etiquetas para la información del jugador y enemigos
            lblVelocidadJugador = new Label();
            lblVelocidadJugador.Text = "Velocidad Jugador: 0";
            lblVelocidadJugador.ForeColor = Color.White; // Texto en blanco
            lblVelocidadJugador.BackColor = Color.Black; // Fondo negro para la etiqueta
            lblVelocidadJugador.Location = new Point(10, 10); // Posición de la etiqueta
            lblVelocidadJugador.AutoSize = true; // Ajuste automático al texto

            lblCombustibleJugador = new Label();
            lblCombustibleJugador.Text = "Combustible: 100";
            lblCombustibleJugador.ForeColor = Color.White;
            lblCombustibleJugador.BackColor = Color.Black; // Fondo negro para la etiqueta
            lblCombustibleJugador.Location = new Point(10, 30); // Posición de la etiqueta
            lblCombustibleJugador.AutoSize = true;

            lblVelocidadEnemigos = new Label();
            lblVelocidadEnemigos.Text = "Velocidad Enemigos: ";
            lblVelocidadEnemigos.ForeColor = Color.White;
            lblVelocidadEnemigos.BackColor = Color.Black; // Fondo negro para la etiqueta
            lblVelocidadEnemigos.Location = new Point(10, 50); // Posición de la etiqueta
            lblVelocidadEnemigos.AutoSize = true;

            // Añadir etiquetas al formulario
            this.Controls.Add(lblVelocidadJugador);
            this.Controls.Add(lblCombustibleJugador);
            this.Controls.Add(lblVelocidadEnemigos);

            this.DoubleBuffered = true; // Activar el doble buffering para evitar parpadeos

            linkedList = new Matriz(GridSize, GridSize);
            jugador = new Jugador(linkedList.GetNode(GridSize / 2, GridSize / 2)); // Moto inicia en el centro de la matriz
            enemigos = new List<Enemigos>();

            // Inicializar lista de timers
            timersEnemigos = new List<System.Windows.Forms.Timer>();


            // Creación de 3 motos enemigas con posiciones únicas
            for (int i = 0; i < 3; i++)
            {
                Node enemyStartNode;
                do
                {
                    enemyStartNode = linkedList.GetNode(new Random().Next(0, GridSize), new Random().Next(0, GridSize));
                } while (enemyStartNode == jugador.GetCurrentPosition() || enemigos.Any(e => e.GetCurrentPosition() == enemyStartNode));

                var enemigo = new Enemigos(enemyStartNode);
                enemigos.Add(enemigo);

                // Configurar temporizador para cada enemigo con su velocidad
                var timerEnemigo = new System.Windows.Forms.Timer();
                timerEnemigo.Interval = GetSpeedInterval(enemigo.velocidad);
                timerEnemigo.Tick += (sender, e) => Movimientoenemigo_Tick(enemigo);
                timerEnemigo.Start();

                timersEnemigos.Add(timerEnemigo);
            }


            // Configurar temporizador para movimiento del jugador
            MovimientoJugador = new System.Windows.Forms.Timer();
            MovimientoJugador.Interval = GetSpeedInterval(jugador.velocidad); // Inicializar con la velocidad del jugador
            MovimientoJugador.Tick += MovimientoJugador_Tick;
            MovimientoJugador.Start();

            this.Paint += new PaintEventHandler(Form1_Paint);
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            // Actualiza las etiquetas con la información inicial
            UpdateInfoLabels();
        }

        private void Movimientoenemigo_Tick(Enemigos enemigo)
        {
            enemigo.MoveRandom();
            UpdateInfoLabels();
            this.Refresh();
        }


        private int GetSpeedInterval(int velocidad)
        {
            // Aquí calculas los milisegundos por nodo según la velocidad
            // Velocidad 10 es 4 nodos por segundo (1000ms / 4 = 250ms por nodo)
            return (int)(1000 / (2.2 + (velocidad - 2) * 0.2));
        }
        private void MovimientoJugador_Tick(object sender, EventArgs e)
        {
            // Ejecutar el movimiento basado en la última tecla presionada
            if (lastDirection != Keys.None)
            {
                switch (lastDirection)
                {
                    case Keys.W:
                        jugador.MoveUp();
                        break;
                    case Keys.S:
                        jugador.MoveDown();
                        break;
                    case Keys.A:
                        jugador.MoveLeft();
                        break;
                    case Keys.D:
                        jugador.MoveRight();
                        break;
                }
            }
            UpdateInfoLabels();
            this.Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Guardar la última dirección de movimiento
            lastDirection = e.KeyCode;
        }

        private void UpdateInfoLabels()
        {
            // Actualizar la etiqueta de velocidad del jugador
            lblVelocidadJugador.Text = $"Velocidad Jugador: {jugador.velocidad}";

            // Actualizar la etiqueta de combustible del jugador
            lblCombustibleJugador.Text = $"Combustible: {jugador.combustible}";

            // Actualizar la etiqueta de velocidad de los enemigos (intervalo)
            var velocidadesEnemigos = string.Join(", ", enemigos.Select(e => e.velocidad));
            lblVelocidadEnemigos.Text = $"Velocidad Enemigos: {velocidadesEnemigos}";
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int panelHeight = 80; // Espacio reservado para los datos
            int matrizSize = 600; // Tamaño fijo de la matriz (cuadrada 30x30)
            int cellSize = matrizSize / GridSize; // Tamaño de cada celda para que sean cuadradas

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    Node node = linkedList.GetNode(i, j);
                    Rectangle rect = new Rectangle(j * cellSize, panelHeight + i * cellSize, cellSize, cellSize); // Dibuja debajo del panel

                    // Dibuja la estela del jugador en amarillo
                    if (jugador.GetTrail().Contains(node))
                    {
                        g.FillRectangle(Brushes.Yellow, rect); // Dibuja la estela en amarillo
                    }
                    // Dibuja la moto del jugador
                    else if (node == jugador.GetCurrentPosition())
                    {
                        g.FillRectangle(Brushes.Red, rect); // Dibuja la moto del jugador como un cuadro rojo
                    }
                    else
                    {
                        // Verifica si alguno de los enemigos ocupa este nodo o si es parte de su estela
                        bool isEnemyTrail = false;
                        foreach (var enemigo in enemigos)
                        {
                            if (enemigo.GetTrail().Contains(node))
                            {
                                g.FillRectangle(Brushes.Blue, rect); // Dibuja la estela del enemigo en azul
                                isEnemyTrail = true;
                                break; // Si es parte de la estela de algún enemigo, no necesitas seguir verificando
                            }
                            else if (node == enemigo.GetCurrentPosition())
                            {
                                g.FillRectangle(Brushes.Green, rect); // Dibuja la moto del enemigo como un cuadro verde
                                isEnemyTrail = true;
                                break;
                            }
                        }

                        // Si no es parte de la estela ni la moto de ningún enemigo, dibuja un cuadro negro
                        if (!isEnemyTrail)
                        {
                            g.FillRectangle(Brushes.Black, rect); // Dibuja los nodos como cuadros negros
                        }
                    }

                    // Dibujar los bordes de las celdas
                    g.DrawRectangle(Pens.Gray, rect);
                }
            }
        }
 
    }
}
