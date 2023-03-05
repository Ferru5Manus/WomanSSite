using System.Security.Cryptography;
using System.Text;
using WomanSite.Database;
using WomanSite.Models;
namespace WomanSite.Controllers
{
    public class AuthController
    {
        public string Register(User user)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    user.password = EncryptPlainTextToCipherText(user.password);
                    db.Users.Add(user);
                }
                Console.WriteLine("Registred sucsessefully");
                return "Успешная регистрация!";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "Ошибка регистрации";
            }

        }
      
        public static string EncryptPlainTextToCipherText(string PlainText)
        {
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(PlainText);

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SecurityKey")));
            objMD5CryptoService.Clear();
            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            objTripleDESCryptoService.Key = securityKeyArray;
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;


            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
            objTripleDESCryptoService.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public string Login(User user)
        {

        }
    }
}
