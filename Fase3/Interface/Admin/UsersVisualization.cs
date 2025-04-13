using Gtk;
using Global;
using Classes;

namespace Interface
{
    public class UsersVisualization : Window
    {
        private Entry idEntry = new Entry();
        private Entry namesEntry = new Entry();
        private Entry lastNamesEntry = new Entry();
        private Entry emailEntry = new Entry();
        private Entry ageEntry = new Entry();
        private Entry passwordEntry = new Entry();

        public UsersVisualization() : base("AutoGest Pro - Visualización de Usuarios")
        {
            InitializeComponents();
        }

        public UsersVisualization(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(400, 516); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Visualización de Usuarios</span>";
            fixedContainer.Put(menuLabel, 15, 15);

            Label idLabel = new Label();
            idLabel.Markup = "<span font='Arial 12' weight='bold'>ID:</span>";
            fixedContainer.Put(idLabel, 82, 82);

            idEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(idEntry, 182, 76);

            Label namesLabel = new Label();
            namesLabel.Markup = "<span font='Arial 12' weight='bold'>Nombres:</span>";
            fixedContainer.Put(namesLabel, 61, 137);

            namesEntry.SetSizeRequest(140, 35);
            namesEntry.Sensitive = false;
            fixedContainer.Put(namesEntry, 182, 131);

            Label lastNamesLabel = new Label();
            lastNamesLabel.Markup = "<span font='Arial 12' weight='bold'>Apellidos:</span>";
            fixedContainer.Put(lastNamesLabel, 59, 192);

            lastNamesEntry.SetSizeRequest(140, 35);
            lastNamesEntry.Sensitive = false;
            fixedContainer.Put(lastNamesEntry, 182, 186);

            Label emailLabel = new Label();
            emailLabel.Markup = "<span font='Arial 12' weight='bold'>Correo:</span>";
            fixedContainer.Put(emailLabel, 69, 247);

            emailEntry.SetSizeRequest(140, 35);
            emailEntry.Sensitive = false;
            fixedContainer.Put(emailEntry, 182, 241);

            Label ageLabel = new Label();
            ageLabel.Markup = "<span font='Arial 12' weight='bold'>Edad:</span>";
            fixedContainer.Put(ageLabel, 75, 302);

            ageEntry.SetSizeRequest(140, 35);
            ageEntry.Sensitive = false;
            fixedContainer.Put(ageEntry, 182, 296);

            Label passwordLabel = new Label();
            passwordLabel.Markup = "<span font='Arial 12' weight='bold'>Contraseña:</span>";
            fixedContainer.Put(passwordLabel, 50, 357);

            passwordEntry.SetSizeRequest(140, 35);
            passwordEntry.Sensitive = false;
            fixedContainer.Put(passwordEntry, 182, 351);

            Button viewButton = new Button("Buscar");
            viewButton.SetSizeRequest(120, 30);
            viewButton.Clicked += OnSearchButtonClicked;
            fixedContainer.Put(viewButton, 140, 406);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 461);

            Add(fixedContainer);

            DeleteEvent += (o, args) =>
            {
                GlobalWindows.DestroyAll();
                Application.Quit();
            };
        }

        private void OnReturnButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.adminMenu.ShowAll();
            CleanEntrys();
            Hide();
        }

        private void OnSearchButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idEntry.Text))
            {
                Login.ShowDialog(this, MessageType.Error, "Debe ingresar un ID de usuario para poder buscar.");
                return;
            }

            if (!int.TryParse(idEntry.Text, out int id))
            {
                Login.ShowDialog(this, MessageType.Error, "ID inválido. Ingrese una ID válido.");
                return;
            }

            if (GlobalStructures.UsersBlockchain.IsEmpty())
            {
                Login.ShowDialog(this, MessageType.Warning, "La lista de usuarios se encuentra actualmente vacía. Ingrese usuarios a listas antes de visualizar.");
                return;
            }

            User user = GlobalStructures.UsersBlockchain.Get(id);

            if (user == null)
            {
                Login.ShowDialog(this, MessageType.Warning, "No se encontró ningún usuario con ese ID. Intente con un ID distinto.");
                return;
            }

            Login.ShowDialog(this, MessageType.Info, $"Visualizando los datos del usuario: {user.Names} {user.LastNames}.");
            namesEntry.Text = user.Names;
            lastNamesEntry.Text = user.LastNames;
            emailEntry.Text = user.Email;
            ageEntry.Text = user.Age.ToString();
            passwordEntry.Text = user.Password;
        }

        private void CleanEntrys()
        {
            idEntry.Text = "";
            namesEntry.Text = "";
            lastNamesEntry.Text = "";
            emailEntry.Text = "";
            ageEntry.Text = "";
            passwordEntry.Text = "";
        }
    }
}