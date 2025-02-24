using System;
using System.Runtime.InteropServices;

namespace Lists 
{
    public unsafe struct StackNode 
    {
        public int Id;
        public int OrderId;
        public double TotalCost;
        public StackNode* Next;

        public StackNode(int id, int orderId, double totalCost) 
        {
            Id = id;
            OrderId = orderId;
            TotalCost = totalCost;
            Next = null;
        }
        
        public override string ToString() => $"Id: {Id}, Id_Orden: {OrderId}, Total: {TotalCost}";
    }
}