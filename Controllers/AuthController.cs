using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text;
using WomanSite.Database;
using WomanSite.Models;
namespace WomanSite.Controllers
{
    public class AuthController
    {
        public bool Login(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var item in db.Users)
                {
                    if (item.name == user.name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
