using Gtk;
using Global;
using Utilities;

namespace Interface
{

    public class AdminMenu : Window
    {
        public AdminMenu() : base("AutoGest Pro - Admin")
        {
            InitializeComponents();
        }

        public AdminMenu(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(350, 510); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Admin</span>";
            fixedContainer.Put(menuLabel, 130, 15);

            Button bulkUploadButton = new Button("Cargas Masivas");
            bulkUploadButton.SetSizeRequest(280, 35);
            bulkUploadButton.Clicked += OnBulkUploadButtonClicked;
            fixedContainer.Put(bulkUploadButton, 35, 65);

            Button entityManagementButton = new Button("Gestión de Entidades");
            entityManagementButton.SetSizeRequest(280, 35);
            entityManagementButton.Clicked += OnEntityManagementButtonClicked;
            fixedContainer.Put(entityManagementButton, 35, 120);

            Button sparePartsUpdateButton = new Button("Actualización de Repuestos");
            sparePartsUpdateButton.SetSizeRequest(280, 35);
            sparePartsUpdateButton.Clicked += OnSparePartsUpdateButtonClicked;
            fixedContainer.Put(sparePartsUpdateButton, 35, 175);

            Button sparePartsVisualizationButton = new Button("Visualización de Repuestos");
            sparePartsVisualizationButton.SetSizeRequest(280, 35);
            sparePartsVisualizationButton.Clicked += OnSparePartsVisualizationButtonClicked;
            fixedContainer.Put(sparePartsVisualizationButton, 35, 230);

            Button generateServicesButton = new Button("Generar Servicios");
            generateServicesButton.SetSizeRequest(280, 35);
            generateServicesButton.Clicked += OnGenerateServicesButtonClicked;
            fixedContainer.Put(generateServicesButton, 35, 285);

            Button loginControlButton = new Button("Control de Logueo");
            loginControlButton.SetSizeRequest(280, 35);
            loginControlButton.Clicked += OnLoginControlButtonClicked;
            fixedContainer.Put(loginControlButton, 35, 340);

            Button generateReportsButton = new Button("Generar Reportes");
            generateReportsButton.SetSizeRequest(280, 35);
            generateReportsButton.Clicked += OnGenerateReportsButtonButtonClicked;
            fixedContainer.Put(generateReportsButton, 35, 395);

            Button logoutButton = new Button("Cerrar Sesión");
            logoutButton.SetSizeRequest(160, 30);
            logoutButton.Clicked += OnLogoutButtonClicked;
            fixedContainer.Put(logoutButton, 95, 455);

            Add(fixedContainer);

            DeleteEvent += (o, args) =>
            {
                GlobalWindows.DestroyAll();
                Application.Quit();
            };
        }

        private void OnBulkUploadButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.bulkUpload.ShowAll();
            Hide();
        }

        private void OnEntityManagementButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.entityManagement.ShowAll();
            Hide();
        }

        private void OnSparePartsUpdateButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.updatedSparePart.ShowAll();
            Hide();
        }

        private void OnSparePartsVisualizationButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.sparePartsVisualization.UpdateData();
            GlobalWindows.sparePartsVisualization.AdjustTraversal(0);
            GlobalWindows.sparePartsVisualization.ShowAll();
            Hide();
        }

        private void OnGenerateServicesButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.generateService.ShowAll();
            Hide();
        }

        private void OnLoginControlButtonClicked(object sender, EventArgs e)
        {
            LoginControl.CreateJson();
            Login.ShowDialog(this, MessageType.Info, "El archivo ControlLogueo.json ha sido creado con exito en la carpeta Reportes.");
        }

        private void OnGenerateReportsButtonButtonClicked(object sender, EventArgs e)
        {
            if (!GlobalStructures.UsersList.IsEmpty())
            {
                string linkedListDot = GlobalStructures.UsersList.GenerateDot();
                Utility.GenerateDotFile("Usuarios", linkedListDot);
                Utility.ConvertDotToImage("Usuarios.dot");
            }

            if (!GlobalStructures.VehiclesList.IsEmpty())
            {
                string doublyLinkedListDot = GlobalStructures.VehiclesList.GenerateDot();
                Utility.GenerateDotFile("Vehículos", doublyLinkedListDot);
                Utility.ConvertDotToImage("Vehículos.dot");
            }

            if (!GlobalStructures.SparePartsTree.IsEmpty())
            {
                string avlTreeDot = GlobalStructures.SparePartsTree.GenerateDot();
                Utility.GenerateDotFile("Repuestos", avlTreeDot);
                Utility.ConvertDotToImage("Repuestos.dot");
            }

            if (!GlobalStructures.ServicesTree.IsEmpty())
            {
                string binaryTreeDot = GlobalStructures.ServicesTree.GenerateDot();
                Utility.GenerateDotFile("Servicios", binaryTreeDot);
                Utility.ConvertDotToImage("Servicios.dot");
            }

            if (!GlobalStructures.InvoicesTree.IsEmpty())
            {
                string bTreeDot = GlobalStructures.InvoicesTree.GenerateDot();
                Utility.GenerateDotFile("Facturas", bTreeDot);
                Utility.ConvertDotToImage("Facturas.dot");
            }

            Login.ShowDialog(this, MessageType.Info, "Los reportes han sido generados con éxito en la carpeta Reportes.");
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