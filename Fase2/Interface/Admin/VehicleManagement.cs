using Gtk;
using Structures;
using Classes;
using Global;

namespace Interface
{
    public class VehicleManagement : Window
    {
        private Entry idEntry = new Entry();
        private Entry idUserEntry = new Entry();
        private Entry brandEntry = new Entry();
        private Entry modelEntry = new Entry();
        private Entry plateEntry = new Entry();

        public VehicleManagement() : base("AutoGest Pro - Ingreso de Vehículo")
        {
            InitializeComponents();
        }

        public VehicleManagement(IntPtr raw) : base(raw)
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

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Gestión de Vehículo</span>";
            fixedContainer.Put(menuLabel, 63, 15);

            Label idLabel = new Label();
            idLabel.Markup = "<span font='Arial 12' weight='bold'>ID:</span>";
            fixedContainer.Put(idLabel, 83, 82);

            idEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(idEntry, 182, 76);

            Label idUserLabel = new Label();
            idUserLabel.Markup = "<span font='Arial 12' weight='bold'>ID Usuario:</span>";
            fixedContainer.Put(idUserLabel, 51, 137);

            idUserEntry.SetSizeRequest(140, 35);
            idUserEntry.Sensitive = false;
            fixedContainer.Put(idUserEntry, 182, 131);

            Label brandLabel = new Label();
            brandLabel.Markup = "<span font='Arial 12' weight='bold'>Marca:</span>";
            fixedContainer.Put(brandLabel, 68, 192);

            brandEntry.SetSizeRequest(140, 35);
            brandEntry.Sensitive = false;
            fixedContainer.Put(brandEntry, 182, 186);

            Label modelLabel = new Label();
            modelLabel.Markup = "<span font='Arial 12' weight='bold'>Modelo:</span>";
            fixedContainer.Put(modelLabel, 63, 247);

            modelEntry.SetSizeRequest(140, 35);
            modelEntry.Sensitive = false;
            fixedContainer.Put(modelEntry, 182, 241);

            Label plateLabel = new Label();
            plateLabel.Markup = "<span font='Arial 12' weight='bold'>Placa:</span>";
            fixedContainer.Put(plateLabel, 70, 302);

            plateEntry.SetSizeRequest(140, 35);
            plateEntry.Sensitive = false;
            fixedContainer.Put(plateEntry, 182, 296);

            Button viewButton = new Button("Visualizar");
            viewButton.SetSizeRequest(120, 30);
            viewButton.Clicked += OnViewButtonClicked;
            fixedContainer.Put(viewButton, 50, 351);

            Button deleteButton = new Button("Eliminar");
            deleteButton.SetSizeRequest(120, 30);
            deleteButton.Clicked += OnDeleteButtonClicked;
            fixedContainer.Put(deleteButton, 230, 351);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 405);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
        }

        private void OnReturnButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.entityManagement.ShowAll();
            CleanEntrys();
            Hide();
        }

        private void OnViewButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idEntry.Text))
            {
                Login.ShowDialog(this, MessageType.Error, "Debe ingresar un ID de vehículo.");
                return;
            }

            if (!int.TryParse(idEntry.Text, out int id))
            {
                Login.ShowDialog(this, MessageType.Error, "ID inválido.");
                return;
            }

            if (GlobalStructures.VehiclesList.IsEmpty())
            {
                Login.ShowDialog(this, MessageType.Warning, "La lista de vehículos se encuentra actualmente vacía. Ingrese vehículos antes de visualizar.");
                return;
            }

            Vehicle vehicle = GlobalStructures.VehiclesList.Get(id);

            if (vehicle == null)
            {
                Login.ShowDialog(this, MessageType.Warning, "No se encontró ningún usuario con ese ID.");
                return;
            }

            Login.ShowDialog(this, MessageType.Info, $"Visualizando los datos del vehículo con placa: {vehicle.Plate}.");
            idUserEntry.Text = vehicle.UserId.ToString();
            brandEntry.Text = vehicle.Brand;
            modelEntry.Text = vehicle.Model.ToString();
            plateEntry.Text = vehicle.Plate;
        }

        private void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idEntry.Text))
            {
                Login.ShowDialog(this, MessageType.Error, "Debe ingresar un ID de vehículo.");
                return;
            }

            if (!int.TryParse(idEntry.Text, out int id))
            {
                Login.ShowDialog(this, MessageType.Error, "ID inválido.");
                return;
            }

            if (GlobalStructures.VehiclesList.IsEmpty())
            {
                Login.ShowDialog(this, MessageType.Warning, "La lista de vehículos se encuentra actualmente vacía. Ingrese vehículos antes de eliminar.");
                return;
            }

            if (GlobalStructures.VehiclesList.Delete(id))
            {
                Login.ShowDialog(this, MessageType.Info, $"Vehículo con ID {id} eliminado correctamente.");
                CleanEntrys();
            }
            else
            {
                Login.ShowDialog(this, MessageType.Warning, "No se encontró ningún vehículo con ese ID.");
            }
        }

        private void CleanEntrys()
        {
            idEntry.Text = "";
            idUserEntry.Text = "";
            brandEntry.Text = "";
            modelEntry.Text = "";
            plateEntry.Text = "";
        }
    }
}