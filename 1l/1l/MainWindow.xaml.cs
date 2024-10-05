using _1l.Core_WPF;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            double[,] matrixData = Threads.fill_matrix(10, 10, 2);
            var matrixList =MatrixViewModel.ConvertToList(matrixData);

            var viewModel = new MatrixViewModel(matrixData);
            DataContext = viewModel;

            CreateColumns(viewModel.Matrix);
        }


        private void CreateColumns(List<List<double>> matrix)
        {
            if (matrix.Count==0) return;

            for (int i = 0; i < matrix.Count; i++)
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