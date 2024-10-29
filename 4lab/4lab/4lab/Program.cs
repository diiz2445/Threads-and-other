using _4lab;

internal class Program
{
    private static void Main(string[] args)
    {
        string outputPath = "output.txt";
        int poolCapacity = 1024; // Максимальный объем пула в байтах
        int numberOfPrinters = 3; // Количество потоков печати

        // Инициализация пула печати
        var printPool = new PrintPool(poolCapacity, outputPath);

        // Запуск потока управления пулом
        Thread controlThread = new(printPool.ProcessPool);
        controlThread.Start();

        // Запуск потоков печати
        List<Thread> printerThreads = new();
        for (int i = 1; i <= numberOfPrinters; i++)
        {
            string filePath = $"input{i}.txt";
            var worker = new Writer(printPool, filePath);
            Thread thread = new(worker.StartPrinting);
            printerThreads.Add(thread);
            thread.Start();
        }

        // Ожидание завершения потоков печати
        foreach (var thread in printerThreads)
        {
            thread.Join();
        }

        // Остановка потока управления пулом
        printPool.StopPool();
        controlThread.Join();

        Console.WriteLine("Все задачи печати выполнены.");
    }
}