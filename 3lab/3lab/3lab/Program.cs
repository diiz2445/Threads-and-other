namespace _3lab
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            VectorOperations vectorOperations = new VectorOperations();

            Console.WriteLine("Введите пары векторов в формате 'number number':");
            List<string> inputVectors = new List<string>
            {
                "1 2 3 4 5 6",
                "2 3 4 5 6 7",
                "4 5 6 7 8"
            };

            // Вызов метода для обработки векторов и получения результатов
            double[] results = vectorOperations.ProcessVectors(inputVectors);

            // Вывод результатов в консоль
            vectorOperations.PrintResults(results);




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