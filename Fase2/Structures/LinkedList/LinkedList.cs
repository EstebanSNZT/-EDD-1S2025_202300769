using Classes;

namespace Structures
{
    public class LinkedList
    {
        private LinkedNode head;

        public LinkedList()
        {
            head = null;
        }

        public void Insert(User user)
        {
            LinkedNode newNode = new LinkedNode(user);

            if (head == null)
            {
                head = newNode;
            }
            else
            {
                LinkedNode current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
        }

        public bool Delete(int id)
        {
            if (head == null) return false;

            if (head.Data.Id == id)
            {
                head = head.Next;
                return true;
            }

            LinkedNode current = head;

            while (current.Next != null && !(current.Next.Data.Id == id))
            {
                current = current.Next;
            }

            if (current.Next != null)
            {
                current.Next = current.Next.Next;
                return true;
            }

            return false;
        }

        public void Print()
        {
            Console.WriteLine("-------------------------------------------------------------------------------");
            LinkedNode current = head;
            while (current != null)
            {
                Console.WriteLine(current.Data.ToString());
                current = current.Next;
            }
        }

        public User Get(int id)
        {
            LinkedNode current = head;
            while (current != null)
            {
                if (current.Data.Id == id)
                {
                    return current.Data;
                }
                current = current.Next;
            }
            return null;
        }

        public User GetByEmail(string email)
        {
            LinkedNode current = head;
            while (current != null)
            {
                if (current.Data.Email.Equals(email))
                {
                    return current.Data;
                }
                current = current.Next;
            }
            return null;
        }


        public bool Contains(int id)
        {
            return Get(id) != null;
        }

        public bool IsEmpty()
        {
            return head == null;
        }

        public string GenerateDot()
        {
            var graph = "digraph LinkedList {\n";
            graph += "    node [shape=record];\n";
            graph += "    rankdir=LR;\n";
            graph += "    subgraph cluster_0 {\n";
            graph += "        label = \"Lista Simple Enlazada\";\n";

            LinkedNode current = head;
            int index = 0;

            while (current != null)
            {
                graph += $"        n{index} {current.Data.ToDotNode()}\n";
                current = current.Next;
                index++;
            }

            for (int i = 0; i < index - 1; i++)
            {
                graph += $"        n{i} -> n{i + 1};\n";
            }

            graph += "    }\n";
            graph += "}\n";
            return graph;
        }
    }
}
