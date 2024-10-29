using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3lab
{
    internal class write_mutex
    {
        public static void Run()
        {
            const string filePath = "C:\\Users\\sfg\\source\\repos\\zos\\3lab\\3lab\\3lab\\bin\\Debug\\net8.0\\shared_text.txt";
            using (Mutex mutex = new Mutex(false, "Global\\MyNamedMutex", out bool createdNew))

            {
                Console.WriteLine("Программа записи: Ожидание доступа к файлу...");
                mutex.WaitOne();
                Console.WriteLine("Программа записи: Доступ получен.");
                try
                {
                   Console.Write("Введите текст для записи в файл: ");
                   string text = Console.ReadLine();
                   // Запись текста в файл
                   File.WriteAllText(filePath, text);
                   Console.WriteLine("Программа записи: Текст успешно записан в файл.");
                }
                catch (Exception ex)
                {
                   Console.WriteLine("Ошибка записи в файл: " + ex.Message);
                }
                finally
                {
                    Thread.Sleep(100);
                    Console.WriteLine("Программа записи: Освобождение доступа к файлу.");
                    mutex.ReleaseMutex();
                }
                
            }
        }
    }
}
