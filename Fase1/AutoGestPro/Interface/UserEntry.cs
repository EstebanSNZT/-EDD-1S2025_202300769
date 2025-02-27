using Gtk;
using Lists;

namespace Interface
{
    public class UserEntry : Window
    {
        private Entry idEntry = new Entry();
        private Entry namesEntry = new Entry();
        private Entry lastNamesEntry = new Entry();
        private Entry emailEntry = new Entry();
        private Entry passwordEntry = new Entry();

        public UserEntry() : base("AutoGest Pro - Ingreso de Usuario")
        {
            InitializeComponents();
        }

        public UserEntry(IntPtr raw) : base(raw)
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
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Ingreso de Usuario</span>";
            fixedContainer.Put(menuLabel, 69, 15);

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

            Label passwordLabel = new Label();
            passwordLabel.Markup = "<span font='Arial 12' weight='bold'>Contraseña:</span>";
            fixedContainer.Put(passwordLabel, 50, 302);

            passwordEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(passwordEntry, 182, 296);

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
            EntryOptions entryOptions = new EntryOptions();
            entryOptions.ShowAll();
            this.Dispose();
        }

        private void OnSaveButtonClicked(object? sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(idEntry.Text) || string.IsNullOrEmpty(namesEntry.Text) ||
                string.IsNullOrEmpty(lastNamesEntry.Text) || string.IsNullOrEmpty(emailEntry.Text) ||
                string.IsNullOrEmpty(passwordEntry.Text))
            {
                Menu.ShowDialog(this, MessageType.Warning, "Por favor, llene todos los campos.");
                return;
            }

            if (!int.TryParse(idEntry.Text, out int id))
            {
                Menu.ShowDialog(this, MessageType.Error, "ID inválido.");
                return;
            }

            GlobalLists.linkedList.Insert(id, namesEntry.Text, lastNamesEntry.Text, emailEntry.Text, passwordEntry.Text);
            GlobalLists.linkedList.Print();

            Menu.ShowDialog(this, MessageType.Info, "Usuario guardado con éxito.");

            idEntry.Text = "";
            namesEntry.Text = "";
            lastNamesEntry.Text = "";
            emailEntry.Text = "";
            passwordEntry.Text = "";
        }
    }
}