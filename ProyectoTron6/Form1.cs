using System.Collections.Generic;

namespace ProyectoTron6
{
    public partial class Form1 : Form
    {
        private Matriz linkedList;
        private Jugador jugador;
        private List<Enemigos> enemigos;
        private const int GridSize = 30; //Tamaño de la matriz 30x30
        private System.Windows.Forms.Timer MovimientoJugador;
        private Keys lastDirection;
        private List<System.Windows.Forms.Timer> timersEnemigos;
        private System.Windows.Forms.Timer ItemUsageTimer;
        //Variables para mostrar la información del jugador y enemigos
        private Label lblVelocidadJugador;
        private Label lblCombustibleJugador;
        private Label lblVelocidadEnemigos;
        private Label lblEstelaJugador;
        private Label lblPoderJugador;
        //Imagenes de los objetos
        private Image imgcombustible;
        private Image imgincremento;
        private Image imgbomba;
        private Image imgescudo;
        private Image imgacelerar;

        private Panel poderesPanel;
        private List<PictureBox> poderesPictureBoxes = new List<PictureBox>();


        private Keys currentDirection = Keys.D;

        public Form1()
        {
            InitializeComponent();

            // Altura reservada para el área de visualización de datos
            int panelHeight = 80; //Altura del área superior de datos
            int matrizSize = 600; //Tamaño fijo de la matriz cuadrada (30x30)

            // Establece el tamaño total de la ventana: altura = matriz + panelHeight
            this.ClientSize = new Size(matrizSize, matrizSize + panelHeight);
            this.MinimumSize = this.ClientSize; // Evitar que la ventana se haga más pequeña que el tamaño necesario
            //Configura el fondo negro para el formulario completo
            this.BackColor = Color.Black;

            //Imagenes cargadas
            imgcombustible = Image.FromFile("Resources/combustible.png");
            imgincremento = Image.FromFile("Resources/incremento.png");
            imgbomba = Image.FromFile("Resources/bomba.png");
            imgescudo = Image.FromFile("Resources/escudo.png");
            imgacelerar = Image.FromFile("Resources/acelerar.png");

            //panel para los poderes
            poderesPanel = new Panel();
            poderesPanel.Location = new Point(400, 10); // Posición en la esquina superior derecha
            poderesPanel.Size = new Size(180, 40); // Tamaño del panel
            poderesPanel.BackColor = Color.Black;
            this.Controls.Add(poderesPanel); // Añadir el panel al formulario


            //etiquetas para la información del jugador y enemigos
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

            lblPoderJugador = new Label();
            lblPoderJugador.Text = "Poder: 100";
            lblPoderJugador.ForeColor = Color.White;
            lblPoderJugador.BackColor = Color.Black; // Fondo negro para la etiqueta
            lblPoderJugador.Location = new Point(140, 30); // Posición de la etiqueta
            lblPoderJugador.AutoSize = true;

            lblEstelaJugador = new Label();
            lblEstelaJugador.Text = "tamaño estela: pinga";
            lblEstelaJugador.ForeColor = Color.White;
            lblEstelaJugador.BackColor = Color.Black; // Fondo negro para la etiqueta
            lblEstelaJugador.Location = new Point(140, 10); // Posición de la etiqueta
            lblEstelaJugador.AutoSize = true;

            lblVelocidadEnemigos = new Label();
            lblVelocidadEnemigos.Text = "Velocidad Enemigos: ";
            lblVelocidadEnemigos.ForeColor = Color.White;
            lblVelocidadEnemigos.BackColor = Color.Black; // Fondo negro para la etiqueta
            lblVelocidadEnemigos.Location = new Point(10, 50); // Posición de la etiqueta
            lblVelocidadEnemigos.AutoSize = true;

            //etiquetas para el formulario
            this.Controls.Add(lblVelocidadJugador);
            this.Controls.Add(lblCombustibleJugador);
            this.Controls.Add(lblVelocidadEnemigos);
            this.Controls.Add(lblEstelaJugador);
            this.Controls.Add(lblPoderJugador);


            this.DoubleBuffered = true; //Activa el doble buffering para evitar parpadeos

            linkedList = new Matriz(GridSize, GridSize);
            jugador = new Jugador(linkedList.GetNode(GridSize / 2, GridSize / 2)); // Moto inicia en el centro de la matriz
            enemigos = new List<Enemigos>();

            //Inicializa lista de timers
            timersEnemigos = new List<System.Windows.Forms.Timer>();

            //Configura temporizador para el uso de ítems
            ItemUsageTimer = new System.Windows.Forms.Timer();
            ItemUsageTimer.Interval = 1000; // 1 segundo
            ItemUsageTimer.Tick += ItemUsageTimer_Tick;
            ItemUsageTimer.Start();

            RespawnItems();

            //Creación de 3 motos enemigas con posiciones únicas
            for (int i = 0; i < 3; i++)
            {
                Nodo enemyStartNode;
                do
                {
                    enemyStartNode = linkedList.GetNode(new Random().Next(0, GridSize), new Random().Next(0, GridSize));
                } while (enemyStartNode == jugador.RPosActual() || enemigos.Any(e => e.RPosActual() == enemyStartNode));

                var enemigo = new Enemigos(enemyStartNode);
                enemigos.Add(enemigo);

                //Configura el temporizador para cada enemigo con su respectiva velocidad
                var timerEnemigo = new System.Windows.Forms.Timer();
                timerEnemigo.Interval = IntervaloVelocidad(enemigo.velocidad);
                timerEnemigo.Tick += (sender, e) => Movimientoenemigo_Tick(enemigo);
                timerEnemigo.Start();

                timersEnemigos.Add(timerEnemigo);
            }


            //Configura temporizador para movimiento del jugador
            MovimientoJugador = new System.Windows.Forms.Timer();
            MovimientoJugador.Interval = IntervaloVelocidad(jugador.velocidad); //Inicializa con la velocidad del jugador
            MovimientoJugador.Tick += MovimientoJugador_Tick;
            MovimientoJugador.Start();

            this.Paint += new PaintEventHandler(Form1_Paint);
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            // Actualiza las etiquetas con la información inicial
            ActualizarLabels();
            ActualizarPoderes();
        }
        private void ItemUsageTimer_Tick(object sender, EventArgs e)
        {
            //Procesa solo ítems en la cola de jugador
            if (jugador.itemQueue.Contador > 0)
            {
                var item = jugador.itemQueue.Peek();  //Verificamos el primer ítem

                //Verificamos si es un ítem y no un poder
                if (item is Item)
                {
                    jugador.itemQueue.Dequeue().Aplicar(jugador);  //Aplica el ítem a la moto del jugador
                    ActualizarLabels();  //Actualiza las etiquetas de combustible, velocidad, etc.
                    ActualizarPoderes();
                }
            }
        }


