namespace Matrix
{
    public unsafe struct HeaderNode
    {
        public int id;
        public HeaderNode* Next;
        public HeaderNode* Prev;
        public InternalNode* Access;

        public HeaderNode(int id)
        {
            this.id = id;
            Next = null;
            Prev = null;
            Access = null;
        }
    }
}