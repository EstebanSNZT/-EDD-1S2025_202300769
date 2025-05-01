namespace Structures
{
    public class HuffmanNode : IComparable<HuffmanNode>
    {
        public char Character { get; set; }
        public int Frequency { get; set; }
        public HuffmanNode Left { get; set; }
        public HuffmanNode Right { get; set; }


        public int CompareTo(HuffmanNode other)
        {
            return Frequency - other.Frequency;
        }

        public bool IsLeaf()
        {
            return Left == null && Right == null;
        }
    }
}