        private void ActualizarPoderes()
        {
            // Limpia los controles existentes
            poderesPanel.Controls.Clear();

            // Usa el método Recorrer para iterar sobre los poderes en la pila
            jugador.poderStack.Recorrer(poder =>
            {
                PictureBox poderPictureBox = new PictureBox();
                poderPictureBox.Size = new Size(40, 40); // Tamaño del PictureBox
                poderPictureBox.Location = new Point(poderesPanel.Controls.Count * 45, 0); // Posiciona cada imagen
                poderPictureBox.SizeMode = PictureBoxSizeMode.StretchImage; // Ajuste de la imagen al tamaño del PictureBox

                // Asigna la imagen según el tipo de poder
                if (poder is Escudo)
                {
                    poderPictureBox.Image = imgescudo;
                }
                else if (poder is HiperVelocidad)
                {
                    poderPictureBox.Image = imgacelerar;
                }

                // Añade el PictureBox al panel
                poderesPanel.Controls.Add(poderPictureBox);
            });
        }
        private void UsarPoder()
        {
            if (jugador.poderStack.Count > 0)
            {
                var poder = jugador.poderStack.Pop();
                poder.Activar(jugador);

                //Actualiza la visualización de poderes
                ActualizarPoderes();
            }
        }
        private void Movimientoenemigo_Tick(Enemigos enemigo)
        {
            enemigo.MoveRandom();
            ActualizarLabels();
            ActualizarPoderes();
            this.Refresh();
        }
        private void EliminarEnemigo(Enemigos enemigo)
        {
            //Detiene el temporizador del enemigo
            var timer = timersEnemigos[enemigos.IndexOf(enemigo)];
            timer.Stop();

            //Elimina el enemigo de la lista de enemigos y timers
            enemigos.Remove(enemigo);
            timersEnemigos.Remove(timer);
        }

