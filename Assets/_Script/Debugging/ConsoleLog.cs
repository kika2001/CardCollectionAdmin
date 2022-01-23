using TMPro;
using UnityEngine;

namespace _Script
{
    public class ConsoleLog : MonoBehaviour
    {
        static TextMeshProUGUI logText;
        public float clearTime;
        private static float currentClearTime;
        public bool isLogOn;
        private static bool isLogMode;
        private void Awake()
        {
            if (logText!=null)
            {
                Destroy(gameObject);
            }
            logText = GetComponent<TextMeshProUGUI>();
            isLogMode = isLogOn;
        }

        public void ChangeLogMode(bool on)
        {
            isLogOn = on;
            isLogMode = isLogOn;
        }
        private void Update()
        {
            if (clearTime > 0)
            {
                if (clearTime <= currentClearTime)
                {
                    //Resets
                    logText.text = "";
                    logText.color = Color.white;
                }
                else
                {
                    currentClearTime += Time.deltaTime;
                }
            }
        
        }
        public static void UpdateLog(string log)
        {
            if (!isLogMode) return;
            string[] parsedText = log.Split('|');
            parsedText[0] = parsedText[0].Trim(' ');
            if (parsedText[0] == "1")
            {
                logText.color = Color.green;
            }
            else if (parsedText[0] == "0")
            {
                logText.color = Color.red;

            }
            string text = "";
            for (int i = 1; i < parsedText.Length; i++)
            {
                text += parsedText[i] + " ";
            }
            logText.text = text;
            currentClearTime = 0;
        
        }
    }
}
