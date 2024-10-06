using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1l
{
    class Tree
    {
        public double d; // поле данных
        public Tree L;    // ссылка на левое поддерево
        public Tree R;   // ссылка на правое поддерево

        public void Add(int x, ref Tree Root) // Метод для вставки элемента в дерево
        {
            if (Root == null)  // Проверяем, существует ли корень дерева
            {
                // Если корень отсутствует, создаем новый узел и делаем его корнем
                Root = new Tree();
                Root.L = null;  // Устанавливаем ссылки на левое и правое поддерево как null
                Root.R = null;
                Root.d = x;  // Присваиваем значение x новому корневому узлу
            }
            else
            {
                if (x < Root.d)
                {
                    // Если значение x меньше значения текущего узла, рекурсивно вызываем метод Add для левого поддерева
                    Add(x, ref Root.L);
                }
                else
                {
                    // Если значение x больше или равно значению текущего узла, рекурсивно вызываем метод Add для правого поддерева
                    Add(x, ref Root.R);
                }
            }
        }
        public static void printFront(Tree Root) //указатель на корень дерева или поддерева, обход которого производится 
        {
            if (Root == null) return;
            Console.Write(Root.d + " ");
            printFront(Root.L); //поддерево слева
            printFront(Root.R); //поддерево  справа

            return;
        }
        public static void printB(Tree Root) //указатель на корень дерева или поддерева, обход которого производится 
        {
            if (Root == null) return;

            printB(Root.L); //поддерево слева
            printB(Root.R); //поддерево  справа
            Console.Write(Root.d + " ");
            return;
        }
        public static void printSim(Tree Root) //указатель на корень дерева или поддерева, обход которого производится 
        {
            if (Root == null) return;

            printSim(Root.L); //поддерево слева
            Console.Write(Root.d + " ");
            printSim(Root.R); //поддерево  справа
            return;
        }


    }
}
