using System;
using System.Runtime.InteropServices;

namespace Lists 
{
    public unsafe struct DoubleNode 
    {
        public int Id;
        public int UserId;
        public IntPtr Brand;
        public int Model;
        public IntPtr Plate;
        public int ServiceCounter;
        public DoubleNode* Next;
        public DoubleNode* Prev;

        public DoubleNode(int id, int userId, string brand, int model, string plate)
        {
            Id = id;
            UserId = userId;
            Brand = Marshal.StringToHGlobalUni(brand);
            Model = model;
            Plate = Marshal.StringToHGlobalUni(plate);
            ServiceCounter = 0;
            Next = null;
            Prev = null;
        }

        public void FreeData()
        {
            if (Brand != IntPtr.Zero) Marshal.FreeHGlobal(Brand);
            if (Plate != IntPtr.Zero) Marshal.FreeHGlobal(Plate);
        }

        public string ToGraph()
        {
            return $"[label = \"{{<data> ID: {Id} \\n ID_Usuario: {UserId} \\n Marca: {GetBrand()} \\n Model: {Model} \\n Placa: {GetPlate()}}}\"];";
        }

        public string? GetBrand() => Marshal.PtrToStringUni(Brand);
        public string? GetPlate() => Marshal.PtrToStringUni(Plate);

        public override string ToString() => $"Id: {Id}, Id Usuario: {UserId}, Marca: {GetBrand()}, Modelo: {Model}, Placa: {GetPlate()}";
    }
}