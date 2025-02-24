using Gtk;
using Lists;

namespace Interface
{
    public class ViewVehicles : Window
    {
        public ViewVehicles(int userId) : base("AutoGest Pro - Ver información de los vehiculos")
        {
            SetSizeRequest(400, 200); //(ancho, alto)
            SetPosition(WindowPosition.Center);

            if (Child != null)
            {
                Remove(Child);
            }
            
            TreeView treeView = GlobalLists.doubleList.GenerateTreeView(userId);
            ScrolledWindow scrolledWindow = new ScrolledWindow();
            scrolledWindow.Add(treeView);
            Add(scrolledWindow);
        }
    }
}