using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
namespace kepiting_pelit_gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public bool IsFileValid(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            int rowCount = lines.Length;
            int colCount = -1; // initial value for column count

            foreach (var line in lines)
            {
                if (colCount == -1)
                {
                    // First line, set the column count
                    colCount = line.Length;
                }
                else if (line.Length != colCount)
                {
                    // Column count doesn't match with previous lines
                    return false;
                }

                // Check if each character in the line is valid, including spaces
                foreach (var c in line)
                {
                    if (c != 'K' && c != 'X' && c != 'R' && c != 'T' && c != ' ')
                    {
                        // Invalid character found
                        return false;
                    }
                }
            }

            return true;
        }


        private void Inputf_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            ofd.Filter = "Text files (*.txt)|*.txt"; 
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog()  == true)
            {
                string nameF = ofd.FileName;
                if (IsFileValid(nameF))
                {
                    // Show selected file name
                    string selectedFileName = ofd.FileName;
                    string namaF = System.IO.Path.GetFileName(selectedFileName);
                    FileInput.Text = namaF;
                    MessageBox.Show("File berhasil diunggah: " + namaF);
                }
                else
                {
                    // Show error message
                    MessageBox.Show("File tidak sesuai ketentuan! Harus berisi karakter 'k', 'r', 'x', 't' dan spasi saja.");
                }
            }
        }

        bool show = true;
        private void Showm_Click(object sender, RoutedEventArgs e)
        {
            if(show)
            {
                Showm.Content = "unshow maze!";
            }
            else
            {
                Showm.Content = "Show Maze!";
            }
            show = !show;
        }

        private void Solvem_Click(object sender, RoutedEventArgs e)
        {
            // Check if a file has been selected
            if (FileInput.Text != "")
            {
                // Read the maze from the selected file
                string[] lines = File.ReadAllLines(FileInput.Text);
                int rowCount = lines.Length;
                int colCount = lines[0].Length;
                char[,] maze = new char[rowCount, colCount];

                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        maze[i, j] = lines[i][j];
                    }
                }

                // Create a new instance of the MazeSolver class
                //MazeSolver mazeSolver = new MazeSolver(maze);

                // Solve the maze using DFS or BFS
                if (show)
                {
                    if (DFS.IsChecked == true)
                    {
                        // Solve the maze using DFS
                       // List<Tuple<int, int>> path = mazeSolver.SolveDFS();

                        // Show the solved maze
                       // mazeCanvas.Children.Clear();
                       // mazeCanvas.Children.Add(CreateMazeGrid(maze, path));
                    }
                    else if (BFS.IsChecked == true)
                    {
                        // Solve the maze using BFS
                        //List<Tuple<int, int>> path = mazeSolver.SolveBFS();

                        // Show the solved maze
                        //mazeCanvas.Children.Clear();
                        //mazeCanvas.Children.Add(CreateMazeGrid(maze, path));
                    }
                }
            }
        }

    }
}
