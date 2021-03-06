using System.Collections.Generic;
using _Script.Communication;
using _Script.Extensions;
using _Script.Tables;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Menu = _Script.MenuManager.Menu;

namespace _Script.Menus
{
    public class CreateCard : Menu
    {
        [SerializeField] TMP_Dropdown rarityDropDown;
        [SerializeField] TMP_InputField inputName;
        [SerializeField] TMP_InputField inputDescription;
        [SerializeField] private TextMeshProUGUI cardName, cardDescription;
        [SerializeField] RarityTable[] raritys;
        string path;

        [SerializeField] FilterBoosterCardTag filterTag;
        [SerializeField] FilterCardSeasons filterSeasons;
        public override void OnEnable()
        {
            base.OnEnable();
            ServerConnection.Instance.ExecutePHP("GetRarities.php", GetRarities);
            filterSeasons.Refresh();
            filterTag.Refresh();
            filterTag.gameObject.SetActive(false);
            filterSeasons.gameObject.SetActive(false);

            inputName.text = "";
            inputDescription.text = "";

            ChangeName("");
            ChangeDescription("");


        }
        private void GetRarities(string text)
        {
            //Debug.Log(text);
            rarityDropDown.ClearOptions();
            raritys = JsonExtension.getJsonArray<RarityTable>(text);
            List<string> rarityNames = new List<string>(); 
            foreach (var rarity in raritys)
            {
                rarityNames.Add(rarity.RarityName);
            }
            rarityDropDown.AddOptions(rarityNames);
            /*
            List<string> rarities = text.Split('|').ToList();
            for (int i = 0; i < rarities.Count; i++)
            {
                if (rarities[i] == " " || rarities[i] == "")
                {
                    rarities.RemoveAt(i);
                }           
            }
            dropDown.AddOptions(rarities);
            */
            
        }

        public void ChangeName(string t)
        {
            if (t=="")
            {
                cardName.text = "Card Name";
            }
            else
            {
                cardName.text = t;
            }
        }
        public void ChangeDescription(string t)
        {
            if (t=="")
            {
                cardDescription.text = "Sample Description";
            }
            else
            {
                cardDescription.text = t;
            }
        }
        
        public void OpenSeasons()
        {
            if (filterTag.gameObject.activeSelf)
            {
                filterTag.gameObject.SetActive(false);
            }
            filterSeasons.gameObject.SetActive(!filterSeasons.gameObject.activeSelf);    
        }

        public void OpenTags()
        {
            if (filterSeasons.gameObject.activeSelf)
            {
                filterSeasons.gameObject.SetActive(false);
            }
            filterTag.gameObject.SetActive(!filterTag.gameObject.activeSelf);
            
        }
        public void Create()
        {
            var seasonsPicked = filterSeasons.CheckedValues();
            string seasons = "";
            if (seasonsPicked.Count>0)
            {
                seasons = $"{seasonsPicked[0].CardSeasonID}";
            }
            
            
            var tagsPicked = filterTag.CheckedValues();
            string tags = "";
            if (tagsPicked.Count>0)
            {
                tags = $"{tagsPicked[0].CardTagID}";
                for (int i = 1; i < tagsPicked.Count; i++)
                {
                    tags += $",{tagsPicked[i].CardTagID}";
                }
            }
            
            
            if (inputName.text.Trim(' ').Length>0 && tag!="" && seasons!="" && inputDescription.text.Trim(' ').Length>0)
            {
                ServerConnection.Instance.ExecutePHP("CreateCard.php", $"name={inputName.text}&description={inputDescription.text}&rarity={raritys[rarityDropDown.value].RarityID}&seasons={seasons}&tags={tags}", ConsoleLog.UpdateLog);
            }
            else
            {
                ConsoleLog.UpdateLog("0 | Please fill all the fields. Dont forget the tags and seasons");
            }
        }
    
    }
}

