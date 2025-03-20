using Classes;

namespace DataStructures
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

        public void Delete(Vehicle data)
        {
            if (head == null) return;

            DoublyLinkedNode current = head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                {
                    if (current.Prev != null)
                        current.Prev.Next = current.Next;
                    else
                        head = current.Next;
                    if (current.Next != null)
                        current.Next.Prev = current.Prev;
                    else
                        tail = current.Prev;

                    return;
                }
                current = current.Next;
            }
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

        public Vehicle GetVehicle(int id)
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
            return GetVehicle(id) != null;
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
    }
}