using System.Text;
using Classes;

namespace Structures
{
    public class BinaryTree
    {
        private BinaryNode root { get; set; }

        public BinaryTree()
        {
            root = null;
        }

        public void Insert(Service data)
        {
            if (root == null)
                root = new BinaryNode(data);
            else
                InsertRecursive(root, data);                
        }

        private void InsertRecursive(BinaryNode node, Service data)
        {
            if (data.Id < node.Data.Id)
            {
                if (node.Left == null)
                    node.Left = new BinaryNode(data);
                else
                    InsertRecursive(node.Left, data);
            }
            else if (data.Id > node.Data.Id)
            {
                if (node.Right == null)
                    node.Right = new BinaryNode(data);
                else
                    InsertRecursive(node.Right, data);
            }
            else
                return;
        }

        public Service Get(int id)
        {
            return GetRecursive(root, id);
        }

        private Service GetRecursive(BinaryNode node, int id)
        {
            if (node == null) return null;

            if (id < node.Data.Id)
                return GetRecursive(node.Left, id);
            else if (id > node.Data.Id)
                return GetRecursive(node.Right, id);
            else
                return node.Data;
        }

        public string GenerateDot()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph BinaryTree {");
            sb.AppendLine("    node [shape=box];");
            sb.AppendLine("    rankdir=TB;");
            sb.AppendLine("    label = \"Esteban Sánchez\";\n");
            GenerateDotNodes(root, sb);
            GenerateDotConnections(root, sb);
            sb.AppendLine("}");
            return sb.ToString();
        }

        public void GenerateDotNodes(BinaryNode node, StringBuilder sb)
        {
            if (node == null) return;
            sb.Append("    ").Append(node.Data.ToDotNode()).AppendLine();
            GenerateDotNodes(node.Left, sb);
            GenerateDotNodes(node.Right, sb);
        }

        public void GenerateDotConnections(BinaryNode node, StringBuilder sb)
        {
            if (node == null) return;
            if (node.Left != null)
                sb.AppendLine($"    \"{node.Data.Id}\" -> \"{node.Left.Data.Id}\";");
            if (node.Right != null)
                sb.AppendLine($"    \"{node.Data.Id}\" -> \"{node.Right.Data.Id}\";");
            GenerateDotConnections(node.Left, sb);
            GenerateDotConnections(node.Right, sb);
        }
    }
}