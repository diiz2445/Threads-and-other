using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1l.Core_WPF
{


    public class TreeBuilder
    {
        // Метод для вставки узла в бинарное дерево
        public Tree Insert(Tree node, double value)
        {
            if (node == null)
            {
                return new Tree(value);
            }
            if (value < node.D)
            {
                node.Left = Insert(node.Left, value);
            }
            else
            {
                node.Right = Insert(node.Right, value);
            }
            return node;
        }

        // Создание дерева на основе строки
        public Tree BuildTreeFromString()
        {
            //var values = input.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
            //                  .Select(double.Parse).ToArray();
            Random rnd = new Random();
            double[] ArrayInt = new double[1000];
            for(int i=0;i<1000;i++)
                ArrayInt[i] = double.Round(rnd.NextDouble()*10,2);
            Tree root = null;
            foreach (var value in ArrayInt)
            {
                root = Insert(root, value);
            }
            return root;
        }
    }
}
