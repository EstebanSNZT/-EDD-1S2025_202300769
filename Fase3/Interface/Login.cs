using Classes;
using Global;
using Gtk;

namespace Interface
{
    public class Login : Window
    {
        private Entry emailEntry = new Entry();
        private Entry passwordEntry = new Entry();

        public Login() : base("AutoGest Pro - Inicio de Sesión")
        {
            InitializeComponents();
        }

        public Login(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
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

            Label emailLabel = new Label();
            emailLabel.Markup = "<span font='Arial 16'>Correo:</span>";
            fixedContainer.Put(emailLabel, 65, 125);

            emailEntry.SetSizeRequest(200, 20);
            fixedContainer.Put(emailEntry, 190, 120);

            Label passwordLabel = new Label();
            passwordLabel.Markup = "<span font='Arial 16'>Contraseña:</span>";
            fixedContainer.Put(passwordLabel, 60, 185);

            passwordEntry.SetSizeRequest(200, 20);
            passwordEntry.Visibility = false;
            fixedContainer.Put(passwordEntry, 190, 180);

            Button loginButton = new Button("Iniciar Sesión");
            loginButton.Clicked += OnLoginButtonClicked;
            loginButton.SetSizeRequest(180, 40);
            fixedContainer.Put(loginButton, 135, 245);

            Add(fixedContainer);

            DeleteEvent += (o, args) =>
            {
                GlobalWindows.DestroyAll();
                Application.Quit();
            };
        }

        private void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string email = emailEntry.Text;
            string password = passwordEntry.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ShowDialog(this, MessageType.Error, "Por favor, complete todos los campos.");
                return;
            }

            if (email.Equals("admin@usac.com") && password.Equals("admin123"))
            {
                ShowDialog(this, MessageType.Info, "¡Bienvenido Admin!");
                LoginControl.GenerateLoginTime(0, email);
                CleanEntrys();
                GlobalWindows.adminMenu.ShowAll();
                Hide();
            }
            else
            {
                User user = GlobalStructures.UsersBlockchain.GetByEmail(email);

                if (user == null || !user.ComparePassword(password))
                {
                    ShowDialog(this, MessageType.Error, "Correo o contraseña incorrectos.");
                    return;
                }

                ShowDialog(this, MessageType.Info, $"¡Bienvenido {user.Names} {user.LastNames}!");
                LoginControl.GenerateLoginTime(user.Id, user.Email);
                CleanEntrys();
                GlobalWindows.userMenu.menuLabel.Markup = $"<span font='Arial 16' weight='bold' foreground='blue'>{user.Email}</span>";
                GlobalWindows.userMenu.ShowAll();
                Hide();
            }
        }

        public static void ShowDialog(Window window, MessageType messageType, string message)
        {
            MessageDialog dialog = new MessageDialog(window, DialogFlags.Modal, messageType, ButtonsType.Ok, message);
            dialog.Run();
            dialog.Destroy();
        }

        public void CleanEntrys()
        {
            emailEntry.Text = "";
            passwordEntry.Text = "";
        }
    }
}