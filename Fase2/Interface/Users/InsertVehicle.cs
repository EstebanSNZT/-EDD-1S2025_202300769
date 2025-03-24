using Gtk;
using Classes;
using Global;

namespace Interface
{
    public class InsertVehicle : Window
    {
        private Entry idEntry = new Entry();
        private Entry idUserEntry = new Entry();
        private Entry brandEntry = new Entry();
        private Entry modelEntry = new Entry();
        private Entry plateEntry = new Entry();

        public InsertVehicle() : base("AutoGest Pro - Insertar Vehículo")
        {
            InitializeComponents();
        }

        public InsertVehicle(IntPtr raw) : base(raw)
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
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Insertar Vehículo</span>";
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
            fixedContainer.Put(idUserEntry, 182, 131);

            Label brandLabel = new Label();
            brandLabel.Markup = "<span font='Arial 12' weight='bold'>Marca:</span>";
            fixedContainer.Put(brandLabel, 68, 192);

            brandEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(brandEntry, 182, 186);

            Label modelLabel = new Label();
            modelLabel.Markup = "<span font='Arial 12' weight='bold'>Modelo:</span>";
            fixedContainer.Put(modelLabel, 63, 247);

            modelEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(modelEntry, 182, 241);

            Label plateLabel = new Label();
            plateLabel.Markup = "<span font='Arial 12' weight='bold'>Placa:</span>";
            fixedContainer.Put(plateLabel, 70, 302);

            plateEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(plateEntry, 182, 296);

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

        private void OnReturnButtonClicked(object sender, EventArgs e)
        {
            CleanEntrys();
            GlobalWindows.userMenu.ShowAll();
            Hide();
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(idEntry.Text) || string.IsNullOrWhiteSpace(idUserEntry.Text) ||
                string.IsNullOrWhiteSpace(brandEntry.Text) || string.IsNullOrWhiteSpace(modelEntry.Text) ||
                string.IsNullOrWhiteSpace(plateEntry.Text))
            {
                Login.ShowDialog(this, MessageType.Error, "Por favor, llene todos los campos.");
                return;
            }

            if (!int.TryParse(idEntry.Text, out int id))
            {
                Login.ShowDialog(this, MessageType.Error, "ID de vehículo inválido.");
                return;
            }

            if (!int.TryParse(idUserEntry.Text, out int idUser))
            {
                Login.ShowDialog(this, MessageType.Error, "ID de usuario inválido.");
                return;
            }

            if (!GlobalStructures.UsersList.Contains(idUser))
            {
                Login.ShowDialog(this, MessageType.Error, "ID de usuario inexiste.");
                return;
            }         

            if (!int.TryParse(modelEntry.Text, out int model))
            {
                Login.ShowDialog(this, MessageType.Error, "Modelo inválido.");
                return;
            }

            if (GlobalStructures.VehiclesList.Contains(id))
            {
                Login.ShowDialog(this, MessageType.Error, "Ya existe un vehículo con ese ID.");
                return;
            }   

            Vehicle newVehicle = new Vehicle(id, idUser, brandEntry.Text, model, plateEntry.Text);
            GlobalStructures.VehiclesList.Insert(newVehicle);

            Login.ShowDialog(this, MessageType.Info, "Vehículo insertado con éxito.");
            CleanEntrys();
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