        private int IntervaloVelocidad(int velocidad)
        {
            //Aquí se calcula los milisegundos por nodo según la velocidad o nodos por segundo.
            //Velocidad 10 es 4 nodos por segundo (1000ms / 4 = 250ms por nodo)
            return (int)(1000 / (2.2 + (velocidad - 2) * 0.2));
        }
        private void VerColision(Moto moto1, Moto moto2)
        {
            if (moto1.RPosActual() == moto2.RPosActual())
            {
                //Si dos motos colisionan en la misma posición
                moto1.Destruir();
                moto2.Destruir();

                //Si uno de los participantes es un enemigo, elimínalo
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
            //Si el jugador está destruido, terminar el juego
            if (jugador.Destruido)
            {
                GameOvermsg();  // Mostrar mensaje si el jugador está destruido
                return;
            }

            //Verifica la destrucción de enemigos y eliminarlos
            foreach (var enemigo in enemigos.ToList()) // Usamos ToList() para evitar problemas al modificar la lista durante la iteración
            {
                if (enemigo.Destruido)
                {
                    EliminarEnemigo(enemigo); //Elimina el enemigo si está destruido
                    continue; //Pasa al siguiente enemigo
                }

                //Movimiento del enemigo
                enemigo.MoveRandom();
            }

            //Movimiento del jugador basado en la última dirección válida (currentDirection)
            switch (currentDirection)
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

            //Actualiza las etiquetas de información y redibujar
            ActualizarLabels();
            ActualizarPoderes();
            this.Refresh();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //Solo cambiar la dirección si no es opuesta a la actual
            switch (e.KeyCode)
            {
                case Keys.W:
                    if (currentDirection != Keys.S)  //No permite giro de 180 grados
                        currentDirection = Keys.W;
                    break;
                case Keys.S:
                    if (currentDirection != Keys.W)
                        currentDirection = Keys.S;
                    break;
                case Keys.A:
                    if (currentDirection != Keys.D)
                        currentDirection = Keys.A;
                    break;
                case Keys.D:
                    if (currentDirection != Keys.A)
                        currentDirection = Keys.D;
                    break;

                //Usa el poder manualmente (ejemplo con tecla Q)
                case Keys.Q:
                     UsarPoder();
                    break;

                //Cambiar poder seleccionado (ejemplo con tecla E)
                case Keys.E:
                     //aca iria cambiar poder si estuviera implementado xdd
                    break;
            }
        }

        private void ActualizarLabels()
        {
            //Actualiza la etiqueta de velocidad del jugador
            lblVelocidadJugador.Text = $"Velocidad Jugador: {jugador.velocidad}";

            // Actualiza la etiqueta de combustible del jugador
            lblCombustibleJugador.Text = $"Combustible: {jugador.combustible}";

            lblEstelaJugador.Text = $"Tamaño de la estela: {jugador.tamañoestela}";

            lblPoderJugador.Text = $"Poderes: {jugador.poderStack.Count}";

           //Actualiza la etiqueta de velocidad de los enemigos (intervalo)
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

                //tamaño del texto
                SizeF textSize = g.MeasureString(message, font);

                //posición para centrar el texto
                float x = (ClientSize.Width - textSize.Width) / 2;
                float y = (ClientSize.Height - textSize.Height) / 2;

                //Dibujar el mensaje en rojo
                g.DrawString(message, font, brush, x, y);
            }

            //Pausa el hilo actual durante 2 segundos (2000 ms)
            System.Threading.Thread.Sleep(2000);

            //Cerrar la aplicación después de 2 segundos
            Application.Exit();
        }
        private void RespawnItems()
        {
            List<Nodo> availableNodes = NodosDisponibles(); 
            Random rand = new Random();
            int cantidadItems = 10; //variable para la cantidad de ítems a generar

            if (availableNodes.Count >= cantidadItems)
            {
                for (int i = 0; i < cantidadItems; i++)
                {
                    //Elige un nodo al azar para colocar el ítem
                    Nodo selectedNode = availableNodes[rand.Next(availableNodes.Count)];
                    availableNodes.Remove(selectedNode); //Remueve el nodo para que no respawnee otro item encima

                    //Asigna ítems basados en el índice 'i' o al azar
                    switch (i % 5) //Se puede ajustar para variarlo
                    {
                        case 0:
                            selectedNode.Data = "Combustible";
                            break;
                        case 1:
                            selectedNode.Data = "Incremento";
                            break;
                        case 2:
                            selectedNode.Data = "Bomba";
                            break;
                        case 3:
                            selectedNode.Data = "Escudo";

                            break;
                        case 4:
                            selectedNode.Data = "Acelerar";
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("No hay suficientes nodos disponibles para generar todos los ítems.");
            }

            this.Refresh(); 
        }
    
        private List<Nodo> NodosDisponibles()
        {
            List<Nodo> availableNodes = new List<Nodo>();

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    Nodo node = linkedList.GetNode(i, j);

                    //Verifica si el nodo está ocupado por el jugador o enemigos
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
                        availableNodes.Add(node); //Añade los nodos vacíos a la lista
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

                    //Dibuja la estela del jugador
                    if (jugador.GetTrail().Contains(node))
                    {
                        g.FillRectangle(Brushes.Yellow, rect);
                    }
                    else if (node == jugador.RPosActual())
                    {
                        g.FillRectangle(Brushes.Red, rect);
                    }
                    //Dibuja los ítems como imágenes
                    else if (node.Data == "Combustible")
                    {
                        g.DrawImage(imgcombustible, rect);
                    }
                    else if (node.Data == "Incremento")
                    {
                        g.DrawImage(imgincremento, rect);
                    }
                    else if (node.Data == "Bomba")
                    {
                        g.DrawImage(imgbomba, rect);
                    }
                    else if (node.Data == "Escudo")
                    {
                        g.DrawImage(imgescudo, rect);
                    }
                    else if (node.Data == "Acelerar")
                    {
                        g.DrawImage(imgacelerar, rect);
                    }
                    else
                    {
                        //Lógica para dibujar enemigos o nodos vacíos
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

                    //Dibuja los bordes de las celdas
                    g.DrawRectangle(Pens.Gray, rect);
                }
            }
        }
    }      
    
}
