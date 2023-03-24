using System;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;

namespace TreasureHunt
{
    public partial class Form1 : Form
    {
        public char[,] grid;
        public Label labelFileName;
        private Button searchButton;
        private int gridSize;
        private int cellSize = 30;
        private bool[,] maze;
        private int startX;
        private int startY;
        private int currentX;
        private int currentY;
        private List<(int, int)> treasuresCoord;
        private List<(int, int)> jellyFishCoord;
        private Bitmap krab;
        private Bitmap jellyFish;
        private Bitmap treasure;
        private Bitmap krusty;
        private Label labelRoute = new Label();
        private Label labelNodes = new Label();
        private Label labelSteps = new Label();
        private Label labelExTime = new Label();

        public Form1()
        {
            InitializeComponent();
            labelFileName = new Label();
            labelFileName.Location = new Point(12, 119);
            labelFileName.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            labelFileName.AutoSize = true;

            grid = new char[0, 0];
            maze = new bool[0, 0];
            krab = new Bitmap("img/krab.png");
            jellyFish = new Bitmap("img/jellyfish.png");
            treasure = new Bitmap("img/treasure.png");
            krusty = new Bitmap("img/krusty.png");

            treasuresCoord = new List<(int, int)>();
            jellyFishCoord = new List<(int, int)>();

            searchButton = new Button();
            this.BackgroundImage = Image.FromFile("img/bg.png");
            this.Icon = new Icon("img/krusty.ico");
        }

        private void GenerateMaze()
        {
            maze = new bool[grid.GetLength(1), grid.GetLength(0)];
            for (int i = 0; i < grid.GetLength(1); i++)
                for (int j = 0; j < grid.GetLength(0); j++)
                    maze[i, j] = false;
            maze[currentX, currentY] = true;
        }

