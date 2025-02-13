using Gtk;

namespace Interface
{
    public class Login : Window
    {
        public Login() : base("AutoGest Pro - Inicio de Sesión")
        {
            SetSizeRequest(450, 310); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label labelWelcome = new Label();
            labelWelcome.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>¡Bienvenido a AutoGest Pro!</span>";
            fixedContainer.Put(labelWelcome, 10, 15);

            Label labelLogin = new Label("Inicio de sesión");
            labelLogin.Markup = "<span font='Arial 18' weight='bold'>Inicio de sesión</span>";
            fixedContainer.Put(labelLogin, 20, 50);

            Label labelUser = new Label("Usuario:");
            labelLogin.Markup = "<span font='Arial 18' weight='bold'>Inicio de sesión</span>";
            fixedContainer.Put(labelUser, 20, 100);

            Button buttonLogin = new Button("Iniciar Sesión");
            buttonLogin.SetSizeRequest(200, 50);
            CssProvider cssProvider = new CssProvider();
            cssProvider.LoadFromData(
                "button.custom-button { " +
                "background-color:rgb(10, 223, 28); " +
                "color: #00008B; " +
                "font-size: 18px; " +
                "font-weight: bold; " +
                "border-radius: 5px; " +
                "}");
            buttonLogin.StyleContext.AddProvider(cssProvider, 800);
            buttonLogin.StyleContext.AddClass("custom-button");
            fixedContainer.Put(buttonLogin, 20, 200);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
        }
    }
}