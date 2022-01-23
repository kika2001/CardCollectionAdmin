using System.Collections.Generic;
using System.Linq;
using _Script.Classes;
using _Script.Communication;
using _Script.Extensions;
using _Script.MenuManager;
using _Script.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Script.Menus
{
    public class InventoryMenu : Menu
    {
        [SerializeField] private List<InventoryItem> items;
        private List<GameObject> itemsGO = new List<GameObject>();
        [SerializeField] private Transform contentTransform;
        [SerializeField] private GameObject itemsTemplate;

        //Tables with all the values to be used
        private List<InventoryCardsTable> inventoryCards;
        private List<InventoryBoostersTable> inventoryBooster;
        private Dictionary<int, string> players;
        private Dictionary<int, string> cards;
        private Dictionary<int, string> boosters;


        [Header("Visuals")] 
        [SerializeField] private Color cardColor;
        [SerializeField] private Color boosterColor;
        
        public override void OnEnable()
        {
            base.OnEnable();
            RefreshTable("");
        }
        
        private void RefreshTable(string text)
        {
            //Deletes for now, but should be a pool for performance
            Debug.Log(text);
            for (int i = 0; i < contentTransform.childCount; i++)
            {
                Destroy(contentTransform.GetChild(i).gameObject);
            }
            itemsGO = new List<GameObject>();
            items = new List<InventoryItem>();
            players = new Dictionary<int, string>();
            cards = new Dictionary<int, string>();
            boosters = new Dictionary<int, string>();
            ServerConnection.Instance.ExecutePHP("GetUsers.php", GetPlayers);
        }

        //GetPlayers
        private void GetPlayers(string text)
        {
            var playerValues = JsonExtension.getJsonArray<UserTable>(text).ToList();
            foreach (var playerValue in playerValues)
            {
                players.Add(playerValue.UserID,playerValue.UserNick);
            }
            ServerConnection.Instance.ExecutePHP("GetInventoryCards.php", GetInventoryCards);
        }
        //Inventory
        private void GetInventoryCards(string text)
        {
            inventoryCards = JsonExtension.getJsonArray<InventoryCardsTable>(text).ToList();
            ServerConnection.Instance.ExecutePHP("GetCards.php", GetCards);
        }

        private void GetCards(string text)
        {
            var cardValues = JsonExtension.getJsonArray<CardTable>(text).ToList();
            foreach (var card in cardValues)
            {
                cards.Add(card.CardID,card.CardName);
            }
            ServerConnection.Instance.ExecutePHP("GetInventoryBoosters.php", GetInventoryBoosters);
        }

        private void GetInventoryBoosters(string text)
        {
            inventoryBooster = JsonExtension.getJsonArray<InventoryBoostersTable>(text).ToList();
            ServerConnection.Instance.ExecutePHP("GetBoosters.php", GetBoosters);
            
        }

        private void GetBoosters(string text)
        {
            var boosterValues = JsonExtension.getJsonArray<BoostersTable>(text).ToList();
            foreach (var booster in boosterValues)
            {
                boosters.Add(booster.BoosterID,booster.BoosterName);
            }
            
            InventorySetup();
            SpawnCards();
        }

        private void InventorySetup()
        {
            foreach (var booster in inventoryBooster)
            {
                InventoryItem i = new InventoryItem(booster);
                items.Add(i);
            }

            foreach (var card in inventoryCards)
            {
                InventoryItem i = new InventoryItem(card);
                items.Add(i); 
            }
        }

        private void SpawnCards()
        {
            foreach (var item in items)
            {
                GameObject go = Instantiate(itemsTemplate, contentTransform);
                itemsGO.Add(go);
                if (item.card==null)
                {
                    go.transform.Find("userId").GetComponent<TextMeshProUGUI>().text = $"{item.booster.PlayerID}";
                    go.transform.Find("userName").GetComponent<TextMeshProUGUI>().text = players[item.booster.PlayerID];
                    go.transform.Find("type").GetComponent<Image>().color = boosterColor;
                    go.transform.Find("type").Find("name").GetComponent<TextMeshProUGUI>().text = boosters[item.booster.BoosterID];
                    go.transform.Find("BtnDelete").GetComponent<Button>().onClick.AddListener(()=> DeleteItem(item));
                    go.transform.Find("creationDate").GetComponent<TextMeshProUGUI>().text = $"{item.dateCreated}";
                }else if (item.booster==null)
                {
                    go.transform.Find("userId").GetComponent<TextMeshProUGUI>().text = $"{item.card.PlayerID}";
                    go.transform.Find("userName").GetComponent<TextMeshProUGUI>().text = players[item.card.PlayerID];
                    go.transform.Find("type").GetComponent<Image>().color = cardColor;
                    go.transform.Find("type").Find("name").GetComponent<TextMeshProUGUI>().text = cards[item.card.CardID];
                    go.transform.Find("BtnDelete").GetComponent<Button>().onClick.AddListener(()=> DeleteItem(item));
                    go.transform.Find("creationDate").GetComponent<TextMeshProUGUI>().text = $"{item.dateCreated}";
                }
                
            }
            /*
            foreach (var card in cards)
            {
                GameObject go = Instantiate(cardTemplate, contentTransform);
                itemsGO.Add(go);
                go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = card.CardName;
                go.transform.Find("BtnEdit").GetComponent<Button>().onClick.AddListener(()=> EditBooster(card));
                go.transform.Find("BtnDelete").GetComponent<Button>().onClick.AddListener(()=> DeleteBooster(card));
            }
            */
        }
        
        private void DeleteItem(InventoryItem item)
        {
            if (item.card==null)
            {
                ServerConnection.Instance.ExecutePHP("DeleteInventoryBooster.php",$"id={item.booster.ID}", RefreshTable);
            }
            else if (item.booster==null)
            {
                ServerConnection.Instance.ExecutePHP("DeleteInventoryCard.php",$"id={item.card.ID}", RefreshTable);
            }
            
        }
        
    }
}
