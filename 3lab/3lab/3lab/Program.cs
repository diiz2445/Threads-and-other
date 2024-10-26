namespace _3lab
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string mutexName = "Global\\MyNamedMutex"; // Имя мьютекса

            // Открытие или создание мьютекса по имени
            using (Mutex mutex = new Mutex(false, mutexName, out bool createdNew))
            {
                if (createdNew)
                {
                    Console.WriteLine("Мьютекс создан текущим приложением.");
                }
                else
                {
                    Console.WriteLine("Мьютекс уже существует. Ожидаем его освобождения.");
                }

                // Попробуем заблокировать мьютекс
                mutex.WaitOne();
                Console.WriteLine("Мьютекс захвачен. Работа с ресурсом...");

                // Имитация работы
                Thread.Sleep(3000);

                // Освобождение мьютекса
                Console.WriteLine("Освобождаем мьютекс.");
                mutex.ReleaseMutex();
            }

            Console.WriteLine("Завершено.");

            Console.ReadKey();

        }

    } 
}