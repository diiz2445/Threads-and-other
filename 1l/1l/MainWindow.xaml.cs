using _1l.Core_WPF;
using System.Reflection.Emit;
using System.Security.Cryptography.Xml;
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
        int TreeCount=0;
        double TreeSum = 0;
        
        string Visibility_Matrix = "Visible";
        string Visibility_Tree = "Hidden";
        string Visibility_Circles = "Hidden";
        string Visibility_Texts = "Hidden";
        public MainWindow()
        {
            InitializeComponent();
            List<List<double>> matrix;
            
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
            multiply_click.Visibility = Visibility.Visible;
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

            //if (string.IsNullOrWhiteSpace(input))
            //{
            //    MessageBox.Show("Введите корректную строку с элементами дерева!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}

            // Создаем дерево на основе строки
            TreeBuilder treeBuilder = new TreeBuilder();
            try
            {
                RootTree = treeBuilder.BuildTreeFromString();

                // Привязываем дерево к TreeView
                TreeViewControl.Items.Clear();
                TreeViewControl.Items.Add(RootTree); // Добавляем корневой элемент
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при построении дерева: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            TreeSum=Tree.Sum(RootTree);
            TreeCount=Tree.Count(RootTree);
            Sum.Text= TreeSum.ToString();
            double average = TreeCount > 0 ? TreeSum / TreeCount : 0;
            Avg.Text = average.ToString();
        }
        private void ToMatrix(object sender, RoutedEventArgs e)//переход к слою с матрицами
        {
            {
                HideTexts_Layer();
                HideCircles_Layer();
                HideTree_Layer();
                Visibility_Matrix = "Visible";
                Visibility_Tree = "Hidden";
                Visibility_Circles = "Hidden";
                Visibility_Texts = "Hidden";
                HideMatrix.Visibility = Visibility.Hidden;
                tree.Visibility = Visibility.Visible;
                Circles.Visibility = Visibility.Visible;
                TreeGrid.Visibility = Visibility.Hidden;

                MatrixGrid.Visibility = Visibility.Visible;

            }
        }
        private void Hide_Matrix_layer()//сокрытие слоя матриц
        {
            
                Visibility_Matrix = "Hidden";
                HideMatrix.Visibility = Visibility.Visible;
                MatrixGrid.Visibility = Visibility.Hidden;

        }
        private void OnGenerateTreeClick(object sender, RoutedEventArgs e)
        {
            string input = InputTextBox.Text;

            //if (string.IsNullOrWhiteSpace(input))
            //{
            //    MessageBox.Show("Введите корректную строку с элементами дерева!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}

            // Создаем дерево на основе строки
            TreeBuilder treeBuilder = new TreeBuilder();
            try
            {
                RootTree = treeBuilder.BuildTreeFromString();

                // Привязываем дерево к TreeView
                TreeViewControl.Items.Clear();
                TreeViewControl.Items.Add(RootTree); // Добавляем корневой элемент
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при построении дерева: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ToTree(object sender, RoutedEventArgs e)//кнопка перехода к слою с деревьями
        {
                HideTexts_Layer();
                HideCircles_Layer();
                Hide_Matrix_layer();
                Visibility_Tree = "Visible";
                tree.Visibility = Visibility.Hidden;
                TreeViewControl.Items.Clear();
                TreeViewControl.Items.Add(RootTree); // Добавляем корневой элемент
                TreeGrid.Visibility = Visibility.Visible;
            
            
        }
        private void HideTree_Layer()//сокрытие слоя с деревьями
        {
            tree.Visibility= Visibility.Visible;//показать кнопку к слою с деревьями
            TreeGrid.Visibility = Visibility.Hidden;//скрыть слой с деревьями
            Visibility_Tree = "Hidden";
            
        }

        private void ToCircles(object sender, RoutedEventArgs e)//переход к слою с кружками
        {
            HideTree_Layer();
            Hide_Matrix_layer();
            HideTexts_Layer();
            Visibility_Circles = "Visible";
            Circles.Visibility = Visibility.Hidden;
        }
        private void HideCircles_Layer()//сокрытие слоя с кружками
        {
            CirclesGrid.Visibility = Visibility.Hidden;
            Visibility_Circles = "Hidden";
            Circles.Visibility= Visibility.Visible;
        }
        private void ToTexts(object sender, RoutedEventArgs e)//переход к слою с текстами
        {
            Hide_Matrix_layer();
            HideCircles_Layer();
            HideTree_Layer();
            Visibility_Texts = "Visible";
            Texts.Visibility = Visibility.Hidden;
            TextsGrid.Visibility = Visibility.Visible;
        }
        private void HideTexts_Layer()//сокрытие слоя с текстовыми заданиями
        {
            TextsGrid.Visibility = Visibility.Hidden;
            Visibility_Texts = "Hidden";
            Texts.Visibility= Visibility.Visible;
        }

        private void Show_Crypto(object sender, RoutedEventArgs e)
        {
            encrypted.Text = TextsViewModel.ShowEncrypted(TextRow.Text,TextKey.Text);
        }
        private void Show_Decrypted(object sender, RoutedEventArgs e)
        {
            Decrypted.Text = TextsViewModel.ShowDecrypted(encrypted.Text, KeyDecrypt.Text);
        }

    }
}