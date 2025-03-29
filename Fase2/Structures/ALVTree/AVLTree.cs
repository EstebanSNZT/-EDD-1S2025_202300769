using System.Text;
using Classes;
using Gtk;

namespace Structures
{
    public class AVLTree
    {
        private AVLNode root { get; set; }

        public AVLTree()
        {
            root = null;
        }

        private int GetHeight(AVLNode node)
        {
            return node == null ? 0 : node.Height;
        }

        private int GetBalance(AVLNode node)
        {
            return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
        }

        private AVLNode RotateRight(AVLNode y)
        {
            AVLNode x = y.Left;

            y.Left = x.Right;
            x.Right = y;

            y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));
            x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));

            return x;
        }

        private AVLNode RotateLeft(AVLNode x)
        {
            AVLNode y = x.Right;

            x.Right = y.Left;
            y.Left = x;

            x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));
            y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));

            return y;
        }

        public void Insert(SparePart data)
        {
            root = InsertRecursive(root, data);
        }

        private AVLNode InsertRecursive(AVLNode node, SparePart data)
        {
            if (node == null) return new AVLNode(data);

            if (data.Id < node.Data.Id)
                node.Left = InsertRecursive(node.Left, data);
            else if (data.Id > node.Data.Id)
                node.Right = InsertRecursive(node.Right, data);
            else
                return node;

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            int balance = GetBalance(node);

            // Left-Left
            if (balance > 1 && data.Id < node.Left.Data.Id)
                return RotateRight(node);

            // Right-Right
            if (balance < -1 && data.Id > node.Right.Data.Id)
                return RotateLeft(node);

            // Left-Right
            if (balance > 1 && data.Id > node.Left.Data.Id)
            {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }

            // Right-Left
            if (balance < -1 && data.Id < node.Right.Data.Id)
            {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            return node;
        }

        public SparePart Get(int id)
        {
            return GetRecursive(root, id);
        }

        private SparePart GetRecursive(AVLNode node, int id)
        {
            if (node == null) return null;

            if (id < node.Data.Id)
                return GetRecursive(node.Left, id);
            else if (id > node.Data.Id)
                return GetRecursive(node.Right, id);
            else
                return node.Data;
        }
        
        public bool IsEmpty()
        {
            return root == null;
        }

        public ListStore PreOrder()
        {
            ListStore result = new ListStore(typeof(int), typeof(string), typeof(string), typeof(string));
            PreOrderRecursive(root, result);
            return result;
        }

        private void PreOrderRecursive(AVLNode node, ListStore result)
        {
            if (node == null) return;
            
            SparePart sparePart = node.Data;
            result.AppendValues(sparePart.Id, sparePart.Spare, sparePart.Details, sparePart.Cost.ToString("F2"));
            PreOrderRecursive(node.Left, result);
            PreOrderRecursive(node.Right, result);
        }

        public ListStore InOrder()
        {
            ListStore result = new ListStore(typeof(int), typeof(string), typeof(string), typeof(string));
            InOrderRecursive(root, result);
            return result;
        }

        private void InOrderRecursive(AVLNode node, ListStore result)
        {
            if (node == null) return;
            
            SparePart sparePart = node.Data;
            InOrderRecursive(node.Left, result);
            result.AppendValues(sparePart.Id, sparePart.Spare, sparePart.Details, sparePart.Cost.ToString("F2"));
            InOrderRecursive(node.Right, result);
        }

        public ListStore PostOrder()
        {
            ListStore result = new ListStore(typeof(int), typeof(string), typeof(string), typeof(string));
            PostOrderRecursive(root, result);
            return result;
        }

        private void PostOrderRecursive(AVLNode node, ListStore result)
        {
            if (node == null) return;
            
            SparePart sparePart = node.Data;
            PostOrderRecursive(node.Left, result);
            PostOrderRecursive(node.Right, result);
            result.AppendValues(sparePart.Id, sparePart.Spare, sparePart.Details, sparePart.Cost.ToString("F2"));
        }

        public string GenerateDot()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph ALVTree {");
            sb.AppendLine("    node [shape=box];");
            sb.AppendLine("    rankdir=TB;");
            sb.AppendLine("    subgraph cluster_0 {");
            sb.AppendLine("        label = \"Árbol AVL\";");
            GenerateDotNodes(root, sb);
            GenerateDotConnections(root, sb);
            sb.AppendLine("    }"); 
            sb.AppendLine("}");
            return sb.ToString();
        }

        public void GenerateDotNodes(AVLNode node, StringBuilder sb)
        {
            if (node == null) return;
            sb.Append("        ").Append(node.Data.ToDotNode()).AppendLine();
            GenerateDotNodes(node.Left, sb);
            GenerateDotNodes(node.Right, sb);
        }

        public void GenerateDotConnections(AVLNode node, StringBuilder sb)
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