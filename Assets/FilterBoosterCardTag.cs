﻿using System;
using System.Collections.Generic;
using System.Linq;
using _Script.Communication;
using _Script.Extensions;
using _Script.Menus;
using _Script.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class FilterBoosterCardTag : FilterSystem
    {
        [Header("Filter")] 
       
        //All the seasons retrieved from the DB
        private List<CardTagTable> tags;
        //Filter which stores all the toggle objects and their values
        private Filter<CardTagTable> tagFilter;
        private void Awake()
        {
            
        }

        public override void Refresh()
        {
            phpPath = "GetCardTags.php";
            base.Refresh();
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
            
            ServerConnection.Instance.ExecutePHP("GetCardTags.php", GetSeasons);
        }
        */
        public override void Get(string text)
        {
            
            itemsGO = new List<GameObject>();
            tags = JsonExtension.getJsonArray<CardTagTable>(text).ToList();
            tagFilter = new Filter<CardTagTable>();
            foreach (var t in tags)
            {
                GameObject go = Instantiate(prefab, content);
                itemsGO.Add(go);
                go.transform.Find("text").GetComponent<TextMeshProUGUI>().text = t.CardTagName;
                var toggleGO= go.transform.Find("Toggle").GetComponent<Toggle>();
                toggleGO.isOn = false;
                    
                var toggle =tagFilter.Add(t, go, toggleGO.isOn);
                toggleGO.onValueChanged.AddListener((value) =>
                {
                    toggle.check=value;
                    OnToggleChange.Invoke();
                }); 
            }
        }
        public List<CardTagTable> CheckedValues() => tagFilter.CheckedValues();
    }
}