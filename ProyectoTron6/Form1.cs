using System.Collections.Generic;

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

            RespawnItems();

            // Creación de 3 motos enemigas con posiciones únicas
            for (int i = 0; i < 3; i++)
            {
                Nodo enemyStartNode;
                do
                {
                    enemyStartNode = linkedList.GetNode(new Random().Next(0, GridSize), new Random().Next(0, GridSize));
                } while (enemyStartNode == jugador.RPosActual() || enemigos.Any(e => e.RPosActual() == enemyStartNode));

                var enemigo = new Enemigos(enemyStartNode);
                enemigos.Add(enemigo);

                // Configurar temporizador para cada enemigo con su velocidad
                var timerEnemigo = new System.Windows.Forms.Timer();
                timerEnemigo.Interval = IntervaloVelocidad(enemigo.velocidad);
                timerEnemigo.Tick += (sender, e) => Movimientoenemigo_Tick(enemigo);
                timerEnemigo.Start();

                timersEnemigos.Add(timerEnemigo);
            }


            // Configurar temporizador para movimiento del jugador
            MovimientoJugador = new System.Windows.Forms.Timer();
            MovimientoJugador.Interval = IntervaloVelocidad(jugador.velocidad); // Inicializar con la velocidad del jugador
            MovimientoJugador.Tick += MovimientoJugador_Tick;
            MovimientoJugador.Start();

            this.Paint += new PaintEventHandler(Form1_Paint);
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            // Actualiza las etiquetas con la información inicial
            ActualizarLabels();
        }

        private void Movimientoenemigo_Tick(Enemigos enemigo)
        {
            enemigo.MoveRandom();
            ActualizarLabels();
            this.Refresh();
        }
        private void EliminarEnemigo(Enemigos enemigo)
        {
            // Detener el temporizador del enemigo
            var timer = timersEnemigos[enemigos.IndexOf(enemigo)];
            timer.Stop();

            // Eliminar el enemigo de la lista de enemigos y timers
            enemigos.Remove(enemigo);
            timersEnemigos.Remove(timer);
        }

        private int IntervaloVelocidad(int velocidad)
        {
            // Aquí calculas los milisegundos por nodo según la velocidad
            // Velocidad 10 es 4 nodos por segundo (1000ms / 4 = 250ms por nodo)
            return (int)(1000 / (2.2 + (velocidad - 2) * 0.2));
        }
        private void VerColision(Moto moto1, Moto moto2)
        {
            if (moto1.RPosActual() == moto2.RPosActual())
            {
                // Si dos motos colisionan en la misma posición
                moto1.Destruir();
                moto2.Destruir();

                // Si uno de los participantes es un enemigo, elimínalo
                if (moto1 is Enemigos)
                {
                    EliminarEnemigo((Enemigos)moto1);
                }

                if (moto2 is Enemigos)
                {
                    EliminarEnemigo((Enemigos)moto2);
                }
            }
        }
        private void MovimientoJugador_Tick(object sender, EventArgs e)
        {
            // Si el jugador está destruido, terminar el juego
            if (jugador.Destruido)
            {
                GameOvermsg();  //Mostrar mensaje si el jugador está destruido
                return;
            }

            // Verificar la destrucción de enemigos y eliminarlos
            foreach (var enemigo in enemigos.ToList()) // Usamos ToList() para evitar problemas al modificar la lista durante la iteración
            {
                if (enemigo.Destruido)
                {
                    EliminarEnemigo(enemigo); // Elimina el enemigo si está destruido
                    continue; // Pasar al siguiente enemigo
                }

                // Movimiento del enemigo
                enemigo.MoveRandom();
            }

            // Movimiento del jugador basado en la última dirección
            if (lastDirection != Keys.None)
            {
                switch (lastDirection)
                {
                    case Keys.W:
                        jugador.MoverArriba();
                        break;
                    case Keys.S:
                        jugador.MoverAbajo();
                        break;
                    case Keys.A:
                        jugador.MoverIzquierda();
                        break;
                    case Keys.D:
                        jugador.MoverDerecha();
                        break;
                }
            }

            // Actualizar las etiquetas de información y redibujar
            ActualizarLabels();
            this.Refresh();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Guardar la última dirección de movimiento
            lastDirection = e.KeyCode;
        }

        private void ActualizarLabels()
        {
            // Actualizar la etiqueta de velocidad del jugador
            lblVelocidadJugador.Text = $"Velocidad Jugador: {jugador.velocidad}";

            // Actualizar la etiqueta de combustible del jugador
            lblCombustibleJugador.Text = $"Combustible: {jugador.combustible}";

            // Actualizar la etiqueta de velocidad de los enemigos (intervalo)
            var velocidadesEnemigos = string.Join(", ", enemigos.Select(e => e.velocidad));
            lblVelocidadEnemigos.Text = $"Velocidad Enemigos: {velocidadesEnemigos}";
        }
        public void GameOvermsg()
        {
            using (Graphics g = CreateGraphics())
            {
                Font font = new Font("Arial", 48, FontStyle.Bold);
                Brush brush = Brushes.Red;
                string message = "GAME OVER";

                // Obtener el tamaño del texto
                SizeF textSize = g.MeasureString(message, font);

                // Calcular la posición para centrar el texto
                float x = (ClientSize.Width - textSize.Width) / 2;
                float y = (ClientSize.Height - textSize.Height) / 2;

                // Dibujar el mensaje en rojo
                g.DrawString(message, font, brush, x, y);
            }

            // Pausar el hilo actual durante 2 segundos (2000 ms)
            System.Threading.Thread.Sleep(2000);

            // Cerrar la aplicación después de 2 segundos
            Application.Exit();
        }
        private void RespawnItems()
        {
            List<Nodo> availableNodes = NodosDisponibles(); // Obtener nodos disponibles
            Random rand = new Random();

            // Crear 3 ítems (FuelCell, TrailGrowth, Bomb)
            if (availableNodes.Count >= 3)
            {
                Nodo fuelNode = availableNodes[rand.Next(availableNodes.Count)];
                fuelNode.Data = "FuelCell";
                availableNodes.Remove(fuelNode);

                Nodo trailNode = availableNodes[rand.Next(availableNodes.Count)];
                trailNode.Data = "TrailGrowth";
                availableNodes.Remove(trailNode);

                Nodo bombNode = availableNodes[rand.Next(availableNodes.Count)];
                bombNode.Data = "Bomb";
                availableNodes.Remove(bombNode);
            }

            // Crear 2 poderes (Shield, HyperSpeed)
            if (availableNodes.Count >= 2)
            {
                Nodo shieldNode = availableNodes[rand.Next(availableNodes.Count)];
                shieldNode.Data = "Shield";
                availableNodes.Remove(shieldNode);

                Nodo speedNode = availableNodes[rand.Next(availableNodes.Count)];
                speedNode.Data = "HyperSpeed";
                availableNodes.Remove(speedNode);
            }

            this.Refresh(); // Refrescar la pantalla para dibujar los ítems y poderes
        }
        private List<Nodo> NodosDisponibles()
        {
            List<Nodo> availableNodes = new List<Nodo>();

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    Nodo node = linkedList.GetNode(i, j);

                    // Verifica si el nodo está ocupado por el jugador o enemigos
                    bool isOccupied = jugador.RPosActual() == node || jugador.GetTrail().Contains(node);

                    foreach (var enemigo in enemigos)
                    {
                        if (enemigo.RPosActual() == node || enemigo.GetTrail().Contains(node))
                        {
                            isOccupied = true;
                            break;
                        }
                    }

                    if (!isOccupied)
                    {
                        availableNodes.Add(node); // Añadir nodos vacíos a la lista
                    }
                }
            }

            return availableNodes;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            int panelHeight = 80;
            int matrizSize = 600;
            int cellSize = matrizSize / GridSize;

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    Nodo node = linkedList.GetNode(i, j);
                    Rectangle rect = new Rectangle(j * cellSize, panelHeight + i * cellSize, cellSize, cellSize);

                    // Dibuja la estela del jugador en amarillo
                    if (jugador.GetTrail().Contains(node))
                    {
                        g.FillRectangle(Brushes.Yellow, rect);
                    }
                    else if (node == jugador.RPosActual())
                    {
                        g.FillRectangle(Brushes.Red, rect);
                    }
                    // Dibuja los ítems
                    else if (node.Data == "FuelCell")
                    {
                        g.FillRectangle(Brushes.Purple, rect);
                    }
                    else if (node.Data == "TrailGrowth")
                    {
                        g.FillRectangle(Brushes.Green, rect);
                    }
                    else if (node.Data == "Bomb")
                    {
                        g.FillRectangle(Brushes.Black, rect);
                    }
                    // Dibuja los poderes
                    else if (node.Data == "Shield")
                    {
                        g.FillRectangle(Brushes.Cyan, rect); // Shield en color cyan
                    }
                    else if (node.Data == "HyperSpeed")
                    {
                        g.FillRectangle(Brushes.Orange, rect); // HyperSpeed en color naranja
                    }
                    else
                    {
                        // Lógica para dibujar enemigos o nodos vacíos
                        bool isEnemyTrail = false;
                        foreach (var enemigo in enemigos)
                        {
                            if (enemigo.GetTrail().Contains(node))
                            {
                                g.FillRectangle(Brushes.Blue, rect);
                                isEnemyTrail = true;
                                break;
                            }
                            else if (node == enemigo.RPosActual())
                            {
                                g.FillRectangle(Brushes.Green, rect);
                                isEnemyTrail = true;
                                break;
                            }
                        }

                        if (!isEnemyTrail)
                        {
                            g.FillRectangle(Brushes.Black, rect);
                        }
                    }

                    // Dibujar bordes de las celdas
                    g.DrawRectangle(Pens.Gray, rect);
                }
            }
        }
    }      
    
}
