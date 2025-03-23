using Classes;
using Structures;
using Utilities;

class Program
{
    public static void Main(string[] args)
    {
        BinaryTree servicesTree = new BinaryTree();
        Service service1 = new Service(5, 1005, 2005, "Filtro de Aire", 20);
        Service service2 = new Service(3, 1003, 2003, "Pastillas de freno", 25.50);
        Service service3 = new Service(7, 1007, 2007, "Batería", 65.99);
        Service service4 = new Service(4, 1004, 2004, "Aceite de Motor", 29.99);
        Service service5 = new Service(6, 1006, 2006, "Liquido de Frenos", 40);
        Service service6 = new Service(1, 1001, 2001, "Filtro de Aceite", 15.80);
        Service service7 = new Service(2, 1002, 2002, "Bujía", 35);
        Service service8 = new Service(9, 1009, 2009, "Neumático", 99.99);
        Service service9 = new Service(10, 1010, 2010, "Escobillas", 75.50);
        Service service10 = new Service(8, 1008, 2008, "Lámpara", 85.99);

        servicesTree.Insert(service1);
        servicesTree.Insert(service2);
        servicesTree.Insert(service3);
        servicesTree.Insert(service4);
        servicesTree.Insert(service5);
        servicesTree.Insert(service6);
        servicesTree.Insert(service7);
        servicesTree.Insert(service8);
        servicesTree.Insert(service9);
        servicesTree.Insert(service10);

        string dotCode = servicesTree.GenerateDot();
        Utility.GenerateDotFile("Servicios", dotCode);
        Utility.ConvertDotToImage("Servicios.dot");
    }
}