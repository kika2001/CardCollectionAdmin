using System.Collections.Generic;
using System.Linq;
using _Script.Communication;
using _Script.Extensions;
using _Script.MenuManager;
using _Script.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Script.Menus
{
    public class GetCardsMenu : Menu
    {
        [SerializeField] List<CardTable> cards;
        private List<GameObject> cardsGO = new List<GameObject>();
        [SerializeField] private Transform contentTransform;
        [SerializeField] private GameObject cardTemplate;

        [Header("Filter")] 
        [SerializeField] private FilterCardSeasons filter;

        [Header("Menus")] 
        [SerializeField] private EditCard editMenu;
        public override void OnEnable()
        {
            base.OnEnable();
            RefreshTable("");
            filter.OnToggleChange.AddListener(UpdateCards);
        }
        private void GetCards(string text)
        {
            cards = JsonExtension.getJsonArray<CardTable>(text).ToList();
            SpawnCards();
            filter.Refresh();
        }

        
        private void SpawnCards()
        {
            foreach (var card in cards)
            {
                GameObject go = Instantiate(cardTemplate, contentTransform);
                cardsGO.Add(go);
                go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = card.CardName;
                go.transform.Find("description").GetComponent<TextMeshProUGUI>().text = card.CardDescription;
                go.transform.Find("BtnEdit").GetComponent<Button>().onClick.AddListener(()=> EditBooster(card));
                go.transform.Find("BtnDelete").GetComponent<Button>().onClick.AddListener(()=> DeleteBooster(card));
            }
        }


        public void UpdateCards()
        {
            var filterCards = new List<CardTable>(cards);
            //Debug.Log($"Filtered Cards Initial Length:{filterCards.Count}");
            
            var checkedSeasons = filter.CheckedValues();
            //Debug.Log($"Checked Seasons length:{checkedSeasons.Count}");
            if (checkedSeasons.Count>0)
            {
                for (int i = filterCards.Count - 1; i >= 0; i--)
                {
                    bool hasID = false;
                    foreach (var s in checkedSeasons)
                    {
                        if (filterCards[i].CardSeasonID==s.CardSeasonID)
                        {
                            hasID = true;
                        }
                    }

                    if (!hasID)
                    {
                        filterCards.RemoveAt(i);
                    }
                }

                /*
                foreach (var s in checkedSeasons)
                {
                    filterCards.RemoveAll(item => item.CardSeasonID != s.CardSeasonID);
                }
                */
            }
            


            //Debug.Log($"Filtered Cards Final Length:{filterCards.Count}. Cards Length: {cards.Count}");
            if (filterCards.Count==cards.Count)
            {
                cardsGO.ForEach(d=> d.SetActive(true));
            }
            else
            {
                cardsGO.ForEach(d=> d.SetActive(false));
                
                for (int i = 0; i < filterCards.Count; i++)
                {
                    cardsGO[cards.IndexOf(filterCards[i])].SetActive(true);
                }
            }
        }
        private void EditBooster(CardTable card)
        {
            gameObject.SetActive(false);
            editMenu.gameObject.SetActive(true);
            
            editMenu.SetCard(card);
            
        }

        private void DeleteBooster(CardTable card)
        {
            ServerConnection.Instance.ExecutePHP("DeleteCard.php",$"cardId={card.CardID}", RefreshTable);
        }
        
        private void RefreshTable(string text)
        {
            //Deletes for now, but should be a pool for performance
            for (int i = 0; i < contentTransform.childCount; i++)
            {
                Destroy(contentTransform.GetChild(i).gameObject);
            }
            cardsGO = new List<GameObject>();
            
            ServerConnection.Instance.ExecutePHP("GetCards.php", GetCards);
        }
    }
    

}