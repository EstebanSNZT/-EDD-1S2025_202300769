using System.Text;
using Classes;
using Global;
using Gtk;

namespace Structures
{
    public class MerkleTree
    {
        private MerkleNode root;
        private List<MerkleNode> leaves;

        public MerkleTree()
        {
            root = null;
            leaves = new List<MerkleNode>();
        }

        public void Add(Invoice data)
        {
            MerkleNode newLeaf = new MerkleNode(data);
            leaves.Add(newLeaf);
            BuildTree();
        }

        private void BuildTree()
        {
            if (IsEmpty())
            {
                root = null;
                return;
            }

            List<MerkleNode> currentLevel = new List<MerkleNode>(leaves);

            while (currentLevel.Count > 1)
            {
                List<MerkleNode> nextLevel = new List<MerkleNode>();

                for (int i = 0; i < currentLevel.Count; i += 2)
                {
                    MerkleNode left = currentLevel[i];
                    MerkleNode right = (i + 1 < currentLevel.Count) ? currentLevel[i + 1] : null;
                    MerkleNode parent = new MerkleNode(left, right);
                    nextLevel.Add(parent);
                }

                currentLevel = nextLevel;
            }

            root = currentLevel[0];
        }

        public bool IsEmpty()
        {
            return leaves.Count == 0;
        }

        public ListStore GetUserInvoices(int userId)
        {
            ListStore result = new ListStore(typeof(int), typeof(int), typeof(string), typeof(string), typeof(string));

            foreach (var leaf in leaves)
            {
                Invoice invoice = leaf.Data;

                Service service = GlobalStructures.ServicesTree.Get(invoice.ServiceId);

                if (service != null)
                {
                    Vehicle vehicle = GlobalStructures.VehiclesList.Get(service.VehicleId);
                    if (vehicle != null && vehicle.UserId == userId)
                        result.AppendValues(invoice.Id, service.Id, invoice.Total.ToString("F2"), invoice.PaymentMethod, invoice.Date);
                }
            }

            return result;
        }

        public string GenerateDot()
        {
            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph MerkleTree {");
            dot.AppendLine("    node [shape=record];");
            dot.AppendLine("    rankdir=BT;");
            dot.AppendLine("    subgraph cluster_0 {");
            dot.AppendLine("        label = \"Árbol de Merkle\";");
            if (IsEmpty()) dot.AppendLine("        empty [label=\"Arbol vacío\"];");
            else
            {
                Dictionary<string, int> nodeIds = new Dictionary<string, int>();
                int idCounter = 0;
                GenerateDotNodes(root, dot, nodeIds, ref idCounter);
                GenerateDotConnections(root, dot, nodeIds);
            }
            dot.AppendLine("    }");
            dot.AppendLine("}");
            return dot.ToString();
        }

        private void GenerateDotNodes(MerkleNode node, StringBuilder dot, Dictionary<string, int> nodeIds, ref int idCounter)
        {
            if (node == null) return;

            if (!nodeIds.ContainsKey(node.Hash)) nodeIds[node.Hash] = idCounter++;
            int nodeId = nodeIds[node.Hash];

            dot.AppendLine($"        node{nodeId} {node.ToDotNode()};");

            GenerateDotNodes(node.Left, dot, nodeIds, ref idCounter);
            GenerateDotNodes(node.Right, dot, nodeIds, ref idCounter);
        }

        private void GenerateDotConnections(MerkleNode node, StringBuilder dot, Dictionary<string, int> nodeIds)
        {
            if (node == null) return;

            int nodeId = nodeIds[node.Hash];

            if (node.Left != null)
            {
                int leftId = nodeIds[node.Left.Hash];
                dot.AppendLine($"        node{leftId} -> node{nodeId};");
            }

            if (node.Right != null)
            {
                int rightId = nodeIds[node.Right.Hash];
                dot.AppendLine($"        node{rightId} -> node{nodeId};");
            }

            GenerateDotConnections(node.Left, dot, nodeIds);
            GenerateDotConnections(node.Right, dot, nodeIds);
        }
    }
}