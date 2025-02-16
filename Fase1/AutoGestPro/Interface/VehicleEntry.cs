using Gtk;

namespace Interface
{
    public class VehicleEntry : Window
    {
        private Entry idEntry = new Entry();
        private Entry idUserEntry = new Entry();
        private Entry brandEntry = new Entry();
        private Entry modelEntry = new Entry();
        private Entry plateEntry = new Entry();

        public VehicleEntry() : base("AutoGest Pro - Ingreso de Vehiculo")
        {
            SetSizeRequest(400, 461); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Ingreso de Vehiculo</span>";
            fixedContainer.Put(menuLabel, 63, 15);

            Label idLabel = new Label();
            idLabel.Markup = "<span font='Arial 12' weight='bold'>ID:</span>";
            fixedContainer.Put(idLabel, 83, 82);

            idEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(idEntry, 182, 76);

            Label idUserLabel = new Label();
            idUserLabel.Markup = "<span font='Arial 12' weight='bold'>ID Usuario:</span>";
            fixedContainer.Put(idUserLabel, 51, 137);

            idUserEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(idUserEntry, 182, 131);

            Label brandLabel = new Label();
            brandLabel.Markup = "<span font='Arial 12' weight='bold'>Marca:</span>";
            fixedContainer.Put(brandLabel, 68, 192);

            brandEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(brandEntry, 182, 186);

            Label modelLabel = new Label();
            modelLabel.Markup = "<span font='Arial 12' weight='bold'>Modelo:</span>";
            fixedContainer.Put(modelLabel, 63, 247);
            
            modelEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(modelEntry, 182, 241);

            Label plateLabel = new Label();
            plateLabel.Markup = "<span font='Arial 12' weight='bold'>Placa:</span>";
            fixedContainer.Put(plateLabel, 70, 302);
            
            plateEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(plateEntry, 182, 296);

            Button saveButton = new Button("Guardar");
            saveButton.SetSizeRequest(120, 30);
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
            EntryOptions entryOptions = new EntryOptions();
            entryOptions.ShowAll();
        }
    }
}