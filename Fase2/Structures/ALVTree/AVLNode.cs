using Classes;

namespace Structures
{
    public class AVLNode
    {
        public SparePart Data { get; set; }
        public AVLNode Left { get; set; }
        public AVLNode Right { get; set; }
        public int Height { get; set; }

        public AVLNode(SparePart data)
        {
            Data = data;
            Left = null;
            Right = null;
            Height = 1;
        }
    }
}