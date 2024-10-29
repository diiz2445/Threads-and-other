using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3lab
{
    internal class HashTable
    {
        private readonly int _k;
        private readonly List<int>[] _table;
        private readonly HashTableMonitor _monitor;

        public HashTable(int rows, int k, int lockCount)
        {
            _k = k;
            _table = new List<int>[rows];
            _monitor = new HashTableMonitor(rows, lockCount);
            for (int i = 0; i < rows; i++)
            {
                _table[i] = new List<int>();
            }
        }

        // Хеш-функция для вычисления строки, в которую попадет число
        private int Hash(int x)
        {
            return x % _k;
        }

        // Метод для добавления числа в таблицу
        public void Add(int x)
        {
            int row = Hash(x); // Находим строку для записи числа
            _monitor.OccupyRow(row); // Занимаем строку
            try
            {
                _table[row].Add(x); // Добавляем число в хеш-таблицу
                Console.WriteLine($"Число {x} добавлено в строку {row}");
            }
            finally
            {
                _monitor.FreeRow(row); // Освобождаем строку после записи
            }
        }

        // Метод для отображения хеш-таблицы (для отладки)
        public void PrintTable()
        {
            Console.WriteLine("результат работы:");
            for (int i = 0; i < _table.Length; i++)
            {
                Console.Write($"Строка {i}: ");
                foreach (int val in _table[i])
                {
                    Console.Write($"{val} ");
                }
                Console.WriteLine();
            }
        }
        public static void Run(int rows=50,int k=10,int threads_count = 5)//rows - количество строк; k - основание хеш-функции; n - количество потоков
        {
            Console.WriteLine("Работа с HashTable:");
            int lockCount = 10; // Количество объектов блокировки меньше, чем количество строк
            HashTable hashTable = new HashTable(rows, k, lockCount);

            // Запуск потоков для добавления случайных чисел в хеш-таблицу
            Thread[] threads = new Thread[5];
            Random rand = new Random();

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < 10; j++)
                    {
                        int num = rand.Next(100);
                        hashTable.Add(num);
                        Thread.Sleep(10); // Задержка для демонстрации работы потоков
                    }
                });
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join(); // Ожидаем завершения всех потоков
            }

            // Выводим хеш-таблицу после завершения работы всех потоков
            hashTable.PrintTable();
        }
    }
}
