using Gtk;

namespace Interface
{
    public class UserManagement : Window
    {
        private Entry idEntry = new Entry();
        private Entry newNamesEntry = new Entry();
        private Entry newLastNamesEntry = new Entry();
        private Entry newEmailEntry = new Entry();
        private Entry currentNamesEntry = new Entry();
        private Entry currentLastNamesEntry = new Entry();
        private Entry currentEmailEntry = new Entry();

        public UserManagement() : base("AutoGest Pro - Gestión de Usuarios")
        {
            InitializeComponents();
        }

        public UserManagement(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(598, 441); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Ingreso de Usuario</span>";
            fixedContainer.Put(menuLabel, 169, 15);

            Label idLabel = new Label();
            idLabel.Markup = "<span font='Arial 12' weight='bold'>ID:</span>";
            fixedContainer.Put(idLabel, 82, 79);

            idEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(idEntry, 170, 76);

            Button searchButton = new Button("Buscar");
            searchButton.SetSizeRequest(120, 30);
            fixedContainer.Put(searchButton, 403, 78);

            Label currentDataLabel = new Label();
            currentDataLabel.Markup = "<span font='Arial 12' weight='bold'>Datos Actuales</span>";
            fixedContainer.Put(currentDataLabel, 192, 129);

            Label newDataLabel = new Label();
            newDataLabel.Markup = "<span font='Arial 12' weight='bold'>Nuevos Datos</span>";
            fixedContainer.Put(newDataLabel, 410, 129);

            Label namesLabel = new Label();
            namesLabel.Markup = "<span font='Arial 12' weight='bold'>Nombres:</span>";
            fixedContainer.Put(namesLabel, 52, 172);

            currentNamesEntry.SetSizeRequest(140, 35);
            currentNamesEntry.Sensitive = false;
            fixedContainer.Put(currentNamesEntry, 170, 166);

            newNamesEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(newNamesEntry, 380, 166);

            Label lastNamesLabel = new Label();
            lastNamesLabel.Markup = "<span font='Arial 12' weight='bold'>Apellidos:</span>";
            fixedContainer.Put(lastNamesLabel, 51, 227);

            currentLastNamesEntry.SetSizeRequest(140, 35);
            currentLastNamesEntry.Sensitive = false;
            fixedContainer.Put(currentLastNamesEntry, 170, 221);

            newLastNamesEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(newLastNamesEntry, 380, 221);

            Label emailLabel = new Label();
            emailLabel.Markup = "<span font='Arial 12' weight='bold'>Correo:</span>";
            fixedContainer.Put(emailLabel, 61, 282);

            currentEmailEntry.SetSizeRequest(140, 35);
            currentEmailEntry.Sensitive = false;
            fixedContainer.Put(currentEmailEntry, 170, 276);

            newEmailEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(newEmailEntry, 380, 276);

            Button deleteButton = new Button("Eliminar");
            deleteButton.SetSizeRequest(120, 30);
            fixedContainer.Put(deleteButton, 51, 331);

            Button updateButton = new Button("Actualizar");
            updateButton.SetSizeRequest(120, 30);
            fixedContainer.Put(updateButton, 197, 331);

            Button viewVehiclesButton = new Button("Ver Información de los Vehículos");
            viewVehiclesButton.SetSizeRequest(160, 30);
            fixedContainer.Put(viewVehiclesButton, 341, 331);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 385);

            Label coordsLabel = new Label("Coordenadas: X = 0, Y = 0");
            fixedContainer.Put(coordsLabel, 0, 0);

            MotionNotifyEvent += (o, args) =>
            {
                int x = (int)args.Event.X;
                int y = (int)args.Event.Y;
                coordsLabel.Text = $"Coordenadas: X = {x}, Y = {y}";
            };

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