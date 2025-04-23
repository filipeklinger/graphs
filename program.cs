using grafo.src.dag;
using grafo.src.shared;

System.Console.WriteLine("Hello");

// Create a new DAG
GraphDAG graph = new GraphDAG();

var nodeA = new Node("A");
var nodeB = new Node("B");
var nodeC = new Node("C");
var nodeD = new Node("D");

graph.AddEdge(nodeA,nodeB);
graph.AddEdge(nodeA,nodeC);
graph.AddEdge(nodeB,nodeD);
graph.AddEdge(nodeC,nodeD);

graph.Print();
System.Console.WriteLine("------- Try to add an edge that would create a cycle---------");
// Try to add an edge that would create a cycle
graph.AddEdge(nodeB,nodeA);
System.Console.WriteLine("----------------");
graph.Print();