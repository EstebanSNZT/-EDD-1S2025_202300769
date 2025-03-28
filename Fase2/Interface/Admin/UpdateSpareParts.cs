using Gtk;
using Classes;
using Global;

namespace Interface
{
    public class UpdatedSpareParts : Window
    {
        private bool found = false;
        private int idFound = 0;
        private Entry idEntry = new Entry();
        private Entry currentIdEntry = new Entry();
        private Entry newSpareEntry = new Entry();
        private Entry newDetailsEntry = new Entry();
        private Entry newCostEntry = new Entry();
        private Entry currentSpareEntry = new Entry();
        private Entry currentDetailsEntry = new Entry();
        private Entry currentCostEntry = new Entry();

        public UpdatedSpareParts() : base("AutoGest Pro - Actualización de Repuestos")
        {
            InitializeComponents();
        }

        public UpdatedSpareParts(IntPtr raw) : base(raw)
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            SetSizeRequest(598, 496); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            if (Child != null && Child.IsRealized)
            {
                Remove(Child);
            }

            Fixed fixedContainer = new Fixed();

            Label menuLabel = new Label();
            menuLabel.Markup = "<span font='Arial 22' weight='bold' foreground='blue'>Actualización de Repuestos</span>";
            fixedContainer.Put(menuLabel, 145, 15);

            Label idLabel = new Label();
            idLabel.Markup = "<span font='Arial 12' weight='bold'>ID:</span>";
            fixedContainer.Put(idLabel, 82, 79);

            idEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(idEntry, 170, 76);

            Button searchButton = new Button("Buscar");
            searchButton.SetSizeRequest(120, 30);
            searchButton.Clicked += OnSearchButtonClicked;
            fixedContainer.Put(searchButton, 403, 78);

            Label currentDataLabel = new Label();
            currentDataLabel.Markup = "<span font='Arial 12' weight='bold'>Datos Actuales</span>";
            fixedContainer.Put(currentDataLabel, 192, 129);

            Label newDataLabel = new Label();
            newDataLabel.Markup = "<span font='Arial 12' weight='bold'>Nuevos Datos</span>";
            fixedContainer.Put(newDataLabel, 410, 129);

            Label currentIdLabel = new Label();
            currentIdLabel.Markup = "<span font='Arial 12' weight='bold'>ID:</span>";
            fixedContainer.Put(currentIdLabel, 82, 172);

            currentIdEntry.SetSizeRequest(140, 35);
            currentIdEntry.Sensitive = false;
            fixedContainer.Put(currentIdEntry, 170, 166);

            Label namesLabel = new Label();
            namesLabel.Markup = "<span font='Arial 12' weight='bold'>Repuesto:</span>";
            fixedContainer.Put(namesLabel, 46, 227);

            currentSpareEntry.SetSizeRequest(140, 35);
            currentSpareEntry.Sensitive = false;
            fixedContainer.Put(currentSpareEntry, 170, 221);

            newSpareEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(newSpareEntry, 380, 221);

            Label lastNamesLabel = new Label();
            lastNamesLabel.Markup = "<span font='Arial 12' weight='bold'>Detalles:</span>";
            fixedContainer.Put(lastNamesLabel, 55, 282);

            currentDetailsEntry.SetSizeRequest(140, 35);
            currentDetailsEntry.Sensitive = false;
            fixedContainer.Put(currentDetailsEntry, 170, 276);

            newDetailsEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(newDetailsEntry, 380, 276);

            Label costLabel = new Label();
            costLabel.Markup = "<span font='Arial 12' weight='bold'>Costo:</span>";
            fixedContainer.Put(costLabel, 64, 337);

            currentCostEntry.SetSizeRequest(140, 35);
            currentCostEntry.Sensitive = false;
            fixedContainer.Put(currentCostEntry, 170, 331);

            newCostEntry.SetSizeRequest(140, 35);
            fixedContainer.Put(newCostEntry, 380, 331);

            Button updateButton = new Button("Actualizar");
            updateButton.SetSizeRequest(120, 30);
            updateButton.Clicked += OnUpdateButtonClicked;
            fixedContainer.Put(updateButton, 239, 386);

