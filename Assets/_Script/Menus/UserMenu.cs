using System;
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
    public class UserMenu : Menu
    {
        [SerializeField] private GameObject userItemPrefab;
        [SerializeField] private List<UserTable>players;
        [SerializeField] private Transform content;

        [SerializeField]private List<GameObject> usersItemsGO = new List<GameObject>();

        [SerializeField] private UserEdit userEdit;
    
        
        [Header("Filter")] 
        [SerializeField] private FilterUserTags filter;
        
        public override void OnEnable()
        {
            base.OnEnable();
            RefreshTable();
            filter.OnToggleChange.AddListener(UpdateCards);
        }

        private void SpawnPlayers(string text)
        {
            players = JsonExtension.getJsonArray<UserTable>(text).ToList();
            filter.Refresh();
            Debug.Log(players.Count);
            foreach (var player in players)
            {
                GameObject go = Instantiate(userItemPrefab, content);
                usersItemsGO.Add(go);
                go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = player.UserNick;
                go.transform.Find("BtnEdit").GetComponent<Button>().onClick.AddListener(() => EditPlayer(player));
                var buttonMenu = go.transform.Find("BtnEdit").GetComponent<ButtonMenu>();
                buttonMenu.closeGO = this;
                buttonMenu.openGO = userEdit;
                go.transform.Find("BtnDelete").GetComponent<Button>().onClick.AddListener(() => DeletePlayer(player));
            }
        }

        private void EditPlayer(UserTable user)
        {
            userEdit.SetPlayer(user);
        }

        public void DeletePlayer(UserTable user)
        {
            ServerConnection.Instance.ExecutePHP("DeleteUser.php",$"playerId={user.UserID}",RefreshTable);
        }

        private void RefreshTable(string text="")
        {
            //Deletes for now, but should be a pool for performance
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }

            usersItemsGO = new List<GameObject>();
            ServerConnection.Instance.ExecutePHP("GetUsers.php", SpawnPlayers);
        }
     
        public void UpdateCards()
        {
            var filterPlayers = new List<UserTable>(players);
            //Debug.Log($"Filtered Cards Initial Length:{filterCards.Count}");
            
            var checkedTags = filter.CheckedValues();
            //Debug.Log($"Checked Seasons length:{checkedSeasons.Count}");
            if (checkedTags.Count>0)
            {
                for (int i = filterPlayers.Count - 1; i >= 0; i--)
                {
                    bool hasID = false;
                    if (filterPlayers[i].UserTags.Length <= 0) continue;
                    var splitedTags = filterPlayers[i].UserTags.Split(',');
                    if (splitedTags.Length > 0)
                    {
                        foreach (var t in checkedTags)
                        {
                            foreach (var splitTag in splitedTags)
                            {
                                if (int.Parse(splitTag) == t.UserTagID)
                                {
                                    hasID = true;
                                }
                            }
                        }
                    }

                    if (!hasID)
                    {
                        filterPlayers.RemoveAt(i);
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
            if (filterPlayers.Count==players.Count)
            {
                usersItemsGO.ForEach(d=> d.SetActive(true));
            }
            else
            {
                usersItemsGO.ForEach(d=> d.SetActive(false));
                
                for (int i = 0; i < filterPlayers.Count; i++)
                {
                    usersItemsGO[players.IndexOf(filterPlayers[i])].SetActive(true);
                }
            }
        }
        
    }
}
