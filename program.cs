using grafo.src.simplified;
using grafo.src.shared;
using grafo.src.basic;
using grafo.src.complete;

System.Console.WriteLine("Hello");

// Create a new DAG
GraphDAGSimple dagSimple = new GraphDAGSimple();

var nodeA = new Node("A");
var nodeB = new Node("B");
var nodeC = new Node("C");
var nodeD = new Node("D");

dagSimple.AddEdge(nodeA, nodeB);
dagSimple.AddEdge(nodeA, nodeC);
dagSimple.AddEdge(nodeB, nodeD);
dagSimple.AddEdge(nodeC, nodeD);

dagSimple.Print();
System.Console.WriteLine("------- Try to add an edge that would create a cycle---------");
// Try to add an edge that would create a cycle
dagSimple.AddEdge(nodeB, nodeA);
System.Console.WriteLine("----------------");
dagSimple.Print();

// Create a new DAG
System.Console.WriteLine("---------BasicDAG-------");
BasicDAG basicDag = new BasicDAG();

// Add some edges
basicDag.AddEdge("A", "B");
basicDag.AddEdge("A", "C");
basicDag.AddEdge("B", "D");
basicDag.AddEdge("C", "D");

// Print the graph
basicDag.PrintGraph();

// Try to add an edge that would create a cycle
bool result = basicDag.AddEdge("D", "A");
Console.WriteLine($"\nAdded edge D -> A: {result}");

// Print the graph again to confirm
basicDag.PrintGraph();

System.Console.WriteLine("------- Complete DAG -------");
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