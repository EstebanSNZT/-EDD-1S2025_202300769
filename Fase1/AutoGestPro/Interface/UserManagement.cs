using Gtk;
using Lists;

namespace Interface
{
    public unsafe class UserManagement : Window
    {
        private bool found = false;
        private int idFound = 0;
        private Entry idEntry = new Entry();
        private Entry currentIdEntry = new Entry();
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
            SetSizeRequest(598, 496); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            if (Child != null)
            {
                Remove(Child);
            }

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Gestión de Usuario</span>";
            fixedContainer.Put(menuLabel, 166, 15);

            Label idLabel = new Label();
            idLabel.Markup = "<span font='Arial 12' weight='bold'>ID:</span>";
            fixedContainer.Put(idLabel, 82, 79);

            idEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(idEntry, 170, 76);

            Button searchButton = new Button("Buscar");
            searchButton.SetSizeRequest(120, 30);
            searchButton.Clicked += OnSearchButtonClicked;
            fixedContainer.Put(searchButton, 403, 78);

            Label currentDataLabel = new Label();
            currentDataLabel.Markup = "<span font='Arial 12' weight='bold'>Datos Actuales</span>";
            fixedContainer.Put(currentDataLabel, 192, 129);

            Label newDataLabel = new Label();
            newDataLabel.Markup = "<span font='Arial 12' weight='bold'>Nuevos Datos</span>";
            fixedContainer.Put(newDataLabel, 410, 129);

            Label currentIdLabel = new Label();
            currentIdLabel.Markup = "<span font='Arial 12' weight='bold'>ID:</span>";
            fixedContainer.Put(currentIdLabel, 82, 172);

            currentIdEntry.SetSizeRequest(140, 35);
            currentIdEntry.Sensitive = false;
            fixedContainer.Put(currentIdEntry, 170, 166);

            Label namesLabel = new Label();
            namesLabel.Markup = "<span font='Arial 12' weight='bold'>Nombres:</span>";
            fixedContainer.Put(namesLabel, 52, 227);

            currentNamesEntry.SetSizeRequest(140, 35);
            currentNamesEntry.Sensitive = false;
            fixedContainer.Put(currentNamesEntry, 170, 221);

            newNamesEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(newNamesEntry, 380, 221);

            Label lastNamesLabel = new Label();
            lastNamesLabel.Markup = "<span font='Arial 12' weight='bold'>Apellidos:</span>";
            fixedContainer.Put(lastNamesLabel, 51, 282);

            currentLastNamesEntry.SetSizeRequest(140, 35);
            currentLastNamesEntry.Sensitive = false;
            fixedContainer.Put(currentLastNamesEntry, 170, 276);

            newLastNamesEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(newLastNamesEntry, 380, 276);

            Label emailLabel = new Label();
            emailLabel.Markup = "<span font='Arial 12' weight='bold'>Correo:</span>";
            fixedContainer.Put(emailLabel, 61, 337);

            currentEmailEntry.SetSizeRequest(140, 35);
            currentEmailEntry.Sensitive = false;
            fixedContainer.Put(currentEmailEntry, 170, 331);

            newEmailEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(newEmailEntry, 380, 331);

            Button deleteButton = new Button("Eliminar");
            deleteButton.SetSizeRequest(120, 30);
            deleteButton.Clicked += OnDeleteButtonClicked;
            fixedContainer.Put(deleteButton, 51, 386);

            Button updateButton = new Button("Actualizar");
            updateButton.SetSizeRequest(120, 30);
            updateButton.Clicked += OnUpdateButtonClicked;
            fixedContainer.Put(updateButton, 197, 386);

