using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using grafo.src.shared;

namespace grafo.src.basic
{
    /// <summary>
    /// Simple Directed Acyclic Graph implementation
    /// </summary>
    public class BasicDAG
    {
        private Dictionary<string, Node> nodes;

        public BasicDAG()
        {
            nodes = new Dictionary<string, Node>();
        }

        /// <summary>
        /// Adds a node to the graph if it doesn't exist
        /// </summary>
        /// <param name="name">Name of the node</param>
        /// <returns>The node object</returns>
        public Node AddNode(string name)
        {
            if (!nodes.ContainsKey(name))
            {
                nodes[name] = new Node(name);
            }
            return nodes[name];
        }

        /// <summary>
        /// Adds a directed edge from source to destination if it won't create a cycle
        /// </summary>
        /// <param name="sourceName">Source node name</param>
        /// <param name="destinationName">Destination node name</param>
        /// <returns>True if edge was added, false if it would create a cycle</returns>
        public bool AddEdge(string sourceName, string destinationName)
        {
            // Ensure both nodes exist
            Node sourceNode = AddNode(sourceName);
            Node destinationNode = AddNode(destinationName);

            // Check if adding this edge would create a cycle
            if (WouldCreateCycle(sourceNode, destinationNode))
            {
                return false;
            }

            // Add the edge
            sourceNode.Neighbors.Add(destinationNode);
            return true;
        }

        /// <summary>
        /// Checks if adding an edge would create a cycle
        /// </summary>
        /// <param name="source">Source node</param>
        /// <param name="destination">Destination node</param>
        /// <returns>True if a cycle would be created</returns>
        private bool WouldCreateCycle(Node source, Node destination)
        {
            // If destination is already reachable from source, adding this edge would create a cycle
            HashSet<Node> visited = new HashSet<Node>();
            Queue<Node> queue = new Queue<Node>();
            
            // Start with the destination node
            queue.Enqueue(destination);
            
            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();
                
                // If we can reach the source node from destination, we would create a cycle
                if (current == source)
                {
                    return true;
                }
                
                if (visited.Contains(current))
                {
                    continue;
                }
                
                visited.Add(current);
                
                // Explore all neighbors
                foreach (var neighbor in current.Neighbors)
                {
                    queue.Enqueue(neighbor);
                }
            }
            
            return false;
        }

        /// <summary>
        /// Prints the graph structure
        /// </summary>
        public void PrintGraph()
        {
            Console.WriteLine("Graph Structure:");
            foreach (var node in nodes.Values)
            {
                Console.Write($"{node.Name} -> ");
                if (node.Neighbors.Count == 0)
                {
                    Console.WriteLine("[No connections]");
                }
                else
                {
                    List<string> neighborNames = new List<string>();
                    foreach (var neighbor in node.Neighbors)
                    {
                        neighborNames.Add(neighbor.Name);
                    }
                    Console.WriteLine(string.Join(", ", neighborNames));
                }
            }
        }
    }
}