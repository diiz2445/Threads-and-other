using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1l.Core_WPF
{
    internal class TreeViewModel
    {
        Tree tree;
        public void Generate_Tree(string str_elements)
        {
            //int.TryParse(str_count_element, out int count);
            string[] array_str_elements = str_elements.Split(' ');
            double[] doubles = new double[array_str_elements.Length];
            int i = 0;
            foreach (string str_element in array_str_elements)
            {
                double.TryParse(str_element, out doubles[i]);
                i++;
            }

        }
    }
}
