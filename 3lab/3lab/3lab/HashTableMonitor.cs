using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3lab
{
    public class HashTableMonitor
    {
        private readonly bool[] _occupiedRows; // Состояние занятости строк
        private readonly object[] _locks; // Массив объектов-закрытий для группы строк
        private readonly int _lockCount; // Количество объектов блокировок

        public HashTableMonitor(int rows, int lockCount)
        {
            _occupiedRows = new bool[rows];
            _lockCount = lockCount;
            _locks = new object[_lockCount];
            for (int i = 0; i < _lockCount; i++)
            {
                _locks[i] = new object(); // Создаем объект-блокировку для каждой группы
            }
        }

        // Метод для получения индекса блокировки для данной строки
        private int GetLockIndex(int rowNumber)
        {
            return rowNumber % _lockCount;
        }

        // Занять строку, чтобы другие потоки не могли записать в нее
        public void OccupyRow(int rowNumber)
        {
            int lockIndex = GetLockIndex(rowNumber);
            lock (_locks[lockIndex]) // Блокируем доступ к группе строк
            {
                while (_occupiedRows[rowNumber])
                {
                    Monitor.Wait(_locks[lockIndex]); // Ожидаем, пока строка освободится
                }
                _occupiedRows[rowNumber] = true;
            }
        }

        // Освободить строку после записи
        public void FreeRow(int rowNumber)
        {
            int lockIndex = GetLockIndex(rowNumber);
            lock (_locks[lockIndex])
            {
                _occupiedRows[rowNumber] = false;
                Monitor.Pulse(_locks[lockIndex]); // Уведомляем другие потоки, что строка свободна
            }
        }
    }
}
