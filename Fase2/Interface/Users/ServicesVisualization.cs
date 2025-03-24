using Gtk;
using Global;

namespace Interface
{
    public class ServicesVisualization : Window
    {
        private ComboBoxText comboBox = new ComboBoxText();
        private TreeView treeView = new TreeView();

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
            SetSizeRequest(680, 496); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            if (Child != null)
            {
                Remove(Child);
            }

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Visualización de Servicios</span>";
            fixedContainer.Put(menuLabel, 124, 15);

            comboBox.AppendText("Pre-Orden");
            comboBox.AppendText("In-Orden");
            comboBox.AppendText("Post-Orden");
            comboBox.Active = 0;
            comboBox.SetSizeRequest(280, 35);
            comboBox.Changed += OnComboBoxChanged;
            fixedContainer.Put(comboBox, 200, 70);

            ScrolledWindow scrolledWindow = new ScrolledWindow();
            ScrolledWindow scrollWindow = new ScrolledWindow();
            scrollWindow.SetSizeRequest(600, 285);
            scrollWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            fixedContainer.Put(scrollWindow, 40, 130);

            treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
            treeView.AppendColumn("Repuesto", new CellRendererText(), "text", 1);
            treeView.AppendColumn("Detalles", new CellRendererText(), "text", 2);
            treeView.AppendColumn("Costo", new CellRendererText(), "text", 3);
            scrollWindow.Add(treeView);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 440);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
        }

        private void OnReturnButtonClicked(object sender, EventArgs e)
        {
            comboBox.Active = 0;
            GlobalWindows.adminMenu.ShowAll();
            Hide();
        }

        private void OnComboBoxChanged(object sender, EventArgs e)
        {
            switch (comboBox.Active)
            {
                case 0:
                    AdjustOrder(GlobalStructures.SparePartsTree.PreOrder());
                    break;
                case 1:
                    AdjustOrder(GlobalStructures.SparePartsTree.InOrder());
                    break;
                case 2:
                    AdjustOrder(GlobalStructures.SparePartsTree.PostOrder());
                    break;
            }
        }

        public void AdjustOrder(ListStore order)
        {
            treeView.Model = order;
        }
    }
}