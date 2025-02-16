using Gtk;

namespace Interface
{
    public class BulkUpload : Window
    {
        public BulkUpload() : base("AutoGest Pro - Carga Masiva")
        {
            SetSizeRequest(350, 245); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label bulkUploadLabel = new Label();
            bulkUploadLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Carga Masiva</span>";
            fixedContainer.Put(bulkUploadLabel, 80, 15);

            ComboBoxText comboBox = new ComboBoxText();
            comboBox.AppendText("Usuarios");
            comboBox.AppendText("Vehículos");
            comboBox.AppendText("Repuestos");
            comboBox.Active = 0;
            comboBox.SetSizeRequest(280, 35);
            fixedContainer.Put(comboBox, 35, 75);

            Button uploadButton = new Button("Cargar");
            uploadButton.SetSizeRequest(280, 35);
            uploadButton.Clicked += OnUploadButtonClicked;
            fixedContainer.Put(uploadButton, 35, 130);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 190);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
        }

        private void OnUploadButtonClicked(object? sender, EventArgs e)
        {
            {
                FileChooserDialog fileChooser = new FileChooserDialog(
                    "Seleccionar Archivo",
                    this,
                    FileChooserAction.Open,
                    "Cancelar", ResponseType.Cancel,
                    "Abrir", ResponseType.Accept
                );

                FileFilter filter = new FileFilter();
                filter.AddPattern("*.json");
                filter.Name = "Archivos JSON";
                fileChooser.AddFilter(filter);

                if (fileChooser.Run() == (int)ResponseType.Accept)
                {
                    string filePath = fileChooser.Filename;
                    Console.WriteLine("Archivo seleccionado: " + filePath);
                }

                fileChooser.Destroy();
            }
        }

        private void OnReturnButtonClicked(object? sender, EventArgs e)
        {
            this.Destroy();
            Menu menu = new Menu();
            menu.ShowAll();
        }
    }
}