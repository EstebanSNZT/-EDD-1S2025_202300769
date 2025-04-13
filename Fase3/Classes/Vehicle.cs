namespace Classes
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Brand { get; set; }
        public int Model { get; set; }
        public string Plate { get; set; }

        public Vehicle(int id, int userId, string brand, int model, string plate)
        {
            Id = id;
            UserId = userId;
            Brand = brand;
            Model = model;
            Plate = plate;
        }

        public override string ToString()
        {
            return $"ID: {Id}, ID_Usuario: {UserId}, Marca: {Brand}, Modelo: {Model}, Placa: {Plate}";
        }

        public string ToDotNode()
        {
            return $"[label = \"{{<data> ID: {Id} \\n ID_Usuario: {UserId} \\n Marca: {Brand} \\n Modelo: {Model} \\n Placa: {Plate}}}\"];";
        }
    }
}