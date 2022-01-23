using System.Collections.Generic;
using System.Linq;
using _Script.Communication;
using _Script.Extensions;
using _Script.Menus;
using _Script.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public abstract class FilterSystem : MonoBehaviour
    {
        [Header("Filter")] 
        //Season prefab
        [SerializeField] protected GameObject prefab;
        //Season items spawned
        protected List<GameObject> itemsGO;
        //Season content where all the items are showing
        [SerializeField] protected Transform content;
    
        public UnityEvent OnToggleChange;
        public UnityEvent OnAfterRefresh;
        protected string phpPath;
    
        private void OnDisable()
        {
            OnToggleChange.RemoveAllListeners();
        }
    
        public virtual void Refresh()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
            itemsGO = new List<GameObject>();
            ServerConnection.Instance.ExecutePHP(phpPath, Get);
        }

        public abstract void Get(string text);
    }
}