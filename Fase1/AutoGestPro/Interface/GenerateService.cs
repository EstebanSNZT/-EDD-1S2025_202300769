using Gtk;
using Lists;

namespace Interface
{
    public unsafe class GenerateService : Window
    {
        private Entry idEntry = new Entry();
        private Entry sparePartIdEntry = new Entry();
        private Entry vehicleIdEntry = new Entry();
        private Entry detailsEntry = new Entry();
        private Entry costEntry = new Entry();

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
            SetSizeRequest(400, 461); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            if (Child != null)
            {
                Remove(Child);
            }
            
            Fixed fixedContainer = new Fixed();

            Label generateServiceLabel = new Label();
            generateServiceLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Generar Servicio</span>";
            fixedContainer.Put(generateServiceLabel, 85, 15);

            Label idLabel = new Label();
            idLabel.Markup = "<span font='Arial 12' weight='bold'>ID:</span>";
            fixedContainer.Put(idLabel, 90, 82);

            idEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(idEntry, 182, 76);

            Label sparePartIdLabel = new Label();
            sparePartIdLabel.Markup = "<span font='Arial 12' weight='bold'>ID Repuesto:</span>";
            fixedContainer.Put(sparePartIdLabel, 51, 137);

            sparePartIdEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(sparePartIdEntry, 182, 131);

            Label vehicleIdLabel = new Label();
            vehicleIdLabel.Markup = "<span font='Arial 12' weight='bold'>ID Vehiculo:</span>";
            fixedContainer.Put(vehicleIdLabel, 55, 192);

            vehicleIdEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(vehicleIdEntry, 182, 186);

            Label detailsLabel = new Label();
            detailsLabel.Markup = "<span font='Arial 12' weight='bold'>Detalles:</span>";
            fixedContainer.Put(detailsLabel, 67, 247);

            detailsEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(detailsEntry, 182, 241);

            Label costLabel = new Label();
            costLabel.Markup = "<span font='Arial 12' weight='bold'>Costo:</span>";
            fixedContainer.Put(costLabel, 75, 302);

            costEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(costEntry, 182, 296);

            Button saveButton = new Button("Guardar");
            saveButton.SetSizeRequest(120, 30);
            saveButton.Clicked += OnSaveButtonClicked;
            fixedContainer.Put(saveButton, 140, 351);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 405);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
        }

        private void OnReturnButtonClicked(object? sender, EventArgs e)
        {
            if(!GlobalLists.stack.IsEmpty())
            {
                Menu.ShowDialog(this, MessageType.Info, "Tienes facturas por cancelar.");
            }
            GlobalWindows.menu.ShowAll();
            CleanEntrys();
            Hide();
        }

        private void CleanEntrys()
        {
            idEntry.Text = "";
            sparePartIdEntry.Text = "";
            vehicleIdEntry.Text = "";
            detailsEntry.Text = "";
            costEntry.Text = "";
        }

        private void OnSaveButtonClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idEntry.Text) || string.IsNullOrEmpty(sparePartIdEntry.Text) ||
                string.IsNullOrEmpty(vehicleIdEntry.Text) || string.IsNullOrEmpty(detailsEntry.Text) ||
                string.IsNullOrEmpty(costEntry.Text))
            {
                Menu.ShowDialog(this, MessageType.Warning, "Por favor, llene todos los campos.");
                return;
            }

            if (!int.TryParse(idEntry.Text, out int id))
            {
                Menu.ShowDialog(this, MessageType.Error, "ID inválido.");
                return;
            }

            if (!int.TryParse(sparePartIdEntry.Text, out int sparePartId))
            {
                Menu.ShowDialog(this, MessageType.Error, "ID de repuesto inválido.");
                return;
            }

            if (!int.TryParse(vehicleIdEntry.Text, out int vehicleId))
            {
                Menu.ShowDialog(this, MessageType.Error, "ID de vehiculo inválido.");
                return;
            }

            if (!double.TryParse(costEntry.Text, out double cost))
            {
                Menu.ShowDialog(this, MessageType.Error, "Costo inválido");
                return;
            }

            DoubleNode* vehicle = GlobalLists.doubleList.GetVehicle(vehicleId);

            if (vehicle == null)
            {
                Menu.ShowDialog(this, MessageType.Error, "ID de vehiculo inexiste.");
                return;
            }

            CircularNode* sparePart = GlobalLists.circularList.GetSparePart(sparePartId);

            if (sparePart == null)
            {
                Menu.ShowDialog(this, MessageType.Error, "ID de repuesto inexiste.");
                return;
            }

            GlobalLists.queue.Enqueue(id, sparePartId, vehicleId, detailsEntry.Text, cost);
            GlobalLists.queue.Print();

            GlobalLists.stack.Push(id, cost + sparePart->Cost);
            GlobalLists.stack.Print();

            GlobalLists.matrix.Insert(vehicleId, sparePartId, detailsEntry.Text);
            GlobalLists.matrix.Print();

            vehicle->ServiceCounter++;

            Menu.ShowDialog(this, MessageType.Info, "Servicio guardado con éxito.");
            
            CleanEntrys();
        }
    }
}