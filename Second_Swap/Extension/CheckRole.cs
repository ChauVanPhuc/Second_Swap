using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using Second_Swap.Models;

namespace Second_Swap.Extension
{
    public static class CheckRole
    {

        public static string CheckRoleLogin(User account)
        {
            

            if (account == null)
            {
                return "";
            }
            else
            {
                return account.Role.Name.ToString();
            }
            /*string role = "";
            if (account.Role.Name.Equals("Guest"))
            {
                return role = "Guest";  
            }
            else if(account.Role.Name.Equals("Student"))
            {
                return role = "Student";
            }
            else if (account.Role.Name.Equals("Coordinator"))
            {
                return role = "Coordinator";
            }
            else if (account.Role.Name.Equals("Maketting"))
            {
                return role = "Maketting";
            }
            else
            {
                return role = "Admmin";
            }*/
        }
    }
}
