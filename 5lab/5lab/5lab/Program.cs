internal class Program
{
    static EventWaitHandle startEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "StartEvent");
    static EventWaitHandle stopEvent = new EventWaitHandle(false, EventResetMode.ManualReset, "StopEvent");

    static void Main()
    {
        // Создание общего события для завершения программы
        EventWaitHandle exitEvent = new EventWaitHandle(false, EventResetMode.ManualReset, "ExitEvent");

        try
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Меню:");
                Console.WriteLine("1 - Перемножить 2 больших числа");
                Console.WriteLine("2 - Удалить дубликаты из массива вещественных чисел");
                Console.WriteLine("3 - Найти самое частое слово в тексте");
                Console.WriteLine("4 - Найти минимальный и максимальный элемент в матрице");
                Console.WriteLine("5 - Найти все простые числа до n");
                Console.WriteLine("0 - Выйти");
                Console.Write("Выберите действие: ");

                string input = Console.ReadLine();

                // Проверка на корректный ввод
                if (!int.TryParse(input, out int choice) || choice < 0 || choice > 5)
                {
                    Console.WriteLine("Неверный выбор. Пожалуйста, выберите действие из списка.");
                    Thread.Sleep(1000); // Подождать немного перед повторным запросом
                    continue; // Повторный запрос ввода
                }

                if (choice == 0)
                {
                    stopEvent.Set();
                    exitEvent.Set();  // Сигнализируем второй программе о завершении
                    break;
                }

                Console.WriteLine($"Отправка команды {choice} второй программе...");
                EventWaitHandle actionEvent = new EventWaitHandle(false, EventResetMode.AutoReset, $"Action{choice}");
                actionEvent.Set();
                startEvent.WaitOne();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            exitEvent.Set();  // Если возникла ошибка, закрыть все ресурсы
            stopEvent.Set();
            Console.WriteLine("Программа завершена.");
            Console.ReadLine();
        }
    }
}