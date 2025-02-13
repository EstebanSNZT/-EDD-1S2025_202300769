using Gtk;
using Interface;

class Program
{
    public static void Main(string[] args)
    {
        Application.Init();

        Login login = new Login();
        login.ShowAll();

        Application.Run();
    }
}