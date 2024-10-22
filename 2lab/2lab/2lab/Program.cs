namespace _2lab
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            double[,] matrix = Threads.fill_matrix_rand(3, 4, 3);
            Threads.print_matrix(matrix);

            Console.Write("\nsums:\n ");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Console.Write((Threads.Sum(matrix))[i]+" | ");
            }
            Console.Write("\n\nвведите строку: ");
            string text = Console.ReadLine();  // Тестовый текст
            int checksum = Threads.Control_sum(text, 4);  // Запуск вычисления контрольной суммы с 4 потоками
            Console.WriteLine($"\nПодана строка: {text}\nМногопоточная контрольная сумма: {checksum}");
        }
    }
}