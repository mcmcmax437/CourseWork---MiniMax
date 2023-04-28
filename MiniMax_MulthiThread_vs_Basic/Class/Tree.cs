using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MiniMax_MulthiThread_vs_Basic.Class
{
     class Tree
    {

        public Node Root { get; private set; }

        public Tree(int depth, int numChildren)
        {
            Random random = new Random();
            Root = GenerateTree(depth, numChildren, random);
        }

        private Node GenerateTree(int depth, int numChildren, Random random)
        {
            int value = random.Next(1, 100);
            Node node = new Node(value);

            if (depth > 0)
            {
                for (int i = 0; i < numChildren; i++)
                {
                    node.Children.Add(GenerateTree(depth - 1, numChildren, random));
                }
            }

            return node;
        }
    }
}
