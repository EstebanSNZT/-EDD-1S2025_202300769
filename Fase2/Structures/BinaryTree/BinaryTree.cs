using System.Text;
using Classes;
using Gtk;

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

        public bool Contains(int id)
        {
            return Get(id) != null;
        }

        public bool IsEmpty()
        {
            return root == null;
        }

        public ListStore PreOrder()
        {
            ListStore result = new ListStore(typeof(int), typeof(int), typeof(int), typeof(string), typeof(string));
            PreOrderRecursive(root, result);
            return result;
        }

        private void PreOrderRecursive(BinaryNode node, ListStore result)
        {
            if (node == null) return;

            Service service = node.Data;
            result.AppendValues(service.Id, service.SparePartId, service.VehicleId, service.Details, service.Cost.ToString("F2"));
            PreOrderRecursive(node.Left, result);
            PreOrderRecursive(node.Right, result);
        }

        public ListStore InOrder()
        {
            ListStore result = new ListStore(typeof(int), typeof(int), typeof(int), typeof(string), typeof(string));
            InOrderRecursive(root, result);
            return result;
        }

        private void InOrderRecursive(BinaryNode node, ListStore result)
        {
            if (node == null) return;

            Service service = node.Data;
            InOrderRecursive(node.Left, result);
            result.AppendValues(service.Id, service.SparePartId, service.VehicleId, service.Details, service.Cost.ToString("F2"));
            InOrderRecursive(node.Right, result);
        }

        public ListStore PostOrder()
        {
            ListStore result = new ListStore(typeof(int), typeof(int), typeof(int), typeof(string), typeof(string));
            PostOrderRecursive(root, result);
            return result;
        }

        private void PostOrderRecursive(BinaryNode node, ListStore result)
        {
            if (node == null) return;

            Service service = node.Data;
            PostOrderRecursive(node.Left, result);
            PostOrderRecursive(node.Right, result);
            result.AppendValues(service.Id, service.SparePartId, service.VehicleId, service.Details, service.Cost.ToString("F2"));
        }

        public string GenerateDot()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph BinaryTree {");
            sb.AppendLine("    node [shape=box];");
            sb.AppendLine("    rankdir=TB;");
            sb.AppendLine("    label = \"Árbol Binario\";\n");
            sb.AppendLine("    labelloc = \"t\";\n");
            sb.AppendLine("    fontsize = 18;\n");
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