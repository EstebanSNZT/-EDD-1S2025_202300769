using Gtk;

namespace Interface
{
    public class Login : Window
    {
        private Entry userEntry;
        private Entry passwordEntry;

        public Login() : base("AutoGest Pro - Inicio de Sesión")
        {
            SetSizeRequest(450, 310); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label welcomeLabel = new Label();
            welcomeLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>¡Bienvenido a AutoGest Pro!</span>";
            fixedContainer.Put(welcomeLabel, 27, 15);

            Label loginLabel = new Label();
            loginLabel.Markup = "<span font='Arial 18' weight='bold'>Inicio de sesión</span>";
            fixedContainer.Put(loginLabel, 135, 65);

            Label userLabel = new Label();
            userLabel.Markup = "<span font='Arial 16'>Usuario:</span>";
            fixedContainer.Put(userLabel, 60, 125);

            userEntry = new Entry();
            userEntry.SetSizeRequest(200, 20);
            fixedContainer.Put(userEntry, 190, 120);

            Label passwordLabel = new Label();
            passwordLabel.Markup = "<span font='Arial 16'>Contraseña:</span>";
            fixedContainer.Put(passwordLabel, 60, 185);

            passwordEntry = new Entry();
            passwordEntry.SetSizeRequest(200, 20);
            passwordEntry.Visibility = false;
            fixedContainer.Put(passwordEntry, 190, 180);

            Button loginButton = new Button("Iniciar Sesión");
            loginButton.Clicked += OnLoginButtonClicked;
            loginButton.SetSizeRequest(180, 40);
            fixedContainer.Put(loginButton, 135, 245);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
        }

        private void OnLoginButtonClicked(object? sender, EventArgs e)
        {
            this.Destroy();
            Menu menu = new Menu();
            menu.ShowAll();
            // string user = entryUser.Text;
            // string password = entryPassword.Text;

            // MessageDialog messageDialog;

            // if (user == "root@gmail.com" && password == "root123")
            // {
            //     messageDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "¡Bienvenido!");
            // }
            // else
            // {
            //     messageDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Usuario o contraseña incorrectos");
            // }

            // messageDialog.Run();
            // messageDialog.Dispose();
        }
    }
}