using System.Collections.Generic;
using System.Linq;
using _Script.Communication;
using _Script.Extensions;
using _Script.MenuManager;
using _Script.Tables;
using TMPro;
using UnityEngine;

namespace _Script.Menus
{
    public class UserEdit : Menu
    {
        private static UserTable _user;
        [SerializeField] private TMP_Dropdown ranksDropDown;
        [SerializeField] private TextMeshProUGUI playerNameText;
        private RankTable[] ranks;
        public override void OnEnable()
        {
            base.OnEnable();
            ServerConnection.Instance.ExecutePHP("GetRanks.php", GetRarities);
        }
        private void GetRarities(string text)
        {
            Debug.Log(text);
            ranksDropDown.ClearOptions();
            ranks = JsonExtension.getJsonArray<RankTable>(text);
            List<string> rankNames = new List<string>(); 
            foreach (var rarity in ranks)
            {
                rankNames.Add(rarity.RankName);
            }
            ranksDropDown.AddOptions(rankNames);
            playerNameText.text = $"Editing Player: <color=#5BF354><u> {_user.UserNick}</color></u>. Current rank: <color=#5BF354><u>{ranks.First(d=> d.RankID == _user.UserRank).RankName}</color></u>";
        }

        public void SetPlayer(UserTable p)
        {
            _user = p;
        }

        public void UpdateRank()
        {
            ServerConnection.Instance.ExecutePHP("EditUser.php",$"playerId={_user.UserID}&playerRank={ranksDropDown.value+1}", Verify);
        }

        private void Verify(string text)
        {
            Debug.Log($"Entered. Text: {text}");
            ConsoleLog.UpdateLog(text);
        
            string[] parsedText = text.Split('|');
            parsedText[0] = parsedText[0].Trim(' ');
            if (parsedText[0] == "1")
            {
                _user.UserRank = ranksDropDown.value;
                Debug.Log($"Player Rank New: {_user.UserRank}");
                //playerNameText.text = $"Editing Player: <color=#5BF354><u> {player.PlayerNick}</color></u>. Current rank: <color=#5BF354><u>{ranks.First(d=> d.RankID == player.PlayerRank).RankName}</color></u>";
            }
        }
    
    }
}
