namespace _2lab
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            double[,] matrix = Threads.fill_matrix_rand(3, 4, 3);
            Threads.print_matrix(matrix);

        }
    }
}