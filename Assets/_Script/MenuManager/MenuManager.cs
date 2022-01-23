using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace _Script.MenuManager
{
    public class MenuManager : MonoBehaviour
    {
        private static MenuManager instance;
        public static MenuManager Instance 
        {
            set
            {
                instance = value;
            }
            get
            {
                return instance;
            }
        }
        private List<Menu> menus = new List<Menu>();
        public UnityEvent AfterSetupEvent;

        private void Awake()
        {
            if (instance!=null)
            {
                Destroy(this.gameObject);
            }
            instance = this;
            menus = transform.GetComponentsInChildren<Menu>(true).ToList();
            DisableAllMenus();
            AfterSetupEvent?.Invoke();
        }
        public void DisableAllMenus()
        {
            foreach (var item in menus)
            {
                item.gameObject.SetActive(false);
            }
        }
        public void OpenOnlyOne(Menu menu)
        {
            foreach (var item in menus)
            {
                if (item!=menu)
                {
                    item.gameObject.SetActive(false);
                }
                else
                {
                    item.gameObject.SetActive(true);
                }
            }
        }
    }
}
