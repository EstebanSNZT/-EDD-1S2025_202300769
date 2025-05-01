using Gtk;
using Global;
using Utilities;
using Structures;

namespace Interface
{
    public class AdminMenu : Window
    {
        public AdminMenu() : base("AutoGest Pro - Admin")
        {
            InitializeComponents();
        }

        public AdminMenu(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(350, 675); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Admin</span>";
            fixedContainer.Put(menuLabel, 130, 15);

            Button bulkUploadButton = new Button("Cargas Masivas");
            bulkUploadButton.SetSizeRequest(280, 35);
            bulkUploadButton.Clicked += OnBulkUploadButtonClicked;
            fixedContainer.Put(bulkUploadButton, 35, 65);

            Button insertUsersButton = new Button("Inserción de Usuarios");
            insertUsersButton.SetSizeRequest(280, 35);
            insertUsersButton.Clicked += OnInsertUsersButtonClicked;
            fixedContainer.Put(insertUsersButton, 35, 120);

            Button usersVisualizationButton = new Button("Visualización de Usuarios");
            usersVisualizationButton.SetSizeRequest(280, 35);
            usersVisualizationButton.Clicked += OnUsersVisualizationButtonClicked;
            fixedContainer.Put(usersVisualizationButton, 35, 175);

            Button sparePartsUpdateButton = new Button("Actualización de Repuestos");
            sparePartsUpdateButton.SetSizeRequest(280, 35);
            sparePartsUpdateButton.Clicked += OnSparePartsUpdateButtonClicked;
            fixedContainer.Put(sparePartsUpdateButton, 35, 230);

            Button sparePartsVisualizationButton = new Button("Visualización de Repuestos");
            sparePartsVisualizationButton.SetSizeRequest(280, 35);
            sparePartsVisualizationButton.Clicked += OnSparePartsVisualizationButtonClicked;
            fixedContainer.Put(sparePartsVisualizationButton, 35, 285);

            Button generateServicesButton = new Button("Generar Servicios");
            generateServicesButton.SetSizeRequest(280, 35);
            generateServicesButton.Clicked += OnGenerateServicesButtonClicked;
            fixedContainer.Put(generateServicesButton, 35, 340);

            Button loginControlButton = new Button("Control de Logueo");
            loginControlButton.SetSizeRequest(280, 35);
            loginControlButton.Clicked += OnLoginControlButtonClicked;
            fixedContainer.Put(loginControlButton, 35, 395);

            Button generateReportsButton = new Button("Generar Reportes");
            generateReportsButton.SetSizeRequest(280, 35);
            generateReportsButton.Clicked += OnGenerateReportsButtonButtonClicked;
            fixedContainer.Put(generateReportsButton, 35, 450);

            Button generateBackup = new Button("Generar Backup");
            generateBackup.SetSizeRequest(280, 35);
            generateBackup.Clicked += OnGenerateBackupButtonClicked;
            fixedContainer.Put(generateBackup, 35, 505);

            Button loadBackup = new Button("Cargar Backup");
            loadBackup.SetSizeRequest(280, 35);
            loadBackup.Clicked += OnLoadBackupButtonClicked;
            fixedContainer.Put(loadBackup, 35, 560);

            Button logoutButton = new Button("Cerrar Sesión");
            logoutButton.SetSizeRequest(160, 30);
            logoutButton.Clicked += OnLogoutButtonClicked;
            fixedContainer.Put(logoutButton, 95, 620);

            Add(fixedContainer);

            DeleteEvent += (o, args) =>
            {
                GlobalWindows.DestroyAll();
                Application.Quit();
            };
        }

        private void OnBulkUploadButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.bulkUpload.ShowAll();
            Hide();
        }

