using _Script.Communication;
using _Script.MenuManager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _Script.Menus
{
    public class CardTagCreate : Menu
    {
        [SerializeField] private TMP_InputField IfCardTagName;
        public UnityEvent OnSubmit;

        public void Open()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                IfCardTagName.text = "";
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        public void CreateCardTag()
        {
            ServerConnection.Instance.ExecutePHP("CreateCardTag.php",$"cardTagName={IfCardTagName.text}", Create);
        }

        private void Create(string text)
        {
            ConsoleLog.UpdateLog(text);
            OnSubmit.Invoke();
            gameObject.SetActive(false);
        }
    }
}