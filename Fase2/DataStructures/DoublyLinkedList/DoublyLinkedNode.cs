using Classes;
namespace DataStructures
{
    public class DoublyLinkedNode
    {
        public Vehicle Data { get; set; }
        public DoublyLinkedNode Next { get; set; }
        public DoublyLinkedNode Prev { get; set; }

        public DoublyLinkedNode(Vehicle data)
        {
            Data = data;
            Next = null;
        }
    }
}