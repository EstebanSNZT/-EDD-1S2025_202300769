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
            return $"{Id},{UserId},{Brand},{Model},{Plate}";
        }
    }
}