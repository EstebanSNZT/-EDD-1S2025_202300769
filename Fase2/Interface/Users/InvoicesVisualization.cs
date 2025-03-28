using Gtk;
using Global;

namespace Interface
{
    public class InvoicesVisualization : Window
    {
        private TreeView treeView = new TreeView();

        public InvoicesVisualization() : base("AutoGest Pro - Visualización de Facturas")
        {
            InitializeComponents();
        }

        public InvoicesVisualization(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(440, 416); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            if (Child != null)
            {
                Remove(Child);
            }

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Visualización de Facturas</span>";
            fixedContainer.Put(menuLabel, 50, 15);

            ScrolledWindow scrolledWindow = new ScrolledWindow();
            ScrolledWindow scrollWindow = new ScrolledWindow();
            scrollWindow.SetSizeRequest(360, 285);
            scrollWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            fixedContainer.Put(scrollWindow, 40, 50);

            treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
            treeView.AppendColumn("Orden", new CellRendererText(), "text", 1);
            treeView.AppendColumn("Total", new CellRendererText(), "text", 3);
            scrollWindow.Add(treeView);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 360);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
        }

        private void OnReturnButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.userMenu.ShowAll();
            Hide();
        }
    }
}