using Gtk;
using Global;
namespace Interface
{
    public class EntityManagement : Window
    {
        public EntityManagement() : base("AutoGest Pro - Opciones de Gestíon de Entidades")
        {
            InitializeComponents();
        }

        public EntityManagement(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(350, 260); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            if (Child != null)
            {
                Remove(Child);
            }

            Fixed fixedContainer = new Fixed();

            Label optionsLabel1 = new Label();
            optionsLabel1.Markup = "<span font='Arial 18' weight='bold'>Selecciona que entidad</span>";
            optionsLabel1.Halign = Align.Center;
            fixedContainer.Put(optionsLabel1, 45, 15);

            Label optionsLabel2 = new Label();
            optionsLabel2.Markup = "<span font='Arial 18' weight='bold'>desea gestionar:</span>";
            optionsLabel2.Halign = Align.Center;
            fixedContainer.Put(optionsLabel2, 89, 40);

            Button usersButton = new Button("Usuarios");
            usersButton.SetSizeRequest(280, 35);
            usersButton.Clicked += OnUsersButtonClicked;
            fixedContainer.Put(usersButton, 35, 93);

            Button vehiclesButton = new Button("Vehiculos");
            vehiclesButton.SetSizeRequest(280, 35);
            vehiclesButton.Clicked += OnVehiclesButtonClicked;
            fixedContainer.Put(vehiclesButton, 35, 148);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 205);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
        }

        private void OnReturnButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.adminMenu.ShowAll();
            Hide();
        }

        private void OnUsersButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.userManagement.ShowAll();
            Hide();
        }

        private void OnVehiclesButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.vehicleManagement.ShowAll();
            Hide();
        }
    }
}