using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_задание_2_приложение
{
    internal class read_mutex
    {
        public static void Run()
        {
            const string filePath = "C:\\Users\\sfg\\source\\repos\\zos\\3lab\\3lab\\3lab\\bin\\Debug\\net8.0\\shared_text.txt";
            using (Mutex mutex = new Mutex(false, "Global\\MyNamedMutex", out bool createdNew))
            {
                while (true)
                {
                    Console.WriteLine("Программа чтения: Ожидание доступа к файлу...");
                    mutex.WaitOne();
                    Console.WriteLine("Программа чтения: Доступ получен.");

                    try
                    {
                        // Проверка существования файла
                        if (File.Exists(filePath))
                        {
                            // Чтение файла с попытками обновления кэша
                            string text = ReadFileWithCacheRefresh(filePath);
                            Console.WriteLine("Программа чтения: Содержимое файла:");
                            Console.WriteLine(text);
                        }
                        else
                        {
                            Console.WriteLine("Файл не найден.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка чтения файла: " + ex.Message);
                    }
                    finally
                    {
                        Console.WriteLine("Программа чтения: Освобождение доступа к файлу.");
                        mutex.ReleaseMutex();
                    }
                    Thread.Sleep(10000);

                }
            }
        }
        static string ReadFileWithCacheRefresh(string filePath)
        {
            string content = "";
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    content = File.ReadAllText(filePath);
                    break;
                }
                catch (IOException)
                {
                    Thread.Sleep(50); // Небольшая задержка для обновления кэша
                }
            }
            return content;
        }
    }
}
