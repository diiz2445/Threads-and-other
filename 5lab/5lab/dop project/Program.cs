using System.Numerics;

internal class Program
{
    // События для управления потоком задач
    static EventWaitHandle startEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "StartEvent");
    static EventWaitHandle stopEvent = new EventWaitHandle(false, EventResetMode.ManualReset, "StopEvent");
    static EventWaitHandle exitEvent = new EventWaitHandle(false, EventResetMode.ManualReset, "ExitEvent");

    static void Main()
    {
        // Массив потоков для выполнения задач
        Thread[] taskThreads = new Thread[5];
        taskThreads[0] = new Thread(MultiplyLargeNumbers);
        taskThreads[1] = new Thread(RemoveDuplicatesFromArray);
        taskThreads[2] = new Thread(FindMostCommonWord);
        taskThreads[3] = new Thread(AnalyzeMatrixElements);
        taskThreads[4] = new Thread(FindPrimesUsingSieve);

        // Запускаем все потоки
        foreach (Thread t in taskThreads)
            t.Start();

        // Ожидаем сигнал завершения работы (exitEvent) от первой программы
        exitEvent.WaitOne();
        Console.WriteLine("Получен сигнал завершения. Закрытие второй программы...");

        // Прерываем и ожидаем завершения всех потоков задач
        foreach (Thread t in taskThreads)
        {
            t.Interrupt(); // Прерываем поток для выхода из блокирующего состояния
            t.Join();      // Ожидаем завершения потока
        }
        Console.WriteLine("Вторая программа завершена.");
    }

    
    static void MultiplyLargeNumbers()
    {
        // Специальное событие для задачи умножения больших чисел
        using (EventWaitHandle actionEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "Action1"))
        {
            // Цикл работы задачи: проверяем exitEvent и ожидаем actionEvent
            while (!exitEvent.WaitOne(0))
            {
                try
                {
                    actionEvent.WaitOne(); // Ожидание сигнала начала выполнения задачи
                    if (exitEvent.WaitOne(0)) break; // Проверяем выход, если exitEvent установлен

                    // Считываем два числа от пользователя
                    Console.Write("Введите первое число: ");
                    BigInteger num1 = BigInteger.Parse(Console.ReadLine());
                    Console.Write("Введите второе число: ");
                    BigInteger num2 = BigInteger.Parse(Console.ReadLine());

                    // Выполняем умножение введенных чисел
                    BigInteger result = num1 * num2;
                    Console.WriteLine($"Результат умножения: {result}");

                    // Отправляем сигнал об окончании задачи
                    startEvent.Set();
                }
                catch (ThreadInterruptedException)
                {
                    break; // Выход из цикла при прерывании потока
                }
            }
        }
    }

    static void RemoveDuplicatesFromArray()
    {
        using (EventWaitHandle actionEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "Action2"))
        {
            while (!exitEvent.WaitOne(0))
            {
                try
                {
                    actionEvent.WaitOne(); // Ожидаем сигнал начала задачи
                    if (exitEvent.WaitOne(0)) break; // Выходим при установке exitEvent

                    // Чтение массива чисел из файла
                    string[] numbers = File.ReadAllLines("array.txt");
                    // Удаляем дубликаты, оставляя только уникальные элементы
                    var uniqueNumbers = numbers.Distinct().ToArray();
                    // Записываем результат в новый файл
                    File.WriteAllLines("unique_array.txt", uniqueNumbers);
                    Console.WriteLine("Дубликаты удалены, результат сохранён в unique_array.txt");

                    // Сообщаем о завершении задачи
                    startEvent.Set();
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }
            }
        }
    }

    static void FindMostCommonWord()
    {
        using (EventWaitHandle actionEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "Action3"))
        {
            while (!exitEvent.WaitOne(0))
            {
                try
                {
                    actionEvent.WaitOne(); // Ждем сигнал запуска задачи
                    if (exitEvent.WaitOne(0)) break; // Проверяем exitEvent на выход

                    // Читаем текст из файла и разбиваем его на слова
                    string text = File.ReadAllText("text.txt");
                    var words = text.Split(new[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
                    // Ищем самое частое слово
                    var mostCommonWord = words.GroupBy(w => w)
                                              .OrderByDescending(g => g.Count())
                                              .First();
                    Console.WriteLine($"Самое частое слово: {mostCommonWord.Key}, Встречается: {mostCommonWord.Count()} раз");

                    // Завершаем выполнение задачи
                    startEvent.Set();
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }
            }
        }
    }

    static void AnalyzeMatrixElements()
    {
        using (EventWaitHandle actionEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "Action4"))
        {
            while (!exitEvent.WaitOne(0))
            {
                try
                {
                    actionEvent.WaitOne(); // Ждем начала задачи
                    if (exitEvent.WaitOne(0)) break; // Проверяем выход

                    // Пользователь задает размер матрицы
                    Console.Write("Введите размер матрицы: ");
                    int n = int.Parse(Console.ReadLine());
                    int[,] matrix = new int[n, n];
                    Random rand = new Random();

                    // Генерируем матрицу случайных чисел
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                            matrix[i, j] = rand.Next(1, 100);

                    Console.WriteLine("Матрица:");
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            Console.Write(matrix[i, j] + "\t");
                        }
                        Console.WriteLine();
                    }

                    // Ищем минимальный элемент ниже главной диагонали и максимальный выше побочной диагонали
                    int minBelowMain = int.MaxValue, maxAboveSecondary = int.MinValue;
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            if (i > j && matrix[i, j] < minBelowMain)
                                minBelowMain = matrix[i, j];

                            if (i + j < n - 1 && matrix[i, j] > maxAboveSecondary)
                                maxAboveSecondary = matrix[i, j];
                        }
                    }

                    // Выводим найденные значения
                    Console.WriteLine($"Минимум ниже главной диагонали: {minBelowMain}");
                    Console.WriteLine($"Максимум выше побочной диагонали: {maxAboveSecondary}");

                    startEvent.Set(); // Сообщаем о завершении задачи
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }
            }
        }
    }

    static void FindPrimesUsingSieve()
    {
        using (EventWaitHandle actionEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "Action5"))
        {
            while (!exitEvent.WaitOne(0))
            {
                try
                {
                    actionEvent.WaitOne(); // Ожидаем сигнал начала задачи
                    if (exitEvent.WaitOne(0)) break; // Проверка на выход

                    // Запрашиваем значение n у пользователя
                    Console.Write("Введите значение n: ");
                    int n = int.Parse(Console.ReadLine());
                    bool[] isPrime = Enumerable.Repeat(true, n + 1).ToArray();
                    isPrime[0] = isPrime[1] = false;

                    // Реализация алгоритма решета Эратосфена
                    for (int i = 2; i * i <= n; i++)
                    {
                        if (isPrime[i])
                        {
                            for (int j = i * i; j <= n; j += i)
                                isPrime[j] = false;
                        }
                    }

                    // Выводим простые числа
                    Console.WriteLine("Простые числа:");
                    for (int i = 2; i <= n; i++)
                    {
                        if (isPrime[i]) Console.Write(i + " ");
                    }
                    Console.WriteLine();

                    startEvent.Set(); // Устанавливаем событие завершения
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }
            }
        }
    }
}