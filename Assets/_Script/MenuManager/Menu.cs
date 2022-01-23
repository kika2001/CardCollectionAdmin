using System.Collections.Generic;
using UnityEngine;

namespace _Script.MenuManager
{
    public class Menu : MonoBehaviour
    {
        public List<GameObject> initialsObjects = new List<GameObject>();
        public virtual void OnEnable()
        {
            //Enable Initials
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            
            foreach (var initial in initialsObjects)
            {
                initial.SetActive(true);
            }
        }

        public virtual void OnDisable()
        {
            
            //Enable Initials
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
