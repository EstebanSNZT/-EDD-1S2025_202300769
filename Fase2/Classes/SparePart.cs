namespace Classes
{
    public class SparePart
    {
        public int Id { get; set; }
        public string Spare { get; set; }
        public string Details { get; set; }
        public double Cost { get; set; }

        public SparePart(int id, string spare, string details, double cost)
        {
            Id = id;
            Spare = spare;
            Details = details;
            Cost = cost;
        }

        public void Update(int id, string spare, string details, double cost)
        {
            Id = id;
            Spare = spare;
            Details = details;
            Cost = cost;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Repuesto: {Spare}, Detalles: {Details}, Costo: ${Cost}";
        }

        public string ToDotNode()
        {
            return $"\"{Id}\" [label=\"ID: {Id}\\nRepuesto: {Spare}\\nDetalles: {Details}\\nCosto: {Cost}\"];";
        }
    }
}