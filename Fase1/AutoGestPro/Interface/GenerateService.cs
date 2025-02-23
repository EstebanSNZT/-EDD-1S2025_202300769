using Gtk;

namespace Interface
{
    public class GenerateService : Window
    {
        private Entry idEntry = new Entry();
        private Entry idPartEntry = new Entry();
        private Entry idVehicleEntry = new Entry();
        private Entry detailsEntry = new Entry();
        private Entry costEntry = new Entry();

        public GenerateService() : base("AutoGest Pro - Generar Servicio")
        {
            InitializeComponents();
        }

        public GenerateService(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(400, 461); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Generar Servicio</span>";
            fixedContainer.Put(menuLabel, 85, 15);

            Label idLabel = new Label();
            idLabel.Markup = "<span font='Arial 12' weight='bold'>ID:</span>";
            fixedContainer.Put(idLabel, 90, 82);

            idEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(idEntry, 182, 76);

            Label idPartLabel = new Label();
            idPartLabel.Markup = "<span font='Arial 12' weight='bold'>ID Repuesto:</span>";
            fixedContainer.Put(idPartLabel, 51, 137);

            idPartEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(idPartEntry, 182, 131);

            Label idVehicleLabel = new Label();
            idVehicleLabel.Markup = "<span font='Arial 12' weight='bold'>ID Vehiculo:</span>";
            fixedContainer.Put(idVehicleLabel, 55, 192);

            idVehicleEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(idVehicleEntry, 182, 186);

            Label detailsLabel = new Label();
            detailsLabel.Markup = "<span font='Arial 12' weight='bold'>Detalles:</span>";
            fixedContainer.Put(detailsLabel, 67, 247);

            detailsEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(detailsEntry, 182, 241);

            Label costLabel = new Label();
            costLabel.Markup = "<span font='Arial 12' weight='bold'>Costo:</span>";
            fixedContainer.Put(costLabel, 75, 302);

            costEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(costEntry, 182, 296);

            Button saveButton = new Button("Guardar");
            saveButton.SetSizeRequest(120, 30);
            saveButton.Clicked += OnSaveButtonClicked;
            fixedContainer.Put(saveButton, 140, 351);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 405);

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

        private void OnSaveButtonClicked(object? sender, EventArgs e)
        {
            
        }
    }
}