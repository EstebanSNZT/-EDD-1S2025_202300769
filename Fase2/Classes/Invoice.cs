namespace Classes
{
    public class Invoice
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public double Total { get; set; }

        public Invoice(int id, int serviceId, double total)
        {
            Id = id;
            ServiceId = serviceId;
            Total = total;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Id_Servicio: {ServiceId}, Total: ${Total}";
        }
    }
}