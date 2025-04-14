using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Classes
{
    public class Invoice
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public double Total { get; set; }
        public string PaymentMethod { get; set; }
        public string Date { get; set; }

        public Invoice(int id, int serviceId, double total, string paymentMethod)
        {
            Id = id;
            ServiceId = serviceId;
            Total = total;
            PaymentMethod = paymentMethod;
            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }

        public string GetHash()
        {
            string data = JsonConvert.SerializeObject(this);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                StringBuilder hash = new StringBuilder();
                foreach (byte b in bytes)
                {
                    hash.Append(b.ToString("x2"));
                }
                return hash.ToString();
            }
        }

        public override string ToString()
        {
            return $"ID: {Id}, Id_Servicio: {ServiceId}, Total: ${Total}";
        }
    }
}