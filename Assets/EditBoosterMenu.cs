using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Script;
using _Script.Communication;
using _Script.Extensions;
using _Script.MenuManager;
using _Script.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Int32;

public class EditBoosterMenu : Menu
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
		[SerializeField] private TextMeshProUGUI previewName, previewDescription;
		private string imagePath="";

		private BoostersTable currentBooster;
		private bool setupFinished = false;

		public void SetBooster(BoostersTable booster)
		{
			currentBooster = booster;
			boosterName.text = booster.BoosterName;
			boosterDescription.text=booster.BoosterDescription;
			imagePath = booster.BoosterImage;
			
			ChangeName(booster.BoosterName);
			ChangeDescription(booster.BoosterDescription);


			
			StartCoroutine(AfterRefreshedCourotine(booster));

		}

		private IEnumerator AfterRefreshedCourotine(BoostersTable booster)
		{
			yield return new WaitUntil(() =>setupFinished);
			var selectCards = booster.BoosterCards.Split(',');
			foreach (var card in selectCards)
			{
				PickCard(availableCards.First(d=>d.CardID==Parse(card)));
			}
		}
		
		public override void OnEnable()
		{
			base.OnEnable();
			
			
			ServerConnection.Instance.ExecutePHP("GetCards.php",SetupCards);
		}

		public void ChangeName(string t)
		{
			if (t=="")
			{
				previewName.text = "Booster Name";
			}
			else
			{
				previewName.text = t;
			}
		}
		public void ChangeDescription(string t)
		{
			if (t=="")
			{
				previewDescription.text = "Sample Description";
			}
			else
			{
				previewDescription.text = t;
			}
		}
		
		private void SetupCards(string text)
		{
			setupFinished = false;
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
			setupFinished = true;
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

		public void EditBooster()
		{
			if (pickedCards.Count <= 0) return;
			string cardIds="";
			cardIds += pickedCards[0].CardID;
			for (int i = 1; i < pickedCards.Count; i++)
			{
				cardIds += $",{pickedCards[i].CardID}";
			}
			ServerConnection.Instance.ExecutePHP("EditBooster.php",$"id={currentBooster.BoosterID}&bName={boosterName.text}&bDescription={boosterDescription.text}&bCards={cardIds}&bImage={imagePath}",CheckResult);
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

		
}
