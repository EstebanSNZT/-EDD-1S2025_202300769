using Gtk;
using Global;

class Program
{
    public static void Main(string[] args)
    {
        Application.Init();

        GlobalWindows.login.ShowAll();

        Application.Run();
    }
}