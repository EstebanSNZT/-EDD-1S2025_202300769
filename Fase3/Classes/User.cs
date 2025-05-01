using System.Security.Cryptography;
using System.Text;

namespace Classes
{
    public class User
    {
        public int Id { get; set; }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Password { get; set; }

        public User(int id, string names, string lastNames, string email, int age, string password)
        {
            Id = id;
            Names = names;
            LastNames = lastNames;
            Email = email;
            Age = age;
            Password = password;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Nombres: {Names}, Apellidos: {LastNames}, Correo: {Email}, Edad: {Age}, Contraseña: {Password}";
        }

        public void HashPassword()
        {
            Password = GeneratePasswordHash(Password.Trim());
        }

        private string GeneratePasswordHash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                StringBuilder hash = new StringBuilder();
                foreach (byte hb in hashBytes)
                {
                    hash.Append(hb.ToString("x2"));
                }
                return hash.ToString();
            }
        }

        public bool ComparePassword(string passwordToCompare)
        {
            string generatedHash = GeneratePasswordHash(passwordToCompare.Trim());
            return generatedHash.Equals(Password, StringComparison.Ordinal);
        }
    }
}