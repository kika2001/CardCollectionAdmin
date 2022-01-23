using _Script.Communication;
using _Script.MenuManager;
using _Script.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _Script.Menus
{
    public class CardTagEdit : Menu
    {
        private static CardTagTable cardTag;
        [SerializeField] private TMP_InputField IfTagName;
        public UnityEvent OnEdit;
        public void SetRank(CardTagTable r)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                cardTag = r;
                IfTagName.text = cardTag.CardTagName;
            }
            else
            {
                gameObject.SetActive(false);
                cardTag = null;
            }
            
        }

        public void UpdateCardTag()
        {
            ServerConnection.Instance.ExecutePHP("EditCardTag.php",$"cardTagId={cardTag.CardTagID}&cardTagName={IfTagName.text}", Verify);
        }

        private void Verify(string text)
        {
            ConsoleLog.UpdateLog(text);
            cardTag = null;
            OnEdit.Invoke();
            gameObject.SetActive(false);
        }
    }
}