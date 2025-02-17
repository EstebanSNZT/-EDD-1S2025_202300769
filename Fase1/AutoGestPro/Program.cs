using Gtk;
using Interface;
using Lists;

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