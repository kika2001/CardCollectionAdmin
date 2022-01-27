using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Script.Communication;
using _Script.Extensions;
using _Script.MenuManager;
using _Script.Menus;
using _Script.Tables;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(ToggleGroup))]
public class FilterCardSeasons : FilterSystem
{
    [Header("Filter")] 
    
    //All the seasons retrieved from the DB
    private List<CardSeasonTable> seasons;
    //Filter which stores all the toggle objects and their values
    private Filter<CardSeasonTable> seasonFilter;
    public bool refreshed;
    
    [SerializeField]private ToggleGroup _group;
    public bool hasOnlyOne;

    public override void Refresh()
    {
        refreshed = false;
        phpPath = "GetCardSeasons.php";
        base.Refresh();
    }

    public void SelectIndexes(string text)
    {
        var splitedTags =text.Split(',');
        foreach (var t in splitedTags)
        {
            foreach (var filterItem in seasonFilter.filterItems)
            {
                if (filterItem.item.CardSeasonID==Int32.Parse(t))
                {
                    filterItem.check = true;
                    filterItem.go.transform.Find("Toggle").GetComponent<Toggle>().isOn=true;
                    Debug.LogWarning($"Selected {filterItem.item.CardSeasonName}");
                }
            }
        }
            
    }
    
    private void OnDisable()
    {
        OnToggleChange.RemoveAllListeners();
    }

    public override void Get(string text)
    {
        itemsGO = new List<GameObject>();
        seasons = JsonExtension.getJsonArray<CardSeasonTable>(text).ToList();
        seasonFilter = new Filter<CardSeasonTable>();
        foreach (var s in seasons)
        {
            GameObject go = Instantiate(prefab, content);
            itemsGO.Add(go);
            go.transform.Find("text").GetComponent<TextMeshProUGUI>().text = s.CardSeasonName;
            var toggleGO= go.transform.Find("Toggle").GetComponent<Toggle>();
            toggleGO.isOn = false;
            if (hasOnlyOne)
            {
                Debug.Log("Added To group");
                toggleGO.group = _group;
            }
            else
            {
                toggleGO.group = null;
            }
            var toggle =seasonFilter.Add(s, go, toggleGO.isOn);
            toggleGO.onValueChanged.AddListener((value) =>
            {
                toggle.check=value;
                OnToggleChange.Invoke();
            }); 
        }

        refreshed = true;
    }

    public List<CardSeasonTable> CheckedValues() => seasonFilter.CheckedValues();
}
