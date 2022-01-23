using System;
using System.Collections.Generic;
using System.Linq;
using _Script.Communication;
using _Script.Extensions;
using _Script.MenuManager;
using _Script.Tables;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Script.Menus
{
    public class BoosterMenu : Menu
    {
        [SerializeField] List<BoostersTable> boosters;
        private List<GameObject> boostersGO = new List<GameObject>();
        [SerializeField] GameObject boosterTemplate;
        [SerializeField] Transform contentTransform;
        
        [Header("Filter")] 
        [SerializeField] private FilterBoosterCardTag filter;
    
        public override void OnEnable()
        {
            base.OnEnable();
            RefreshTable("");
            filter.OnToggleChange.AddListener(UpdateCards);
        }
        private void GetBoosters(string text)
        {
            boosters = JsonExtension.getJsonArray<BoostersTable>(text).ToList();
            SpawnBoosters();
            filter.Refresh();
        
        }
        
   
        private void SpawnBoosters()
        {
            foreach (var booster in boosters)
            {
                GameObject go = Instantiate(boosterTemplate, contentTransform);
                boostersGO.Add(go);
                go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = booster.BoosterName;
                go.transform.Find("BtnEdit").GetComponent<Button>().onClick.AddListener(()=> EditBooster(booster));
                go.transform.Find("BtnDelete").GetComponent<Button>().onClick.AddListener(()=> DeleteBooster(booster));
            }
        }

        private void EditBooster(BoostersTable booster)
        {
        
        }

        private void DeleteBooster(BoostersTable booster)
        {
            ServerConnection.Instance.ExecutePHP("DeleteBooster.php",$"boosterId={booster.BoosterID}", RefreshTable);
        }

        private void RefreshTable(string text)
        {
            for (int i = 0; i < boostersGO.Count; i++)
            {
                Destroy(boostersGO[i]);
            }
            ServerConnection.Instance.ExecutePHP("GetBoosters.php", GetBoosters);
        }

        public void UpdateCards()
        {
            var filteredBoosters = new List<BoostersTable>(boosters);
            //Debug.Log($"Filtered Cards Initial Length:{filterCards.Count}");
            
            var checkedTags = filter.CheckedValues();
            //Debug.Log($"Checked Seasons length:{checkedSeasons.Count}");
            if (checkedTags.Count>0)
            {
                for (int i = filteredBoosters.Count - 1; i >= 0; i--)
                {
                    bool hasID = false;
                    if (filteredBoosters[i].BoosterTags.Length <= 0) continue;
                    var splitedTags = filteredBoosters[i].BoosterTags.Split(',');
                    if (splitedTags.Length > 0)
                    {
                        foreach (var t in checkedTags)
                        {
                            foreach (var splitTag in splitedTags)
                            {
                            
                                if (int.Parse(splitTag)== t.CardTagID)
                                {
                                    hasID = true;
                                }

                            }
                        }
                    }
                    if (!hasID)
                    {
                        filteredBoosters.RemoveAt(i);
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
            if (filteredBoosters.Count==boosters.Count)
            {
                boostersGO.ForEach(d=> d.SetActive(true));
            }
            else
            {
                boostersGO.ForEach(d=> d.SetActive(false));
                
                for (int i = 0; i < filteredBoosters.Count; i++)
                {
                    boostersGO[boosters.IndexOf(filteredBoosters[i])].SetActive(true);
                }
            }
        }
    }
}
