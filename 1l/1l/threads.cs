using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

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
        public static T[,] fill_matrix<T>(T[,] matrix,int n,int m,int k)//k = кол-во потоков
        
        {
            Thread[] threads = create_threads(k);
            //T[,] matrix = new T[n,m];
            // Запускаем потоки для заполнения строк матрицы
            for (int i = 0; i < k; i++)
            {

                int rowIndex = i; // Локальная переменная для использования в потоке
                threads[i] = new Thread(() => Fill_Row(matrix, rowIndex, m,k));
                threads[i].Start();
            }
            // Ждем завершения всех потоков
            for (int i = 0; i < k; i++)
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
        static void Fill_Row<T>(T[,] matrix, int rowIndex, int m, int k)//заполнениe строки; k - кол-во потоков 
        {
            while (rowIndex < matrix.GetLength(1))
            {
                dynamic[] temp = new dynamic[m];
                for (int j = 0; j < m; j++)
                {
                    temp[j] = rowIndex * m + j;
                    matrix[rowIndex, j] = temp[j];
                }
                rowIndex += k;
            }
        }
        static void Multiply_Row(double[,] matrix, int rowIndex, int m, double c,int k)//умножение строки на множитель c, k - кол-во потоков
        {
            while (rowIndex < matrix.GetLength(1))
            {

                for (int j = 0; j < m; j++)
                {
                    matrix[rowIndex, j] *=c;
                }
                rowIndex += k;
            }
        }
        static void Sort_Row(double[,] matrix, int rowIndex,int m,int k)//сортировка строки; m - длина строки, k - кол-во потоков 
        {
            while (rowIndex < matrix.GetLength(1))
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
                rowIndex += k;
            }
        }
        public static double[,] sort(double[,] matrix,int k)//сортировка строк матрицы по убыванию; k - кол-во потоков
        {
            double[,] sorted_matrix = matrix;
            Thread[] threads = Threads.create_threads(matrix.GetLength(0));
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                int rowIndex = i; // Локальная переменная для использования в потоке
                threads[i] = new Thread(() => Sort_Row(matrix, rowIndex, matrix.GetLength(1),k));
                threads[i].Start();
                threads[i].Join();
            }

            return sorted_matrix;
        }

        public static double[,] multiply_matrix(double[,] matrix,int k)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            
            double[,] multi_matrix = matrix;
            double c = InputDataWithCheck.InputDoubleWithValidation("ввод множителя: ",int.MinValue,int.MaxValue);//ввод целого значения с валидацией 
            //sw.Start();
            Thread[] threads = create_threads(k);

            // Запускаем потоки для заполнения строк матрицы
            for (int i = 0; i < k; i++)
            {
                int rowIndex = i; // Локальная переменная для использования в потоке
                threads[i] = new Thread(() => Multiply_Row(multi_matrix, rowIndex, matrix.GetLength(1),c,k));
                threads[i].Start();
            }
            // Ждем завершения всех потоков
            for (int i = 0; i < k; i++)
            {
                threads[i].Join();
               
            }
            
            //sw.Stop();
            //Console.WriteLine(sw.ElapsedMilliseconds.ToString());
            return multi_matrix;
        }
        


        }
    }
