using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5lab
{
    internal class Maclaurin
    {
        //События для синхронизации потоков
        static ManualResetEvent factorialCalculatedEvent = new ManualResetEvent(false);
        static ManualResetEvent powerCalculatedEvent = new ManualResetEvent(false);

        // Переменные для хранения результатов вычислений
        static double factorialResult;
        static double powerResult;

        public static void Run()
        {
            // Запрашиваем значение x и количество членов ряда
            Console.WriteLine("Вычисление e^x с использованием ряда Маклорена.");
            Console.Write("Введите значение x: ");
            double x = Convert.ToDouble(Console.ReadLine());
            Console.Write("Введите количество членов ряда: ");
            int numTerms = Convert.ToInt32(Console.ReadLine());

            double sum = 1.0; // Первый член ряда всегда равен 1

            // Вычисляем сумму членов ряда
            for (int i = 1; i <= numTerms; i++)
            {
                // Запускаем потоки для вычисления факториала и степени
                Thread factorialThread = new Thread(() => CalculateFactorial(i));
                Thread powerThread = new Thread(() => CalculatePower(x, i));

                factorialThread.Start();
                powerThread.Start();

                // Ожидаем завершения вычислений факториала и степени
                WaitHandle.WaitAll(new WaitHandle[] { factorialCalculatedEvent, powerCalculatedEvent });

                // Вычисляем текущий член ряда и добавляем к сумме
                double term = powerResult / factorialResult;
                sum += term;

                // Выводим промежуточные результаты
                Console.WriteLine($"Член {i}: {term} (Степень: {powerResult}, Факториал: {factorialResult})");

                // Сбрасываем события для следующей итерации
                factorialCalculatedEvent.Reset();
                powerCalculatedEvent.Reset();
            }

            // Выводим итоговый результат
            Console.WriteLine($"\nПриближенное значение e^{x} по ряду Маклорена: {sum}");
        }

        // Функция для вычисления факториала
        static void CalculateFactorial(int n)
        {
            double factorial = 1;
            for (int i = 1; i <= n; i++)
            {
                factorial *= i;
            }
            factorialResult = factorial;

            // Сигнализируем о завершении вычисления факториала
            factorialCalculatedEvent.Set();
        }

        // Функция для вычисления степени
        static void CalculatePower(double x, int n)
        {
            powerResult = Math.Pow(x, n);

            // Сигнализируем о завершении вычисления степени
            powerCalculatedEvent.Set();
        }
    }
}
