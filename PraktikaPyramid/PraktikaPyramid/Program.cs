using System;

namespace MB_PYramid
{
    class Program
    {

        const char STAGE_SPLITER = '\n';
        const char BLOCK_SPLITER = ' ';

        static void Main(string[] args)
        {
            //string pyramid =
            //    "0\n" + //EVEN
            //    "1 2\n" +   //ODD
            //    "3 4 5\n" + //EVEN
            //    "6 7 8 9\n" +   //ODD
            //    "10 11 12 13 14"; //EVEN

            //string pyramid =
            //    "1\n" +
            //    "8 9\n" +
            //    "1 5 9\n" +
            //    "4 5 2 3";


            //string pyramid =
            //    "1\n" + //1 ODD
            //    "2 3\n" +   //2 EVEN
            //    "4 5 6\n" + //3 ODD
            //    "7 8 9 10\n" +   // 4 EVEN
            //    "11 12 13 14 15\n" + //5 ODD
            //    "16 17 18 19 20 21"; //6 EVEN

            string pyramid = "" +
            "215\n" +
            "192 124\n" +
            "117 269 442\n" +
            "218 836 347 235\n" +
            "320 805 522 417 345\n" +
            "229 601 728 835 133 124\n" +
            "248 202 277 433 207 263 257\n" +
            "359 464 504 528 516 716 871 182\n" +
            "461 441 426 656 863 560 380 171 923\n" +
            "381 348 573 533 448 632 387 176 975 449\n" +
            "223 711 445 645 245 543 931 532 937 541 444\n" +
            "330 131 333 928 376 733 017 778 839 168 197 197\n" +
            "131 171 522 137 217 224 291 413 528 520 227 229 928\n" +
            "223 626 034 683 839 052 627 310 713 999 629 817 410 121\n" +
            "924 622 911 233 325 139 721 218 253 223 107 233 230 124 233";

            int Hight = pyramid.Split(STAGE_SPLITER).Length;

            //Split all structure into blocks
            string[] pyramidBlocks = pyramid.Split(new char[] { STAGE_SPLITER, BLOCK_SPLITER });
            BinaryTree TreeGraph = ConvertToBinaryTree(ref pyramidBlocks, Hight);
            BinaryTree.TrimTree(TreeGraph.Root);
            Console.WriteLine("Max Sum: {0}",TreeGraph.CountMaxSum(TreeGraph.Root));
            Console.WriteLine("Path {0}", TreeGraph.PrintPath(TreeGraph.Root));
            Console.ReadKey();
        }
        static bool IsOdd(int? digit)
        {
            return (digit % 2 == 0) ? false : true;
        }
        static BinaryTree ConvertToBinaryTree(ref string[] treeBlocks, int Hight)
        {
            BinaryNode[] nodes = Array.ConvertAll(treeBlocks, value => new BinaryNode(null, null, int.Parse(value)));
            BinaryTree tree = new BinaryTree(nodes[0]);

            //Checks if head node should be looking odd or even number below first
            int addOne = IsOdd(nodes[0].Value) ? 0 : 1;
            bool oddInNextLevel;

            //Return value if Height is only 1
            if (Hight < 2)
            {
                return tree;
            }
            //for each level starting from top
            for (int level = 1 ; level < Hight; level++)
            {
                //Check is important in level
                oddInNextLevel = IsOdd(level + 1 + addOne);

                //foreach block per level
                for (int block = 0; block < level; block++)
                {
                    //Level start index in binary tree =>  Xn = N(N+1)/2

                    //      /ODD\
                    //     /EVEN \
                    //    /  ODD  \
                    //   /  EVEN   \

                    //Check is important in level
                    if (oddInNextLevel == IsOdd(nodes[((level + 1) * level / 2 + block)].Value))
                    {
                        nodes[(level * (level - 1) / 2 + block)].AddLeftChild(nodes[((level + 1) * level / 2 + block)]);
                    }
                    if(oddInNextLevel == IsOdd(nodes[((level + 1) * level / 2 + block + 1)].Value))
                    {
                        nodes[(level * (level - 1) / 2 + block)].AddRightChild(nodes[((level + 1) * level / 2 + block + 1)]);
                    }
                }
            }
            return tree;
        }
    }
    class BinaryTree
    {
        public BinaryNode Root { get; private set; }

        public BinaryTree(BinaryNode root)
        {
            this.Root = root;
        }

        public static int? TrimTree(BinaryNode start)
        {
            int? MaxSum = start.Value;
            if (start.LeftChild != null && start.RightChild == null)//left
            {
                MaxSum += TrimTree(start.LeftChild);//RecursionToLeftChild
            }
            else if (start.LeftChild == null && start.RightChild != null)//right
            {
                MaxSum += TrimTree(start.RightChild);//RecursionToRightChild
            }
            else if (start.LeftChild != null && start.RightChild != null)//both
            {
                int? leftTemp = TrimTree(start.LeftChild);//RecursionToLeftChild
                int? rightTemp = TrimTree(start.RightChild);//RecursionToRightChild

                //removes smaller branch
                if (leftTemp >= rightTemp)
                {
                    MaxSum += leftTemp;
                    start.RemoveRightChild();
                }
                else
                {
                    MaxSum += rightTemp;
                    start.RemoveLeftChild();
                }
            }
            else if (start.LeftChild == null && start.RightChild == null)//End
            {
                // Console.WriteLine("End");
            }
            else//Error
            {

            }
            return MaxSum;
        }

        internal int? CountMaxSum(BinaryNode node)
        {
            int? sum = node.Value;
            if (node.LeftChild == null && node.RightChild == null)
            {
                return sum;
            }
            else if(node.LeftChild != null)
            {
                sum  += CountMaxSum(node.LeftChild);
            }
            else if (node.RightChild != null)
            {
                sum += CountMaxSum(node.RightChild);
            }
            else
            {
                //error
            }
            return sum;
        }

        internal string PrintPath(BinaryNode node)
        {
            string _path = node.Value.ToString();
            if (node.LeftChild == null && node.RightChild == null)
            {
                return _path;
            }
            else if (node.LeftChild != null)
            {
                _path += ", " +PrintPath(node.LeftChild);
            }
            else if (node.RightChild != null)
            {
                _path += ", " +PrintPath(node.RightChild);
            }
            else
            {
                //error
            }
            return _path;
        }
    }
    class BinaryNode
    {
        public int? Value { get; private set; }
        public BinaryNode LeftChild { get; private set; }
        public BinaryNode RightChild { get; private set; }

        public BinaryNode(BinaryNode left, BinaryNode right, int? value)
        {
            this.Value = value; 

            this.LeftChild = left;
            this.RightChild = right;
            this.Value = value;
        }

        internal void AddLeftChild(BinaryNode node)
        {
            this.LeftChild = node;
        }

        internal void AddRightChild(BinaryNode node)
        {
            this.RightChild = node;
        }

        internal void Remove(BinaryNode node)
        {
            this.RightChild = node;
        }

        internal void RemoveLeftChild()
        {
            this.LeftChild = null;
        }

        internal void RemoveRightChild()
        {
            this.RightChild = null;
        }
    }
}


