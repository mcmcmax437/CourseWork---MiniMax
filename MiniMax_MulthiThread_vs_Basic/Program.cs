using MiniMax_MulthiThread_vs_Basic.Class;
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

            int numNodes = CountNodes(tree.Root);
            Console.WriteLine($"Number of nodes in the tree: {numNodes}");

            Console.WriteLine("Starting basic minimax...");
            Stopwatch basicStopwatch = Stopwatch.StartNew();          
            int basicResult = Minimax(true, depth, tree.Root);
            basicStopwatch.Stop();

            Console.WriteLine($"Basic minimax result: {basicResult}");
            Console.WriteLine($"Time taken: {basicStopwatch.ElapsedMilliseconds}ms");

            Console.WriteLine("Starting parallel/multithreaded minimax...");
            Stopwatch parallelStopwatch = Stopwatch.StartNew();
            int parallelResult = ParallelMinimax(true, tree.Root, depth);
            parallelStopwatch.Stop();

            Console.WriteLine($"Parallel/multithreaded minimax result: {parallelResult}");
            Console.WriteLine($"Time taken: {parallelStopwatch.ElapsedMilliseconds}ms");


            float parallTIME = parallelStopwatch.ElapsedMilliseconds;
            float basicTIME = basicStopwatch.ElapsedMilliseconds;
            float res = basicTIME / parallTIME;

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

        static int CountNodes(Node node)
        {
            int count = 1; 

            foreach (Node child in node.Children)
            {
                count += CountNodes(child); 
            }

            return count;
        }

    }
}

