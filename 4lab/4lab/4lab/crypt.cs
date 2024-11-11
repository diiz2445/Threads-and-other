using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5lab
{
    internal class crypt
    {
        // Семафоры для синхронизации потоков
        static Semaphore readSemaphore; 
        static Semaphore encodeSemaphore; 
        static Semaphore writeSemaphore; 
        static Mutex bufferMutex = new Mutex(); // Мьютекс для контроля доступа к буферу(релизим при дуступности)

        // Глобальные переменные и файлы
        static string fileText = File.ReadAllText("input_2.txt"); // Чтение исходного текста из файла
        static int bufferSize = 8;
        static int encoderThreads = 2; // Количество потоков для кодирования (n потоков)
        static char[] buffer = new char[bufferSize]; // Буфер \
        static int fileIndex = 0; // положение в исходном файле
        static int encodedCount = 0; // Кол-во закодированных символов
        static StreamWriter writer = new StreamWriter("Encrypted.txt"); // Файл для записи закодированного текста

        // Поток для добавления символов в буфер
        static void AddLetter()
        {
            // Пока есть символы для записи в буфер
            while (fileIndex < fileText.Length)
            {
                readSemaphore.WaitOne(); // Ожидаем разрешения на чтение
                bufferMutex.WaitOne(); // Захватываем мьютекс для безопасного доступа к буферу

                // Добавляем символы в буфер до его заполнения или до конца текста
                int count = 0;
                while (fileIndex < fileText.Length && count < bufferSize)
                {
                    buffer[count++] = fileText[fileIndex++]; 
                }

                bufferMutex.ReleaseMutex(); // Освобождаем от контроля буффер
                encodeSemaphore.Release(encoderThreads); // освобождение для шифорвщика
            }
        }

        
        static void EncodeText(object threadIndex)
        {
            int i = (int)threadIndex; // Номер текущего потока
            while (fileIndex < fileText.Length)
            {
                encodeSemaphore.WaitOne(); // Ожидание разрешения на кодирование
                bufferMutex.WaitOne(); // Захват мьютекса для работы с буфером

                // Кодирование символов в буфере, которые относятся к данному потоку
                for (int j = i; j < bufferSize; j += encoderThreads)
                {
                    char c = buffer[j];

                    // Меняем регистр
                    if (char.IsLower(c))
                        c = char.ToUpper(c);
                    else if (char.IsUpper(c))
                        c = char.ToLower(c);

                    // Сдвигаем букву на следующую по алфавиту
                    if (char.IsLetter(c))
                    {
                        if (c == 'Z')
                            c = 'A';
                        else if (c == 'z')
                            c = 'a';
                        else
                            c++;
                    }
                    encodedCount++; // Увеличиваем счетчик закодированных символов
                    buffer[j] = c; // Записываем закодированный символ в буфер
                }

                bufferMutex.ReleaseMutex(); //релизим мьютекс буфера
                if (encodedCount == bufferSize)
                    writeSemaphore.Release(); // разрешаем запись
            }
        }

        // Поток для записи закодированного текста в файл
        static void SaveToFile()
        {
            while (fileIndex < fileText.Length)
            {
                writeSemaphore.WaitOne(); // Ожидаем разрешения на запись
                bufferMutex.WaitOne(); // Захватываем мьютекс для работы с буфером

                writer.Write(buffer); // Записываем содержимое буфера в файл
                encodedCount = 0; // Обнуляем счетчик закодированных символов
                Array.Clear(buffer, 0, buffer.Length); // Очищаем буфер

                readSemaphore.Release(); // Разрешаем потоку добавления новых символов начать работу
                bufferMutex.ReleaseMutex(); // Освобождаем мьютекс буфера
            }
        }

        // Функция для шифрования текста
        public static void EncryptText()
        {
            Console.Write("введите длину буфера в символах: ");
            int.TryParse(Console.ReadLine(),out bufferSize);
            Console.WriteLine("Исходный текст:\n" + fileText + "\n"); // Выводим исходный текст

            // Инициализируем семафоры
            readSemaphore = new Semaphore(1, 1); // Для одного потока чтения
            encodeSemaphore = new Semaphore(0, encoderThreads); // Для потоков кодирования
            writeSemaphore = new Semaphore(0, 1); // Для одного потока записи

            // Создаем и запускаем потоки
            Thread addThread = new Thread(AddLetter);
            addThread.Start(); // Поток добавления символов в буфер

            Thread[] encodeThreads = new Thread[encoderThreads];
            for (int i = 0; i < encoderThreads; i++)
            {
                encodeThreads[i] = new Thread(EncodeText);
                encodeThreads[i].Start(i); // Запускаем каждый поток кодирования
            }

            Thread saveThread = new Thread(SaveToFile);
            saveThread.Start(); // Поток для записи закодированного текста

            // Ожидаем завершения всех потоков
            addThread.Join();
            foreach (Thread t in encodeThreads)
                t.Join();
            saveThread.Join();

            writer.Close(); // Закрываем файл для записи

            // Выводим зашифрованный текст
            string encryptedText = File.ReadAllText("Encrypted.txt");
            Console.WriteLine("Зашифрованный текст:\n" + encryptedText + "\n");
        }

        // Функция для дешифрования текста
        public static void DecryptText()
        {
            string encryptedText = File.ReadAllText("Encrypted.txt"); // Чтение зашифрованного текста
            StringBuilder decryptedText = new StringBuilder();


            // Проходим по каждому символу зашифрованного текста
            foreach (char c in encryptedText)
            {
                char decryptedChar = c;

                // Меняем регистр
                if (char.IsLower(c))
                    decryptedChar = char.ToUpper(c);
                else if (char.IsUpper(c))
                    decryptedChar = char.ToLower(c);

                // Сдвигаем букву на предыдущую по алфавиту
                if (char.IsLetter(decryptedChar))
                {
                    if (decryptedChar == 'A')
                        decryptedChar = 'Z';
                    else if (decryptedChar == 'a')
                        decryptedChar = 'z';
                    else
                        decryptedChar--;
                }
                decryptedText.Append(decryptedChar); // Добавляем расшифрованный символ
            }

            // Записываем расшифрованный текст в файл и выводим на экран
            using (StreamWriter decryptedWriter = new StreamWriter("Decrypted.txt"))
            {
                decryptedWriter.Write(decryptedText.ToString());
            }
            Console.WriteLine("Расшифрованный текст:\n" + decryptedText.ToString() + "\n");
        }
    }
}
