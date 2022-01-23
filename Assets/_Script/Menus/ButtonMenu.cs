using _Script.MenuManager;
using UnityEngine;
using UnityEngine.UI;

namespace _Script.Menus
{
    public class ButtonMenu : MonoBehaviour
    {
        public Menu closeGO;
        public Menu openGO;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Interact);
        
        }

        public void Interact()
        {
            if (openGO!=null)
            {
                openGO.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("There's no GameObject to open!");
            }

            if (closeGO!=null)
            {
                closeGO.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("There's no GameObject to close!");
            }
        }

    }
}
