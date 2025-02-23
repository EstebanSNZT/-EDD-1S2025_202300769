using Gtk;

namespace Interface
{
    public class Menu : Window
    {
        public Menu() : base("AutoGest Pro - Menú")
        {
            InitializeComponents();
        }

        public Menu(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(350, 400); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Menú</span>";
            fixedContainer.Put(menuLabel, 135, 15);

            Button bulkUploadButton = new Button("Cargas Masivas");
            bulkUploadButton.SetSizeRequest(280, 35);
            bulkUploadButton.Clicked += OnButtonBulkUploadClicked;
            fixedContainer.Put(bulkUploadButton, 35, 65);

            Button individualEntryButton = new Button("Ingreso Individual");
            individualEntryButton.SetSizeRequest(280, 35);
            individualEntryButton.Clicked += OnButtonIndividualEntryClicked;
            fixedContainer.Put(individualEntryButton, 35, 120);

            Button userManagementButton = new Button("Gestión de Usuarios");
            userManagementButton.SetSizeRequest(280, 35);
            userManagementButton.Clicked += OnButtonUserManagementClicked;
            fixedContainer.Put(userManagementButton, 35, 175);

            Button generateServiceButton = new Button("Generar Servicio");
            generateServiceButton.SetSizeRequest(280, 35);
            generateServiceButton.Clicked += OnButtonGenerateServiceClicked;
            fixedContainer.Put(generateServiceButton, 35, 230);

            Button cancelInvoiceButton = new Button("Cancelar factura");
            cancelInvoiceButton.SetSizeRequest(280, 35);
            fixedContainer.Put(cancelInvoiceButton, 35, 285);

            Button reportsButton = new Button("Reportes");
            reportsButton.SetSizeRequest(280, 35);
            reportsButton.Clicked += OnButtonReportsClicked;
            fixedContainer.Put(reportsButton, 35, 340);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
        }

        private void OnButtonBulkUploadClicked(object? sender, EventArgs e)
        {
            this.Destroy();
            BulkUpload bulkUpload = new BulkUpload();
            bulkUpload.ShowAll();
        }

        private void OnButtonIndividualEntryClicked(object? sender, EventArgs e)
        {
            this.Destroy();
            EntryOptions entryOptions = new EntryOptions();
            entryOptions.ShowAll();
        }

        private void OnButtonGenerateServiceClicked(object? sender, EventArgs e)
        {
            this.Destroy();
            GenerateService generateService = new GenerateService();
            generateService.ShowAll();
        }

        private void OnButtonUserManagementClicked(object? sender, EventArgs e)
        {
            this.Destroy();
            UserManagement userManagement = new UserManagement();
            userManagement.ShowAll();
        }

        private void OnButtonReportsClicked(object? sender, EventArgs e)
        {
            this.Destroy();
            ReportsMenu reportsMenu = new ReportsMenu();
            reportsMenu.ShowAll();
        }

        public static void ShowDialog(Window window, MessageType messageType, string message)
        {
            MessageDialog dialog = new MessageDialog(window, DialogFlags.Modal, messageType, ButtonsType.Ok, message);
            dialog.Run();
            dialog.Destroy();
        }
    }
}