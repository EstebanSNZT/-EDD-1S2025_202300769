using System;
using System.Runtime.InteropServices;

namespace Matrix
{
    public unsafe struct InternalNode
    {
        public IntPtr Details;
        public int PosX;     
        public int PosY;
        public InternalNode* Up;
        public InternalNode* Down; 
        public InternalNode* Right;
        public InternalNode* Left;

        public InternalNode(int posX, int posY, string details)
        {
            PosX = posX;
            PosY = posY;
            Details = Marshal.StringToHGlobalUni(details);
            Up = null;
            Down = null;
            Right = null;
            Left = null;
        }   

        public string? GetDetails() => Marshal.PtrToStringUni(Details);
    }
}