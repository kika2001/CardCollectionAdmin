using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Script;
using _Script.Classes;
using _Script.Communication;
using _Script.Extensions;
using _Script.MenuManager;
using _Script.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Int32;

namespace _Script.Menus
{
	public class GiftPickMenu : Menu
{
	[Header("Card Menu")]
	[SerializeField] private Transform cardsContent;
	[SerializeField] private GameObject cardPrefab;
	
	private List<CardTable> cardsList = new List<CardTable>();
	private List<GameObject> cardsListGO = new List<GameObject>();
	
	//----------------------------------------------------------------------------------
	[Header("Booster Menu")]
	[SerializeField] private Transform boostersContent;
	[SerializeField] private GameObject boosterPrefab;
	
	private List<BoostersTable> boostersList = new List<BoostersTable>();
	private List<GameObject> boostersListGO = new List<GameObject>();
	//----------------------------------------------------------------------------------
	
	[Header("Picked Menu")]
	[SerializeField] private Transform pickedContent;
	[SerializeField] private GameObject pickedPrefab;
	
	private List<PickedItem> pickedItems = new List<PickedItem>();
	private List<GameObject> pickedCardsGO = new List<GameObject>();
	//----------------------------------------------------------------------------------
	[Header("Send Menu")]
	[SerializeField]
	private GiftSendMenu sendMenu;

	[Header("UI elements")] [SerializeField]
	private Button btnConfirm;
	public override void OnEnable()
	{
		base.OnEnable();
		ServerConnection.Instance.ExecutePHP("GetCards.php",SetupCards);
		ServerConnection.Instance.ExecutePHP("GetBoosters.php",SetupBooster);
		
		foreach (var t in pickedCardsGO)
		{
			Destroy(t);
		}
		pickedCardsGO = new List<GameObject>();
		pickedItems = new List<PickedItem>();
	}

	private void SetupCards(string text)
	{
		foreach (var t in cardsListGO)
		{
			Destroy(t);
		}
		cardsListGO = new List<GameObject>();
		cardsList = new List<CardTable>();
		cardsList = JsonExtension.getJsonArray<CardTable>(text).ToList();
		foreach (var card in cardsList)
		{
			GameObject go = Instantiate(cardPrefab, cardsContent);
			cardsListGO.Add(go);
			go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = card.CardName;
			PickedItem item = new PickedItem(card,1);
			go.transform.Find("BtnPick").GetComponent<Button>().onClick.AddListener(() => PickItem(item));
		}
	}
	private void SetupBooster(string text)
	{
		foreach (var t in boostersListGO)
		{
			Destroy(t);
		}
		boostersListGO = new List<GameObject>();
		boostersList = new List<BoostersTable>();
		boostersList = JsonExtension.getJsonArray<BoostersTable>(text).ToList();
		foreach (var booster in boostersList)
		{
			GameObject go = Instantiate(boosterPrefab, boostersContent);
			boostersListGO.Add(go);
			go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = booster.BoosterName;
			PickedItem item = new PickedItem(booster,1);
			go.transform.Find("BtnPick").GetComponent<Button>().onClick.AddListener(() => PickItem(item));
		}
	}

	private void PickItem(PickedItem item)
	{
		if (item.booster==null)
		{
			int index = cardsList.IndexOf(item.card);
		
			cardsList.RemoveAt(index);
			Destroy(cardsListGO[index]);
			cardsListGO.RemoveAt(index);

		
			
			var exists = pickedItems.Where(b=>b.booster==null).ToList().Exists(d => d.card.CardID == item.card.CardID);
			if (!exists)
			{
				GameObject go = Instantiate(pickedPrefab, pickedContent);
		
				pickedItems.Add(item);
				pickedCardsGO.Add(go);
			
				go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = item.card.CardName;
				go.transform.Find("BtnRemove").GetComponent<Button>().onClick.AddListener(() => RemoveItem(item));
				go.transform.Find("amount").GetComponent<TextMeshProUGUI>().text = item.amount.ToString();
				go.transform.Find("IFAmount").GetComponent<TMP_InputField>().onValueChanged.AddListener((value) =>
				{
					if (value.Length>0)
					{
						item.amount =Parse(value);
					}
					else
					{
						item.amount = 1;
					}
					
				});
			}/*
			else
			{
				var i = pickedItems.Where(b=>b.booster==null).ToList().FindIndex(_=> _.card.CardID== item.card.CardID);
				pickedItems[i].amount += item.amount;
				pickedCardsGO[i].transform.Find("amount").GetComponent<TextMeshProUGUI>().text = pickedItems[i].amount.ToString();
			}
			*/
		
			
			
		}else 
		if (item.card==null)
		{
			int index = boostersList.IndexOf(item.booster);
		
			boostersList.RemoveAt(index);
			Destroy(boostersListGO[index]);
			boostersListGO.RemoveAt(index);

		
			var exists = pickedItems.Where(b=>b.card==null).ToList().Exists(d => d.booster.BoosterID == item.booster.BoosterID);
			if (!exists)
			{
				GameObject go = Instantiate(pickedPrefab, pickedContent);
		
				pickedItems.Add(item);
				pickedCardsGO.Add(go);
			
				go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = item.booster.BoosterName;
				go.transform.Find("BtnRemove").GetComponent<Button>().onClick.AddListener(() => RemoveItem(item));
				go.transform.Find("amount").GetComponent<TextMeshProUGUI>().text = item.amount.ToString();
				go.transform.Find("IFAmount").GetComponent<TMP_InputField>().onValueChanged.AddListener((value) =>
				{
					if (value.Length>0)
					{
						item.amount =Parse(value);
					}
					else
					{
						item.amount = 1;
					}
					
				});
			}
			/*
			else
			{
				var i = pickedItems.Where(b=>b.card==null).ToList().FindIndex(_=> _.booster.BoosterID== item.booster.BoosterID);
				pickedItems[i].amount += item.amount;
				pickedCardsGO[i].transform.Find("amount").GetComponent<TextMeshProUGUI>().text = pickedItems[i].amount.ToString();
			}
			*/
		}

		CheckNumberPicked();
	}

	private void RemoveItem(PickedItem it)
	{
		int i = pickedItems.IndexOf(it);
		
		pickedItems.RemoveAt(i);
		Destroy(pickedCardsGO[i]);
		pickedCardsGO.RemoveAt(i);
		
		if (it.booster==null)
		{
			GameObject go = Instantiate(cardPrefab, cardsContent);
			cardsList.Add(it.card);
			cardsListGO.Add(go);
			go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = it.card.CardName;
			PickedItem item = new PickedItem(it.card,1);
			go.transform.Find("BtnPick").GetComponent<Button>().onClick.AddListener(() => PickItem(item));
		}else 
		if (it.card==null)
		{
			GameObject go = Instantiate(boosterPrefab, boostersContent);
			boostersList.Add(it.booster);
			boostersListGO.Add(go);
			go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = it.booster.BoosterName;
			PickedItem item = new PickedItem(it.booster,1);
			go.transform.Find("BtnPick").GetComponent<Button>().onClick.AddListener(() => PickItem(item));
		}
		CheckNumberPicked();
	}

	private void CheckNumberPicked()
	{
		if (pickedItems.Count>0)
		{
			btnConfirm.interactable = true;
		}
		else
		{
			btnConfirm.interactable = false;
		}
	}

	public void Confirm()
	{
		sendMenu.ReceiveItems(pickedItems);
		sendMenu.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}
}
	
}

