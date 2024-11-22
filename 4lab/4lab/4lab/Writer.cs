using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4lab
{
    internal class Writer
    {
        // Семафоры для управления доступом к буферу
        static Semaphore dataAvailable;  // Если есть данные в буфере
        static Semaphore spaceAvailable; // Если есть место в буфере

        // Мьютекс для безопасного доступа к буферу
        static Mutex bufferMutex = new Mutex();

        // Параметры буфера и количество файлов
        static int bufferSize; // Размер буфера
        static int fileCount;   // Количество файлов

        // Буфер для текста и флаг завершения
        static Queue<string> buffer = new Queue<string>();
        static bool isReadingComplete = false;

        // Массив для хранения путей к файлам
        string[] filePaths;
        string output_filename = "result.txt";

        // Метод для чтения текста из файла и добавления его в буфер
        void ReadFileToBuffer(object fileIndex)
        {
            // Читаем весь текст из указанного файла в строку 'content'
            string content = File.ReadAllText(filePaths[(int)fileIndex]);

            // Длина текста из файла
            int contentLength = content.Length;

            // Позиция в тексте, с которой будем добавлять текст в буфер
            int offset = 0;

            // Продолжаем, пока не добавим весь текст
            while (offset < contentLength)
            {
                int sizeToAdd;

                // Блокируем буфер для безопасного доступа к его размеру
                bufferMutex.WaitOne();

                // Определяем, сколько символов можно добавить в буфер
                // Если размер, который можно добавить, равен 0, то буфер заполнен
                // Ждем, пока в буфере не освободится место
                while ((sizeToAdd = Math.Min(bufferSize - GetBufferSize(), contentLength - offset)) <= 0)
                {
                    // Освобождаем блокировку буфера, чтобы другие потоки могли его использовать
                    bufferMutex.ReleaseMutex();

                    // Ждем, пока 'spaceAvailable' сигнализирует, что место в буфере доступно
                    spaceAvailable.WaitOne();

                    // После того как освободилось место, снова блокируем буфер для проверки
                    bufferMutex.WaitOne();
                }

                // Извлекаем часть текста, которая помещается в буфер
                // Берем текст от текущей позиции 'offset' до конца или на 'sizeToAdd' символов
                string chunkToAdd = content.Substring(offset, sizeToAdd);

                // Смещаем 'offset', чтобы при следующем проходе начать с новой позиции
                offset += sizeToAdd;

                // Добавляем часть текста в буферную очередь
                buffer.Enqueue(chunkToAdd);

                // Сообщаем пользователю о текущем размере данных в буфере
                Console.WriteLine($"Поток {((int)fileIndex + 1)} добавил информацию, теперь в пуле {GetBufferSize()} байт");

                // Уведомляем поток записи, что данные доступны (увеличиваем семафор данных)
                dataAvailable.Release();

                // Освобождаем мьютекс, позволяя другим потокам использовать буфер
                bufferMutex.ReleaseMutex();
            }

        }
        // Метод для записи текста из буфера в результирующий файл
        void WriteBufferToFile()
        {
            using (StreamWriter writer = new StreamWriter(output_filename))
            {
                while (true)
                {
                    dataAvailable.WaitOne(); // Ждем, если буфер пуст
                    bufferMutex.WaitOne();   // Блокируем буфер

                    // Если буфер не пуст, записываем текст в файл
                    if (buffer.Count > 0)
                    {
                        string textToWrite = buffer.Dequeue();
                        writer.Write(textToWrite);

                        // Считаем размер после записи
                        Console.WriteLine($"Поток управления выдал: '{textToWrite}', теперь в пуле {GetBufferSize()} байт");

                        // Проверяем, можем ли мы освободить место в семафоре
                        if (spaceAvailable.WaitOne(0) == false)
                        {
                            spaceAvailable.Release();
                        }
                    }

                    // Завершаем работу, если все данные записаны
                    if (isReadingComplete && buffer.Count == 0)
                    {
                        bufferMutex.ReleaseMutex(); // Освобождаем буфер перед выходом
                        break;
                    }

                    bufferMutex.ReleaseMutex(); // Освобождаем буфер
                }
            }
        }



        // Метод для получения общего размера данных в буфере

        static int GetBufferSize()
        {
            int size = 0;
            foreach (var text in buffer)
            {
                if (text != null) // Проверка, что текст не null
                {
                    size += text.Length; // Считаем длину каждого текста в байтах
                }
            }
            return size;
        }

        public void Run()
        {
            // Запрашиваем у пользователя количество файлов и размер буфера
            Console.Write("Введите количество файлов для чтения: ");
            fileCount = int.Parse(Console.ReadLine());

            Console.Write("Введите размер буфера: ");
            bufferSize = int.Parse(Console.ReadLine());

            // Создаем временные файлы с текстом от пользователя
            filePaths = new string[fileCount];
            for (int i = 0; i < fileCount; i++)
            {
                Console.Write($"Введите текст для файла {i + 1}: ");
                string userInput = Console.ReadLine();

                string fileName = $"input_{i + 1}.txt";
                File.WriteAllText(fileName, userInput);
                filePaths[i] = fileName;
            }

            // Инициализация семафоров
            spaceAvailable = new Semaphore(bufferSize, bufferSize); // Семафор для свободного места
            dataAvailable = new Semaphore(0, bufferSize);           // Семафор для данных

            // Запуск потока для записи данных из буфера в файл
            Thread writerThread = new Thread(WriteBufferToFile);
            writerThread.Start();

            // Запуск потоков для чтения файлов
            Thread[] readerThreads = new Thread[fileCount];
            for (int i = 0; i < fileCount; i++)
            {
                readerThreads[i] = new Thread(ReadFileToBuffer);
                readerThreads[i].Start(i);
            }

            // Ожидаем завершения всех потоков чтения
            foreach (var thread in readerThreads) thread.Join();

            isReadingComplete = true; // Указываем, что чтение завершено
            dataAvailable.Release();  // Разблокируем поток записи

            // Ожидаем завершения записи и выводим результат
            writerThread.Join();
            Console.WriteLine($"запись в файл {output_filename} произведена");
            Console.WriteLine("\nСодержимое результирующего файла:");
            Console.WriteLine(File.ReadAllText(output_filename));
        }
    }
}
