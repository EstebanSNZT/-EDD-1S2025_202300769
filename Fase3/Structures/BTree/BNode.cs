using Classes;

namespace Structures
{
    public class BNode
    {
        public List<Invoice> Keys { get; set; }
        public List<BNode> Children { get; set; }
        public bool IsLeaf { get; set; }
        private int maxKeys;
        private int minKeys;

        public BNode(int order, int maxKeys, int minKeys)
        {
            Keys = new List<Invoice>(maxKeys);
            Children = new List<BNode>(order);
            IsLeaf = true;
            this.maxKeys = maxKeys;
            this.minKeys = minKeys;
        }

        public void InsertInvoice(Invoice invoice)
        {
            Keys.Insert(GetInsertIndex(invoice.Id), invoice);
        }

        public int GetInsertIndex(int id)
        {
            int i = Keys.Count - 1;

            while (i >= 0 && id < Keys[i].Id)
            {
                i--;
            }

            return i + 1;
        }

        public int GetSearchIndex(int id)
        {
            int i = 0;

            while (i < Keys.Count && id > Keys[i].Id)
            {
                i++;
            }

            return i;
        }

        public bool IsFull()
        {
            return Keys.Count == maxKeys;
        }

        public bool IsUnderflow()
        {
            return Keys.Count < minKeys;
        }

    }
}