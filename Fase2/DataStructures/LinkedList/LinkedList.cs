using Classes;

namespace DataStructures
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

        public void Delete(User data)
        {
            if (head == null) return;

            if (head.Data.Equals(data))
            {
                head = head.Next;
                return;
            }

            LinkedNode current = head;

            while (current.Next != null && !current.Next.Data.Equals(data))
            {
                current = current.Next;
            }

            if (current.Next != null)
            {
                current.Next = current.Next.Next;
            }
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

        public User GetUser(int id)
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

        public bool Contains(int id)
        {
            return GetUser(id) != null;
        }

        public bool IsEmpty()
        {
            return head == null;
        }
    }
}
