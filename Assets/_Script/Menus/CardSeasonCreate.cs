using _Script.Communication;
using _Script.MenuManager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _Script.Menus
{
    public class CardSeasonCreate : Menu
    {
        [SerializeField] private TMP_InputField IfCardSeasonName;
        public UnityEvent OnSubmit;

        public void Open()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                IfCardSeasonName.text = "";
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        public void CreateCardSeason()
        {
            ServerConnection.Instance.ExecutePHP("CreateCardSeason.php",$"cardSeasonName={IfCardSeasonName.text}", Create);
        }

        private void Create(string text)
        {
            ConsoleLog.UpdateLog(text);
            OnSubmit.Invoke();
            gameObject.SetActive(false);
        }
    }
}