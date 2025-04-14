namespace Classes
{
    public class Service
    {
        public int Id { get; set; }
        public int SparePartId { get; set; }
        public int VehicleId { get; set; }
        public string Details { get; set; }
        public double Cost { get; set; }

        public Service(int id, int sparePartId, int vehicleId, string details, double cost)
        {
            Id = id;
            SparePartId = sparePartId;
            VehicleId = vehicleId;
            Details = details;
            Cost = cost;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Id_Repuesto: {SparePartId}, Id_Vehículo: {VehicleId}, Detalles: {Details}, Costo: ${Cost}";
        }
    }
}