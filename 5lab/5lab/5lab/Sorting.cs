using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace _5lab
{
    internal class Sorting
    {
        //Массив событий для сигнализации завершения сортировки каждой части списка
        static ManualResetEvent[] sortEvents = new ManualResetEvent[3];

        // Событие для начала слияния частей после завершения их сортировки
        static EventWaitHandle mergeEvent = new ManualResetEvent(false);

        // Глобальные списки для хранения частей оригинального списка
        static List<int> listPart1 = new List<int>();
        static List<int> listPart2 = new List<int>();
        static List<int> listPart3 = new List<int>();

        //Сортированный список
        static List<int> sortedList = new List<int>();

        public void Run(List<int> originalList)
        {
            // Выводим исходный список
            Console.WriteLine("Исходный список:");
            foreach (var item in originalList)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();

            // Разделяем исходный список на три части
            int partSize = originalList.Count / 3;
            listPart1 = originalList.GetRange(0, partSize);
            listPart2 = originalList.GetRange(partSize, partSize);
            listPart3 = originalList.GetRange(partSize * 2, originalList.Count - (partSize * 2));

            // Инициализируем события для каждого потока сортировки
            for (int i = 0; i < sortEvents.Length; i++)
            {
                sortEvents[i] = new ManualResetEvent(false);
            }

            // Запускаем потоки для сортировки каждой части
            new Thread(() => SortListPart(listPart1, 0)).Start();
            new Thread(() => SortListPart(listPart2, 1)).Start();
            new Thread(() => SortListPart(listPart3, 2)).Start();

            // Ожидаем завершения всех потоков сортировки
            WaitHandle.WaitAll(sortEvents);

            // Сигнализируем, что можно начинать слияние
            mergeEvent.Set();

            // Начинаем слияние отсортированных частей
            MergeParts();

            // Выводим окончательный отсортированный список
            Console.WriteLine("Отсортированный список:");
            foreach (var item in sortedList)
            {
                Console.Write(item + " ");
            }
        }

        // Метод для сортировки части списка
        static void SortListPart(List<int> listPart, int partIndex)
        {
            listPart.Sort(); // Сортируем часть списка
            sortEvents[partIndex].Set(); // Сигнализируем, что сортировка этой части завершена
        }

        // Метод для слияния трех отсортированных частей списка
        static void MergeParts()
        {
            mergeEvent.WaitOne(); // Ожидаем сигнала, что все части отсортированы

            // Индексы для отслеживания текущего положения в каждой части списка
            int i = 0, j = 0, k = 0;

            // Слияние частей списка в окончательный отсортированный список
            while (i < listPart1.Count || j < listPart2.Count || k < listPart3.Count)
            {
                int minValue = int.MaxValue; // Переменная для поиска минимального значения среди частей

                // Определяем минимальный элемент среди текущих элементов каждой части списка
                if (i < listPart1.Count) minValue = Math.Min(minValue, listPart1[i]);
                if (j < listPart2.Count) minValue = Math.Min(minValue, listPart2[j]);
                if (k < listPart3.Count) minValue = Math.Min(minValue, listPart3[k]);

                // Добавляем минимальный элемент в итоговый отсортированный список
                if (i < listPart1.Count && listPart1[i] == minValue) sortedList.Add(listPart1[i++]);
                else if (j < listPart2.Count && listPart2[j] == minValue) sortedList.Add(listPart2[j++]);
                else if (k < listPart3.Count && listPart3[k] == minValue) sortedList.Add(listPart3[k++]);
            }
        }
    }
}
