using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Script.Communication;
using _Script.Extensions;
using _Script.Menus;
using _Script.Tables;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FilterUserTags : FilterSystem
{
    [Header("Filter")] 
    private List<UserTagTable> tags;
    //Filter which stores all the toggle objects and their values
    private Filter<UserTagTable> tagFilter;


    private void Awake()
    {
        phpPath = "GetUserTags.php";
    }

    private void OnDisable()
    {
        OnToggleChange.RemoveAllListeners();
    }

    /*
    public void Refresh()
    {
        for (int i = 0; i < tagContent.childCount; i++)
        {
            Destroy(tagContent.GetChild(i).gameObject);
        }
        tagItemsGO = new List<GameObject>();
        
        ServerConnection.Instance.ExecutePHP("GetUserTags.php", GetUserTags);
    }
    */

    public override void Get(string text)
    {
        itemsGO = new List<GameObject>();
        tags = JsonExtension.getJsonArray<UserTagTable>(text).ToList();
        tagFilter = new Filter<UserTagTable>();
        foreach (var u in tags)
        {
            GameObject go = Instantiate(prefab, content);
            itemsGO.Add(go);
            go.transform.Find("text").GetComponent<TextMeshProUGUI>().text = u.UserTagName;
            var toggleGO= go.transform.Find("Toggle").GetComponent<Toggle>();
            toggleGO.isOn = false;
                
            var toggle =tagFilter.Add(u, go, toggleGO.isOn);
            toggleGO.onValueChanged.AddListener((value) =>
            {
                toggle.check=value;
                OnToggleChange.Invoke();
            }); 
        }
    }

    public List<UserTagTable> CheckedValues() => tagFilter.CheckedValues();
}
