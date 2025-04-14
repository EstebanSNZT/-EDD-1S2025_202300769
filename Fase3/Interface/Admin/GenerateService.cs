using Gtk;
using Global;
using Classes;

namespace Interface
{
    public class GenerateService : Window
    {
        private Entry idEntry = new Entry();
        private Entry sparePartIdEntry = new Entry();
        private Entry vehicleIdEntry = new Entry();
        private Entry detailsEntry = new Entry();
        private ComboBoxText paymentMethodComboBox = new ComboBoxText();
        private Entry costEntry = new Entry();
        private int invoiceId = 1;

        public GenerateService() : base("AutoGest Pro - Generar Servicio")
        {
            InitializeComponents();
        }

        public GenerateService(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(430, 515); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label generateServiceLabel = new Label();
            generateServiceLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Generar Servicio</span>";
            fixedContainer.Put(generateServiceLabel, 85, 15);

            Label idLabel = new Label();
            idLabel.Markup = "<span font='Arial 12' weight='bold'>ID:</span>";
            fixedContainer.Put(idLabel, 103, 82);

            idEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(idEntry, 212, 76);

            Label sparePartIdLabel = new Label();
            sparePartIdLabel.Markup = "<span font='Arial 12' weight='bold'>ID Repuesto:</span>";
            fixedContainer.Put(sparePartIdLabel, 64, 137);

            sparePartIdEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(sparePartIdEntry, 212, 131);

            Label vehicleIdLabel = new Label();
            vehicleIdLabel.Markup = "<span font='Arial 12' weight='bold'>ID Vehiculo:</span>";
            fixedContainer.Put(vehicleIdLabel, 68, 192);

            vehicleIdEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(vehicleIdEntry, 212, 186);

            Label detailsLabel = new Label();
            detailsLabel.Markup = "<span font='Arial 12' weight='bold'>Detalles:</span>";
            fixedContainer.Put(detailsLabel, 80, 247);

            detailsEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(detailsEntry, 212, 241);

            Label paymentMethodLabel = new Label();
            paymentMethodLabel.Markup = "<span font='Arial 12' weight='bold'>Método de Pago:</span>";
            fixedContainer.Put(paymentMethodLabel, 51, 302);

            paymentMethodComboBox.AppendText("Efectivo");
            paymentMethodComboBox.AppendText("Tarjeta de Débito");
            paymentMethodComboBox.AppendText("Tarjeta de Crédito");
            paymentMethodComboBox.Active = -1;
            paymentMethodComboBox.SetSizeRequest(168, 35);
            fixedContainer.Put(paymentMethodComboBox, 212, 296);            

            Label costLabel = new Label();
            costLabel.Markup = "<span font='Arial 12' weight='bold'>Costo:</span>";
            fixedContainer.Put(costLabel, 88, 357);

            costEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(costEntry, 212, 351);

            Button saveButton = new Button("Guardar");
            saveButton.SetSizeRequest(120, 30);
            saveButton.Clicked += OnSaveButtonClicked;
            fixedContainer.Put(saveButton, 155, 406);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 460);

            Add(fixedContainer);

            DeleteEvent += (o, args) =>
            {
                GlobalWindows.DestroyAll();
                Application.Quit();
            };
        }

        private void OnReturnButtonClicked(object sender, EventArgs e)
        {
            CleanEntrys();
            GlobalWindows.adminMenu.ShowAll();
            Hide();
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idEntry.Text) || string.IsNullOrEmpty(sparePartIdEntry.Text) ||
                string.IsNullOrEmpty(vehicleIdEntry.Text) || string.IsNullOrEmpty(detailsEntry.Text) ||
                string.IsNullOrEmpty(costEntry.Text) || paymentMethodComboBox.Active == -1)
            {
                Login.ShowDialog(this, MessageType.Warning, "Por favor, llene todos los campos.");
                return;
            }

            if (!int.TryParse(idEntry.Text, out int id))
            {
                Login.ShowDialog(this, MessageType.Error, "ID inválido.");
                return;
            }

            if (!int.TryParse(sparePartIdEntry.Text, out int sparePartId))
            {
                Login.ShowDialog(this, MessageType.Error, "ID de repuesto inválido.");
                return;
            }

            if (!int.TryParse(vehicleIdEntry.Text, out int vehicleId))
            {
                Login.ShowDialog(this, MessageType.Error, "ID de vehiculo inválido.");
                return;
            }

            if (!double.TryParse(costEntry.Text, out double cost))
            {
                Login.ShowDialog(this, MessageType.Error, "Costo inválido");
                return;
            }

            if (!GlobalStructures.VehiclesList.Contains(vehicleId))
            {
                Login.ShowDialog(this, MessageType.Error, "ID de vehiculo inexiste.");
                return;
            }

            SparePart sparePart = GlobalStructures.SparePartsTree.Get(sparePartId);
            
            if (sparePart == null)
            {
                Login.ShowDialog(this, MessageType.Error, "ID de repuesto inexiste.");
                return;
            }

            if (GlobalStructures.ServicesTree.Contains(id))
            {
                Login.ShowDialog(this, MessageType.Error, "Ya existe un servicio con ese ID.");
                return;
            }

            Service newService = new Service(id, sparePartId, vehicleId, detailsEntry.Text, cost);
            GlobalStructures.ServicesTree.Add(newService);

            GlobalStructures.Graph.AddEdge($"V{vehicleId}", $"R{sparePartId}");
            
            double total = newService.Cost + sparePart.Cost;
            string paymentMethod = paymentMethodComboBox.ActiveText;

            Invoice newInvoice = new Invoice(invoiceId, newService.Id, total, paymentMethod);
            GlobalStructures.InvoicesTree.Add(newInvoice);
            invoiceId++;

            Login.ShowDialog(this, MessageType.Info, "Servicio y factura generados con éxito.");
            CleanEntrys();
        }

        private void CleanEntrys()
        {
            idEntry.Text = "";
            sparePartIdEntry.Text = "";
            vehicleIdEntry.Text = "";
            detailsEntry.Text = "";
            costEntry.Text = "";
            paymentMethodComboBox.Active = -1;
        }
    }
}