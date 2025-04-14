using System.Text.Json.Nodes;
using Classes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Global
{
    public static class LoginControl
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

        public static string GenerateJson()
        {
            return LoginControlJson.ToString(Formatting.Indented);
        }
    }
}