using Classes;
using Gtk;
using Global;
using System.Text;

namespace Structures
{
    public class BTree
    {
        private BNode root;
        private readonly int order;
        private readonly int maxKeys;
        private readonly int minKeys;

        public BTree(int order)
        {
            this.order = order;
            maxKeys = order - 1;
            minKeys = (order / 2) - 1;
            root = new BNode(order, maxKeys, minKeys);
        }

        public void Insert(Invoice invoice)
        {
            if (root.IsFull())
            {
                BNode newRoot = new BNode(order, maxKeys, minKeys);
                newRoot.IsLeaf = false;
                newRoot.Children.Add(root);
                SplitChild(newRoot, 0);
                root = newRoot;
            }

            InsertNotFilled(root, invoice);
        }

        private void SplitChild(BNode father, int index)
        {
            BNode child = father.Children[index];
            BNode newChild = new BNode(order, maxKeys, minKeys);
            newChild.IsLeaf = child.IsLeaf;
            Invoice middleInvoice = child.Keys[minKeys];

            for (int i = minKeys + 1; i < maxKeys; i++)
            {
                newChild.Keys.Add(child.Keys[i]);
            }

            if (!child.IsLeaf)
            {
                for (int i = minKeys + 1; i < order; i++)
                {
                    newChild.Children.Add(child.Children[i]);
                }
                child.Keys.RemoveRange(minKeys + 1, child.Children.Count - (minKeys + 1));
            }

            child.Keys.RemoveRange(minKeys, child.Keys.Count - minKeys);

            father.Children.Insert(index + 1, newChild);

            father.InsertInvoice(middleInvoice);
        }

        public void InsertNotFilled(BNode node, Invoice invoice)
        {
            if (node.IsLeaf)
            {
                node.InsertInvoice(invoice);
            }
            else
            {
                int i = node.GetInsertIndex(invoice.Id);

                if (node.Children[i].IsFull())
                {
                    SplitChild(node, i);

                    if (invoice.Id > node.Keys[i].Id)
                    {
                        i++;
                    }
                }

                InsertNotFilled(node.Children[i], invoice);
            }
        }

        public Invoice Get(int id)
        {
            return GetRecursive(root, id);
        }

        public Invoice GetRecursive(BNode node, int id)
        {
            int i = node.GetSearchIndex(id);

            if (i < node.Keys.Count && id == node.Keys[i].Id)
            {
                return node.Keys[i];
            }

            if (node.IsLeaf)
            {
                return null;
            }

            return GetRecursive(node.Children[i], id);
        }

        public ListStore GetUserInvoices(int userId)
        {
            ListStore result = new ListStore(typeof(int), typeof(int), typeof(string));
            GetUserInvoicesRecursive(root, result, userId);
            return result;
        }

        public void GetUserInvoicesRecursive(BNode node, ListStore result, int userId)
        {
            if (node == null) return;

            foreach (var invoice in node.Keys)
            {
                Service service = GlobalStructures.ServicesTree.Get(invoice.ServiceId);
                if (service != null)
                {
                    Vehicle vehicle = GlobalStructures.VehiclesList.Get(service.VehicleId);
                    if (vehicle != null && vehicle.UserId == userId)
                        result.AppendValues(invoice.Id, invoice.ServiceId, invoice.Total.ToString("F2"));
                }
            }

            if (!node.IsLeaf)
            {
                foreach (var child in node.Children)
                {
                    GetUserInvoicesRecursive(child, result, userId);
                }
            }
        }

        public void Delete(int id)
        {
            DeleteRecursive(root, id);

            if (root.Keys.Count == 0 && !root.IsLeaf)
            {
                BNode oldRoot = root;
                root = root.Children[0];
            }
        }

        private void DeleteRecursive(BNode node, int id)
        {
            int index = node.GetSearchIndex(id);

            if (index < node.Keys.Count && node.Keys[index].Id == id)
            {
                if (node.IsLeaf)
                {
                    node.Keys.RemoveAt(index);
                }
                else
                {
                    DeleteInternalNode(node, index);
                }
            }
            else
            {
                if (node.IsLeaf)
                {
                    return;
                }

                bool lastChild = index == node.Keys.Count;

                if (node.Children[index].IsUnderflow())
                {
                    FillChild(node, index);
                }

                if (lastChild && index > node.Children.Count - 1)
                {
                    DeleteRecursive(node.Children[index - 1], id);
                }
                else
                {
                    DeleteRecursive(node.Children[index], id);
                }
            }
        }

