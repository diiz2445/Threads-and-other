using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5lab
{
    internal class merge
    {
        public static void Run()
        {
            // Пути к исходным файлам с числами и результату
            string[] filePaths = { "file1.txt", "file2.txt", "file3.txt" }; // Пути к файлам
            string outputFilePath = "output.txt"; // Путь к результирующему файлу

            // Создаем события для синхронизации
            ManualResetEvent[] readEvents = new ManualResetEvent[filePaths.Length]; // Массив событий для каждого потока чтения
            AutoResetEvent writeEvent = new AutoResetEvent(false); // Событие для синхронизации потока записи

            // Массивы для хранения текущих значений и статусов завершения чтения для каждого потока
            int[] currentValues = new int[filePaths.Length]; // Текущие значения, прочитанные каждым потоком
            bool[] endOfFiles = new bool[filePaths.Length]; // Статус завершения чтения для каждого файла

            // Запуск потоков для чтения данных из файлов
            Thread[] readThreads = new Thread[filePaths.Length];
            for (int i = 0; i < filePaths.Length; i++)
            {
                int index = i; // Индекс для каждого потока чтения
                readEvents[index] = new ManualResetEvent(false); // Инициализация события синхронизации для этого потока
                readThreads[index] = new Thread(() => ReadFile(filePaths[index], index, ref currentValues, ref endOfFiles, readEvents, writeEvent));
                readThreads[index].Start(); // Запуск потока чтения
            }

            // Запуск потока для записи данных в результирующий файл
            Thread writeThread = new Thread(() => WriteToFile(outputFilePath, currentValues, endOfFiles, readEvents, writeEvent));
            writeThread.Start();

            // Ожидаем завершения всех потоков
            writeThread.Join(); // Ожидаем завершения потока записи
            foreach (var thread in readThreads)
            {
                thread.Join(); // Ожидаем завершения всех потоков чтения
            }

            // Выводим содержимое исходных файлов
            Console.WriteLine("Содержимое исходных файлов:");
            for (int i = 0; i < filePaths.Length; i++)
            {
                Console.WriteLine($"file{i + 1}.txt:");
                PrintFileContents(filePaths[i]);
            }

            // Выводим содержимое конечного файла
            Console.WriteLine("\nСодержимое выходного файла:");
            PrintFileContents(outputFilePath);
        }

        // Функция для чтения данных из файла
        static void ReadFile(string filePath, int index, ref int[] currentValues, ref bool[] endOfFiles, ManualResetEvent[] readEvents, AutoResetEvent writeEvent)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Преобразуем строку в целое число и сохраняем в массив
                    currentValues[index] = int.Parse(line);

                    // Сигнализируем, что данные были прочитаны
                    readEvents[index].Set();

                    // Ожидаем, пока поток записи обработает данные перед чтением следующего числа
                    writeEvent.WaitOne();
                }

                // Помечаем, что все данные из текущего файла были прочитаны
                endOfFiles[index] = true;
                // Сигнализируем, что завершили чтение файла
                readEvents[index].Set();
            }
        }

        // Функция для записи данных в выходной файл в порядке появления
        static void WriteToFile(string outputFilePath, int[] currentValues, bool[] endOfFiles, ManualResetEvent[] readEvents, AutoResetEvent writeEvent)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                bool allFilesDone = false;
                while (!allFilesDone)
                {
                    allFilesDone = true;

                    for (int i = 0; i < currentValues.Length; i++)
                    {
                        // Ждем, пока поток чтения предоставит данные
                        readEvents[i].WaitOne();

                        // Если текущий поток чтения не закончил работу, записываем значение
                        if (!endOfFiles[i])
                        {
                            writer.WriteLine(currentValues[i]);
                            allFilesDone = false; // Есть еще данные для записи
                        }

                        // Сбрасываем событие для этого потока чтения и разрешаем ему продолжить
                        readEvents[i].Reset();
                        writeEvent.Set();
                    }
                }
            }
        }

        // Функция для вывода содержимого файла на консоль
        static void PrintFileContents(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}
