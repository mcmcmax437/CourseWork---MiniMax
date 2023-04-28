using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMax_MulthiThread_vs_Basic.Class
{
    class Node
    {
        public int Value { get; private set; }
        public List<Node> Children { get; private set; }
        public Node(int value)
        {
            Value = value;
            Children = new List<Node>();
        }
    }
}
