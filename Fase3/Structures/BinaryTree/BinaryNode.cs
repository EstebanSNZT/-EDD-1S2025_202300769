using Classes;

namespace Structures
{
    public class BinaryNode
    {
        public Service Data { get; set; }
        public BinaryNode Left { get; set; }
        public BinaryNode Right { get; set; }

        public BinaryNode(Service data)
        {
            Data = data;
            Left = null;
            Right = null;
        }
    }
}