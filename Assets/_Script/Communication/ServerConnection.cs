using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace _Script.Communication
{
    public class ServerConnection : MonoBehaviour
    {
        private static ServerConnection _instance;
        [SerializeField] GameObject loadingScreen;
        public static ServerConnection Instance
        {
            get => _instance;
            private set { _instance = value; }
        }
        public ServerSettings settings;
        private void Awake()
        {
            if (Instance!=null)
            {
                Destroy(gameObject);
            }
            loadingScreen.SetActive(false);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        public void ExecutePHP(string phpPath,string getters, Action<string> action = null)
        {
            string url = $"{settings.url}/{phpPath}?{settings.LoginInfo}&{getters}";
            Debug.Log(url);
            StartCoroutine(OpenUrl(url, action));
        }
        public void ExecutePHP(string phpPath, Action<string> action = null)
        {
            string url = $"{settings.url}/{phpPath}?{settings.LoginInfo}";
            Debug.Log(url);
            StartCoroutine(OpenUrl(url, action));
        }

        public void LoadingScreenActive(bool b)
        {
            loadingScreen.SetActive(b);
        }

        private IEnumerator OpenUrl(string url, Action<string> action=null)
        {
            //Debug.Log(url);
            loadingScreen.SetActive(true);
            UnityWebRequest w = UnityWebRequest.Get(url);
            yield return w.SendWebRequest();
            if (w.isDone)
            {
                if (action!=null)
                {
                    action(w.downloadHandler.text);
                }
                loadingScreen.SetActive(false);
            }
        }
        
        
        public void ExecutePostPHP(string phpPath, string path, UnityWebRequest file,Action<string> action = null)
        {
            string url = $"{settings.url}/{phpPath}";
            StartCoroutine(OpenUrlPost(url, path,file,action));
        }

        private IEnumerator OpenUrlPost(string url, string path,UnityWebRequest file, Action<string> action = null)
        {
            
            WWWForm form = new WWWForm();
            form.AddBinaryData("submit", file.downloadHandler.data, Path.GetFileName(path));


            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                Debug.Log(www.url);
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    if (action != null)
                    {
                        action(www.downloadHandler.text);
                    }
                    //Debug.Log("0| Image upload complete!");
                }
            }
        }
        

       
    }
}
