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

        public void Add(SparePart data)
        {
            root = AddRecursive(root, data);
        }

        private AVLNode AddRecursive(AVLNode node, SparePart data)
        {
            if (node == null) return new AVLNode(data);

            if (data.Id < node.Data.Id)
                node.Left = AddRecursive(node.Left, data);
            else if (data.Id > node.Data.Id)
                node.Right = AddRecursive(node.Right, data);
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

        public string PlainText()
        {
            StringBuilder text = new StringBuilder();
            PlainTextRecursive(root, text);
            Console.WriteLine($"Árbol AVL en Texto Plano:\n{text}");
            return text.ToString();
        }

        public void PlainTextRecursive(AVLNode node, StringBuilder text)
        {
            if (node == null) return;

            PlainTextRecursive(node.Left, text);
            text.AppendLine(node.Data.ToString());
            PlainTextRecursive(node.Right, text);
        }

        public void LoadPlainText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("El texto proporcionado está vacío.");
                return;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("El texto proporcionado está vacío.");
                return;
            }

            string[] lines = text.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(',');

                if (parts.Length != 4)
                {
                    Console.WriteLine($"Línea ignorada (formato incorrecto): {line}");
                    continue;
                }

                if (!int.TryParse(parts[0], out int id) || !double.TryParse(parts[3], out double cost))
                {
                    Console.WriteLine($"Línea ignorada (datos numéricos inválidos): {line}");
                    continue;
                }

                string spare = parts[1];
                string details = parts[2];

                if (Contains(id))
                {
                    Console.WriteLine($"Línea ignorada (ID ya existe): {line}");
                    continue;
                }
                else
                {
                    SparePart sparePart = new SparePart(id, spare, details, cost);
                    Add(sparePart);
                }
            }
        }

        public string GenerateDot()
        {
            StringBuilder graph = new StringBuilder();
            graph.AppendLine("digraph ALVTree {");
            graph.AppendLine("    node [shape=box];");
            graph.AppendLine("    rankdir=TB;");
            graph.AppendLine("    subgraph cluster_0 {");
            graph.AppendLine("        label = \"Árbol AVL\";");
            GenerateDotNodes(root, graph);
            GenerateDotConnections(root, graph);
            graph.AppendLine("    }");
            graph.AppendLine("}");
            return graph.ToString();
        }

        public void GenerateDotNodes(AVLNode node, StringBuilder graph)
        {
            if (node == null) return;
            graph.Append("        ").Append(node.ToDotNode()).AppendLine();
            GenerateDotNodes(node.Left, graph);
            GenerateDotNodes(node.Right, graph);
        }

        public void GenerateDotConnections(AVLNode node, StringBuilder graph)
        {
            if (node == null) return;
            if (node.Left != null)
                graph.AppendLine($"        \"{node.Data.Id}\" -> \"{node.Left.Data.Id}\";");
            if (node.Right != null)
                graph.AppendLine($"        \"{node.Data.Id}\" -> \"{node.Right.Data.Id}\";");
            GenerateDotConnections(node.Left, graph);
            GenerateDotConnections(node.Right, graph);
        }
    }
}