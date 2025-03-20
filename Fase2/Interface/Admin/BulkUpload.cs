using Gtk;
using Global;
using Classes;
using Newtonsoft.Json;
using Utilities;

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

        private void OnUploadButtonClicked(object sender, EventArgs e)
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
                            User newUser = new User(localUser.ID, localUser.Nombres, localUser.Apellidos, localUser.Correo, localUser.Edad, localUser.Contrasenia);
                            GlobalDataStructures.UsersList.Insert(newUser);
                        }
                        GlobalDataStructures.UsersList.Print();
                    }

                }
                else if (selectedText == "Vehículos")
                {
                    var localVehicles = JsonConvert.DeserializeObject<LocalVehicles[]>(jsonContent);
                    if (localVehicles != null)
                    {
                        foreach (var localVehicle in localVehicles)
                        {
                            if (GlobalDataStructures.UsersList.Contains(localVehicle.ID_Usuario))
                            {
                                Vehicle newVehicle = new Vehicle(localVehicle.ID, localVehicle.ID_Usuario, localVehicle.Marca, localVehicle.Modelo, localVehicle.Placa);
                                GlobalDataStructures.VehiclesList.Insert(newVehicle);
                            }
                            else
                            {
                                Console.WriteLine($"Vehiculo con ID {localVehicle.ID} no ingresado dado que el usuario con ID {localVehicle.ID_Usuario} no existe.");
                            }
                        }
                        GlobalDataStructures.VehiclesList.Print();
                    }
                }
                else if (selectedText == "Repuestos")
                {
                    var localSpareParts = JsonConvert.DeserializeObject<LocalSpareParts[]>(jsonContent);
                    if (localSpareParts != null)
                    {
                        foreach (var localSparePart in localSpareParts)
                        {
                            SparePart newSparePart = new SparePart(localSparePart.ID, localSparePart.Repuesto, localSparePart.Detalles, localSparePart.Costo);
                            GlobalDataStructures.sparePartsTree.Insert(newSparePart);
                        }

                    }
                }
                Login.ShowDialog(this, MessageType.Info, "Archivo JSON cargado correctamente.");
            }
            catch (Exception ex)
            {
                Login.ShowDialog(this, MessageType.Error, $"Error al cargar el archivo JSON: {ex.Message}");
            }
        }

        private void OnReturnButtonClicked(object sender, EventArgs e)
        {
            comboBox.Active = 0;
            GlobalWindows.adminMenu.ShowAll();
            Hide();
        }
    }

    public class LocalUser
    {
        public int ID { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public int Edad { get; set; }
        public string Contrasenia { get; set; }
    }

    public class LocalVehicles
    {
        public int ID { get; set; }
        public int ID_Usuario { get; set; }
        public string Marca { get; set; }
        public int Modelo { get; set; }
        public string Placa { get; set; }
    }

    public class LocalSpareParts
    {
        public int ID { get; set; }
        public string Repuesto { get; set; }
        public string Detalles { get; set; }
        public double Costo { get; set; }
    }
}