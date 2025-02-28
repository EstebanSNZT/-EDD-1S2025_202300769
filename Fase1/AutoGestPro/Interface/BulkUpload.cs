using Gtk;
using Lists;
using Locals;
using Newtonsoft.Json;

namespace Interface
{
    public class BulkUpload : Window
    {
        private ComboBoxText comboBox = new ComboBoxText();

        public BulkUpload() : base("AutoGest Pro - Carga Masiva")
        {
            InitializeComponents();
        }

        public BulkUpload(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(350, 245); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            if (Child != null)
            {
                Remove(Child);
            }

            Fixed fixedContainer = new Fixed();

            Label bulkUploadLabel = new Label();
            bulkUploadLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Carga Masiva</span>";
            fixedContainer.Put(bulkUploadLabel, 80, 15);

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
            FileChooserDialog fileChooser = new FileChooserDialog(
            "Seleccione un archivo JSON",
            this,
            FileChooserAction.Open,
            "Cancelar", ResponseType.Cancel,
            "Abrir", ResponseType.Accept);

            FileFilter filter = new FileFilter();
            filter.Name = "Archivos JSON";
            filter.AddPattern("*.json");
            fileChooser.AddFilter(filter);

            // Si el usuario selecciona un archivo
            if (fileChooser.Run() == (int)ResponseType.Accept)
            {
                string filePath = fileChooser.Filename;
                LoadJSON(filePath);
            }

            fileChooser.Destroy();
        }

        private void LoadJSON(string filePath)
        {
            string selectedText = comboBox.ActiveText;

            try
            {
                string jsonContent = File.ReadAllText(filePath);

                if (selectedText == "Usuarios")
                {
                    var localUsers = JsonConvert.DeserializeObject<LocalUser[]>(jsonContent);
                    if (localUsers != null)
                    {
                        foreach (var localUser in localUsers)
                        {
                            GlobalLists.linkedList.Insert(localUser.ID, localUser.Nombres, localUser.Apellidos, localUser.Correo, localUser.Contrasenia);
                        }
                        GlobalLists.linkedList.Print();
                    }

                }
                else if (selectedText == "Vehículos")
                {
                    var localVehicles = JsonConvert.DeserializeObject<LocalVehicles[]>(jsonContent);
                    if (localVehicles != null)
                    {
                        foreach (var localVehicle in localVehicles)
                        {
                            if (GlobalLists.linkedList.Contains(localVehicle.ID_Usuario))
                            {
                                GlobalLists.doubleList.Insert(localVehicle.ID, localVehicle.ID_Usuario, localVehicle.Marca, localVehicle.Modelo, localVehicle.Placa);
                            }
                            else
                            {
                                Console.WriteLine($"Vehiculo con ID {localVehicle.ID} no ingresado dado que el usuario con ID {localVehicle.ID_Usuario} no existe.");
                            }
                        }
                        GlobalLists.doubleList.Print();
                    }
                }
                else if (selectedText == "Repuestos")
                {
                    var localParts = JsonConvert.DeserializeObject<LocalSparePart[]>(jsonContent);
                    if (localParts != null)
                    {
                        foreach (var localPart in localParts)
                        {
                            GlobalLists.circularList.Insert(localPart.ID, localPart.Repuesto, localPart.Detalles, localPart.Costo);
                        }
                        GlobalLists.circularList.Print();
                    }
                }
                Menu.ShowDialog(this, MessageType.Info, "Archivo JSON cargado correctamente.");
            }
            catch (Exception ex)
            {
                Menu.ShowDialog(this, MessageType.Error, $"Error al cargar el archivo JSON: {ex.Message}");
            }
        }

        private void OnReturnButtonClicked(object? sender, EventArgs e)
        {
            comboBox.Active = 0;
            GlobalWindows.menu.ShowAll();
            Hide();
        }
    }
}