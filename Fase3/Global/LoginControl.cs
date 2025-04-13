using System.Text.Json.Nodes;
using Classes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Global
{
    public class LoginControl
    {
        public static int LoggedUserId = 0;
        private static string LoggedUserEmail = "";
        private static string LoginTime = "";
        private static string LogoutTime = "";
        private static JArray LoginControlJson = new JArray();

        public static void AddLogin()
        {
            if (string.IsNullOrEmpty(LoggedUserEmail) || string.IsNullOrEmpty(LoginTime) || string.IsNullOrEmpty(LogoutTime))
                return;
                
            LoginControlJson.Add(new JObject
            {
                {"usuario", LoggedUserEmail},
                {"entrada", LoginTime},
                {"salida", LogoutTime}
            });
        }

        public static void GenerateLoginTime(int userId, string userEmail)
        {
            LoggedUserId = userId;
            LoggedUserEmail = userEmail;
            LoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff");
        }

        public static void GenerateLogoutTime()
        {
            LogoutTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff");
        }

        public static void CreateJson()
        {
            string folder = Path.Combine(Directory.GetCurrentDirectory(), "Reportes");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string filePath = Path.Combine(folder, "ControlLogueo.json");

            string json = LoginControlJson.ToString(Formatting.Indented);
            File.WriteAllText(filePath, json);
            Console.WriteLine("El archivo ControlLogueo.json ha sido creado con exito.");
        }
    }
}