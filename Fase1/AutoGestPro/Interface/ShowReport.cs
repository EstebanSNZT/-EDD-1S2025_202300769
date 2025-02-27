using Gtk;

namespace Interface
{
    public class ShowReport : Window
    {
        public ShowReport(string imageName, bool hOrV) : base("AutoGest Pro - Reporte")
        {
            InitializeComponents(imageName, hOrV);
        }

        private void InitializeComponents(string imageName, bool hOrV)
        {
            SetPosition(WindowPosition.Center);

            if (hOrV) SetSizeRequest(1300, 300);
            else SetSizeRequest(400, 900);

            string imagePath = @"C:\Users\Esteban Sánchez\Documents\GitHub\EDD\-EDD-1S2025_202300769\Fase1\AutoGestPro\reports\" + imageName;

            Image image = new Image(imagePath);

            ScrolledWindow scrolledWindow = new ScrolledWindow
            {
                HscrollbarPolicy = PolicyType.Automatic,
                VscrollbarPolicy = PolicyType.Automatic
            };
            scrolledWindow.Add(image);

            Box box = new Box(Orientation.Vertical, 10);
            box.PackStart(scrolledWindow, true, true, 10);

            Add(box);
        }
    }
}