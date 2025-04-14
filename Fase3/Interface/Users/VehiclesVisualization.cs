using Gtk;
using Global;

namespace Interface
{
    public class VehiclesVisualization : Window
    {
        private TreeView treeView = new TreeView();
        private ListStore userVehicles = new ListStore(typeof(int), typeof(int), typeof(string), typeof(int), typeof(string));

        public VehiclesVisualization() : base("AutoGest Pro - Visualización de Vehículos")
        {
            InitializeComponents();
        }

        public VehiclesVisualization(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(440, 416); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Visualización de Vehículos</span>";
            fixedContainer.Put(menuLabel, 43, 15);

            ScrolledWindow scrolledWindow = new ScrolledWindow();
            ScrolledWindow scrollWindow = new ScrolledWindow();
            scrollWindow.SetSizeRequest(360, 265);
            scrollWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            fixedContainer.Put(scrollWindow, 40, 70);

            treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
            treeView.AppendColumn("ID Usuario", new CellRendererText(), "text", 1);
            treeView.AppendColumn("Marca", new CellRendererText(), "text", 2);
            treeView.AppendColumn("Modelo", new CellRendererText(), "text", 3);
            treeView.AppendColumn("Placa", new CellRendererText(), "text", 4);
            scrollWindow.Add(treeView);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 360);

            Add(fixedContainer);

            DeleteEvent += (o, args) =>
            {
                GlobalWindows.DestroyAll();
                Application.Quit();
            };
        }

        private void OnReturnButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.userMenu.ShowAll();
            Hide();
        }

        public void UpdateData(int userId)
        {
            userVehicles = GlobalStructures.VehiclesList.GetUserVehicles(userId);
            treeView.Model = userVehicles;
        }
    }
}