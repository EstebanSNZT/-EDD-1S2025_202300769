using Gtk;
using Lists;

namespace Interface
{
    public class PartEntry : Window
    {
        private Entry idEntry = new Entry();
        private Entry partEntry = new Entry();
        private Entry detailsEntry = new Entry();
        private Entry costEntry = new Entry();

        public PartEntry() : base("AutoGest Pro - Ingreso de Repuesto")
        {
            InitializeComponents();
        }

        public PartEntry(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(400, 405); //(ancho, alto)
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

            Label partLabel = new Label();
            partLabel.Markup = "<span font='Arial 12' weight='bold'>Repuesto:</span>";
            fixedContainer.Put(partLabel, 51, 137);

            partEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(partEntry, 182, 131);

            Label detailsLabel = new Label();
            detailsLabel.Markup = "<span font='Arial 12' weight='bold'>Detalles:</span>";
            fixedContainer.Put(detailsLabel, 56, 192);

            detailsEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(detailsEntry, 182, 186);

            Label costLabel = new Label();
            costLabel.Markup = "<span font='Arial 12' weight='bold'>Costo:</span>";
            fixedContainer.Put(costLabel, 62, 247);

            costEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(costEntry, 182, 241);

            Button saveButton = new Button("Guardar");
            saveButton.SetSizeRequest(120, 30);
            saveButton.Clicked += OnSaveButtonClicked;
            fixedContainer.Put(saveButton, 140, 295);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 349);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
        }

        private void OnReturnButtonClicked(object? sender, EventArgs e)
        {
            this.Destroy();
            EntryOptions entryOptions = new EntryOptions();
            entryOptions.ShowAll();
        }

        private void OnSaveButtonClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idEntry.Text) || string.IsNullOrEmpty(partEntry.Text) ||
                string.IsNullOrEmpty(detailsEntry.Text) || string.IsNullOrEmpty(costEntry.Text))
            {
                Menu.ShowDialog(this, MessageType.Error, "Por favor, llene todos los campos.");
                return;
            }

            if (!int.TryParse(idEntry.Text, out int id))
            {
                Menu.ShowDialog(this, MessageType.Error, "ID inválido.");
                return;
            }

            if (!double.TryParse(costEntry.Text, out double cost))
            {
                Menu.ShowDialog(this, MessageType.Error, "Costo inválido");
                return;
            }

            GlobalLists.circularList.Insert(id, partEntry.Text, detailsEntry.Text, cost);
            GlobalLists.circularList.Print();

            Menu.ShowDialog(this, MessageType.Info, "Repuesto guardado con éxito.");

            idEntry.Text = "";
            partEntry.Text = "";
            detailsEntry.Text = "";
            costEntry.Text = "";
        }
    }
}