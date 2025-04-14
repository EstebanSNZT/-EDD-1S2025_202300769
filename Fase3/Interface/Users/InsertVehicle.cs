using Gtk;
using Classes;
using Global;

namespace Interface
{
    public class InsertVehicle : Window
    {
        private Entry idEntry = new Entry();
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
            SetSizeRequest(400, 405); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Insertar Vehículo</span>";
            fixedContainer.Put(menuLabel, 88, 15);

            Label idLabel = new Label();
            idLabel.Markup = "<span font='Arial 12' weight='bold'>ID:</span>";
            fixedContainer.Put(idLabel, 83, 82);

            idEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(idEntry, 182, 76);

            Label brandLabel = new Label();
            brandLabel.Markup = "<span font='Arial 12' weight='bold'>Marca:</span>";
            fixedContainer.Put(brandLabel, 68, 137);

            brandEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(brandEntry, 182, 131);

            Label modelLabel = new Label();
            modelLabel.Markup = "<span font='Arial 12' weight='bold'>Modelo:</span>";
            fixedContainer.Put(modelLabel, 63, 192);

            modelEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(modelEntry, 182, 186);

            Label plateLabel = new Label();
            plateLabel.Markup = "<span font='Arial 12' weight='bold'>Placa:</span>";
            fixedContainer.Put(plateLabel, 70, 247);

            plateEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(plateEntry, 182, 241);

            Button saveButton = new Button("Guardar");
            saveButton.SetSizeRequest(120, 30);
            saveButton.Clicked += OnSaveButtonClicked;
            fixedContainer.Put(saveButton, 140, 296);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 350);

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
            GlobalWindows.userMenu.ShowAll();
            Hide();
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(idEntry.Text) || string.IsNullOrWhiteSpace(brandEntry.Text) ||
                string.IsNullOrWhiteSpace(modelEntry.Text) || string.IsNullOrWhiteSpace(plateEntry.Text))
            {
                Login.ShowDialog(this, MessageType.Error, "Por favor, llene todos los campos.");
                return;
            }

            int idUser = LoginControl.LoggedUserId;

            if (!int.TryParse(idEntry.Text, out int id))
            {
                Login.ShowDialog(this, MessageType.Error, "ID de vehículo inválido. Ingrese una ID de vehículo válido");
                return;
            }

            if (!int.TryParse(modelEntry.Text, out int model))
            {
                Login.ShowDialog(this, MessageType.Error, "Modelo inválido. Ingrese un modelo válido.");
                return;
            }

            if (!GlobalStructures.UsersBlockchain.Contains(idUser))
            {
                Login.ShowDialog(this, MessageType.Warning, "No se encontró ningún usuario con ese ID. Intente con un ID distinto.");
                return;
            }

            if (GlobalStructures.VehiclesList.Contains(id))
            {
                Login.ShowDialog(this, MessageType.Error, "Ya existe un vehículo con ese ID. Ingrese un ID diferente.");
                return;
            }   

            Vehicle newVehicle = new Vehicle(id, idUser, brandEntry.Text, model, plateEntry.Text);
            GlobalStructures.VehiclesList.Add(newVehicle);

            Login.ShowDialog(this, MessageType.Info, "Vehículo insertado con éxito.");
            CleanEntrys();
        }

        private void CleanEntrys()
        {
            idEntry.Text = "";
            brandEntry.Text = "";
            modelEntry.Text = "";
            plateEntry.Text = "";
        }
    }
}