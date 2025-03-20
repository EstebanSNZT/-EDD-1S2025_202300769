using Classes;

namespace DataStructures
{
    public class LinkedNode
    {
        public User Data { get; set; }
        public LinkedNode Next { get; set; }

        public LinkedNode(User data)
        {
            Data = data;
            Next = null;
        }
    }
}