using Gtk;
using Global;
using Structures;
using Utilities;

class Program
{
    public static void Main(string[] args)
    {
        Application.Init();

        GlobalWindows.login.ShowAll();

        Application.Run();
    }
}