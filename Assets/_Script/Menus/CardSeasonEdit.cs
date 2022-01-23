using _Script.Communication;
using _Script.MenuManager;
using _Script.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _Script.Menus
{
    public class CardSeasonEdit : Menu
    {
        private static CardSeasonTable cardSeason;
        [SerializeField] private TMP_InputField IfSeasonName;
        public UnityEvent OnEdit;
        public void SetSeason(CardSeasonTable r)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                cardSeason = r;
                IfSeasonName.text = cardSeason.CardSeasonName;
            }
            else
            {
                gameObject.SetActive(false);
                cardSeason = null;
            }
            
        }

        public void UpdateCardSeason()
        {
            ServerConnection.Instance.ExecutePHP("EditCardSeason.php",$"cardSeasonId={cardSeason.CardSeasonID}&cardSeasonName={IfSeasonName.text}", Verify);
        }

        private void Verify(string text)
        {
            ConsoleLog.UpdateLog(text);
            cardSeason = null;
            OnEdit.Invoke();
            gameObject.SetActive(false);
        }
    }
}