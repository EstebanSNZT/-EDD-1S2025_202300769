using Gtk;
using Interface;
using Lists;
using Matrix;

class Program
{
    public static void Main(string[] args)
    {
        Application.Init();

        GlobalWindows.login.ShowAll();

        Application.Run();
    }
}