using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _1l
{
    internal class Threads
    {
       public void fill(int n)
        {

        }
       public static Thread[] create_threads(int n)
        {
            // Создаем массив потоков
            Thread[] threads = new Thread[n];
            return threads;
            
        }
        public static int[,] fill_matrix(int n,int m)
        {
            Thread[] threads = create_threads(n);
            int[,] matrix = new int[n,m];
            // Запускаем потоки для заполнения строк матрицы
            for (int i = 0; i < n; i++)
            {
                int rowIndex = i; // Локальная переменная для использования в потоке
                threads[i] = new Thread(() => Fill_Row(matrix, rowIndex, m));
                threads[i].Start();
            }
            // Ждем завершения всех потоков
            for (int i = 0; i < n; i++)
            {
                threads[i].Join();
            }
            return matrix;
        }
        public static void print(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }
        static void Fill_Row(int[,] matrix, int rowIndex, int m)//пример заполнения в убывающей последовательности по строкам
        {
            for (int j = 0; j < m; j++)
            {
                matrix[rowIndex, j] = rowIndex * m - j;
            }
        }

    }
}
