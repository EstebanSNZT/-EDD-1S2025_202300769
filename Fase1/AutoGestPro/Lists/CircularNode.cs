using System;
using System.Runtime.InteropServices;

namespace Lists 
{
    public unsafe struct CircularNode 
    {
        public int Id;
        public IntPtr SparePart;
        public IntPtr Details;
        public double Cost;
        public CircularNode* Next;

        public CircularNode(int id, string sparePart, string details, double cost)
        {
            Id = id;
            SparePart = Marshal.StringToHGlobalUni(sparePart);
            Details = Marshal.StringToHGlobalUni(details);
            Cost = cost;
            Next = null;
        }

        public void FreeData()
        {
            if (SparePart != IntPtr.Zero) Marshal.FreeHGlobal(SparePart);
            if (Details!= IntPtr.Zero) Marshal.FreeHGlobal(Details);
        }

        public string ToGraph()
        {
            return $"[label = \"{{<data> ID: {Id} \\n Repuesto: {GetSparePart()} \\n Detalles: {GetDetails()} \\n Costos: {Cost}}}\"];";
        }

        public string? GetSparePart() => Marshal.PtrToStringUni(SparePart);
        public string? GetDetails() => Marshal.PtrToStringUni(Details);

        public override string ToString() => $"Id: {Id}, Repuesto: {GetSparePart()}, Detalles: {GetDetails()}, Costo: {Cost}";
    }
}