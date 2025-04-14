using Gtk;
using Global;

namespace Interface
{

    public class UserMenu : Window
    {
        public Label menuLabel = new Label();
        public UserMenu() : base("AutoGest Pro - Usuario")
        {
            InitializeComponents();
        }

        public UserMenu(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(350, 345); //(ancho, alto)
            SetPosition(WindowPosition.Center);
            
            Fixed fixedContainer = new Fixed();

            fixedContainer.Put(menuLabel, 35, 15);

            Button insertVehicleButton = new Button("Insertar Vehículo");
            insertVehicleButton.SetSizeRequest(280, 35);
            insertVehicleButton.Clicked += OnInsertVehicleButtonClicked;
            fixedContainer.Put(insertVehicleButton, 35, 65);

            Button vehiclesVisualizationButton = new Button("Visualización de Vehículos");
            vehiclesVisualizationButton.SetSizeRequest(280, 35);
            vehiclesVisualizationButton.Clicked += OnVehiclesVisualizationButtonClicked;
            fixedContainer.Put(vehiclesVisualizationButton, 35, 120);

            Button servicesVisualizationButton = new Button("Visualización de Servicios");
            servicesVisualizationButton.SetSizeRequest(280, 35);
            servicesVisualizationButton.Clicked += OnServicesVisualizationButtonClicked;
            fixedContainer.Put(servicesVisualizationButton, 35, 175);

            Button invoicesVisualizationButton = new Button("Visualización de Facturas");
            invoicesVisualizationButton.SetSizeRequest(280, 35);
            invoicesVisualizationButton.Clicked += OnInvoicesVisualizationButtonClicked;
            fixedContainer.Put(invoicesVisualizationButton, 35, 230);

            Button logoutButton = new Button("Cerrar Sesión");
            logoutButton.SetSizeRequest(160, 30);
            logoutButton.Clicked += OnLogoutButtonClicked;
            fixedContainer.Put(logoutButton, 95, 290);

            Add(fixedContainer);

            DeleteEvent += (o, args) =>
            {
                GlobalWindows.DestroyAll();
                Application.Quit();
            };
        }

        private void OnInsertVehicleButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.insertVehicle.ShowAll();
            Hide();
        }

        private void OnVehiclesVisualizationButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.vehiclesVisualization.UpdateData(LoginControl.LoggedUserId);
            GlobalWindows.vehiclesVisualization.ShowAll();
            Hide();
        }

        private void OnServicesVisualizationButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.servicesVisualization.UpdateData(LoginControl.LoggedUserId);
            GlobalWindows.servicesVisualization.AdjustTraversal(0);
            GlobalWindows.servicesVisualization.ShowAll();
            Hide();
        }

        private void OnInvoicesVisualizationButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.invoicesVisualization.UpdateData(LoginControl.LoggedUserId);
            GlobalWindows.invoicesVisualization.ShowAll();
            Hide();
        }

        private void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            LoginControl.GenerateLogoutTime();
            LoginControl.AddLogin();
            GlobalWindows.login.ShowAll();
            Hide();
        }
    }
}