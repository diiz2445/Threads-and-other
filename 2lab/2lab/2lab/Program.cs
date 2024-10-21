namespace _2lab
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            double[,] matrix = Threads.fill_matrix_rand(3, 4, 3);
            Threads.print_matrix(matrix);

            Console.WriteLine("\nsums:");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Console.Write(double.Round(Threads.Sum(matrix)[i],2)+" | ");
            }
            string text = "ABCDE";  // Тестовый текст
            int checksum = Threads.Control_sum(text, 4);  // Запуск вычисления контрольной суммы с 4 потоками
            Console.WriteLine($"\n\nПодана строка: {text}\nМногопоточная контрольная сумма: {checksum}");
        }
    }
}