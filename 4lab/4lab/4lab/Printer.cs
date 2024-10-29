using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4lab
{
    public class PrintPool
    {
        private readonly int _poolCapacity;                       // Максимальный объем пула в байтах
        private readonly Queue<string> _printQueue = new();       // Очередь печати
        private readonly object _lock = new();                    // Объект синхронизации
        private int _currentPoolSize = 0;                         // Текущий размер пула
        private readonly string _outputFilePath;                  // Путь для файла вывода
        private bool _isRunning = true;                           // Флаг для работы пула

        public PrintPool(int poolCapacity, string outputFilePath)
        {
            _poolCapacity = poolCapacity;
            _outputFilePath = outputFilePath;
        }

        // Метод добавления текста в пул
        public void AddToPool(string text)
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text);      // Перевод текста в байты для учета объема
            lock (_lock)
            {
                while (_currentPoolSize + textBytes.Length > _poolCapacity)
                {
                    Monitor.Wait(_lock);                          // Ожидаем освобождения места в пуле
                }

                _printQueue.Enqueue(text);
                _currentPoolSize += textBytes.Length;
                Console.WriteLine($"Добавлено в пул: {text} (размер: {textBytes.Length} байт)");
                Monitor.PulseAll(_lock);                          // Оповещаем поток управления
            }
        }

        // Метод управления для вывода текста из пула в файл
        public void ProcessPool()
        {
            using StreamWriter writer = new(_outputFilePath, append: true);

            while (_isRunning || _printQueue.Count > 0)
            {
                string textToPrint = null;
                lock (_lock)
                {
                    while (_printQueue.Count == 0)
                    {
                        Monitor.Wait(_lock);                     // Ждем, пока появятся данные для печати
                        if (!_isRunning && _printQueue.Count == 0) return;
                    }

                    textToPrint = _printQueue.Dequeue();
                    _currentPoolSize -= Encoding.UTF8.GetBytes(textToPrint).Length;
                    Monitor.PulseAll(_lock);                     // Оповещаем потоки печати об освобождении места
                }

                if (textToPrint != null)
                {
                    writer.WriteLine(textToPrint);
                    Console.WriteLine($"Записано в файл: {textToPrint}");
                }
            }
        }

        // Завершение работы пула
        public void StopPool()
        {
            _isRunning = false;
            lock (_lock)
            {
                Monitor.PulseAll(_lock);                         // Оповещаем все потоки для завершения работы
            }
        }
    }
}
