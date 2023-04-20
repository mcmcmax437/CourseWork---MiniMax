using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MiniMax_MulthiThread_vs_Basic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter tree depth:");
            int depth = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter number of children per node:");
            int numChildren = int.Parse(Console.ReadLine());

            Tree tree = new Tree(depth, numChildren);

            Console.WriteLine("Starting basic minimax...");
            Stopwatch basicStopwatch = Stopwatch.StartNew();          
            int basicResult = Minimax(true, depth, tree.Root);
            basicStopwatch.Stop();

            Console.WriteLine($"Basic minimax result: {basicResult}");
            Console.WriteLine($"Time taken: {basicStopwatch.ElapsedMilliseconds}ms");

            Console.WriteLine("Starting parallel/minithreaded minimax...");
            Stopwatch parallelStopwatch = Stopwatch.StartNew();
            int parallelResult = ParallelMinimax(true, tree.Root, depth);
            parallelStopwatch.Stop();

            Console.WriteLine($"Parallel/minithreaded minimax result: {parallelResult}");
            Console.WriteLine($"Time taken: {parallelStopwatch.ElapsedMilliseconds}ms");



            long res = basicStopwatch.ElapsedMilliseconds / parallelStopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Speed Up = {basicStopwatch.ElapsedMilliseconds} / {parallelStopwatch.ElapsedMilliseconds} = {res}");

            Console.ReadLine();
        }

        static int ParallelMinimax(bool isMaximizingPlayer, Node node, int depth)
        {
            if (depth == 0 || node.Children.Count == 0)
                return node.Value;

            object lockObject = new object();
            int result = isMaximizingPlayer ? int.MinValue : int.MaxValue;
            int state = result;
            Parallel.ForEach(node.Children, () => isMaximizingPlayer ? int.MinValue : int.MaxValue, (child, state, localResult) =>
            {
                localResult = Minimax(!isMaximizingPlayer, depth - 1, child);
                result = GetResult(result, (int)localResult, isMaximizingPlayer);
                return result;
            },
            (localResult) =>
            {
                lock (lockObject)
                {
                    result = GetResult(result, localResult, isMaximizingPlayer);
                }
            });

            return result;
        }




        static int Minimax(bool isMaximizingPlayer, int depth, Node node)
        {
            if (depth == 0 || node.Children.Count == 0)
                return node.Value;

            int result = isMaximizingPlayer ? int.MinValue : int.MaxValue;

            foreach (Node child in node.Children)
            {
                int childValue = Minimax(!isMaximizingPlayer, depth - 1, child);

                result = GetResult(result, childValue, isMaximizingPlayer);
            }

            return result;
        }


        static int GetResult(int oldVa1ue, int newValue, bool isMaximizingPlayer)
        {
            return (isMaximizingPlayer && newValue > oldVa1ue) || (!isMaximizingPlayer && newValue < oldVa1ue)
                ? newValue
                : oldVa1ue;
        }


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
}

