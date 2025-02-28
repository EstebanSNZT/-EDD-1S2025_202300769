using System.IO;
using Gtk;

namespace Interface
{
    public class ShowReport : Window
    {
        private Image image = new Image();
        private ScrolledWindow scrolledWindow = new ScrolledWindow
        {
            HscrollbarPolicy = PolicyType.Automatic,
            VscrollbarPolicy = PolicyType.Automatic
        };
        private Box box = new Box(Orientation.Vertical, 10);

        public ShowReport(int width, int height) : base("AutoGest Pro - Reporte")
        {
            InitializeComponents(width, height);
        }

        private void InitializeComponents(int width, int height)
        {
            SetPosition(WindowPosition.Center);
            SetSizeRequest(width, height);
            scrolledWindow.Add(image);
            box.PackStart(scrolledWindow, true, true, 10);
            Add(box);

            DeleteEvent += OnWindowDeleteEvent;
        }

        public void SetImage(string imageName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            string imagePath = System.IO.Path.Combine(currentDirectory, "reports", imageName);

            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException($"La imagen '{imageName}' no se encontró en la ruta: {imagePath}");
            }

            image.File = imagePath;
        }

        private void OnWindowDeleteEvent(object sender, DeleteEventArgs args)
        {
            Hide();
            args.RetVal = true;
        }
    }
}