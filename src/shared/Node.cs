using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace grafo.src.shared
{
    /// <summary>
    /// Represents a Graph Node
    /// </summary>
    public class Node
    {
        public string Name { get; private set; }
        public List<Node> Neighbors { get; private set; }

        public Node(string name)
        {
            Name = name;
            Neighbors = new List<Node>();
        }
    }
}