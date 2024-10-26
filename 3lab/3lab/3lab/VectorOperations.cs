using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3lab
{
    class VectorOperations
    {
        // Метод для вычисления скалярного произведения
        private double CalculateDotProduct(double[] vectorA, double[] vectorB)
        {
            return vectorA.Zip(vectorB, (a, b) => a * b).Sum();
        }

        // Метод для обработки строки и запуска потоков
        public double[] ProcessVectors(List<string> vectorPairs)
        {
            double[] results = new double[vectorPairs.Count];
            Thread[] threads = new Thread[vectorPairs.Count];

            for (int i = 0; i < vectorPairs.Count; i++)
            {
                int index = i;  // Сохраняем индекс для использования внутри лямбда-выражения
                threads[i] = new Thread(() =>
                {
                    var vectors = vectorPairs[index].Split(' ').Select(double.Parse).ToArray();
                    var vectorA = vectors.Take(vectors.Length / 2).ToArray();
                    var vectorB = vectors.Skip(vectors.Length / 2).ToArray();

                    results[index] = CalculateDotProduct(vectorA, vectorB);
                });

                threads[i].Start();
            }

            // Ожидание завершения всех потоков
            foreach (var thread in threads)
            {
                thread.Join();
            }

            return results;
        }

        // Метод для вывода значений
        public void PrintResults(double[] results)
        {
            Console.WriteLine("Результаты скалярных произведений:");
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
    }
}
