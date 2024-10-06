using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace _1l.Core_WPF
{
    public class MatrixRow
    {
        public List<double> Values { get; set; }
    }
    public class MatrixViewModel
    {
        public List<List<double>> Matrix { get; set; } // Список списков для хранения матрицы
        public MatrixViewModel(double[,] matrixData)
        {
            // Инициализация списка списков для хранения строк матрицы
            Matrix = new List<List<double>>();

            // Проходим по строкам и столбцам двумерного массива и копируем данные
            for (int i = 0; i < matrixData.GetLength(0); i++)
            {
                // Создаем новый список для хранения одной строки матрицы
                List<double> row = new List<double>();

                for (int j = 0; j < matrixData.GetLength(1); j++)
                {
                    row.Add(matrixData[i, j]); // Добавляем элемент в строку
                }

                Matrix.Add(row); // Добавляем строку в матрицу
            }
        }
        public static List<List<double>> ConvertToList(double[,] matrixData)
        {
            var result = new List<List<double>>();
            for (int i = 0; i < matrixData.GetLength(0); i++)
            {
                var row = new List<double>();
                for (int j = 0; j < matrixData.GetLength(1); j++)
                {
                    row.Add(matrixData[i, j]);
                }
                result.Add(row);
            }
            return result;
        }
        public static List<List<double>> GenerateMatrix(string row,string col,string thread)
        {
            return(MatrixViewModel.ConvertToList(GetDoubleMatrix(row, col, thread)));
        }
        public static double[,] GetDoubleMatrix(string row, string col, string thread)
        {
            // Получаем значения из полей ввода
            int rows; int.TryParse(row, out rows);       // количество строк
            int columns; int.TryParse(col, out columns); // количество столбцов
            int threads; int.TryParse(thread, out threads); // количество потоков (можно не использовать в примере)

            // Генерируем матрицу с полученными параметрами
            double[,] matrixData = Threads.fill_matrix(rows, columns, threads);
            return matrixData;
        }

        public static double[,] multiply(string row,string col, string thread,string k)
        {
            double[,] matrixData = GetDoubleMatrix(row, col, thread);
            matrixData = Threads.multiply_matrix(matrixData,k);
            return matrixData;

        }
        public static List<List<double>> multiply_list(string row, string col, string thread, string k)
        {
            return ConvertToList(multiply(row, col, thread, k));
        }
        public static List<List<double>> multiply_list(double[,]matrix,string k)
        {
            double[,] matrix_temp = Threads.multiply_matrix(matrix,k);
            return ConvertToList(matrix_temp);
        }



    }
}
