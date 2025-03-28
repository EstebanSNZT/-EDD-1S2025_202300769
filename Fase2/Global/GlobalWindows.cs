using Interface;

namespace Global
{
    public class GlobalWindows
    {
        //--------------------Admin----------------------
        public static Login login = new Login();
        public static AdminMenu adminMenu = new AdminMenu();
        public static BulkUpload bulkUpload = new BulkUpload();
        public static EntityManagement entityManagement = new EntityManagement();
        public static UserManagement userManagement = new UserManagement();
        public static VehicleManagement vehicleManagement = new VehicleManagement();
        public static UpdatedSpareParts updatedSparePart = new UpdatedSpareParts();
        public static SparePartsVisualization sparePartsVisualization = new SparePartsVisualization();
        public static GenerateService generateService = new GenerateService();

        //--------------------Users----------------------
        public static UserMenu userMenu = new UserMenu();
        public static InsertVehicle insertVehicle = new InsertVehicle();
        public static ServicesVisualization servicesVisualization = new ServicesVisualization();
        public static InvoicesVisualization invoicesVisualization = new InvoicesVisualization();
        public static CancelInvoice cancelInvoice = new CancelInvoice();
    }
}