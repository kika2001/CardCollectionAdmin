using System.Collections.Generic;
using System.Linq;
using _Script.Communication;
using _Script.Extensions;
using _Script.MenuManager;
using _Script.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace _Script.Menus
{
    public class RankEdit : Menu
    {
        private static RankTable rank;
        [SerializeField] private TMP_InputField IfRankName;
        public UnityEvent OnEdit;
        public void SetRank(RankTable r)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                rank = r;
                IfRankName.text = rank.RankName;
            }
            else
            {
                gameObject.SetActive(false);
                rank = null;
            }
            
        }

        public void UpdateRank()
        {
            ServerConnection.Instance.ExecutePHP("EditRank.php",$"rankId={rank.RankID}&rankName={IfRankName.text}", Verify);
        }

        private void Verify(string text)
        {
            ConsoleLog.UpdateLog(text);
            rank = null;
            OnEdit.Invoke();
            gameObject.SetActive(false);
        }
    }
}