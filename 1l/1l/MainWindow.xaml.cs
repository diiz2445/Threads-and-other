using _1l.Core_WPF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace _1l
{
    public partial class MainWindow : Window
    {
        List<List<double>> matrix_list;
        double[,] matrix;
        public MainWindow()
        {
            InitializeComponent();
            List<List<double>> matrix;
            MatrixDataGrid.Visibility = Visibility.Hidden;
        }
        private void CreateColumns(List<List<double>> matrix)
        {
            if (matrix.Count==0) return;

            MatrixDataGrid.Columns.Clear(); // очищаем предыдущие столбцы, чтобы избежать их дублирования

            for (int i = 0; i < matrix[0].Count; i++) // изменил цикл на количество столбцов
            {
                var column = new DataGridTextColumn
                {
                    Binding = new Binding($"[{i}]")
                };
                MatrixDataGrid.Columns.Add(column);
            }

            MatrixDataGrid.ItemsSource = matrix;
        }
        
        private void CreateColumns_multiply(List<List<double>> matrix)
        {
            if (matrix.Count == 0) return;

            MatrixDataGrid_multi.Columns.Clear(); // очищаем предыдущие столбцы, чтобы избежать их дублирования

            for (int i = 0; i < matrix[0].Count; i++) // изменил цикл на количество столбцов
            {
                var column = new DataGridTextColumn
                {
                    Binding = new Binding($"[{i}]")
                };
                MatrixDataGrid_multi.Columns.Add(column);
            }

            MatrixDataGrid_multi.ItemsSource = matrix;
        }
        private void CreateColumns_sort(List<List<double>> matrix)
        {
            if (matrix.Count == 0) return;

            MatrixDataGrid_sorted.Columns.Clear(); // очищаем предыдущие столбцы, чтобы избежать их дублирования

            for (int i = 0; i < matrix[0].Count; i++) // изменил цикл на количество столбцов
            {
                var column = new DataGridTextColumn
                {
                    Binding = new Binding($"[{i}]")
                };
                MatrixDataGrid_sorted.Columns.Add(column);
            }

            MatrixDataGrid_sorted.ItemsSource = matrix;
        }
        private void GenerateMatrix_Click(object sender, RoutedEventArgs e)
        {
            // Получаем значения из полей ввода
            int rows = int.Parse(RowsInput.Text);       // количество строк
            int columns = int.Parse(ColumnsInput.Text); // количество столбцов
            int threads = int.Parse(ThreadsInput.Text); // количество потоков (можно не использовать в примере)

            matrix_list = new List<List<double>>();
            matrix = Threads.fill_matrix(rows, columns, threads);
            matrix_list = MatrixViewModel.GenerateMatrix(RowsInput.Text, ColumnsInput.Text, ThreadsInput.Text);
           
            MatrixDataGrid.CanUserAddRows = false; // отключаем возможность добавления новой строки
            CreateColumns(matrix_list);
            CreateColumns_sort(matrix_list);
            MatrixDataGrid.ItemsSource = matrix_list;
            MatrixDataGrid_sorted.ItemsSource= MatrixViewModel.sorted_list(RowsInput.Text, ColumnsInput.Text, ThreadsInput.Text);
            MatrixDataGrid.Visibility = Visibility.Visible;
            MatrixDataGrid_sorted.Visibility = Visibility.Visible;
        }

        private void multiply_Click(object sender, RoutedEventArgs e)
        {
            MatrixDataGrid.CanUserAddRows = false; // отключаем возможность добавления новой строки
            CreateColumns_multiply(matrix_list);

            MatrixDataGrid_multi.ItemsSource = MatrixViewModel.multiply_list(matrix, Multiply.Text);
            MatrixDataGrid_multi.Visibility = Visibility.Visible;

        }
        
        
        
    }
}