            Button returnButton = new Button("Volver");
            returnButton.SetSizeRequest(50, 20);
            returnButton.Clicked += OnReturnButtonClicked;
            fixedContainer.Put(returnButton, 20, 440);

            Add(fixedContainer);

            DeleteEvent += (o, args) => Application.Quit();
        }

        private void OnReturnButtonClicked(object sender, EventArgs e)
        {
            found = false;
            CleanEntrys();
            GlobalWindows.adminMenu.ShowAll();
            Hide();
        }

        private void OnSearchButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idEntry.Text))
            {
                Login.ShowDialog(this, MessageType.Error, "Debe ingresar un ID de repuesto.");
                return;
            }

            if (!int.TryParse(idEntry.Text, out idFound))
            {
                Login.ShowDialog(this, MessageType.Error, "ID inválido.");
                return;
            }

            if (GlobalStructures.SparePartsTree.IsEmpty())
            {
                Login.ShowDialog(this, MessageType.Warning, "El árbol de repuestos se encuentra actualmente vacío. Ingrese repuestos antes de continuar.");
                return;
            }

            SparePart sparePartSearched = GlobalStructures.SparePartsTree.Get(idFound);

            if (sparePartSearched == null)
            {
                Login.ShowDialog(this, MessageType.Error, "No se encontró un repuesto con el ID ingresado.");
                return;
            }
            else
            {
                Login.ShowDialog(this, MessageType.Info, "Repuesto encontrado.");
                found = true;
                newSpareEntry.Text = "";
                newDetailsEntry.Text = "";
                newCostEntry.Text = "";
                currentIdEntry.Text = sparePartSearched.Id.ToString();
                currentSpareEntry.Text = sparePartSearched.Spare;
                currentDetailsEntry.Text = sparePartSearched.Details;
                currentCostEntry.Text = sparePartSearched.Cost.ToString();
            }
        }

        private void OnUpdateButtonClicked(object sender, EventArgs e)
        {

            if (!found)
            {
                Login.ShowDialog(this, MessageType.Warning, "Busque un repuesto que actualizar.");
                return;
            }

            if (string.IsNullOrEmpty(newSpareEntry.Text) && string.IsNullOrEmpty(newDetailsEntry.Text) && string.IsNullOrEmpty(newCostEntry.Text))
            {
                Login.ShowDialog(this, MessageType.Error, "Debe llenar por lo menos un campo de los nuevos datos para actualizar.");
                return;
            }

            SparePart sparePartSearched = GlobalStructures.SparePartsTree.Get(idFound);

            if (sparePartSearched != null)
            {
                if (!string.IsNullOrEmpty(newCostEntry.Text))
                {
                    if (double.TryParse(newCostEntry.Text, out double cost))
                    {
                        sparePartSearched.Cost = cost;
                    }
                    else
                    {
                        Login.ShowDialog(this, MessageType.Error, "Costo inválido.");
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(newSpareEntry.Text))
                    sparePartSearched.Spare = newSpareEntry.Text;

                if (!string.IsNullOrEmpty(newDetailsEntry.Text))
                    sparePartSearched.Details = newDetailsEntry.Text;

                Login.ShowDialog(this, MessageType.Info, "Repuesto actualizado exitosamente.");
                newSpareEntry.Text = "";
                newDetailsEntry.Text = "";
                newCostEntry.Text = "";
                currentSpareEntry.Text = sparePartSearched.Spare;
                currentDetailsEntry.Text = sparePartSearched.Details;
                currentCostEntry.Text = sparePartSearched.Cost.ToString();
            }
            else
            {
                Login.ShowDialog(this, MessageType.Error, "Error al actualizar los datos del usuario.");
                return;
            }
        }

        private void CleanEntrys()
        {
            idEntry.Text = "";
            currentIdEntry.Text = "";
            newSpareEntry.Text = "";
            newDetailsEntry.Text = "";
            newCostEntry.Text = "";
            currentSpareEntry.Text = "";
            currentDetailsEntry.Text = "";
            currentCostEntry.Text = "";
        }
    }
}