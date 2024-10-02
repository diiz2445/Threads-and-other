using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace _1l
{
    public class Tree
    {
        public double D { get; set; }
        public Tree Left { get; set; }
        public Tree Right { get; set; }

        // Конструктор
        public Tree(double d)
        {
            D = d;
            Left = null;
            Right = null;
        }

        // Свойство для привязки дочерних узлов
        public List<Tree> Children
        {
            get
            {
                var children = new List<Tree>();
                if (Left != null) children.Add(Left);
                if (Right != null) children.Add(Right);
                return children;
            }
        }
    }
}




