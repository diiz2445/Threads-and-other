using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4lab
{
    internal class Writer
    {
        private readonly PrintPool _pool;
        private readonly string _filePath;

        public Writer(PrintPool pool, string filePath)
        {
            _pool = pool;
            _filePath = filePath;
        }

        public void StartPrinting()
        {
            foreach (string line in File.ReadLines(_filePath))
            {
                _pool.AddToPool(line);
                Thread.Sleep(500); // Имитация времени работы с текстом
            }
        }
    }
}
