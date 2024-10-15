using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1l
{
    internal class Texts
    {
        public static string ParallelEncrypt(string text, string key)
        {
            int blockSize = key.Length;
            int n = text.Length;
            int numBlocks = (n + blockSize - 1) / blockSize; // Количество блоков
            char[] encryptedText = new char[n]; // Массив для зашифрованного текста

            // Массив потоков
            Thread[] threads = new Thread[numBlocks];

            // Запуск потоков на шифрование блоков
            for (int blockIndex = 0; blockIndex < numBlocks; blockIndex++)
            {
                int startIdx = blockIndex * blockSize;
                int currentBlockSize = Math.Min(blockSize, n - startIdx);
                string block = text.Substring(startIdx, currentBlockSize);

                // Запоминаем локальные переменные для потока
                int localBlockIndex = blockIndex;
                string localBlock = block;

                // Создаем новый поток для обработки блока
                threads[blockIndex] = new Thread(() =>
                {
                    // Перестановка символов внутри блока
                    char[] reversedBlock = ReverseBlock(localBlock);

                    // Применение ключа к блоку
                    for (int j = 0; j < currentBlockSize; j++)
                    {
                        char encryptedChar = (char)(reversedBlock[j] ^ key[j]);
                        encryptedText[startIdx + j] = encryptedChar; // Запись результата в общий массив
                    }
                });

                // Стартуем поток
                threads[blockIndex].Start();
            }

            // Ожидание завершения всех потоков
            for (int i = 0; i < numBlocks; i++)
            {
                threads[i].Join();
            }

            return new string(encryptedText);
        }
        public static string ParallelDecrypt(string encryptedText, string key)
        {
            int blockSize = key.Length;
            int n = encryptedText.Length;
            int numBlocks = (n + blockSize - 1) / blockSize; // Количество блоков
            char[] decryptedText = new char[n]; // Массив для расшифрованного текста

            // Массив потоков
            Thread[] threads = new Thread[numBlocks];

            // Запуск потоков на расшифрование блоков
            for (int blockIndex = 0; blockIndex < numBlocks; blockIndex++)
            {
                int startIdx = blockIndex * blockSize;
                int currentBlockSize = Math.Min(blockSize, n - startIdx);
                string block = encryptedText.Substring(startIdx, currentBlockSize);

                // Запоминаем локальные переменные для потока
                int localBlockIndex = blockIndex;
                string localBlock = block;

                // Создаем новый поток для обработки блока
                threads[blockIndex] = new Thread(() =>
                {
                    // Обратная операция XOR с ключом
                    char[] decryptedBlock = new char[currentBlockSize];
                    for (int j = 0; j < currentBlockSize; j++)
                    {
                        decryptedBlock[j] = (char)(localBlock[j] ^ key[j]);
                    }

                    // Восстановление исходного порядка символов (обратный реверс)
                    char[] originalBlock = ReverseBlock(new string(decryptedBlock));

                    // Запись расшифрованного блока в итоговый массив
                    for (int j = 0; j < currentBlockSize; j++)
                    {
                        decryptedText[startIdx + j] = originalBlock[j];
                    }
                });

                // Стартуем поток
                threads[blockIndex].Start();
            }

            // Ожидание завершения всех потоков
            for (int i = 0; i < numBlocks; i++)
            {
                threads[i].Join();
            }

            return new string(decryptedText);
        }
        // Метод для перестановки символов внутри блока (реверс)
        private static char[] ReverseBlock(string block)
        {
            char[] reversedBlock = block.ToCharArray();
            Array.Reverse(reversedBlock);
            return reversedBlock;
        }
    }
}
