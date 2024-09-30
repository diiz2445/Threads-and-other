using _1l;
using System;
using System.Threading;

class Program
{
    static void Main()
    {
        //Console.ReadLine();
        int n = 5; // Количество строк
        int m = 6; // Количество столбцов
        int[,]matrix = Threads.fill_matrix(n, m);
        Threads.print(matrix);
    }

}
