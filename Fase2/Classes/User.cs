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

        public string ToDotNode()
        {
            return $"[label = \"{{<data> ID: {Id} \\n Nombre: {Names} {LastNames} \\n Correo: {Email} \\n Edad: {Age} \\n Contraseña: {Password}}}\"];";
        }
    }
}