            Button viewVehiclesButton = new Button("Ver información de los vehículos");
            viewVehiclesButton.SetSizeRequest(160, 30);
            viewVehiclesButton.Clicked += OnViewVehiclesButtonClicked;
            fixedContainer.Put(viewVehiclesButton, 341, 386);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 440);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
        }

        private void OnReturnButtonClicked(object? sender, EventArgs e)
        {
            found = false;
            GlobalWindows.menu.ShowAll();
            CleanEntrys();
            Hide();
        }

        private void CleanEntrys()
        {
            idEntry.Text = "";
            currentIdEntry.Text = "";
            newNamesEntry.Text = "";
            newLastNamesEntry.Text = "";
            newEmailEntry.Text = "";
            currentNamesEntry.Text = "";
            currentLastNamesEntry.Text = "";
            currentEmailEntry.Text = "";
        }

        private void OnSearchButtonClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idEntry.Text))
            {
                Menu.ShowDialog(this, MessageType.Error, "Debe ingresar un ID de usuario.");
                return;
            }

            if (!int.TryParse(idEntry.Text, out idFound))
            {
                Menu.ShowDialog(this, MessageType.Error, "ID inválido.");
                return;
            }

            if (GlobalLists.linkedList.IsEmpty())
            {
                Menu.ShowDialog(this, MessageType.Warning, "La lista de usuarios se encuentra actualmente vacía. Ingrese usuarios antes de continuar.");
                return;
            }

            LinkedNode* userSearched = GlobalLists.linkedList.GetUser(idFound);

            if (userSearched == null)
            {
                Menu.ShowDialog(this, MessageType.Warning, "No se encontró un usuario con ese ID.");
                return;
            }
            else
            {
                Menu.ShowDialog(this, MessageType.Info, "Usuario encontrado.");
                found = true;
                currentIdEntry.Text = userSearched->Id.ToString();
                currentNamesEntry.Text = userSearched->GetNames();
                currentLastNamesEntry.Text = userSearched->GetLastNames();
                currentEmailEntry.Text = userSearched->GetEmail();
            }
        }

        private void OnDeleteButtonClicked(object? sender, EventArgs e)
        {
            if (!found)
            {
                Menu.ShowDialog(this, MessageType.Warning, "Busque un usuario que eliminar.");
                return;
            }

            GlobalLists.linkedList.Delete(idFound);
            GlobalLists.doubleList.Delete(idFound);
            Menu.ShowDialog(this, MessageType.Info, "Usuario y vehiculos del usuario eliminados correctamente.");

            found = false;
            CleanEntrys();
        }

        private void OnUpdateButtonClicked(object? sender, EventArgs e)
        {
            if (!found)
            {
                Menu.ShowDialog(this, MessageType.Warning, "Busque un usuario que actualizar.");
                return;
            }

            if (string.IsNullOrEmpty(newNamesEntry.Text) && string.IsNullOrEmpty(newLastNamesEntry.Text) && string.IsNullOrEmpty(newEmailEntry.Text))
            {
                Menu.ShowDialog(this, MessageType.Error, "Debe llenar por lo menos un campo de los nuevos datos.");
                return;
            }

            LinkedNode* updatedUser = GlobalLists.linkedList.UpdateUser(idFound, newNamesEntry.Text, newLastNamesEntry.Text, newEmailEntry.Text);
            if (updatedUser != null)
            {
                Menu.ShowDialog(this, MessageType.Info, "Datos del usuario actualizados correctamente.");
                GlobalLists.linkedList.Print();
                newNamesEntry.Text = "";
                newLastNamesEntry.Text = "";
                newEmailEntry.Text = "";
                currentNamesEntry.Text = updatedUser->GetNames();
                currentLastNamesEntry.Text = updatedUser->GetLastNames();
                currentEmailEntry.Text = updatedUser->GetEmail();
            }
            else
            {
                Menu.ShowDialog(this, MessageType.Error, "Error al actualizar los datos del usuario.");
            }
        }

        private void OnViewVehiclesButtonClicked(object? sender, EventArgs e)
        {
            if (!found)
            {
                Menu.ShowDialog(this, MessageType.Warning, "Busque un usuario para ver sus vehículos.");
                return;
            }

            ViewVehicles viewVehicles = new ViewVehicles(idFound);
            viewVehicles.ShowAll();
        }
    }
}