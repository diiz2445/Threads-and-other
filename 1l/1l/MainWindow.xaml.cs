using _1l.Core_WPF;
using System.Reflection.Emit;
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
        public Tree RootTree { get; set; }
        string Visibility_Matrix = "Visible";
        string Visibility_Tree = "Hidden";
        public MainWindow()
        {
            InitializeComponent();
            List<List<double>> matrix;
            string input = "10 5 15 3 7 12 18";

            
            DataContext = this;

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

        private void Generate_TreeClick(object sender, RoutedEventArgs e)
        {
            string input = InputTextBox.Text;

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Введите корректную строку с элементами дерева!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Создаем дерево на основе строки
            TreeBuilder treeBuilder = new TreeBuilder();
            try
            {
                RootTree = treeBuilder.BuildTreeFromString(input);

                // Привязываем дерево к TreeView
                TreeViewControl.Items.Clear();
                TreeViewControl.Items.Add(RootTree); // Добавляем корневой элемент
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при построении дерева: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Hide_Matrix_layer(object sender, RoutedEventArgs e)
        {
            {

                Visibility_Matrix = "Visible";
                Visibility_Tree = "Hidden";
                HideMatrix.Visibility = Visibility.Hidden;
                tree.Visibility = Visibility.Visible;
                TreeGrid.Visibility = Visibility.Hidden;

                RowsInput.Visibility = Visibility.Visible;
                ColumnsInput.Visibility = Visibility.Visible;
                ThreadsInput.Visibility = Visibility.Visible;
                MatrixDataGrid.Visibility = Visibility.Visible;
                GenerateMatrix.Visibility = Visibility.Visible;
                MatrixDataGrid_multi.Visibility = Visibility.Visible;
                MatrixDataGrid_sorted.Visibility = Visibility.Visible;
                rows.Visibility = Visibility.Visible;
                cols.Visibility = Visibility.Visible;
                thrds.Visibility = Visibility.Visible;
                multiplier.Visibility = Visibility.Visible;
                Multiply.Visibility = Visibility.Visible;
                multiply_click.Visibility = Visibility.Visible;


            }
        }


        private void Hide_Matrix_layer()
        {
            if (Visibility_Matrix == "Visible")
            {
                Visibility_Matrix = "Hidden";
                HideMatrix.Visibility = Visibility.Visible;
                HideMatrix.Content = "Show Matrix Layer";

                RowsInput.Visibility = Visibility.Hidden;
                ColumnsInput.Visibility = Visibility.Hidden;
                ThreadsInput.Visibility = Visibility.Hidden;
                MatrixDataGrid.Visibility = Visibility.Hidden;
                GenerateMatrix.Visibility = Visibility.Hidden;
                MatrixDataGrid_multi.Visibility = Visibility.Hidden;
                MatrixDataGrid_sorted.Visibility = Visibility.Hidden;
                rows.Visibility = Visibility.Hidden;
                cols.Visibility = Visibility.Hidden;
                thrds.Visibility = Visibility.Hidden;
                multiplier.Visibility = Visibility.Hidden;
                Multiply.Visibility = Visibility.Hidden;
                multiply_click.Visibility = Visibility.Hidden;


            }
            else
            {
                Visibility_Matrix = "Visible";
                Visibility_Tree = "Hidden";
                HideMatrix.Visibility = Visibility.Hidden;
                tree.Visibility = Visibility.Visible;
            }
        }

        private void OnGenerateTreeClick(object sender, RoutedEventArgs e)
        {
            string input = InputTextBox.Text;

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Введите корректную строку с элементами дерева!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Создаем дерево на основе строки
            TreeBuilder treeBuilder = new TreeBuilder();
            try
            {
                RootTree = treeBuilder.BuildTreeFromString(input);

                // Привязываем дерево к TreeView
                TreeViewControl.Items.Clear();
                TreeViewControl.Items.Add(RootTree); // Добавляем корневой элемент
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при построении дерева: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ToTree(object sender, RoutedEventArgs e)
        {
            if (Visibility_Tree == "Hidden")
            {
                
                Hide_Matrix_layer();
                Visibility_Tree = "Visible";
                Visibility_Matrix = "Hidden";
                tree.Visibility = Visibility.Hidden;

                // Привязываем дерево к TreeView
                TreeViewControl.Items.Clear();
                TreeViewControl.Items.Add(RootTree); // Добавляем корневой элемент
                TreeGrid.Visibility = Visibility.Visible;
            }
            else 
            {
                Hide_Matrix_layer();
                Visibility_Tree = "Hidden";
                tree.Visibility = Visibility.Visible;
                TreeGrid.Visibility = Visibility.Visible;
            }
        }
    }
}