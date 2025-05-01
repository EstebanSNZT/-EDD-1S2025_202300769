using Gtk;
using Global;

namespace Interface
{
    public class InvoicesVisualization : Window
    {
        private TreeView treeView = new TreeView();
        private ListStore userInvoices = new ListStore(typeof(int), typeof(int), typeof(string), typeof(string), typeof(string));

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
            SetSizeRequest(540, 416); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Visualización de Facturas</span>";
            fixedContainer.Put(menuLabel, 80, 15);

            ScrolledWindow scrollWindow = new ScrolledWindow();
            scrollWindow.SetSizeRequest(460, 265);
            scrollWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            fixedContainer.Put(scrollWindow, 40, 70);

            treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
            treeView.AppendColumn("Orden", new CellRendererText(), "text", 1);
            treeView.AppendColumn("Total", new CellRendererText(), "text", 2);
            treeView.AppendColumn("Método de pago", new CellRendererText(), "text", 3);
            treeView.AppendColumn("Fecha", new CellRendererText(), "text", 4);
            scrollWindow.Add(treeView);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 360);

            Add(fixedContainer);

            DeleteEvent += (o, args) =>
            {
                GlobalWindows.DestroyAll();
                Application.Quit();
            };
        }

        private void OnReturnButtonClicked(object sender, EventArgs e)
        {
            GlobalWindows.userMenu.ShowAll();
            Hide();
        }

        public void UpdateData(int userId)
        {
            userInvoices = GlobalStructures.InvoicesTree.GetUserInvoices(userId);
            treeView.Model = userInvoices;
        }
    }
}