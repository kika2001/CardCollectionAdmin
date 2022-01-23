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

public class FilterCardSeasons : FilterSystem
{
    [Header("Filter")] 
    
    //All the seasons retrieved from the DB
    private List<CardSeasonTable> seasons;
    //Filter which stores all the toggle objects and their values
    private Filter<CardSeasonTable> seasonFilter;

    private void Awake()
    {
        phpPath = "GetCardSeasons.php";
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
                
            var toggle =seasonFilter.Add(s, go, toggleGO.isOn);
            toggleGO.onValueChanged.AddListener((value) =>
            {
                toggle.check=value;
                OnToggleChange.Invoke();
            }); 
        }
    }

    public List<CardSeasonTable> CheckedValues() => seasonFilter.CheckedValues();
}
