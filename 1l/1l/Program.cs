using System;
using System.Threading;

class Program
{
    static void Main()
    {
        //Console.ReadLine();
        int n = 5; // Количество строк
        int m = 5; // Количество столбцов
        int[,] matrix = new int[n, m];

        // Создаем массив потоков
        Thread[] threads = new Thread[n];

        // Запускаем потоки для заполнения строк матрицы
        for (int i = 0; i < n; i++)
        {
            int rowIndex = i; // Локальная переменная для использования в потоке
            threads[i] = new Thread(() => FillRow(matrix, rowIndex, m));
            threads[i].Start();
        }

        // Ждем завершения всех потоков
        for (int i = 0; i < n; i++)
        {
            threads[i].Join();
        }

        // Вывод матрицы на экран
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                Console.Write(matrix[i, j] + "\t");
            }
            Console.WriteLine();
        }
    }

    static void FillRow(int[,] matrix, int rowIndex, int m)
    {
        for (int j = 0; j < m; j++)
        {
            matrix[rowIndex, j] = rowIndex * m + j; // Пример заполнения
        }
    }
}