        private async void UpdateMaze(string inputString)
        {
            foreach (char inputChar in inputString)
            {
                switch (inputChar)
                {
                    case 'L':
                        if (currentX > 0) currentX--;
                        break;
                    case 'R':
                        if (currentX < grid.GetLength(1) - 1) currentX++;
                        break;
                    case 'U':
                        if (currentY > 0) currentY--;
                        break;
                    case 'D':
                        if (currentY < grid.GetLength(0) - 1) currentY++;
                        break;
                }
                maze[currentX, currentY] = true;
                this.Invalidate();
                await Task.Delay(500); // Add a delay
            }
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int i = 0; i < grid.GetLength(1); i++)
                for (int j = 0; j < grid.GetLength(0); j++)
                {
                    if (maze[i, j] == true)
                        g.FillRectangle(Brushes.Yellow, i * cellSize + 197, j * cellSize + 56, cellSize, cellSize);
                    else
                        g.FillRectangle(Brushes.White, i * cellSize + 197, j * cellSize + 56, cellSize, cellSize);
                    g.DrawRectangle(Pens.Black, i * cellSize + 197, j * cellSize + 56, cellSize, cellSize);
                }
            g.DrawImage(krusty, startX * cellSize + 197, startY * cellSize + 56, cellSize, cellSize);
            drawTreasure(g, cellSize);
            drawJellyFish(g, cellSize);
            g.DrawImage(krab, currentX * cellSize + 197, currentY * cellSize + 56, cellSize, cellSize);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.Controls.Contains(labelFileName))
            {
                this.Controls.Remove(labelFileName);
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.txt";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                string fileName = Path.GetFileName(path);
                labelFileName.Text = fileName;
                labelFileName.BackColor = Color.Transparent;
                this.Controls.Add(labelFileName);
                grid = GetFile.GetGrid(path);
                if (!GridOperations.IsValidGrid(grid))
                {
                    MessageBox.Show("Grid contains non-valid characters.\nPlease input valid grid!");
                }
                treasuresCoord = GridOperations.getTreasuresCoord(grid);
                jellyFishCoord = GridOperations.getJellyFishCoord(grid);
                startX = GridOperations.getStartCoord(grid).Item2;
                startY = GridOperations.getStartCoord(grid).Item1;
                currentX = startX;
                currentY = startY;
                gridSize = grid.GetLength(0);

                GenerateMaze();
                this.Paint += new PaintEventHandler(Form1_Paint);
            }
        }


        private void drawTreasure(Graphics g, int cellSize)
        {
            for (int i = 0; i < treasuresCoord.Count; i++)
            {
                g.DrawImage(treasure, treasuresCoord[i].Item2 * cellSize + 197, treasuresCoord[i].Item1 * cellSize + 56, cellSize, cellSize);
            }
        }

        private void drawJellyFish(Graphics g, int cellSize)
        {
            for (int i = 0; i < jellyFishCoord.Count; i++)
            {
                g.FillRectangle(Brushes.Black, jellyFishCoord[i].Item2 * cellSize + 197, jellyFishCoord[i].Item1 * cellSize + 56, cellSize, cellSize);
                g.DrawImage(jellyFish, jellyFishCoord[i].Item2 * cellSize + 197, jellyFishCoord[i].Item1 * cellSize + 56, cellSize, cellSize);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (grid == null)
            {
                MessageBox.Show("Please select a correct .txt file.");
            }
            else if (!GridOperations.IsValidGrid(grid))
            {
                MessageBox.Show("Grid contains non-valid characters.\nPlease input valid grid!");
            }
            else if (!radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("Please select one of the algorithms.");
            }
            else if (radioButton1.Checked || radioButton2.Checked)
            {
                UpdateMaze(" ");
                this.Controls.Remove(searchButton);
                this.Controls.Remove(labelRoute); this.Controls.Remove(labelNodes);
                this.Controls.Remove(labelSteps); this.Controls.Remove(labelExTime);
                searchButton = new Button();
                searchButton.Text = "Search";
                searchButton.Location = new Point((grid.GetLength(1) / 2 - 1) * cellSize + 197, grid.GetLength(0) * cellSize + 65);
                searchButton.AutoSize = true;
                searchButton.Click += searchButton_Click;
                this.Controls.Add(searchButton);
            }
        }

        private void searchButton_Click(object? sender, EventArgs e)
        {
            string shortestRoute;
            int nodes;
            long exTime;
            if (radioButton1.Checked)
            {
                var treasureHunter = new TreasureHunterBFS(grid);
                shortestRoute = treasureHunter.FindShortestRoute();
                nodes = treasureHunter.countNodes;
                exTime = treasureHunter.elapsedTime.ElapsedMilliseconds;
            }
            else
            {
                var treasureHunter = new TreasureHunterDFS(grid);
                shortestRoute = treasureHunter.FindShortestRoute();
                nodes = treasureHunter.GetNodes();
                exTime = treasureHunter.GetElapsedTime();
            }
            this.Controls.Remove(labelRoute); this.Controls.Remove(labelNodes);
            this.Controls.Remove(labelSteps); this.Controls.Remove(labelExTime);
            labelRoute.AutoSize = true;
            labelNodes.AutoSize = true;
            labelSteps.AutoSize = true;
            labelExTime.AutoSize = true;
            labelRoute.BackColor = Color.Transparent;
            labelNodes.BackColor = Color.Transparent;
            labelSteps.BackColor = Color.Transparent;
            labelExTime.BackColor = Color.Transparent;
            labelRoute.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);
            labelNodes.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);
            labelSteps.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);
            labelExTime.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);
            labelRoute.Text = "Route: " + string.Join("-", shortestRoute.ToCharArray());
            labelNodes.Text = "Nodes: " + nodes;
            labelSteps.Text = "Steps: " + shortestRoute.Length;
            labelExTime.Text = "Execution Time: " + exTime + " ms";
            labelRoute.Location = new Point(197, searchButton.Location.Y + searchButton.Height + 10);
            labelNodes.Location = new Point(labelRoute.Location.X, labelRoute.Location.Y + labelRoute.Height + 10);
            labelSteps.Location = new Point(labelRoute.Location.X + labelRoute.Width + 175, labelRoute.Location.Y);
            labelExTime.Location = new Point(labelSteps.Location.X, labelSteps.Location.Y + labelSteps.Height + 10);
            this.Controls.Add(labelRoute);
            this.Controls.Add(labelNodes);
            this.Controls.Add(labelSteps);
            this.Controls.Add(labelExTime);
            UpdateMaze(shortestRoute);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}