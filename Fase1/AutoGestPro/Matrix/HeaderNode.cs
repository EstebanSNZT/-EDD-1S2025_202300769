namespace Matrix
{
    public unsafe struct HeaderNode
    {
        public int id;
        public HeaderNode* Next;
        public HeaderNode* Prev;
        public InternalNode* accessFirst;
        public InternalNode* accessLast;

        public HeaderNode(int id)
        {
            this.id = id;
            Next = null;
            Prev = null;
            accessFirst = null;
        }
    }
}