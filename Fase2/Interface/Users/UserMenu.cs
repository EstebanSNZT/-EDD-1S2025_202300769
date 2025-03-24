using Gtk;
using Global;

namespace Interface
{

    public class UserMenu : Window
    {
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

            if (Child != null)
            {
                Remove(Child);
            }

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>User</span>";
            fixedContainer.Put(menuLabel, 130, 15);

            Button insertVehicleButton = new Button("Insertar Vehículo");
            insertVehicleButton.SetSizeRequest(280, 35);
            insertVehicleButton.Clicked += OnInsertVehicleButtonClicked;
            fixedContainer.Put(insertVehicleButton, 35, 65);

            Button servicesVisualizationButton = new Button("Visualización de Servicios");
            servicesVisualizationButton.SetSizeRequest(280, 35);
            servicesVisualizationButton.Clicked += OnServicesVisualizationButtonClicked;
            fixedContainer.Put(servicesVisualizationButton, 35, 120);

            Button invoicesVisualizationButton = new Button("Visualización de Facturas");
            invoicesVisualizationButton.SetSizeRequest(280, 35);
            invoicesVisualizationButton.Clicked += OnInvoicesVisualizationButtonClicked;
            fixedContainer.Put(invoicesVisualizationButton, 35, 175);

            Button cancelInvoicesButton = new Button("Cancelar Facturas");
            cancelInvoicesButton.SetSizeRequest(280, 35);
            cancelInvoicesButton.Clicked += OnCancelInvoicesButtonClicked;
            fixedContainer.Put(cancelInvoicesButton, 35, 230);

            Button logoutButton = new Button("Cerrar Sesión");
            logoutButton.SetSizeRequest(160, 30);
            logoutButton.Clicked += OnLogoutButtonClicked;
            fixedContainer.Put(logoutButton, 95, 290);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
        }

        private void OnInsertVehicleButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.insertVehicle.ShowAll();
            Hide();
        }

        private void OnServicesVisualizationButtonClicked(object sender, EventArgs e)
        {
            Hide();
        }

        private void OnInvoicesVisualizationButtonClicked(object sender, EventArgs e)
        {
            Hide();
        }

        private void OnCancelInvoicesButtonClicked(object sender, EventArgs e)
        {
            Hide();
        }

        private void OnGenerateServicesButtonClicked(object sender, EventArgs e)
        {
            Hide();
        }

        private void OnLogControlButtonClicked(object sender, EventArgs e)
        {
            Hide();
        }

        private void OnGenerateReportsButtonButtonClicked(object sender, EventArgs e)
        {
            Hide();
        }

        private void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.login.ShowAll();
            Hide();
        }
    }
}