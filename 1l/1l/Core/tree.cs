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
        
        public static double Sum(Tree node)
            {
           if (node == null) return 0;

                double sum = node.D; // Начинаем с текущего значения узла

                // Создаем поток для обхода левого поддерева
                double leftSum = 0;
                Thread leftThread = null;

                if (node.Left != null)
                {
                    leftThread = new Thread(() => { leftSum = Sum(node.Left); });
                    leftThread.Start();
                }

                // Обрабатываем правое поддерево в текущем потоке
                double rightSum = Sum(node.Right);

                // Ожидаем завершения работы потока для левого поддерева
                if (leftThread != null)
                {
                    leftThread.Join(); // Ожидаем завершения работы потока
                }

                // Суммируем результаты
                return sum + leftSum + rightSum;
            

        }
        public static int Count(Tree node)
        {
            if (node == null) return 0;

            // Начинаем с текущего узла
            int count = 1;

            // Переменные для хранения результатов поддеревьев
            int leftCount = 0;
            int rightCount = 0;

            // Создаем поток для обхода левого поддерева
            Thread leftThread = null;
            if (node.Left != null)
            {
                leftThread = new Thread(() => { leftCount = Count(node.Left); });
                leftThread.Start();
            }

            // Обрабатываем правое поддерево в текущем потоке
            rightCount = Count(node.Right);

            // Ожидаем завершения работы потока для левого поддерева
            if (leftThread != null)
            {
                leftThread.Join(); // Ожидаем завершения работы потока
            }

            // Суммируем количество узлов
            return count + leftCount + rightCount;
        }
    }
}




