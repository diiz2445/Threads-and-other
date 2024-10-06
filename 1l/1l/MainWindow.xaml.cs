using _1l.Core_WPF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace _1l
{
    public partial class MainWindow : Window
    {
        List<List<double>> matrix;

        public MainWindow()
        {
            InitializeComponent();
            LoadMatrix();
            List<List<double>> matrix;
        }
        private void LoadMatrix()
        {
            // Пример матрицы произвольного размера
            double[,] matrixData = Threads.fill_matrix(11, 12, 2);
            var matrixList =MatrixViewModel.ConvertToList(matrixData);

            var viewModel = new MatrixViewModel(matrixData);
            DataContext = viewModel;

            CreateColumns(viewModel.Matrix);
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
        private void GenerateMatrix_Click(object sender, RoutedEventArgs e)
        {
            // Получаем значения из полей ввода
            int rows = int.Parse(RowsInput.Text);       // количество строк
            int columns = int.Parse(ColumnsInput.Text); // количество столбцов
            int threads = int.Parse(ThreadsInput.Text); // количество потоков (можно не использовать в примере)

            matrix = new List<List<double>>();
            matrix = MatrixViewModel.GenerateMatrix(RowsInput.Text, ColumnsInput.Text, ThreadsInput.Text);
            // Создаем ViewModel
            //var viewModel = new MatrixViewModel(matrixData);
            
            MatrixDataGrid.CanUserAddRows = false; // отключаем возможность добавления новой строки
            CreateColumns(matrix);

            // Устанавливаем источник данных для DataGrid
            MatrixDataGrid.ItemsSource = matrix;
        }
    }
}