        private void OnInsertUsersButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.usersInsertion.ShowAll();
            Hide();
        }

        private void OnUsersVisualizationButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.usersVisualization.ShowAll();
            Hide();
        }

        private void OnSparePartsUpdateButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.updatedSparePart.ShowAll();
            Hide();
        }

        private void OnSparePartsVisualizationButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.sparePartsVisualization.UpdateData();
            GlobalWindows.sparePartsVisualization.AdjustTraversal(0);
            GlobalWindows.sparePartsVisualization.ShowAll();
            Hide();
        }

        private void OnGenerateServicesButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.generateService.ShowAll();
            Hide();
        }

        private void OnLoginControlButtonClicked(object sender, EventArgs e)
        {
            string loginJson = LoginControl.GenerateJson();
            Utility.GenerateJsonFile("ControlLogueo", loginJson, "Reportes");
            Login.ShowDialog(this, MessageType.Info, "El archivo ControlLogueo.json ha sido creado con exito en la carpeta Reportes.");
        }

        private void OnGenerateReportsButtonButtonClicked(object sender, EventArgs e)
        {
            if (!GlobalStructures.UsersBlockchain.IsEmpty())
            {
                string blockchainDot = GlobalStructures.UsersBlockchain.GenerateDot();
                Utility.GenerateDotFile("Usuarios", blockchainDot);
                Utility.ConvertDotToImage("Usuarios.dot");
            }

            if (!GlobalStructures.VehiclesList.IsEmpty())
            {
                string doublyLinkedListDot = GlobalStructures.VehiclesList.GenerateDot();
                Utility.GenerateDotFile("Vehículos", doublyLinkedListDot);
                Utility.ConvertDotToImage("Vehículos.dot");
            }

            if (!GlobalStructures.SparePartsTree.IsEmpty())
            {
                string avlTreeDot = GlobalStructures.SparePartsTree.GenerateDot();
                Utility.GenerateDotFile("Repuestos", avlTreeDot);
                Utility.ConvertDotToImage("Repuestos.dot");
            }

            if (!GlobalStructures.ServicesTree.IsEmpty())
            {
                string binaryTreeDot = GlobalStructures.ServicesTree.GenerateDot();
                Utility.GenerateDotFile("Servicios", binaryTreeDot);
                Utility.ConvertDotToImage("Servicios.dot");
            }

            if (!GlobalStructures.Graph.IsEmpty())
            {
                string graphDot = GlobalStructures.Graph.GenerateDot();
                Utility.GenerateDotFile("Grafo", graphDot);
                Utility.ConvertDotToImage("Grafo.dot");
            }

            if (!GlobalStructures.InvoicesTree.IsEmpty())
            {
                string bTreeDot = GlobalStructures.InvoicesTree.GenerateDot();
                Utility.GenerateDotFile("Facturas", bTreeDot);
                Utility.ConvertDotToImage("Facturas.dot");
            }

            Login.ShowDialog(this, MessageType.Info, "Los reportes han sido generados con éxito en la carpeta Reportes.");
        }

        private void OnGenerateBackupButtonClicked(object sender, EventArgs e)
        {
            HuffmanCompressor huffmanCompressor = new HuffmanCompressor();

            if (!GlobalStructures.UsersBlockchain.IsEmpty())
            {
                string blockchainJson = GlobalStructures.UsersBlockchain.GenerateJson();
                Utility.GenerateJsonFile("Blocks", blockchainJson, "Backup");
            }

            if (!GlobalStructures.VehiclesList.IsEmpty())
            {
                string vehicleText = GlobalStructures.VehiclesList.PlainText();
                huffmanCompressor.Compress(vehicleText, "Vehicles");
            }

            if (!GlobalStructures.SparePartsTree.IsEmpty())
            {
                string sparePartsText = GlobalStructures.SparePartsTree.PlainText();
                huffmanCompressor.Compress(sparePartsText, "SpareParts");
            }

            Login.ShowDialog(this, MessageType.Info, "El backup ha sido creado con éxito en la carpeta Backup.");
        }

        private void OnLoadBackupButtonClicked(object sender, EventArgs e)
        {
            string BlocksJson = Utility.LoadJsonFile("Blocks", "Backup");

            if (!string.IsNullOrEmpty(BlocksJson))
            {
                Blockchain blockchain = new Blockchain();
                blockchain.LoadJson(BlocksJson);
                if (blockchain.AnalyzeBlockchain())
                {
                    GlobalStructures.UsersBlockchain = blockchain;
                    Console.WriteLine("Blockchain cargado desde el backup.\n");
                }
                else
                {
                    Console.WriteLine("El backup del blockchain es inválido.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Error al cargar el backup del blockchain.");
                Console.WriteLine("Asegúrese de que Blocks.json exista.");
            }

            HuffmanCompressor huffmanCompressor = new HuffmanCompressor();
            string vehiclesText = huffmanCompressor.Decompress("Vehicles");

            if (!string.IsNullOrEmpty(vehiclesText))
            {
                GlobalStructures.VehiclesList.LoadPlainText(vehiclesText);
                Console.WriteLine("Vehículos cargados desde el backup.\n");
            }
            else
            {
                Console.WriteLine("Error al cargar el backup de vehículos.");
                Console.WriteLine("Asegúrese de que Vehicles.edd y VehiclesHuffmanTree.json existan.");
            }

            string sparePartsText = huffmanCompressor.Decompress("SpareParts");

            if (!string.IsNullOrEmpty(sparePartsText))
            {
                GlobalStructures.SparePartsTree.LoadPlainText(sparePartsText);
                Console.WriteLine("Repuestos cargados desde el backup.\n");
            }
            else
            {
                Console.WriteLine("Error al cargar el backup de repuestos.");
                Console.WriteLine("Asegúrese de que SpareParts.edd y SparePartsHuffmanTree.json existan.");
            }

            Login.ShowDialog(this, MessageType.Info, "El backup ha sido cargado con éxito desde la carpeta Backup.");
        }

        private void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            LoginControl.GenerateLogoutTime();
            LoginControl.AddLogin();
            GlobalWindows.login.ShowAll();
            Hide();
        }
    }
}