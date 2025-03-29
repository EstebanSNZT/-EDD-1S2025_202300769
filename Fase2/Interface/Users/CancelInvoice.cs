using Gtk;
using Classes;
using Global;

namespace Interface
{
    public class CancelInvoice : Window
    {
        private bool found = false;
        private int idFound = 0;
        private Entry idEntry = new Entry();
        private Entry idSearchedEntry = new Entry();
        private Entry orderSearchedEntry = new Entry();
        private Entry totalSearchedEntry = new Entry();

        public CancelInvoice() : base("AutoGest Pro - Cancelar Factura")
        {
            InitializeComponents();
        }

        public CancelInvoice(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(510, 415); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Cancelar Factura</span>";
            fixedContainer.Put(menuLabel, 140, 15);

            Label idLabel = new Label();
            idLabel.Markup = "<span font='Arial 12' weight='bold'>ID:</span>";
            fixedContainer.Put(idLabel, 60, 79);

            idEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(idEntry, 140, 76);

            Button searchButton = new Button("Buscar");
            searchButton.SetSizeRequest(120, 30);
            searchButton.Clicked += OnSearchButtonClicked;
            fixedContainer.Put(searchButton, 343, 78);

            Label idSearchedLabel = new Label();
            idSearchedLabel.Markup = "<span font='Arial 12' weight='bold'>ID:</span>";
            fixedContainer.Put(idSearchedLabel, 60, 147);

            idSearchedEntry.SetSizeRequest(140, 35);
            idSearchedEntry.Sensitive = false;
            fixedContainer.Put(idSearchedEntry, 140, 141);

            Label orderLabel = new Label();
            orderLabel.Markup = "<span font='Arial 12' weight='bold'>Orden:</span>";
            fixedContainer.Put(orderLabel, 50, 202);

            orderSearchedEntry.SetSizeRequest(140, 35);
            orderSearchedEntry.Sensitive = false;
            fixedContainer.Put(orderSearchedEntry, 140, 196);

            Label totalLabel = new Label();
            totalLabel.Markup = "<span font='Arial 12' weight='bold'>Total:</span>";
            fixedContainer.Put(totalLabel, 50, 257);

            totalSearchedEntry.SetSizeRequest(140, 35);
            totalSearchedEntry.Sensitive = false;
            fixedContainer.Put(totalSearchedEntry, 140, 251);

            Button payButton = new Button("Pagar");
            payButton.SetSizeRequest(120, 30);
            payButton.Clicked += OnPayButtonClicked;
            fixedContainer.Put(payButton, 190, 306);

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
            found = false;
            CleanEntrys();
            GlobalWindows.userMenu.ShowAll();
            Hide();
        }

        private void OnSearchButtonClicked(object sender, EventArgs e)
        {
            int userId = LoginControl.LoggedUserId;

            if (string.IsNullOrEmpty(idEntry.Text))
            {
                Login.ShowDialog(this, MessageType.Error, "Debe ingresar un ID de factura.");
                return;
            }

            if (!int.TryParse(idEntry.Text, out idFound))
            {
                Login.ShowDialog(this, MessageType.Error, "ID inválido.");
                return;
            }

            if (GlobalStructures.InvoicesTree.IsEmpty())
            {
                Login.ShowDialog(this, MessageType.Warning, "El árbol de facturas se encuentra actualmente vacío. Ingrese facturas antes de continuar.");
                return;
            }

            Invoice invoice = GlobalStructures.InvoicesTree.Get(idFound);

            if (invoice == null)
            {
                Login.ShowDialog(this, MessageType.Warning, "No se encontró ninguna factura con ese ID en sus facturas por pagar.");
                return;
            }

            Service service = GlobalStructures.ServicesTree.Get(invoice.ServiceId);

            Vehicle vehicle = GlobalStructures.VehiclesList.Get(service.VehicleId);

            if(vehicle == null || vehicle.UserId != userId)
            {
                Login.ShowDialog(this, MessageType.Warning, "No tiene permitido pagar esa factura.");
                return;
            }         

            Login.ShowDialog(this, MessageType.Info, "Factura encontrada.");
            found = true;
            idSearchedEntry.Text = "";
            orderSearchedEntry.Text = "";
            totalSearchedEntry.Text = "";
            idSearchedEntry.Text = invoice.Id.ToString();
            orderSearchedEntry.Text = invoice.ServiceId.ToString();
            totalSearchedEntry.Text = invoice.Total.ToString();
        }

        private void OnPayButtonClicked(object sender, EventArgs e)
        {
            if (!found)
            {
                Login.ShowDialog(this, MessageType.Error, "Debe buscar una factura primero.");
                return;
            }

            GlobalStructures.InvoicesTree.Delete(idFound);
            Login.ShowDialog(this, MessageType.Info, "Factura pagada correctamente.");
            CleanEntrys();
        }

        private void CleanEntrys()
        {
            idEntry.Text = "";
            idSearchedEntry.Text = "";
            orderSearchedEntry.Text = "";
            totalSearchedEntry.Text = "";
        }
    }
}