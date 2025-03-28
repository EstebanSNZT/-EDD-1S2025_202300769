using Classes;
using Gtk;
using Global;

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
            root = new BNode(order, maxKeys);
        }

        public void Insert(Invoice invoice)
        {
            if (root.Keys.Count == maxKeys)
            {
                BNode newRoot = new BNode(order, maxKeys);
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
            BNode newChild = new BNode(order, maxKeys);
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
                int i = node.CorrectIndex(invoice.Id);

                if (node.Children[i].Keys.Count == maxKeys)
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

        public ListStore getUserInvoices(int userId)
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
    }
}