using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace grafo.src.complete
{
    /// <summary>
    /// Represents a node in the Directed Acyclic Graph
    /// </summary>
    /// <typeparam name="T">The type of data stored in the node</typeparam>
    public class Node<T>
    {
        public T Data { get; set; }
        public List<Node<T>> Children { get; private set; }
        public List<Node<T>> Parents { get; private set; }
        public string Name { get; set; }

        public Node(T data, string name = null)
        {
            Data = data;
            Name = name ?? data.ToString();
            Children = new List<Node<T>>();
            Parents = new List<Node<T>>();
        }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Represents a Directed Acyclic Graph (DAG)
    /// </summary>
    /// <typeparam name="T">The type of data stored in each node</typeparam>
    public class DirectedAcyclicGraph<T>
    {
        private readonly Dictionary<string, Node<T>> _nodes;

        public DirectedAcyclicGraph()
        {
            _nodes = new Dictionary<string, Node<T>>();
        }

        /// <summary>
        /// Adds a node to the graph
        /// </summary>
        /// <param name="data">The data to store in the node</param>
        /// <param name="name">The name of the node (optional)</param>
        /// <returns>The newly created node</returns>
        public Node<T> AddNode(T data, string name = null)
        {
            name = name ?? data.ToString();
            
            if (_nodes.ContainsKey(name))
            {
                throw new ArgumentException($"Node with name '{name}' already exists.");
            }

            var node = new Node<T>(data, name);
            _nodes.Add(name, node);
            return node;
        }

        /// <summary>
        /// Gets a node by its name
        /// </summary>
        /// <param name="name">The name of the node to get</param>
        /// <returns>The node with the specified name</returns>
        public Node<T> GetNode(string name)
        {
            if (!_nodes.ContainsKey(name))
            {
                throw new KeyNotFoundException($"Node with name '{name}' not found.");
            }

            return _nodes[name];
        }

        /// <summary>
        /// Adds a directed edge from source node to destination node
        /// </summary>
        /// <param name="sourceName">The name of the source node</param>
        /// <param name="destinationName">The name of the destination node</param>
        /// <returns>True if the edge was added, false if it would create a cycle</returns>
        public bool AddEdge(string sourceName, string destinationName)
        {
            var sourceNode = GetNode(sourceName);
            var destinationNode = GetNode(destinationName);
            
            return AddEdge(sourceNode, destinationNode);
        }

        /// <summary>
        /// Adds a directed edge from source node to destination node
        /// </summary>
        /// <param name="source">The source node</param>
        /// <param name="destination">The destination node</param>
        /// <returns>True if the edge was added, false if it would create a cycle</returns>
        public bool AddEdge(Node<T> source, Node<T> destination)
        {
            // Check if adding this edge would create a cycle
            if (WouldCreateCycle(source, destination))
            {
                return false;
            }

            // Add the edge
            source.Children.Add(destination);
            destination.Parents.Add(source);
            
            return true;
        }

        /// <summary>
        /// Checks if adding an edge from source to destination would create a cycle
        /// </summary>
        /// <param name="source">The source node</param>
        /// <param name="destination">The destination node</param>
        /// <returns>True if a cycle would be created, false otherwise</returns>
        private bool WouldCreateCycle(Node<T> source, Node<T> destination)
        {
            // If destination is already a parent (direct or indirect) of source, adding an edge would create a cycle
            var visited = new HashSet<Node<T>>();
            var queue = new Queue<Node<T>>();
            
            queue.Enqueue(destination);
            
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                
                if (current == source)
                {
                    return true;
                }
                
                if (visited.Contains(current))
                {
                    continue;
                }
                
                visited.Add(current);
                
                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
            
            return false;
        }

        /// <summary>
        /// Removes an edge from the graph
        /// </summary>
        /// <param name="sourceName">The name of the source node</param>
        /// <param name="destinationName">The name of the destination node</param>
        /// <returns>True if the edge was removed, false if it didn't exist</returns>
        public bool RemoveEdge(string sourceName, string destinationName)
        {
            var sourceNode = GetNode(sourceName);
            var destinationNode = GetNode(destinationName);
            
            return RemoveEdge(sourceNode, destinationNode);
        }

        /// <summary>
        /// Removes an edge from the graph
        /// </summary>
        /// <param name="source">The source node</param>
        /// <param name="destination">The destination node</param>
        /// <returns>True if the edge was removed, false if it didn't exist</returns>
        public bool RemoveEdge(Node<T> source, Node<T> destination)
        {
            bool childRemoved = source.Children.Remove(destination);
            bool parentRemoved = destination.Parents.Remove(source);
            
            return childRemoved && parentRemoved;
        }

        /// <summary>
        /// Removes a node from the graph and all its edges
        /// </summary>
        /// <param name="name">The name of the node to remove</param>
        /// <returns>True if the node was removed, false if it didn't exist</returns>
        public bool RemoveNode(string name)
        {
            if (!_nodes.ContainsKey(name))
            {
                return false;
            }
            
            var node = _nodes[name];
            
            // Remove all edges where this node is the source
            foreach (var child in node.Children.ToList())
            {
                RemoveEdge(node, child);
            }
            
            // Remove all edges where this node is the destination
            foreach (var parent in node.Parents.ToList())
            {
                RemoveEdge(parent, node);
            }
            
            // Remove the node itself
            return _nodes.Remove(name);
        }

        /// <summary>
        /// Gets all nodes in topological order
        /// </summary>
        /// <returns>A list of nodes in topological order</returns>
        public List<Node<T>> GetTopologicalOrder()
        {
            var result = new List<Node<T>>();
            var visited = new HashSet<Node<T>>();
            var temporaryMarks = new HashSet<Node<T>>();
            
            foreach (var node in _nodes.Values)
            {
                if (!visited.Contains(node))
                {
                    TopologicalSortVisit(node, visited, temporaryMarks, result);
                }
            }
            
            result.Reverse();
            return result;
        }
        
        private void TopologicalSortVisit(Node<T> node, HashSet<Node<T>> visited, HashSet<Node<T>> temporaryMarks, List<Node<T>> result)
        {
            if (temporaryMarks.Contains(node))
            {
                throw new InvalidOperationException("Graph has a cycle and is not a DAG.");
            }
            
            if (!visited.Contains(node))
            {
                temporaryMarks.Add(node);
                
                foreach (var child in node.Children)
                {
                    TopologicalSortVisit(child, visited, temporaryMarks, result);
                }
                
                temporaryMarks.Remove(node);
                visited.Add(node);
                result.Add(node);
            }
        }

        /// <summary>
        /// Prints the graph structure
        /// </summary>
        public void PrintGraph()
        {
            Console.WriteLine("Graph Structure:");
            foreach (var node in _nodes.Values)
            {
                Console.Write($"{node.Name} -> ");
                if (node.Children.Count == 0)
                {
                    Console.WriteLine("[No children]");
                }
                else
                {
                    Console.WriteLine(string.Join(", ", node.Children.Select(n => n.Name)));
                }
            }
        }

        /// <summary>
        /// Gets all paths from source to destination
        /// </summary>
        /// <param name="sourceName">The name of the source node</param>
        /// <param name="destinationName">The name of the destination node</param>
        /// <returns>A list of all possible paths</returns>
        public List<List<Node<T>>> GetAllPaths(string sourceName, string destinationName)
        {
            var source = GetNode(sourceName);
            var destination = GetNode(destinationName);
            
            var paths = new List<List<Node<T>>>();
            var currentPath = new List<Node<T>>();
            
            FindAllPaths(source, destination, currentPath, paths);
            
            return paths;
        }
        
        private void FindAllPaths(Node<T> current, Node<T> destination, List<Node<T>> currentPath, List<List<Node<T>>> paths)
        {
            currentPath.Add(current);
            
            if (current == destination)
            {
                paths.Add(new List<Node<T>>(currentPath));
            }
            else
            {
                foreach (var child in current.Children)
                {
                    if (!currentPath.Contains(child))
                    {
                        FindAllPaths(child, destination, currentPath, paths);
                    }
                }
            }
            
            currentPath.RemoveAt(currentPath.Count - 1);
        }
    }

    // Example usage class
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a new DAG
            var graph = new DirectedAcyclicGraph<string>();
            
            // Add nodes
            graph.AddNode("Task 1", "T1");
            graph.AddNode("Task 2", "T2");
            graph.AddNode("Task 3", "T3");
            graph.AddNode("Task 4", "T4");
            graph.AddNode("Task 5", "T5");
            
            // Add edges
            graph.AddEdge("T1", "T2");
            graph.AddEdge("T1", "T3");
            graph.AddEdge("T2", "T4");
            graph.AddEdge("T3", "T4");
            graph.AddEdge("T4", "T5");
            
            // Print the graph
            graph.PrintGraph();
            
            // Get topological order
            var order = graph.GetTopologicalOrder();
            Console.WriteLine("\nTopological Order:");
            Console.WriteLine(string.Join(" -> ", order));
            
            // Find all paths from T1 to T5
            var paths = graph.GetAllPaths("T1", "T5");
            Console.WriteLine("\nAll paths from T1 to T5:");
            foreach (var path in paths)
            {
                Console.WriteLine(string.Join(" -> ", path));
            }
            
            // Try to add an edge that would create a cycle (should return false)
            bool added = graph.AddEdge("T5", "T1");
            Console.WriteLine($"\nAdded edge T5 -> T1: {added}");
            
            // Remove an edge
            bool removed = graph.RemoveEdge("T1", "T3");
            Console.WriteLine($"\nRemoved edge T1 -> T3: {removed}");
            
            // Print updated graph
            Console.WriteLine("\nUpdated graph:");
            graph.PrintGraph();
        }
    }
}