using _Script.Communication;
using _Script.MenuManager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _Script.Menus
{
    public class UserTagCreate : Menu
    {
        [SerializeField] private TMP_InputField IfUserTagName;
        public UnityEvent OnSubmit;

        public void Open()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                IfUserTagName.text = "";
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        public void CreateUserTag()
        {
            ServerConnection.Instance.ExecutePHP("CreateUserTag.php",$"userTagName={IfUserTagName.text}", Create);
        }

        private void Create(string text)
        {
            ConsoleLog.UpdateLog(text);
            OnSubmit.Invoke();
            gameObject.SetActive(false);
        }
    }
}