        private void DeleteInternalNode(BNode node, int index)
        {
            Invoice invoice = node.Keys[index];

            if (node.Children[index].Keys.Count > minKeys)
            {
                Invoice predecessor = GetPredecessor(node, index);
                node.Keys[index] = predecessor;
                DeleteRecursive(node.Children[index], predecessor.Id);
            }
            else if (node.Children[index + 1].Keys.Count > minKeys)
            {
                Invoice successor = GetSuccessor(node, index);
                node.Keys[index] = successor;
                DeleteRecursive(node.Children[index + 1], successor.Id);
            }
            else
            {
                MergeNodes(node, index);
                DeleteRecursive(node.Children[index], invoice.Id);
            }
        }

        private Invoice GetPredecessor(BNode node, int index)
        {
            BNode current = node.Children[index];
            while (!current.IsLeaf)
            {
                current = current.Children[current.Keys.Count];
            }
            return current.Keys[current.Keys.Count - 1];
        }

        private Invoice GetSuccessor(BNode node, int index)
        {
            BNode current = node.Children[index + 1];
            while (!current.IsLeaf)
            {
                current = current.Children[0];
            }
            return current.Keys[0];
        }

        private void FillChild(BNode node, int index)
        {
            if (index > 0 && node.Children[index - 1].Keys.Count > minKeys)
            {
                BorrowFromPrevious(node, index);
            }
            else if (index < node.Keys.Count && node.Children[index + 1].Keys.Count > minKeys)
            {
                BorrowFromNext(node, index);
            }
            else
            {
                if (index < node.Keys.Count)
                {
                    MergeNodes(node, index);
                }
                else
                {
                    MergeNodes(node, index - 1);
                }
            }
        }

        private void MergeNodes(BNode node, int index)
        {
            BNode child = node.Children[index];
            BNode sibling = node.Children[index + 1];

            child.Keys.Add(node.Keys[index]);

            for (int i = 0; i < sibling.Keys.Count; i++)
            {
                child.Keys.Add(sibling.Keys[i]);
            }

            if (!child.IsLeaf)
            {
                for (int i = 0; i < sibling.Children.Count; i++)
                {
                    child.Children.Add(sibling.Children[i]);
                }
            }

            node.Keys.RemoveAt(index);
            node.Children.RemoveAt(index + 1);
        }

        private void BorrowFromPrevious(BNode node, int index)
        {
            BNode child = node.Children[index];
            BNode sibling = node.Children[index - 1];

            child.Keys.Insert(0, node.Keys[index - 1]);

            if (!child.IsLeaf)
            {
                child.Children.Insert(0, sibling.Children[sibling.Keys.Count]);
                sibling.Children.RemoveAt(sibling.Keys.Count);
            }

            node.Keys[index - 1] = sibling.Keys[sibling.Keys.Count - 1];
            sibling.Keys.RemoveAt(sibling.Keys.Count - 1);
        }

        private void BorrowFromNext(BNode node, int index)
        {
            BNode child = node.Children[index];
            BNode sibling = node.Children[index + 1];

            child.Keys.Add(node.Keys[index]);

            if (!child.IsLeaf)
            {
                child.Children.Add(sibling.Children[0]);
                sibling.Children.RemoveAt(0);
            }

            node.Keys[index] = sibling.Keys[0];
            sibling.Keys.RemoveAt(0);
        }

        public bool IsEmpty()
        {
            return root.Keys.Count == 0;
        }

        public string GenerateDot()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph BTree {");
            sb.AppendLine("    node [shape=record];");
            sb.AppendLine("    rankdir=TB;");
            sb.AppendLine("    subgraph cluster_0 {");
            sb.AppendLine("        label = \"Árbol B\";");

            int nodeCounter = 0;
            GenerateDotRecursive(root, sb, ref nodeCounter);

            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private void GenerateDotRecursive(BNode node, StringBuilder sb, ref int nodeCounter)
        {
            if (node == null)
                return;

            int currentNode = nodeCounter++;

            StringBuilder nodeLabel = new StringBuilder();
            nodeLabel.Append($"        n{currentNode} [label=\"");

            for (int i = 0; i < node.Keys.Count; i++)
            {
                if (i > 0)
                    nodeLabel.Append("|");
                nodeLabel.Append($"<f{i}> |Id: {node.Keys[i].Id}, Orden: {node.Keys[i].ServiceId}, Total: {node.Keys[i].Total}|");
            }

            if (node.Keys.Count > 0)
                nodeLabel.Append($"<f{node.Keys.Count}>");

            nodeLabel.Append("\"];");
            sb.AppendLine(nodeLabel.ToString());

            if (!node.IsLeaf)
            {
                for (int i = 0; i <= node.Keys.Count; i++)
                {
                    int childPosition = nodeCounter;
                    GenerateDotRecursive(node.Children[i], sb, ref nodeCounter);
                    sb.AppendLine($"        n{currentNode}:f{i} -> n{childPosition};");
                }
            }
        }
    }
}