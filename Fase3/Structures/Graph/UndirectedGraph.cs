using System.Text;

namespace Structures
{
    public class UndirectedGraph
    {
        private readonly Dictionary<string, List<string>> adjacencyList;

        public UndirectedGraph()
        {
            adjacencyList = new Dictionary<string, List<string>>();
        }

        public void AddEdge(string nodeVehicle, string nodeSparePart)
        {
            if (string.IsNullOrEmpty(nodeVehicle) || string.IsNullOrEmpty(nodeSparePart))
            {
                throw new ArgumentException("Los nombres de los nodos no pueden ser nulos o vacíos.");
            }

            ConnectNode(nodeVehicle, nodeSparePart);
            ConnectNode(nodeSparePart, nodeVehicle);
        }

        private void ConnectNode(string nodeA, string nodeB)
        {
            if(!adjacencyList.ContainsKey(nodeA))
                adjacencyList[nodeA] = new List<string>();
            
            if(!adjacencyList[nodeA].Contains(nodeB))
                adjacencyList[nodeA].Add(nodeB);
        }

        public bool IsEmpty()
        {
            return adjacencyList.Count == 0;
        }

        public string GenerateDot()
        {
            StringBuilder graph = new StringBuilder();
            graph.AppendLine("graph UndirectedGraph {");
            graph.AppendLine("    node [shape=ellipse];");
            graph.AppendLine("    rankdir=TB;");
            graph.AppendLine("    subgraph cluster_0 {");
            graph.AppendLine("        label = \"Grafo No Dirigido\";");
            foreach (var node in adjacencyList)
            {
                string currentId = node.Key;
                graph.AppendLine($"        {currentId} [label=\"{currentId}\"];");
            }

            foreach (var node in adjacencyList.Where(node => node.Key.StartsWith("V")))
            {
                string currentId = node.Key;
                List<string> nodeConnections = node.Value;

                foreach (var connection in nodeConnections)
                {
                    if (connection.StartsWith("R"))
                    {
                        graph.AppendLine($"        {currentId} -- {connection};");
                    }
                }
            }

            if(!IsEmpty())
            {
                graph.AppendLine($"        {RankSameNodes("V")}");
                graph.AppendLine($"        {RankSameNodes("R")}");
            }

            graph.AppendLine("    }");
            graph.AppendLine("}");
            return graph.ToString();
        }

        private string RankSameNodes(string start)
        {
            var rank = "{ rank=same; ";
            foreach (var id in adjacencyList.Keys.Where(id => id.StartsWith(start)))
            {
                rank += $"{id}; ";
            }
            rank += "};";
            return rank;
        }
    }
}