using Classes;

namespace Structures
{
    public class BNode
    {
        public List<Invoice> Keys { get; set; }
        public List<BNode> Children { get; set; }
        public bool IsLeaf { get; set; }

        public BNode(int order, int maxKeys)
        {
            Keys = new List<Invoice>(maxKeys);
            Children = new List<BNode>(order);
        }

        public void InsertInvoice(Invoice invoice)
        {
            Keys.Insert(CorrectIndex(invoice.Id), invoice);
        }

        public int CorrectIndex(int invoiceId)
        {
            int i = Keys.Count - 1;

            while (i >= 0 && invoiceId < Keys[i].Id)
            {
                i--;
            }

            return i + 1;
        }

    }
}