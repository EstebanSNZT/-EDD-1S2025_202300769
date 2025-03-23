using Interface;

namespace Global
{
    public class GlobalWindows
    {
        public static Login login = new Login();
        public static AdminMenu adminMenu = new AdminMenu();
        public static BulkUpload bulkUpload = new BulkUpload();
        public static EntityManagement entityManagement = new EntityManagement();
        public static UserManagement userManagement = new UserManagement();
        public static VehicleManagement vehicleManagement = new VehicleManagement();
    }
}