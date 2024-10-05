using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1l
{
    class Tree
    {
        public int d; // поле данных
        public Tree L;    // ссылка на левое поддерево
        public Tree R;   // ссылка на правое поддерево


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
