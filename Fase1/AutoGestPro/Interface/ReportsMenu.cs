using Gtk;

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

            Fixed fixedContainer = new Fixed();

            Label reportsLabel = new Label();
            reportsLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Reportes</span>";
            fixedContainer.Put(reportsLabel, 113, 15);

            Button usersReportButton = new Button("Reporte de Usuarios");
            usersReportButton.SetSizeRequest(280, 35);
            fixedContainer.Put(usersReportButton, 35, 70);

            Button vehiclesReportButton = new Button("Reporte de Vehículos");
            vehiclesReportButton.SetSizeRequest(280, 35);
            fixedContainer.Put(vehiclesReportButton, 35, 125);

            Button sparePartsReportButton = new Button("Reporte de Repuestos");
            sparePartsReportButton.SetSizeRequest(280, 35);
            fixedContainer.Put(sparePartsReportButton, 35, 180);

            Button servicesReportButton = new Button("Reporte de Servicios");
            servicesReportButton.SetSizeRequest(280, 35);
            fixedContainer.Put(servicesReportButton, 35, 235);

            Button invoicesReportButton = new Button("Reporte de Facturación");
            invoicesReportButton.SetSizeRequest(280, 35);
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
            this.Destroy();
            Menu menu = new Menu();
            menu.ShowAll();
        }
    }
}