using Classes;
namespace Structures
{
    public class DoublyLinkedNode
    {
        public Vehicle Data { get; set; }
        public DoublyLinkedNode Next { get; set; }
        public DoublyLinkedNode Prev { get; set; }

        public DoublyLinkedNode(Vehicle data)
        {
            Data = data;
            Next = null;
        }

        public string ToDotNode()
        {
            return $"[label = \"{{<data> ID: {Data.Id} \\n ID_Usuario: {Data.UserId} \\n Marca: {Data.Brand} \\n Modelo: {Data.Model} \\n Placa: {Data.Plate}}}\"];";
        }
    }
}