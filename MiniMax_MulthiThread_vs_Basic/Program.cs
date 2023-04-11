using System;
using System.Collections.Generic;
using System.Diagnostics;

class Node
{
    public int data;
    public List<Node> children;

    public Node(int data)
    {
        this.data = data;
        this.children = new List<Node>();
    }

    public void AddChild(Node child)
    {
        this.children.Add(child);
    }
}

class Tree
{
    public Node root;

    public Tree()
    {
        this.root = null;
    }

    public void CreateTree(int depth, int numChildren)
    {
        Random random = new Random();
        Queue<Node> queue = new Queue<Node>();
        this.root = new Node(random.Next(1, 100));
        queue.Enqueue(this.root);
        while (queue.Count > 0 && depth > 0)
        {
            int size = queue.Count;
            for (int i = 0; i < size; i++)
            {
                Node node = queue.Dequeue();
                for (int j = 0; j < numChildren; j++)
                {
                    Node child = new Node(random.Next(1, 100));
                    node.AddChild(child);
                    queue.Enqueue(child);
                }
            }
            depth--;
        }
    }

    public int PrintTree()
    {
        return PrintTreeHelper(this.root, "");
    }

    private int PrintTreeHelper(Node node, string indent)
    {
        if (node == null) return 0;
       // Console.WriteLine(indent + node.data);                //друк дерева
        int count = 1;
        foreach (Node child in node.children)
        {
            count += PrintTreeHelper(child, indent + "  ");
        }
        return count;
    }

    public int Minimax()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        int result = MinimaxHelper(this.root, true);

        stopwatch.Stop();
        Console.WriteLine("Minimax algorithm took " + stopwatch.ElapsedMilliseconds + " milliseconds");
        return result;
    }

    private int MinimaxHelper(Node node, bool isMaximizing)
    {
        if (node.children.Count == 0)
        {
            return node.data;
        }

        if (isMaximizing)
        {
            int bestValue = int.MinValue;
            foreach (Node child in node.children)
            {
                int value = MinimaxHelper(child, false);
                bestValue = Math.Max(bestValue, value);
            }
            return bestValue;
        }
        else
        {
            int bestValue = int.MaxValue;
            foreach (Node child in node.children)
            {
                int value = MinimaxHelper(child, true);
                bestValue = Math.Min(bestValue, value);
            }
            return bestValue;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Tree tree = new Tree();
        int depth = 20;
        int numChildren = 2;

        tree.CreateTree(depth, numChildren);
        int totalNodes = tree.PrintTree();
        Console.WriteLine("-----------------------");
        Console.WriteLine("The total number of nodes in the tree is " + totalNodes);
        Console.WriteLine("-----------------------");
        int result = tree.Minimax();

        Console.WriteLine("The result of the minimax algorithm is " + result);
    }
}
