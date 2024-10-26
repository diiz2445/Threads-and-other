using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2lab
{
    class TrapezoidArea : Threads
    {
        // Функция f(x), определяющая кривую.
        static double F(double x)
        {
            return Math.Sin(x); // Пример функции, принимающей положительные значения на заданном отрезке.
        }

        // Глобальная переменная для хранения итоговой площади
        static double totalArea = 0.0;

        // Объект для синхронизации доступа к общей переменной totalArea
        static readonly object locker = new object();

        // Метод для вычисления площади одного элемента трапеции на интервале [x_i, x_{i+1}] методом прямоугольников
        static void CalculateTrapezoidArea(double start, double end, int m)
        {
            double delta = (end - start) / m;  // Шаг разбиения каждого подотрезка
            double localArea = 0.0;           // Локальная площадь для одного элемента

            for (int j = 0; j < m; j++)
            {
                double x_j = start + j * delta;
                double x_j1 = x_j + delta;
                double height = F((x_j + x_j1) / 2);  // Высота прямоугольника (значение функции в середине отрезка)
                localArea += height * delta;          // Приближенная площадь прямоугольника
            }

            // Синхронизируем добавление к общей площади
            lock (locker)
            {
                totalArea += localArea;
            }
        }

        public static void Calculate(double start,double end, int n, int m)
        {
            
            double deltaX = (end - start) / n;  // Шаг разбиения основного отрезка

            // Массив потоков для параллельного расчета
            Thread[] threads = create_threads(n);

            for (int i = 0; i < n; i++)
            {
                double x_i = start + i * deltaX;
                double x_i1 = x_i + deltaX;

                // Создаем новый поток для расчета площади одного элемента
                threads[i] = new Thread(() => CalculateTrapezoidArea(x_i, x_i1, m));
                threads[i].Start();
            }

            // Ожидаем завершения всех потоков
            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            // Вывод результата
            Console.WriteLine("Приближенная площадь криволинейной трапеции: " + double.Round(totalArea,3));
        }
    }
}
