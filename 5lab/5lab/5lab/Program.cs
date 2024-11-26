namespace _5lab;

internal class Program
{
    //wait handle для 5.4
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


        Sorting exmpl = new Sorting();
        Console.WriteLine("5.1\nВведите список целых чисел");
        List<int> input_list = new List<int>();
        Random rnd = new Random();
        try
        {
            //string input_str = Console.ReadLine();
            //List<int> input_list = input_str.Split().Select(int.Parse).ToList();
            int count;
            int.TryParse(Console.ReadLine(),out count);
            for(int i = 0; i < count; i++)
                input_list.Add(rnd.Next(1,100));
            exmpl.Run(input_list);
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }

        //Console.WriteLine("\n\n5.2");
        //Maclaurin.Run();

        //Console.WriteLine("\n5.3");
        //Console.ReadLine();
        //merge.Run();
    }

    
}