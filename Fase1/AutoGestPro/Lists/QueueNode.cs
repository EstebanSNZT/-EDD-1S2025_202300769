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

        public string ToGraph(int serviceNum)
        {
            return $"[label = \"{{<data> Servicio {serviceNum} \\n ID: {Id} \\n Id_Repuesto: {SparePartId} \\n Id_Vehículo: {VehicleId} \\n Detalles: {GetDetails()} \\n Costo: {Cost}}}\"];";
        }

        public string? GetDetails() => Marshal.PtrToStringUni(Details);
    
        public override string ToString() => $"Id: {Id}, Id Repuesto: {SparePartId}, Id Vehículo: {VehicleId}, Detalles: {GetDetails()}, Costo: {Cost}";
    }
}