using _Script.Communication;
using _Script.MenuManager;
using _Script.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _Script.Menus
{
    public class UserTagEdit : Menu
    {
        private static UserTagTable userTag;
        [SerializeField] private TMP_InputField IfRankName;
        public UnityEvent OnEdit;
        public void SetRank(UserTagTable r)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                userTag = r;
                IfRankName.text = userTag.UserTagName;
            }
            else
            {
                gameObject.SetActive(false);
                userTag = null;
            }
            
        }

        public void UpdateUserTag()
        {
            ServerConnection.Instance.ExecutePHP("EditUserTag.php",$"userTagId={userTag.UserTagID}&userTagName={IfRankName.text}", Verify);
        }

        private void Verify(string text)
        {
            ConsoleLog.UpdateLog(text);
            userTag = null;
            OnEdit.Invoke();
            gameObject.SetActive(false);
        }
    }
}