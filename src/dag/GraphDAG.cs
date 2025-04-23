using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using grafo.src.shared;

namespace grafo.src.dag
{
    public class GraphDAG
    {
        /// <summary>
        /// Represents all nodes in graph
        /// </summary>
        private readonly Dictionary<string, Node> _nodes;
        public GraphDAG()
        {
            _nodes = [];
        }


        private void AddNode(Node node)
        {
            if (!_nodes.ContainsKey(node.Name))
            {
                _nodes[node.Name] = node;
            }
        }

        public bool AddEdge(Node source, Node destination)
        {
            AddNode(source);
            AddNode(destination);

            if (WouldCreateCycle(source, destination))
            {
                System.Console.WriteLine($"[warning] Add this Edge '{source.Name}->{destination.Name}' will create an Cycle");
                return false;
            }

            source.Neighbors.Add(destination);
            return true;
        }

        private static bool WouldCreateCycle(Node source, Node destination)
        {
            var visited = new HashSet<Node>();
            var queue = new Queue<Node>();

            //start with destination node
            queue.Enqueue(destination);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                //If we can reach the source from destination, we would create a cycle
                if (current == source)
                {
                    return true;
                }

                if (visited.Contains(current))
                {
                    continue;
                }
                visited.Add(current);

                //add neighbors to be explored from queue
                foreach (var neighbor in current.Neighbors)
                {
                    queue.Enqueue(neighbor);
                }
            }
            return false;
        }

        public void Print()
        {
            System.Console.WriteLine("Graph Structure");
            foreach (var nodeDict in _nodes)
            {
                var name = nodeDict.Key;
                var node = nodeDict.Value;
                System.Console.Write($"{name} -> ");
                if (node.Neighbors.Count == 0)
                {
                    System.Console.Write("[No connections]");
                }
                else
                {
                    var neighbors = node.Neighbors.Select(n => n.Name);
                    System.Console.Write(string.Join(", ", neighbors));
                }
                System.Console.WriteLine(" ");

            }
        }
    }
}