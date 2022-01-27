using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Script;
using _Script.Communication;
using _Script.Extensions;
using _Script.Tables;
using DefaultNamespace;
using TMPro;
using UnityEditor;
using UnityEngine;
using Menu = _Script.MenuManager.Menu;

namespace _Script.Menus
{
    public class EditCard : Menu
    {
        [SerializeField] TMP_Dropdown rarityDropDown;
        [SerializeField] TMP_InputField inputName;
        [SerializeField] TMP_InputField inputDescription;
        [SerializeField] private TextMeshProUGUI cardName, cardDescription;
        [SerializeField] List<RarityTable> raritys;
        string path;

        [SerializeField] FilterBoosterCardTag filterTag;
        [SerializeField] FilterCardSeasons filterSeasons;

        private CardTable currentCard;
        public void SetCard(CardTable card)
        {
            currentCard = card;
            inputName.text = card.CardName;
            inputDescription.text = card.CardDescription;
            
            ChangeName(card.CardName);
            ChangeDescription(card.CardDescription);
            StartCoroutine(AfterRefreshedCourotine(card));
            //StartCoroutine(WaitForFilterSeasonCourotine(card));

        }

        private IEnumerator AfterRefreshedCourotine(CardTable card)
        {
            yield return new WaitUntil(() =>filterTag.refreshed && filterSeasons.refreshed && raritys.Count>0);
            filterTag.SelectIndexes(card.CardTags);
            filterSeasons.SelectIndexes(card.CardSeasonID.ToString());
            rarityDropDown.value = raritys.IndexOf(raritys.First(d => d.RarityID == card.RarityID));
        }
        public override void OnEnable()
        {
            base.OnEnable();
            ServerConnection.Instance.ExecutePHP("GetRarities.php", GetRarities);
            filterSeasons.Refresh();
            filterTag.Refresh();
            filterTag.gameObject.SetActive(false);
            filterSeasons.gameObject.SetActive(false);
        }

        private void GetRarities(string text)
        {
            //Debug.Log(text);
            rarityDropDown.ClearOptions();
            raritys = JsonExtension.getJsonArray<RarityTable>(text).ToList();
            List<string> rarityNames = new List<string>();
            foreach (var rarity in raritys)
            {
                rarityNames.Add(rarity.RarityName);
            }

            rarityDropDown.AddOptions(rarityNames);

        }

        public void ChangeName(string t)
        {
            if (t == "")
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
            if (t == "")
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

        public void Edit()
        {
            var seasonsPicked = filterSeasons.CheckedValues();
            string seasons = "";
            if (seasonsPicked.Count > 0)
            {
                seasons = $"{seasonsPicked[0].CardSeasonID}";
            }


            var tagsPicked = filterTag.CheckedValues();
            string tags = "";
            if (tagsPicked.Count > 0)
            {
                tags = $"{tagsPicked[0].CardTagID}";
                for (int i = 1; i < tagsPicked.Count; i++)
                {
                    tags += $",{tagsPicked[i].CardTagID}";
                }
            }


            if (inputName.text.Trim(' ').Length > 0 && tag != "" && seasons != "" &&
                inputDescription.text.Trim(' ').Length > 0)
            {
                ServerConnection.Instance.ExecutePHP("EditCard.php",
                    $"id={currentCard.CardID}&name={inputName.text}&description={inputDescription.text}&rarity={raritys[rarityDropDown.value].RarityID}&season={seasons}&tags={tags}",
                    ConsoleLog.UpdateLog);
            }
            else
            {
                ConsoleLog.UpdateLog("0 | Please fill all the fields. Dont forget the tags and seasons");
            }
        }
    }
}