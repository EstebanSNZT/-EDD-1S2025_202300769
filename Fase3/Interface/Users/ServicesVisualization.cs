using Gtk;
using Global;

namespace Interface
{
    public class ServicesVisualization : Window
    {
        private ComboBoxText comboBox = new ComboBoxText();
        private TreeView treeView = new TreeView();
        private ListStore preOrder;
        private ListStore inOrder;
        private ListStore postOrder;

        public ServicesVisualization() : base("AutoGest Pro - Visualización de Servicios")
        {
            InitializeComponents();
        }

        public ServicesVisualization(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(600, 496); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Visualización de Servicios</span>";
            fixedContainer.Put(menuLabel, 131, 15);

            comboBox.AppendText("Pre-Orden");
            comboBox.AppendText("In-Orden");
            comboBox.AppendText("Post-Orden");
            comboBox.Active = 0;
            comboBox.SetSizeRequest(280, 35);
            comboBox.Changed += OnComboBoxChanged;
            fixedContainer.Put(comboBox, 160, 70);

            ScrolledWindow scrollWindow = new ScrolledWindow();
            scrollWindow.SetSizeRequest(520, 285);
            scrollWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            fixedContainer.Put(scrollWindow, 40, 130);

            treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
            treeView.AppendColumn("ID Repuesto", new CellRendererText(), "text", 1);
            treeView.AppendColumn("ID Vehículo", new CellRendererText(), "text", 2);
            treeView.AppendColumn("Detalles", new CellRendererText(), "text", 3);
            treeView.AppendColumn("Costo", new CellRendererText(), "text", 4);
            scrollWindow.Add(treeView);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 440);

            Add(fixedContainer);

            DeleteEvent += (o, args) =>
            {
                GlobalWindows.DestroyAll();
                Application.Quit();
            };
        }

        private void OnReturnButtonClicked(object sender, EventArgs e)
        {
            comboBox.Active = 0;
            GlobalWindows.userMenu.ShowAll();
            Hide();
        }

        private void OnComboBoxChanged(object sender, EventArgs e)
        {
            AdjustTraversal(comboBox.Active);
        }

        public void AdjustTraversal(int traversalNumber)
        {
            switch (traversalNumber)
            {
                case 0:
                    treeView.Model = preOrder;
                    break;
                case 1:
                    treeView.Model = inOrder;
                    break;
                case 2:
                    treeView.Model = postOrder;
                    break;
            }
        }

        public void UpdateData(int userId)
        {
            preOrder = GlobalStructures.ServicesTree.PreOrder(userId);
            inOrder = GlobalStructures.ServicesTree.InOrder(userId);
            postOrder = GlobalStructures.ServicesTree.PostOrder(userId);
        }
    }
}