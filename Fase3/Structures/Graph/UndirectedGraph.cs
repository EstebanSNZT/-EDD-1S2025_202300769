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
            var graph = "graph UndirectedGraph {\n";
            graph += "    node [shape=ellipse];\n";
            graph += "    rankdir=TB;\n";
            graph += "    subgraph cluster_0 {\n";
            graph += "        label = \"Esteban Sánchez - 202300769\";\n";
            foreach (var node in adjacencyList)
            {
                string currentId = node.Key;
                graph += $"        {currentId} [label=\"{currentId}\"];\n";
            }

            foreach (var node in adjacencyList.Where(node => node.Key.StartsWith("V")))
            {
                string currentId = node.Key;
                List<string> nodeConnections = node.Value;

                foreach (var connection in nodeConnections)
                {
                    if (connection.StartsWith("R"))
                    {
                        graph += $"        {currentId} -- {connection};\n";
                    }
                }
            }

            if(!IsEmpty())
            {
                graph += $"        {RankSameNodes("V")}";
                graph += $"        {RankSameNodes("R")}";
            }

            graph += "    }\n";
            graph += "}\n";
            return graph;
        }

        private string RankSameNodes(string start)
        {
            var rank = "{ rank=same; ";
            foreach (var id in adjacencyList.Keys.Where(id => id.StartsWith(start)))
            {
                rank += $"{id}; ";
            }
            rank += "};\n";
            return rank;
        }
    }
}