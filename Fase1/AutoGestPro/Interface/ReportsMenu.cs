using Gtk;
using Lists;
using Utilities;

namespace Interface
{
    public unsafe class ReportsMenu : Window
    {
        public ReportsMenu() : base("AutoGest Pro - Reportes")
        {
            InitializeComponents();
        }

        public ReportsMenu(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(350, 573); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            if (Child != null)
            {
                Remove(Child);
            }

            Fixed fixedContainer = new Fixed();

            Label reportsLabel = new Label();
            reportsLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Reportes</span>";
            fixedContainer.Put(reportsLabel, 113, 15);

            Button usersReportButton = new Button("Reporte de Usuarios");
            usersReportButton.SetSizeRequest(280, 35);
            usersReportButton.Clicked += OnUsersReportButtonClicked;
            fixedContainer.Put(usersReportButton, 35, 70);

            Button vehiclesReportButton = new Button("Reporte de Vehículos");
            vehiclesReportButton.SetSizeRequest(280, 35);
            vehiclesReportButton.Clicked += OnVehiclesReportButtonClicked;
            fixedContainer.Put(vehiclesReportButton, 35, 125);

            Button sparePartsReportButton = new Button("Reporte de Repuestos");
            sparePartsReportButton.SetSizeRequest(280, 35);
            sparePartsReportButton.Clicked += OnSparePartsReportButtonClicked;
            fixedContainer.Put(sparePartsReportButton, 35, 180);

            Button servicesReportButton = new Button("Reporte de Servicios");
            servicesReportButton.SetSizeRequest(280, 35);
            servicesReportButton.Clicked += OnServicesReportButtonClicked;
            fixedContainer.Put(servicesReportButton, 35, 235);

            Button invoicesReportButton = new Button("Reporte de Facturación");
            invoicesReportButton.SetSizeRequest(280, 35);
            invoicesReportButton.Clicked += OnInvoicesReportButtonClicked;
            fixedContainer.Put(invoicesReportButton, 35, 290);

            Button binnacleButton = new Button("Bitácora");
            binnacleButton.SetSizeRequest(280, 35);
            binnacleButton.Clicked += OnBinnacleButtonClicked;
            fixedContainer.Put(binnacleButton, 35, 345);

            Button top5ServicesButton = new Button("Top 5 vehículos con más servicios");
            top5ServicesButton.SetSizeRequest(280, 35);
            top5ServicesButton.Clicked += OnTop5ServicesButtonClicked;
            fixedContainer.Put(top5ServicesButton, 35, 400);

            Button top5ModelButton = new Button("Top 5 vehículos más antiguos");
            top5ModelButton.SetSizeRequest(280, 35);
            top5ModelButton.Clicked += OnTop5ModelButtonClicked;
            fixedContainer.Put(top5ModelButton, 35, 455);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 518);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
        }

        private void OnReturnButtonClicked(object? sender, EventArgs e)
        {
            GlobalWindows.menu.ShowAll();
            Hide();
        }

        private void OnUsersReportButtonClicked(object? sender, EventArgs e)
        {
            if (GlobalLists.linkedList.IsEmpty())
            {
                Menu.ShowDialog(this, MessageType.Warning, "La lista de usuarios se encuentra actualmente vacía.");
                return;
            }

            string dotCode = GlobalLists.linkedList.GenerateGraph();
            Utility.GenerateDotFile("Users", dotCode);
            Utility.ConvertDotToImage("Users.dot");
            GlobalWindows.showReport1.SetImage("Users.png");
            GlobalWindows.showReport1.ShowAll();
        }

        private void OnVehiclesReportButtonClicked(object? sender, EventArgs e)
        {
            if (GlobalLists.doubleList.IsEmpty())
            {
                Menu.ShowDialog(this, MessageType.Warning, "La lista de vehículos se encuentra actualmente vacía.");
                return;
            }

            string dotCode = GlobalLists.doubleList.GenerateGraph();
            Utility.GenerateDotFile("Vehicles", dotCode);
            Utility.ConvertDotToImage("Vehicles.dot");
            GlobalWindows.showReport1.SetImage("Vehicles.png");
            GlobalWindows.showReport1.ShowAll();
        }

        private void OnSparePartsReportButtonClicked(object? sender, EventArgs e)
        {
            if (GlobalLists.circularList.IsEmpty())
            {
                Menu.ShowDialog(this, MessageType.Warning, "La lista de repuestos se encuentra actualmente vacía.");
                return;
            }

            string dotCode = GlobalLists.circularList.GenerateGraph();
            Utility.GenerateDotFile("SpareParts", dotCode);
            Utility.ConvertDotToImage("SpareParts.dot");
            GlobalWindows.showReport1.SetImage("SpareParts.png");
            GlobalWindows.showReport1.ShowAll();
        }

        private void OnServicesReportButtonClicked(object? sender, EventArgs e)
        {
            if (GlobalLists.queue.IsEmpty())
            {
                Menu.ShowDialog(this, MessageType.Warning, "La cola de servicios se encuentra actualmente vacía.");
                return;
            }

            string dotCode = GlobalLists.queue.GenerateGraph();
            Utility.GenerateDotFile("Services", dotCode);
            Utility.ConvertDotToImage("Services.dot");
            GlobalWindows.showReport1.SetImage("Services.png");
            GlobalWindows.showReport1.ShowAll();
        }

        private void OnInvoicesReportButtonClicked(object? sender, EventArgs e)
        {
            if (GlobalLists.stack.IsEmpty())
            {
                Menu.ShowDialog(this, MessageType.Warning, "La pila de facturación se encuentra actualmente vacía.");
                return;
            }

            string dotCode = GlobalLists.stack.GenerateGraph();
            Utility.GenerateDotFile("Invoices", dotCode);
            Utility.ConvertDotToImage("Invoices.dot");
            GlobalWindows.showReport2.SetImage("Invoices.png");
            GlobalWindows.showReport2.ShowAll();
        }

        private void OnBinnacleButtonClicked(object? sender, EventArgs e)
        {
            if (GlobalLists.queue.IsEmpty())
            {
                Menu.ShowDialog(this, MessageType.Warning, "No hay servicios actualmente.");
                return;
            }

            string dotCode = GlobalLists.matrix.GenerateGraph();
            Utility.GenerateDotFile("Binnacle", dotCode);
            Utility.ConvertDotToImage("Binnacle.dot");
            GlobalWindows.showReport3.SetImage("Binnacle.png");
            GlobalWindows.showReport3.ShowAll();
        }

        private void OnTop5ServicesButtonClicked(object? sender, EventArgs e)
        {
            if (GlobalLists.doubleList.IsEmpty())
            {
                Menu.ShowDialog(this, MessageType.Warning, "La lista de vehículos se encuentra actualmente vacía.");
                return;
            }

            string message = GlobalLists.doubleList.Top5Services();
            Menu.ShowDialog(this, MessageType.Info, message);
        }

        private void OnTop5ModelButtonClicked(object? sender, EventArgs e)
        {
            if (GlobalLists.doubleList.IsEmpty())
            {
                Menu.ShowDialog(this, MessageType.Warning, "La lista de vehículos se encuentra actualmente vacía.");
                return;
            }

            string message = GlobalLists.doubleList.Top5Model();
            Menu.ShowDialog(this, MessageType.Info, message);
        }
    }
}