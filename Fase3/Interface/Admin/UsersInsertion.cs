using Gtk;
using Global;
using Classes;

namespace Interface
{
    public class UsersInsertion : Window
    {
        private Entry idEntry = new Entry();
        private Entry namesEntry = new Entry();
        private Entry lastNamesEntry = new Entry();
        private Entry emailEntry = new Entry();
        private Entry ageEntry = new Entry();
        private Entry passwordEntry = new Entry();

        public UsersInsertion() : base("AutoGest Pro - Inserción de Usuarios")
        {
            InitializeComponents();
        }

        public UsersInsertion(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(400, 516); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Inserción de Usuarios</span>";
            fixedContainer.Put(menuLabel, 49, 15);

            Label idLabel = new Label();
            idLabel.Markup = "<span font='Arial 12' weight='bold'>ID:</span>";
            fixedContainer.Put(idLabel, 82, 82);

            idEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(idEntry, 182, 76);

            Label namesLabel = new Label();
            namesLabel.Markup = "<span font='Arial 12' weight='bold'>Nombres:</span>";
            fixedContainer.Put(namesLabel, 61, 137);

            namesEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(namesEntry, 182, 131);

            Label lastNamesLabel = new Label();
            lastNamesLabel.Markup = "<span font='Arial 12' weight='bold'>Apellidos:</span>";
            fixedContainer.Put(lastNamesLabel, 59, 192);

            lastNamesEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(lastNamesEntry, 182, 186);

            Label emailLabel = new Label();
            emailLabel.Markup = "<span font='Arial 12' weight='bold'>Correo:</span>";
            fixedContainer.Put(emailLabel, 69, 247);

            emailEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(emailEntry, 182, 241);

            Label ageLabel = new Label();
            ageLabel.Markup = "<span font='Arial 12' weight='bold'>Edad:</span>";
            fixedContainer.Put(ageLabel, 75, 302);

            ageEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(ageEntry, 182, 296);

            Label passwordLabel = new Label();
            passwordLabel.Markup = "<span font='Arial 12' weight='bold'>Contraseña:</span>";
            fixedContainer.Put(passwordLabel, 50, 357);

            passwordEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(passwordEntry, 182, 351);

            Button viewButton = new Button("Insertar");
            viewButton.SetSizeRequest(120, 30);
            viewButton.Clicked += OnInsertButtonClicked;
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

        private void OnInsertButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idEntry.Text) || string.IsNullOrEmpty(namesEntry.Text) ||
                string.IsNullOrEmpty(lastNamesEntry.Text) || string.IsNullOrEmpty(emailEntry.Text) ||
                string.IsNullOrEmpty(ageEntry.Text) || string.IsNullOrEmpty(passwordEntry.Text))
            {
                Login.ShowDialog(this, MessageType.Warning, "Por favor, llene todos los campos para poder insertar.");
                return;
            }

            if (!int.TryParse(idEntry.Text, out int id))
            {
                Login.ShowDialog(this, MessageType.Error, "ID inválido. Ingrese una ID válido.");
                return;
            }

            if (!int.TryParse(ageEntry.Text, out int age))
            {
                Login.ShowDialog(this, MessageType.Error, "Edad inválida. Ingrese una edad válida.");
                return;
            }

            if (age < 0)
            {
                Login.ShowDialog(this, MessageType.Error, "La edad no puede ser negativa. Ingrese una edad válida.");
                return;
            }

            if (GlobalStructures.UsersBlockchain.Contains(id))
            {
                Login.ShowDialog(this, MessageType.Error, "Ya existe un usuario con ese ID. Ingrese un ID diferente.");
                return;
            }

            if (GlobalStructures.UsersBlockchain.ContainsByEmail(emailEntry.Text))
            {
                Login.ShowDialog(this, MessageType.Error, "Ya existe un usuario con ese correo. Ingrese un correo diferente.");
                return;
            }

            User newUser = new User(id, namesEntry.Text, lastNamesEntry.Text, emailEntry.Text, age, passwordEntry.Text);
            GlobalStructures.UsersBlockchain.AddBlock(newUser);

            Login.ShowDialog(this, MessageType.Info, "Usuario insertado con éxito.");
            CleanEntrys();
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