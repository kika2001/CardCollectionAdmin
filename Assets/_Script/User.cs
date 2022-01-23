using _Script.Tables;

namespace _Script
{
    public class User
    {
        public static AdminsTable UserAdminTable { get; private set; }

        public static void ChangeCurrentLogin(AdminsTable adminTableAccount)
        {
            UserAdminTable = adminTableAccount;
        }

        public static void LogOut()
        {
            UserAdminTable = null;
        }
        
    }
}