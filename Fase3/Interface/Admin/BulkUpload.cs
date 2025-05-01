using Gtk;
using Global;
using Classes;
using Newtonsoft.Json;

namespace Interface
{
    public class BulkUpload : Window
    {
        private ComboBoxText comboBox = new ComboBoxText();
        private static readonly string[] PaymentMethods = ["Efectivo", "Tarjeta de Débito", "Tarjeta de Crédito", "Transferencia"];
        private static readonly Random Random = new Random();

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

            Fixed fixedContainer = new Fixed();

            Label bulkUploadLabel = new Label();
            bulkUploadLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Carga Masiva</span>";
            fixedContainer.Put(bulkUploadLabel, 80, 15);

            comboBox.AppendText("Usuarios");
            comboBox.AppendText("Vehículos");
            comboBox.AppendText("Repuestos");
            comboBox.AppendText("Servicios");
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

            DeleteEvent += (o, args) =>
            {
                GlobalWindows.DestroyAll();
                Application.Quit();
            };
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
                            if (GlobalStructures.UsersBlockchain.Contains(localUser.ID))
                            {
                                Console.WriteLine($"El usuario con ID {localUser.ID} no cargado dado que un usuario con ese ID ya existe.");
                                continue;
                            }

                            if (GlobalStructures.UsersBlockchain.ContainsByEmail(localUser.Correo))
                            {
                                Console.WriteLine($"El usuario con correo {localUser.Correo} no cargado dado que un usuario con ese correo ya existe.");
                                continue;
                            }

                            User newUser = new User(localUser.ID, localUser.Nombres, localUser.Apellidos, localUser.Correo, localUser.Edad, localUser.Contrasenia);
                            newUser.HashPassword();
                            GlobalStructures.UsersBlockchain.AddBlock(newUser);
                        }
                        GlobalStructures.UsersBlockchain.ViewAllBlocks();
                    }

                }
                else if (selectedText == "Vehículos")
                {
                    var localVehicles = JsonConvert.DeserializeObject<LocalVehicles[]>(jsonContent);
                    if (localVehicles != null)
                    {
                        foreach (var localVehicle in localVehicles)
                        {
                            if (GlobalStructures.VehiclesList.Contains(localVehicle.ID))
                            {
                                Console.WriteLine($"El vehiculo con ID {localVehicle.ID} no cargado dado que un vehículo con ese ID ya existe.");
                            }
                            else
                            {
                                if (GlobalStructures.UsersBlockchain.Contains(localVehicle.ID_Usuario))
                                {
                                    Vehicle newVehicle = new Vehicle(
                                        localVehicle.ID,
                                        localVehicle.ID_Usuario,
                                        localVehicle.Marca,
                                        localVehicle.Modelo,
                                        localVehicle.Placa
                                    );
                                    GlobalStructures.VehiclesList.Add(newVehicle);
                                }
                                else
                                {
                                    Console.WriteLine($"Vehiculo con ID {localVehicle.ID} no cargado dado que el usuario con ID {localVehicle.ID_Usuario} no existe.");
                                    continue;
                                }

                            }
                        }
                    }
                }
                else if (selectedText == "Repuestos")
                {
                    var localSpareParts = JsonConvert.DeserializeObject<LocalSpareParts[]>(jsonContent);
                    if (localSpareParts != null)
                    {
                        foreach (var localSparePart in localSpareParts)
                        {
                            if (GlobalStructures.SparePartsTree.Contains(localSparePart.ID))
                            {
                                Console.WriteLine($"El repuesto con ID {localSparePart.ID} no cargado dado que un repuesto con ese ID ya existe.");
                            }
                            else
                            {
                                SparePart newSparePart = new SparePart(
                                    localSparePart.ID,
                                    localSparePart.Repuesto,
                                    localSparePart.Detalles,
                                    localSparePart.Costo
                                );
                                GlobalStructures.SparePartsTree.Add(newSparePart);
                            }
                        }

                    }
                }
                else if (selectedText == "Servicios")
                {
                    var localServices = JsonConvert.DeserializeObject<LocalServices[]>(jsonContent);
                    if (localServices != null)
                    {
                        foreach (var localService in localServices)
                        {
                            if (!GlobalStructures.VehiclesList.Contains(localService.Id_Vehiculo))
                            {
                                Console.WriteLine($"El servicio con ID {localService.Id} no cargado dado que el vehiculo con ID {localService.Id_Vehiculo} no existe.");
                                continue;
                            }

                            SparePart sparePart = GlobalStructures.SparePartsTree.Get(localService.Id_Repuesto);

                            if (sparePart == null)
                            {
                                Console.WriteLine($"El servicio con ID {localService.Id} no cargado dado que el repuesto con ID {localService.Id_Repuesto} no existe.");
                                continue;
                            }

                            if (GlobalStructures.ServicesTree.Contains(localService.Id))
                            {
                                Console.WriteLine($"El servicio con ID {localService.Id} no cargado dado que un servicio con ese ID ya existe.");
                                continue;
                            }

                            Service newService = new Service(
                                localService.Id, localService.Id_Repuesto,
                                localService.Id_Vehiculo,
                                localService.Detalles,
                                localService.Costo
                            );
                            GlobalStructures.ServicesTree.Add(newService);

                            GlobalStructures.Graph.AddEdge($"V{newService.VehicleId}", $"R{newService.SparePartId}");
                            double total = newService.Cost + sparePart.Cost;
                            string paymentMethod = GetRandomPaymentMethod();

                            Invoice newInvoice = new Invoice(
                                GlobalStructures.invoiceId,
                                newService.Id,
                                total,
                                paymentMethod
                            );
                            GlobalStructures.InvoicesTree.Add(newInvoice);
                            GlobalStructures.invoiceId++;
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

        public static string GetRandomPaymentMethod()
        {
            int index = Random.Next(PaymentMethods.Length);
            return PaymentMethods[index];
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

    public class LocalServices
    {
        public int Id { get; set; }
        public int Id_Repuesto { get; set; }

        public int Id_Vehiculo { get; set; }
        public string Detalles { get; set; }
        public double Costo { get; set; }
    }
}