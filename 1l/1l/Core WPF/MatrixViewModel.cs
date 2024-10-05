using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
