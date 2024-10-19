using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2lab
{
    class InputDataWithCheck
    {
        static public double InputDoubleWithValidation(string s, int left, int right) // Ввод целого числа с проверкой правильности ввода, в том числе принадлежности к указанному диапазону.                                                                                         
        {
            bool ok;
            double a;
            do
            {
                Console.WriteLine(s);
                ok = double.TryParse(Console.ReadLine(), out a);
                if (ok)
                    if (a < left || a > right)
                        ok = false;
                if (!ok)
                {
                    ConsoleColor tmp = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nВведенные данные имеют неверный формат или не принадлежат диапазону [{left}; {right}]");
                    Console.WriteLine("Повторите ввод\n");
                    Console.ForegroundColor = tmp;
                }
            } while (!ok);
            return (double)a;
        }
    }
}
