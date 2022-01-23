using System;
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
    public class GiftSendMenu : Menu
    {
        private List<PickedItem> itemsPicked = new List<PickedItem>();
        private List<ToggleObject<UserTable>> playerToggles = new List<ToggleObject<UserTable>>();
        private RankTable[] ranks;

        [Header("Spawn")] 
        [SerializeField] private Transform playerContent;
        [SerializeField] private GameObject playerItemPrefab;

        [Header("Menu")]
        [SerializeField] private GiftPickMenu giftPick;
        public void ReceiveItems(List<PickedItem> itms)
        {
            itemsPicked = itms;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            playerIds = "";
            ServerConnection.Instance.ExecutePHP("GetRanks.php", GetRarities);
        }

        private void GetRarities(string text)
        {
            ranks = JsonExtension.getJsonArray<RankTable>(text);
            ServerConnection.Instance.ExecutePHP("GetUsers.php",SetupPlayers);
        }
        
        private void SetupPlayers(string text)
        {
            foreach (var t in playerToggles)
            {
                Destroy(t.go);
            }
            playerToggles = new List<ToggleObject<UserTable>>();
            
            var playerList = JsonExtension.getJsonArray<UserTable>(text).ToList();
            foreach (var player in playerList)
            {
                GameObject go = Instantiate(playerItemPrefab, playerContent);
                ToggleObject<UserTable> toggle = new ToggleObject<UserTable>(player, go);
                playerToggles.Add(toggle);
                go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = player.UserName;
                go.transform.Find("tag").GetComponent<TextMeshProUGUI>().text = ranks.First(d => d.RankID == player.UserRank).RankName;
                go.transform.Find("togglePick").GetComponent<Toggle>().onValueChanged.AddListener( (value) => { toggle.check=value; }); 
            }
        }

        public void SendGifts()
        {
            SendGiftsCards();
        }

        private string playerIds;
        private void SendGiftsCards()
        {
            //Exemplo : INSERT INTO projects(name, start_date, end_date) VALUES ('AI for Marketing','2019-08-01','2019-12-31'),('ML for Sales','2019-05-15','2019-11-20');
            //INSERT INTO InventoryCards(PlayerID,CardID,DateCreated) VALUES (
            //Send Cards
            
            /*
            var cardList = itemsPicked.Where(d => d.booster == null).ToList();
            string text="";
            bool firstPlaced = false;
            for (int p = 0; p < playerToggles.Count; p++)
            {
                for (int c = 0; c < cardList.Count; c++)
                {
                    if (!playerToggles[p].check) break;
                    if (!firstPlaced)
                    {
                        text += $"({playerToggles[p].item.PlayerID},{cardList[c].card.CardID},{DateTime.Today})";
                        firstPlaced = true;
                    }
                    else
                    {
                        text += $",({playerToggles[p].item.PlayerID},{cardList[c].card.CardID},{DateTime.Today})";
                    }
                }
                
            }
            */
            
            playerIds = "";
            string cardIds = "";
            var playerList = playerToggles.Where(d => d.check).ToList();
            for (int i = 0; i < playerList.Count; i++)
            {
                if (i==0)
                {
                    playerIds += $"{playerList[i].item.UserID}";
                }
                else
                {
                    playerIds += $",{playerList[i].item.UserID}";
                }
            }
            
            var cardList = itemsPicked.Where(d => d.itemType==ItemType.Card).ToList();
            if (cardList.Count>0)
            {
                for (int i = 0; i < cardList.Count; i++)
                {
                    for (int a = 0; a < cardList[i].amount; a++)
                    {
                        if (i==0 && a==0)
                        {
                            cardIds += $"{cardList[i].card.CardID}";
                        }
                        else
                        {
                            cardIds += $",{cardList[i].card.CardID}";
                        }
                    }
                    
                }
            
                ServerConnection.Instance.ExecutePHP("CreateInventoryCards.php",$"playerIds={playerIds}&cardIds={cardIds}",SendGiftBoosters);
            }
            else
            {
                SendGiftBoosters("Didnt have cards to send");
            }
            
            
            //Send Boosters
        }

        private void SendGiftBoosters(string t)
        {
            Debug.LogWarning($"Cards Result: {t}");
            
            string boosterIds = "";
            var boosterList = itemsPicked.Where(d => d.itemType==ItemType.Booster).ToList();
            if (boosterList.Count>0)
            {
                for (int i = 0; i < boosterList.Count; i++)
                {
                    for (int a = 0; a < boosterList[i].amount; a++)
                    {
                        if (i == 0 && a==0)
                        {
                            boosterIds += $"{boosterList[i].booster.BoosterID}";
                        }
                        else
                        {
                            boosterIds += $",{boosterList[i].booster.BoosterID}";
                        }
                    }
                }
            
                ServerConnection.Instance.ExecutePHP("CreateInventoryBoosters.php",$"playerIds={playerIds}&boosterIds={boosterIds}",OnFinish);
            }
            else
            {
                OnFinish("Didnt have boosters to send");
            }
           
            
            //Send Boosters
        }

        private void OnFinish(string text)
        {
            Debug.LogWarning($"Booster Result: {text}");
            ReturnMenu();
        }

        private void ReturnMenu()
        {
            giftPick.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    [Serializable]
    public class ToggleObject<T>
    {
        public bool check;
        public T item;
        public GameObject go;
        
        public ToggleObject(T i,GameObject g, bool value=false)
        {
            item = i;
            check = value;
            go = g;
        }
    }
}