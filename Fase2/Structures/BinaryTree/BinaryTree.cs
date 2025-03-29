using System.Text;
using Classes;
using Gtk;
using Global;

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

        public ListStore PreOrder(int userId)
        {
            ListStore result = new ListStore(typeof(int), typeof(int), typeof(int), typeof(string), typeof(string));
            PreOrderRecursive(root, result, userId);
            return result;
        }

        private void PreOrderRecursive(BinaryNode node, ListStore result, int userId)
        {
            if (node == null) return;

            Service service = node.Data;
            Vehicle vehicle = GlobalStructures.VehiclesList.Get(service.VehicleId);
            if (vehicle != null && vehicle.UserId == userId)
                result.AppendValues(service.Id, service.SparePartId, service.VehicleId, service.Details, service.Cost.ToString("F2"));

            PreOrderRecursive(node.Left, result, userId);
            PreOrderRecursive(node.Right, result, userId);
        }

        public ListStore InOrder(int userId)
        {
            ListStore result = new ListStore(typeof(int), typeof(int), typeof(int), typeof(string), typeof(string));
            InOrderRecursive(root, result, userId);
            return result;
        }

        private void InOrderRecursive(BinaryNode node, ListStore result, int userId)
        {
            if (node == null) return;

            InOrderRecursive(node.Left, result, userId);

            Service service = node.Data;
            Vehicle vehicle = GlobalStructures.VehiclesList.Get(service.VehicleId);
            if (vehicle != null && vehicle.UserId == userId)
                result.AppendValues(service.Id, service.SparePartId, service.VehicleId, service.Details, service.Cost.ToString("F2"));

            InOrderRecursive(node.Right, result, userId);
        }

        public ListStore PostOrder(int userId)
        {
            ListStore result = new ListStore(typeof(int), typeof(int), typeof(int), typeof(string), typeof(string));
            PostOrderRecursive(root, result, userId);
            return result;
        }

        private void PostOrderRecursive(BinaryNode node, ListStore result, int userId)
        {
            if (node == null) return;

            PostOrderRecursive(node.Left, result, userId);
            PostOrderRecursive(node.Right, result, userId);

            Service service = node.Data;
            Vehicle vehicle = GlobalStructures.VehiclesList.Get(service.VehicleId);
            if (vehicle != null && vehicle.UserId == userId)
                result.AppendValues(service.Id, service.SparePartId, service.VehicleId, service.Details, service.Cost.ToString("F2"));
        }

        public string GenerateDot()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph BinaryTree {");
            sb.AppendLine("    node [shape=box];");
            sb.AppendLine("    rankdir=TB;");
            sb.AppendLine("    subgraph cluster_0 {");
            sb.AppendLine("        label = \"Árbol Binario\";\n");
            GenerateDotNodes(root, sb);
            GenerateDotConnections(root, sb);
            sb.AppendLine("    }"); 
            sb.AppendLine("}");
            return sb.ToString();
        }

        public void GenerateDotNodes(BinaryNode node, StringBuilder sb)
        {
            if (node == null) return;
            sb.Append("        ").Append(node.Data.ToDotNode()).AppendLine();
            GenerateDotNodes(node.Left, sb);
            GenerateDotNodes(node.Right, sb);
        }

        public void GenerateDotConnections(BinaryNode node, StringBuilder sb)
        {
            if (node == null) return;
            if (node.Left != null)
                sb.AppendLine($"        \"{node.Data.Id}\" -> \"{node.Left.Data.Id}\";");
            if (node.Right != null)
                sb.AppendLine($"        \"{node.Data.Id}\" -> \"{node.Right.Data.Id}\";");
            GenerateDotConnections(node.Left, sb);
            GenerateDotConnections(node.Right, sb);
        }
    }
}