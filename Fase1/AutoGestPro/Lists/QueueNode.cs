using System;
using System.Runtime.InteropServices;

namespace Lists 
{
    public unsafe struct QueueNode 
    {
        public int Id;
        public int SparePartId;
        public int VehicleId;
        public IntPtr Details;
        public double Cost;
        public QueueNode* Next;

        public QueueNode(int id, int sparePartId, int vehicleId, string details, double cost) 
        {
            Id = id;
            SparePartId = sparePartId;
            VehicleId = vehicleId;
            Details = Marshal.StringToHGlobalUni(details);
            Cost = cost;
            Next = null;
        }

        public void FreeData()
        {
            if (Details != IntPtr.Zero) Marshal.FreeHGlobal(Details);
        }

        public string? GetDetails() => Marshal.PtrToStringUni(Details);
    
        public override string ToString() => $"Id: {Id}, Id Repuesto: {SparePartId}, Id Vehiculo: {VehicleId}, Details: {GetDetails()}, Costo: {Cost}";
    }
}