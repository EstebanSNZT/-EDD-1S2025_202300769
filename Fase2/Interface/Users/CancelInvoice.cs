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
        private Entry costSearchedEntry = new Entry();

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

            if (Child != null && Child.IsRealized)
            {
                Remove(Child);
            }

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

            Label costLabel = new Label();
            costLabel.Markup = "<span font='Arial 12' weight='bold'>Costo:</span>";
            fixedContainer.Put(costLabel, 50, 257);

            costSearchedEntry.SetSizeRequest(140, 35);
            costSearchedEntry.Sensitive = false;
            fixedContainer.Put(costSearchedEntry, 140, 251);

            Button payButton = new Button("Pagar");
            payButton.SetSizeRequest(120, 30);
            payButton.Clicked += OnPayButtonClicked;
            fixedContainer.Put(payButton, 190, 306);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 360);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
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
        }

        private void OnPayButtonClicked(object sender, EventArgs e)
        {
        }

        private void CleanEntrys()
        {
            idEntry.Text = "";
            idSearchedEntry.Text = "";
            orderSearchedEntry.Text = "";
            costSearchedEntry.Text = "";
        }
    }
}