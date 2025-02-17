using System;
using System.Runtime.InteropServices;

namespace Lists 
{
    public unsafe struct CircularNode 
    {
        public int Id;
        public IntPtr Part;
        public IntPtr Details;
        public double Cost;
        public CircularNode* Next;

        public CircularNode(int id, string part, string details, double cost)
        {
            Id = id;
            Part = Marshal.StringToHGlobalUni(part);
            Details = Marshal.StringToHGlobalUni(details);
            Cost = cost;
            Next = null;
        }

        public void FreeData()
        {
            if (Part != IntPtr.Zero) Marshal.FreeHGlobal(Part);
            if (Details!= IntPtr.Zero) Marshal.FreeHGlobal(Details);
        }

        public string? GetPart() => Marshal.PtrToStringUni(Part);
        public string? GetDetails() => Marshal.PtrToStringUni(Details);

        public override string ToString() => $"Id: {Id}, Repuesto: {GetPart()}, Detalles: {GetDetails()}, Costo: {Cost}";
    }
}