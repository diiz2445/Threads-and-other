using _1l.Core_WPF;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace _1l
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadMatrix();
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
    }
}