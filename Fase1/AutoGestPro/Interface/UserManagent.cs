using Gtk;

namespace Interface
{
    public class UserManagent : Window
    {
        private Entry idEntry = new Entry();
        private Entry namesEntry = new Entry();
        private Entry lastNamesEntry = new Entry();
        private Entry emailEntry = new Entry();
        private Entry currentNames = new Entry();
        private Entry currentLastNames = new Entry();
        private Entry currentEmail = new Entry();

        public UserManagent() : base("AutoGest Pro - Ingreso de Usuario")
        {
            SetSizeRequest(600, 406); //(ancho, alto)
            SetPosition(WindowPosition.Center);

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

            Button saveButton = new Button("Guardar");
            saveButton.SetSizeRequest(120, 30);
            fixedContainer.Put(saveButton, 140, 296);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 350);

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