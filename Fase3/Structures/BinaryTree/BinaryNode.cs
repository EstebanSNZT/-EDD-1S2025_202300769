using Classes;

namespace Structures
{
    public class BinaryNode
    {
        public Service Data { get; set; }
        public BinaryNode Left { get; set; }
        public BinaryNode Right { get; set; }

        public BinaryNode(Service data)
        {
            Data = data;
            Left = null;
            Right = null;
        }

        public string ToDotNode()
        {
            return $"\"{Data.Id}\" [label=\"ID: {Data.Id}\\nId_Repuesto: {Data.SparePartId}\\nId_Vehículo: {Data.VehicleId}\\nDetalles: {Data.Details}\\nCosto: {Data.Cost}\"];";
        }
    }
}