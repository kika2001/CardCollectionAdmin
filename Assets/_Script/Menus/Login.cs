using _Script.Communication;
using _Script.Extensions;
using _Script.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _Script.Menus
{
    public class Login : _Script.MenuManager.Menu
    {
        [SerializeField] private TMP_InputField userInput, passInput;
        //private Admin adminAccount;
        public UnityEvent onLoginSuccessfully;
        public void TryLogin()
        {
            ServerConnection.Instance.ExecutePHP("AdminLogin.php",$"loginUser={userInput.text}&loginPass={passInput.text}",Verify);
        }

        private void Verify(string text)
        {
            var adminAccounts = JsonExtension.getJsonArray<AdminsTable>(text);
            if (adminAccounts.Length>0)
            {
                //adminAccount = adminAccounts[0];
                User.ChangeCurrentLogin(adminAccounts[0]);
                onLoginSuccessfully?.Invoke();
            }
            else
            {
                ConsoleLog.UpdateLog("0| User or Password wrong.");
            }
        }
    
    }
}
