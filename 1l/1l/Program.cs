using _1l;
using System;
using System.Threading;

class Program
{
    static void Main()
    {
        //Console.ReadLine();
        int n = 15; // Количество строк
        int m = 15; // Количество столбцов

        //1 задание (параллельное заполнение)
        double[,]matrix = Threads.fill_matrix(n, m,4);
        Threads.print_matrix(matrix);

        //2 задание (сортировка)
        Console.WriteLine("\nsort\n");
        double[,] sorted_matrix = Threads.sort(matrix);
        Threads.print_matrix(sorted_matrix);

        //3 задание (множитель)
        Console.WriteLine("\nmulti\n");
        double[,] muliplied_matrix = Threads.multiply_matrix(matrix);
        Threads.print_matrix(muliplied_matrix);

        Threads.multiply(matrix);




        Console.ReadKey();
    }

}
