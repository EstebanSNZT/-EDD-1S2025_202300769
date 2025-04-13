using Interface;

namespace Global
{
    public class GlobalWindows
    {
        //--------------------Admin----------------------
        public static Login login = new Login();
        public static AdminMenu adminMenu = new AdminMenu();
        public static BulkUpload bulkUpload = new BulkUpload();
        public static UsersInsertion usersInsertion = new UsersInsertion();
        public static UsersVisualization usersVisualization = new UsersVisualization();
        public static UpdatedSpareParts updatedSparePart = new UpdatedSpareParts();
        public static SparePartsVisualization sparePartsVisualization = new SparePartsVisualization();
        public static GenerateService generateService = new GenerateService();

        //--------------------Users----------------------
        public static UserMenu userMenu = new UserMenu();
        public static InsertVehicle insertVehicle = new InsertVehicle();
        public static ServicesVisualization servicesVisualization = new ServicesVisualization();
        public static InvoicesVisualization invoicesVisualization = new InvoicesVisualization();
        public static CancelInvoice cancelInvoice = new CancelInvoice();


        public static void DestroyAll()
        {
            login.Destroy();
            adminMenu.Destroy();
            bulkUpload.Destroy();
            usersInsertion.Destroy();
            usersVisualization.Destroy();
            updatedSparePart.Destroy();
            sparePartsVisualization.Destroy();
            generateService.Destroy();
            userMenu.Destroy();
            insertVehicle.Destroy();
            servicesVisualization.Destroy();
            invoicesVisualization.Destroy();
            cancelInvoice.Destroy();
        }
    }
}