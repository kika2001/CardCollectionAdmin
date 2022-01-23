using System.Collections.Generic;
using _Script.Communication;
using _Script.Extensions;
using _Script.Tables;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Menu = _Script.MenuManager.Menu;

namespace _Script.Menus
{
    public class CreateCard : Menu
    {
        [SerializeField] TMP_Dropdown rarityDropDown;
        [SerializeField] TMP_InputField inputName;
        [SerializeField] RarityTable[] raritys;
        [SerializeField] private FilterSystem filter;
        string path;
        public override void OnEnable()
        {
            base.OnEnable();
            ServerConnection.Instance.ExecutePHP("GetRarities.php", GetRarities);
            filter.Refresh();
        }
        private void GetRarities(string text)
        {
            //Debug.Log(text);
            rarityDropDown.ClearOptions();
            raritys = JsonExtension.getJsonArray<RarityTable>(text);
            List<string> rarityNames = new List<string>(); 
            foreach (var rarity in raritys)
            {
                rarityNames.Add(rarity.RarityName);
            }
            rarityDropDown.AddOptions(rarityNames);
            /*
            List<string> rarities = text.Split('|').ToList();
            for (int i = 0; i < rarities.Count; i++)
            {
                if (rarities[i] == " " || rarities[i] == "")
                {
                    rarities.RemoveAt(i);
                }           
            }
            dropDown.AddOptions(rarities);
            */
            
        }
        /*
        public void PickImage() //Only works in Editor
        {
#if UNITY_EDITOR
            path = EditorUtility.OpenFilePanel("Show all Images (.png)", "", "png");
#endif
            //ANDROID
            //PickImage();
        }   

        public void PickImageWindows()
        {        
            FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));

            // Set default filter that is selected when the dialog is shown (optional)
            // Returns true if the default filter is set successfully
            // In this case, set Images filter as the default filter
            FileBrowser.SetDefaultFilter(".jpg");

            // Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
            // Note that when you use this function, .lnk and .tmp extensions will no longer be
            // excluded unless you explicitly add them as parameters to the function
            FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

            // Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
            // It is sufficient to add a quick link just once
            // Name: Users
            // Path: C:\Users
            // Icon: default (folder icon)
            FileBrowser.AddQuickLink("Users", "C:\\Users", null);

            // Show a save file dialog 
            // onSuccess event: not registered (which means this dialog is pretty useless)
            // onCancel event: not registered
            // Save file/folder: file, Allow multiple selection: false
            // Initial path: "C:\", Initial filename: "Screenshot.png"
            // Title: "Save As", Submit button text: "Save"
            // FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\\", "Screenshot.png", "Save As", "Save" );

            // Show a select folder dialog 
            // onSuccess event: print the selected folder's path
            // onCancel event: print "Canceled"
            // Load file/folder: folder, Allow multiple selection: false
            // Initial path: default (Documents), Initial filename: empty
            // Title: "Select Folder", Submit button text: "Select"
            // FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "Selected: " + paths[0] ); },
            //						   () => { Debug.Log( "Canceled" ); },
            //						   FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select" );

            // Coroutine example
            StartCoroutine(ShowLoadDialogCoroutine());
        

        }
    
        IEnumerator ShowLoadDialogCoroutine()
        {
            // Show a load file dialog and wait for a response from user
            // Load file/folder: both, Allow multiple selection: true
            // Initial path: default (Documents), Initial filename: empty
            // Title: "Load File", Submit button text: "Load"
            yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

            // Dialog is closed
            // Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
            Debug.Log(FileBrowser.Success);

            if (FileBrowser.Success)
            {
                // Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
                for (int i = 0; i < FileBrowser.Result.Length; i++)
                    Debug.Log(FileBrowser.Result[i]);

                // Read the bytes of the first file via FileBrowserHelpers
                // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
                byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

                // Or, copy the first file to persistentDataPath
                string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
                FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
            }
        }
        */
        public void Create()
        {
            if (inputName.text.Trim(' ').Length>0)
            {
                ServerConnection.Instance.ExecutePHP("SetCard.php", $"name={inputName.text}&rarity={rarityDropDown.value + 1}", ConsoleLog.UpdateLog);
            }
            else
            {
                ConsoleLog.UpdateLog("0 | Please write the name correctly");
            }
        }
    
    }
}

