namespace _2lab
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            double[,] matrix = Threads.fill_matrix_rand(3, 4, 3);
            Threads.print_matrix(matrix);

            Console.WriteLine("sums:");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Console.Write(double.Round(Threads.Sum(matrix)[i],2)+" ");
            }

            Console.WriteLine();
            string text = "ABCDE";
            Console.WriteLine(Threads.Control_sum(text,3));


            double a = 0.0;      // Начало отрезка
            double b = Math.PI;  // Конец отрезка
            int n = 10;          // Количество частей, на которые делим отрезок [a, b]
            int m = 100;         // Количество подотрезков внутри каждого элемента для метода прямоугольников
            for (int i = 0; i < n; i++)
            {
                TrapezoidArea.Calculate(a, b, n, m);
                b++;
            }

        }
    }
}