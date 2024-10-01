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
       
       public static Thread[] create_threads(int n)
        {
            // Создаем массив потоков
            Thread[] threads = new Thread[n];
            return threads;
            
        }
        public static double[,] fill_matrix(int n,int m)
        {
            Thread[] threads = create_threads(n);
            double[,] matrix = new double[n,m];
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
        public static void print_matrix(double[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(Math.Round(matrix[i, j],2) + "\t");
                }
                Console.WriteLine();
            }
        }
        static void Fill_Row(double[,] matrix, int rowIndex, int m)//пример заполнения в убывающей последовательности по строкам
        {
            for (int j = 0; j < m; j++)
            {
                matrix[rowIndex, j] = rowIndex * m + j;
            }
        }
        static void Multiply_Row(double[,] matrix, int rowIndex, int m, double k)//пример заполнения в убывающей последовательности по строкам
        {
            for (int j = 0; j < m; j++)
            {
                matrix[rowIndex, j] *=k;
            }
        }
        static void Sort_Row(double[,] matrix, int rowIndex,int m)
        {
            
            double[] row = new double[m];

            // Копируем элементы строки в одномерный массив
            for (int j = 0; j < m; j++)
            {
                row[j] = matrix[rowIndex, j];
            }

            // Сортируем строку по убыванию
            Array.Sort(row);
            Array.Reverse(row); // Реверсируем массив для получения убывания

            // Заполняем отсортированную строку обратно в двумерный массив
            for (int j = 0; j < m; j++)
            {
                matrix[rowIndex, j] = row[j];
            }
        }
        public static double[,] sort(double[,] matrix)//сортировка строк по возрастанию 
        {
            double[,] sorted_matrix = matrix;
            Thread[] threads = Threads.create_threads(matrix.GetLength(0));
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                int rowIndex = i; // Локальная переменная для использования в потоке
                threads[i] = new Thread(() => Sort_Row(matrix, rowIndex, matrix.GetLength(1)));
                threads[i].Start();
            }

            return sorted_matrix;
        }

        public static double[,] multiply_matrix(double[,] matrix)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            
            double[,] multi_matrix = matrix;
            double k = InputDataWithCheck.InputDoubleWithValidation("ввод множителя: ",int.MinValue,int.MaxValue);//ввод целого значения с валидацией 
            sw.Start();
            Thread[] threads = create_threads(multi_matrix.GetLength(0));

            // Запускаем потоки для заполнения строк матрицы
            for (int i = 0; i < multi_matrix.GetLength(0); i++)
            {
                int rowIndex = i; // Локальная переменная для использования в потоке
                threads[i] = new Thread(() => Multiply_Row(multi_matrix, rowIndex, matrix.GetLength(1),k));
                threads[i].Start();
            }
            // Ждем завершения всех потоков
            for (int i = 0; i < multi_matrix.GetLength(0); i++)
            {
                threads[i].Join();
               
            }
            
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds.ToString());
            return multi_matrix;
        }
        public static double[,] multiply(double[,] matrix)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            
            double[,] multi_matrix = matrix;
            double k = InputDataWithCheck.InputDoubleWithValidation("ввод множителя: ", int.MinValue, int.MaxValue);//ввод целого значения с валидацией 
            sw.Start();
            for (int i = 0; i < multi_matrix.GetLength(0); i++)
                for (int j = 0; j < multi_matrix.GetLength(1); j++)
                    multi_matrix[i,j] *= k;
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds.ToString());
            return multi_matrix;
        }


        }
    }
