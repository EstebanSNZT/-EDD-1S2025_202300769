using Gtk;
using Lists;
using Utilities;

namespace Interface
{
    public class ReportsMenu : Window
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
            SetSizeRequest(350, 463); //(ancho, alto)
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
            fixedContainer.Put(binnacleButton, 35, 345);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 408);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
        }

        private void OnReturnButtonClicked(object? sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.ShowAll();
            this.Dispose();
        }

        private void OnUsersReportButtonClicked(object? sender, EventArgs e)
        {
            if(GlobalLists.linkedList.IsEmpty())
            {
                Menu.ShowDialog(this, MessageType.Warning, "La lista de usuarios se encuentra actualmente vacía.");
                return;
            }

            string dotCode = GlobalLists.linkedList.GenerateGraph();
            Utility.GenerateDotFile("Users", dotCode);
            Utility.ConvertDotToImage("Users.dot");
            ShowReport showReport = new ShowReport("Users.png", true);
            showReport.ShowAll();
        }

        private void OnVehiclesReportButtonClicked(object? sender, EventArgs e)
        {
            if(GlobalLists.doubleList.IsEmpty())
            {
                Menu.ShowDialog(this, MessageType.Warning, "La lista de vehículos se encuentra actualmente vacía.");
                return;
            }

            string dotCode = GlobalLists.doubleList.GenerateGraph();
            Utility.GenerateDotFile("Vehicles", dotCode);
            Utility.ConvertDotToImage("Vehicles.dot");
            ShowReport showReport = new ShowReport("Vehicles.png", true);
            showReport.ShowAll();
        }

        private void OnSparePartsReportButtonClicked(object? sender, EventArgs e)
        {
            if(GlobalLists.circularList.IsEmpty())
            {
                Menu.ShowDialog(this, MessageType.Warning, "La tabla de repuestos se encuentra actualmente vacía.");
                return;
            }

            string dotCode = GlobalLists.circularList.GenerateGraph();
            Utility.GenerateDotFile("SpareParts", dotCode);
            Utility.ConvertDotToImage("SpareParts.dot");
            ShowReport showReport = new ShowReport("SpareParts.png", true);
            showReport.ShowAll();
        }

        private void OnServicesReportButtonClicked(object? sender, EventArgs e)
        {
            if(GlobalLists.queue.IsEmpty())
            {
                Menu.ShowDialog(this, MessageType.Warning, "La pila de servicios se encuentra actualmente vacía.");
                return;
            }

            string dotCode = GlobalLists.queue.GenerateGraph();
            Utility.GenerateDotFile("Services", dotCode);
            Utility.ConvertDotToImage("Services.dot");
            ShowReport showReport = new ShowReport("Services.png", true);
            showReport.ShowAll();
        }

        private void OnInvoicesReportButtonClicked(object? sender, EventArgs e)
        {
            if(GlobalLists.stack.IsEmpty())
            {
                Menu.ShowDialog(this, MessageType.Warning, "La cola de facturación se encuentra actualmente vacía.");
                return;
            }

            string dotCode = GlobalLists.stack.GenerateGraph();
            Utility.GenerateDotFile("Invoices", dotCode);
            Utility.ConvertDotToImage("Invoices.dot");
            ShowReport showReport = new ShowReport("Invoices.png", false);
            showReport.ShowAll();
        }
    }
}