using Gtk;
using Lists;
using System.Runtime.InteropServices;

namespace Interface
{
    public unsafe class CancelInvoice : Window
    {
        private Entry idEntry = new Entry();
        private Entry orderIdEntry = new Entry();
        private Entry totalCostEntry = new Entry();

        public CancelInvoice() : base("AutoGest Pro - Cancelar factura")
        {
            InitializeComponents();
        }

        public CancelInvoice(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(400, 297); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            if (Child != null)
            {
                Remove(Child);
            }

            StackNode* invoice = GlobalLists.stack.Pop();

            Fixed fixedContainer = new Fixed();

            Label cancelInvoiceLabel = new Label();
            cancelInvoiceLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Factura Cancela</span>";
            fixedContainer.Put(cancelInvoiceLabel, 85, 15);

            Label idLabel = new Label();
            idLabel.Markup = "<span font='Arial 12' weight='bold'>ID:</span>";
            fixedContainer.Put(idLabel, 90, 82);

            idEntry.SetSizeRequest(140, 35);
            idEntry.Sensitive = false;
            idEntry.Text = invoice->Id.ToString();
            fixedContainer.Put(idEntry, 182, 76);

            Label orderIdLabel = new Label();
            orderIdLabel.Markup = "<span font='Arial 12' weight='bold'>ID Orden:</span>";
            fixedContainer.Put(orderIdLabel, 51, 137);

            orderIdEntry.SetSizeRequest(140, 35);
            orderIdEntry.Sensitive = false;
            orderIdEntry.Text = invoice->OrderId.ToString();
            fixedContainer.Put(orderIdEntry, 182, 131);

            Label totalCostLabel = new Label();
            totalCostLabel.Markup = "<span font='Arial 12' weight='bold'>Total:</span>";
            fixedContainer.Put(totalCostLabel, 55, 192);

            totalCostEntry.SetSizeRequest(140, 35);
            totalCostEntry.Sensitive = false;
            totalCostEntry.Text = invoice->TotalCost.ToString();
            fixedContainer.Put(totalCostEntry, 182, 186);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 241);

            Label coordsLabel = new Label("Coordenadas: X = 0, Y = 0");
            fixedContainer.Put(coordsLabel, 0, 0);

            MotionNotifyEvent += (o, args) =>
            {
                int x = (int)args.Event.X;
                int y = (int)args.Event.Y;
                coordsLabel.Text = $"Coordenadas: X = {x}, Y = {y}";
            };

            Add(fixedContainer);
            NativeMemory.Free(invoice);

            DeleteEvent += (o, args) => Application.Quit();
        }

        private void OnReturnButtonClicked(object? sender, EventArgs e)
        {
            if(!GlobalLists.stack.IsEmpty())
            {
                Menu.ShowDialog(this, MessageType.Info, "Tienes facturas por cancelar.");
            }
            Menu menu = new Menu();
            menu.ShowAll();
            this.Destroy();
        }
    }
}