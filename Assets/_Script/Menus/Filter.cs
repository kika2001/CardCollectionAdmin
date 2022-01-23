using System.Collections.Generic;
using UnityEngine;

namespace _Script.Menus
{
    public class Filter<T> 
    {
        public List<ToggleObject<T>> filterItems = new List<ToggleObject<T>>();

        public ToggleObject<T> Add(T value,GameObject go, bool b)
        {
            ToggleObject<T> toggle = new ToggleObject<T>(value, go, b);
            filterItems.Add(toggle);
            return toggle;
        }

        public List<T> CheckedValues()
        {
            List<T> itemsChecked = new List<T>();
            foreach (var i in filterItems)
            {
                if (i.check)
                {
                    itemsChecked.Add(i.item);
                }
            }
            return itemsChecked;
        }
    }
}