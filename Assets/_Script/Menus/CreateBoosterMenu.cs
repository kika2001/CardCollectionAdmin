using System.Collections.Generic;
using System.Linq;
using _Script.Communication;
using _Script.Extensions;
using _Script.MenuManager;
using _Script.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Script.Menus
{
	public class CreateBoosterMenu : Menu
	{
		[Header("Menus")] [SerializeField] private Menu displayMenu;
	
		[Header("Available Menu")]
		[SerializeField] private Transform availableContent;
		[SerializeField] private GameObject availableItem;
	
		[SerializeField]private List<CardTable> availableCards = new List<CardTable>();
		private List<GameObject> availableCardsGO = new List<GameObject>();
		//----------------------------------------------------------------------------------
	
		[Header("Picked Menu")]
		[SerializeField] private Transform pickedContent;
		[SerializeField] private GameObject pickedItem;
	
		[SerializeField]private List<CardTable> pickedCards = new List<CardTable>();
		private List<GameObject> pickedCardsGO = new List<GameObject>();
		//----------------------------------------------------------------------------------
		[Header("Other Infos")] 
		[SerializeField] private TMP_InputField boosterName;
		[SerializeField] private TMP_InputField boosterDescription;
		private string imagePath="";
	
		public override void OnEnable()
		{
			base.OnEnable();
			boosterName.text = "";
			boosterDescription.text="";
			imagePath = "";
			ServerConnection.Instance.ExecutePHP("GetCards.php",SetupCards);
		}

		private void SetupCards(string text)
		{
			foreach (var t in availableCardsGO)
			{
				Destroy(t);
			}
			availableCardsGO = new List<GameObject>();
			availableCards = new List<CardTable>();

			foreach (var t in pickedCardsGO)
			{
				Destroy(t);
			}
			pickedCardsGO = new List<GameObject>();
			pickedCards = new List<CardTable>();
		
			availableCards = JsonExtension.getJsonArray<CardTable>(text).ToList();
			foreach (var card in availableCards)
			{
				GameObject go = Instantiate(availableItem, availableContent);
				availableCardsGO.Add(go);
				go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = card.CardName;
				go.transform.Find("BtnPick").GetComponent<Button>().onClick.AddListener(() => PickCard(card));
			}
		}

		private void PickCard(CardTable card)
		{
			int i = availableCards.IndexOf(card);
		
			availableCards.RemoveAt(i);
			Destroy(availableCardsGO[i]);
			availableCardsGO.RemoveAt(i);
		
			GameObject go = Instantiate(pickedItem, pickedContent);
			pickedCards.Add(card);
			pickedCardsGO.Add(go);
			go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = card.CardName;
			go.transform.Find("BtnRemove").GetComponent<Button>().onClick.AddListener(() => RemoveCard(card));
		}

		private void RemoveCard(CardTable card)
		{
			int i = pickedCards.IndexOf(card);
		
			pickedCards.RemoveAt(i);
			Destroy(pickedCardsGO[i]);
			pickedCardsGO.RemoveAt(i);
		
			GameObject go = Instantiate(availableItem, availableContent);
			availableCards.Add(card);
			availableCardsGO.Add(go);
			go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = card.CardName;
			go.transform.Find("BtnPick").GetComponent<Button>().onClick.AddListener(() => PickCard(card));
		}

		public void CreateBooster()
		{
			if (pickedCards.Count <= 0) return;
			string cardIds="";
			cardIds += pickedCards[0].CardID;
			for (int i = 1; i < pickedCards.Count; i++)
			{
				cardIds += $",{pickedCards[i].CardID}";
			}
			ServerConnection.Instance.ExecutePHP("CreateBooster.php",$"bName={boosterName.text}&bDescription={boosterDescription.text}&bCards={cardIds}&bImage={imagePath}",CheckResult);
			Debug.Log($"Cards IDS={cardIds}");
		
		}

		private void CheckResult(string text)
		{
			ConsoleLog.UpdateLog(text);
			string[] parsedText = text.Split('|');
			parsedText[0] = parsedText[0].Trim(' ');
			if (parsedText[0] == "1") //Works
			{
				displayMenu.gameObject.SetActive(true);
				gameObject.SetActive(false);
			}
		}

		/*
	public void PickImage()
	{
		
		// Set filters (optional)
		// It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
		// if all the dialogs will be using the same filters
		FileBrowser.SetFilters( true, new FileBrowser.Filter( "Images", ".jpg", ".png" ), new FileBrowser.Filter( "Text Files", ".txt", ".pdf" ) );

		// Set default filter that is selected when the dialog is shown (optional)
		// Returns true if the default filter is set successfully
		// In this case, set Images filter as the default filter
		FileBrowser.SetDefaultFilter( ".jpg" );

		// Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
		// Note that when you use this function, .lnk and .tmp extensions will no longer be
		// excluded unless you explicitly add them as parameters to the function
		FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );

		// Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
		// It is sufficient to add a quick link just once
		// Name: Users
		// Path: C:\Users
		// Icon: default (folder icon)
		FileBrowser.AddQuickLink( "Users", "C:\\Users", null );
		

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
		StartCoroutine( ShowLoadDialogCoroutine() );
	}

	private void UploadPhoto(string[] paths)
	{
		
		Debug.Log($"Ficheiro Escolhido: {paths[0]}");
		var file = UnityWebRequest.Get(paths[0]);
		ServerConnection.Instance.ExecutePostPHP("TestUpload/upload.php",paths[0],file,CheckIfUploaded);
		

	}

	private void CheckIfUploaded(string obj)
	{
		Debug.Log(obj);
	}

	IEnumerator ShowLoadDialogCoroutine()
	{
		// Show a load file dialog and wait for a response from user
		// Load file/folder: both, Allow multiple selection: true
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Load File", Submit button text: "Load"
		yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load" );

		// Dialog is closed
		// Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
		Debug.Log( FileBrowser.Success );

		if( FileBrowser.Success )
		{
			// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
			UploadPhoto(FileBrowser.Result);
			
		}
	}
	*/
	}
}
