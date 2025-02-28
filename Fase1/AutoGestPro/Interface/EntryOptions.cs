using Gtk;

namespace Interface
{
    public class EntryOptions : Window
    {
        public EntryOptions() : base("AutoGest Pro - Opciones de Ingreso Indivual")
        {
            InitializeComponents();
        }

        public EntryOptions(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(350, 315); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            if (Child != null)
            {
                Remove(Child);
            }

            Fixed fixedContainer = new Fixed();

            Label optionsLabel1 = new Label();
            optionsLabel1.Markup = "<span font='Arial 18' weight='bold'>Selecciona que desea</span>";
            optionsLabel1.Halign = Align.Center;
            fixedContainer.Put(optionsLabel1, 50, 15);

            Label optionsLabel2 = new Label();
            optionsLabel2.Markup = "<span font='Arial 18' weight='bold'>ingresar:</span>";
            optionsLabel2.Halign = Align.Center;
            fixedContainer.Put(optionsLabel2, 125, 40);

            Button usersButton = new Button("Usuarios");
            usersButton.SetSizeRequest(280, 35);
            usersButton.Clicked += OnUsersButtonClicked;
            fixedContainer.Put(usersButton, 35, 93);

            Button vehiclesButton = new Button("Vehiculos");
            vehiclesButton.SetSizeRequest(280, 35);
            vehiclesButton.Clicked += OnVehiclesButtonClicked;
            fixedContainer.Put(vehiclesButton, 35, 148);

            Button sparePartsButton = new Button("Repuestos");
            sparePartsButton.SetSizeRequest(280, 35);
            sparePartsButton.Clicked += OnPartsButtonClicked;
            fixedContainer.Put(sparePartsButton, 35, 203);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 260);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
        }

        private void OnReturnButtonClicked(object? sender, EventArgs e)
        {
            GlobalWindows.menu.ShowAll();
            Hide();
        }

        private void OnUsersButtonClicked(object? sender, EventArgs e)
        {
            GlobalWindows.userEntry.ShowAll();
            Hide();
        }

        private void OnVehiclesButtonClicked(object? sender, EventArgs e)
        {
            GlobalWindows.vehicleEntry.ShowAll();
            Hide();
        }

        private void OnPartsButtonClicked(object? sender, EventArgs e)
        {
            GlobalWindows.sparePartEntry.ShowAll();
            Hide();
        }
    }
}