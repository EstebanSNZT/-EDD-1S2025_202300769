using Gtk;

namespace Interface
{
    public class GlobalWindows
    {
        public static Login login = new Login();
        public static Menu menu = new Menu();
        public static ReportsMenu reportsMenu = new ReportsMenu();
        public static BulkUpload bulkUpload = new BulkUpload();
        public static EntryOptions entryOptions = new EntryOptions();
        public static GenerateService generateService = new GenerateService();
        public static UserEntry userEntry = new UserEntry();
        public static VehicleEntry vehicleEntry = new VehicleEntry();
        public static SparePartEntry sparePartEntry = new SparePartEntry();
        public static UserManagement userManagement = new UserManagement();
        public static CancelInvoice cancelInvoice = new CancelInvoice();
        public static ShowReport showReport1 = new ShowReport(1300, 300);
        public static ShowReport showReport2 = new ShowReport(300, 650);
        public static ShowReport showReport3 = new ShowReport(900, 650);
    }
}