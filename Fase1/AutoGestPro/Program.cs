using Gtk;
using Interface;
using Lists;
using Matrix;

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