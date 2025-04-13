using Classes;

namespace Structures
{
    public class DoublyLinkedList
    {
        public DoublyLinkedNode head;
        public DoublyLinkedNode tail;

        public DoublyLinkedList()
        {
            head = null;
            tail = null;
        }

        public void Insert(Vehicle data)
        {
            DoublyLinkedNode newNode = new DoublyLinkedNode(data);

            if (head == null)
            {
                head = tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                newNode.Prev = tail;
                tail = newNode;
            }
        }

        public bool Delete(int id)
        {
            if (head == null) return false;

            DoublyLinkedNode current = head;
            while (current != null)
            {
                if (current.Data.Id == id)
                {
                    if (current.Prev != null)
                        current.Prev.Next = current.Next;
                    else
                        head = current.Next;

                    if (current.Next != null)
                        current.Next.Prev = current.Prev;
                    else
                        tail = current.Prev;

                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        public void DeleteByUserId(int userId)
        {
            if (head == null) return;

            DoublyLinkedNode current = head;
            while (current != null)
            {
                if (current.Data.UserId == userId)
                {
                    if (current.Prev != null)
                        current.Prev.Next = current.Next;
                    else
                        head = current.Next;
                    if (current.Next != null)
                        current.Next.Prev = current.Prev;
                    else
                        tail = current.Prev;
                }
                current = current.Next;
            }
        }

        public Vehicle Get(int id)
        {
            DoublyLinkedNode current = head;
            while (current != null)
            {
                if (current.Data.Id == id) return current.Data;
                current = current.Next;
            }
            return null;
        }

        public bool IsEmpty()
        {
            return head == null;
        }

        public bool Contains(int id)
        {
            return Get(id) != null;
        }

        public void Print()
        {
            Console.WriteLine("-------------------------------------------------------------------------------");
            DoublyLinkedNode current = head;
            while (current != null)
            {
                Console.WriteLine(current.Data.ToString());
                current = current.Next;
            }
        }

        public string GenerateDot()
        {
            var graph = "digraph DoublyLinkedList {\n";
            graph += "    node [shape=record];\n";
            graph += "    rankdir=LR;\n";
            graph += "    subgraph cluster_0 {\n";
            graph += "        label = \"Lista Doblemente Enlazada\";\n";
            DoublyLinkedNode current = head;
            int index = 0;

            while (current != null)
            {
                graph += $"        n{index} {current.Data.ToDotNode()}\n";
                current = current.Next;
                index++;
            }

            for (int i = 0; i < index; i++)
            {
                if (i < index - 1)
                {
                    graph += $"        n{i} -> n{i + 1};\n";
                }
                if (i > 0)
                {
                    graph += $"        n{i} -> n{i - 1};\n";
                }
            }

            graph += "    }\n";
            graph += "}\n";
            return graph;
        }
    }
}