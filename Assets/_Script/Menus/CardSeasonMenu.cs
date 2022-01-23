using System.Collections;
using System.Collections.Generic;
using _Script.Communication;
using _Script.Extensions;
using _Script.MenuManager;
using _Script.Menus;
using _Script.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSeasonMenu : Menu
{
    [SerializeField] private GameObject cardSeasonItemPrefab;
    [SerializeField] private CardSeasonTable[] cardSeasons;
    [SerializeField] private Transform content;

    [SerializeField]private List<GameObject> cardSeasonItemsGO = new List<GameObject>();

    [SerializeField] private CardSeasonEdit cardSeasonEdit;
    [SerializeField] private CardSeasonCreate cardSeasonCreate;
    private void Awake()
    {
        cardSeasonEdit.OnEdit.AddListener(()=>RefreshTable());
        cardSeasonCreate.OnSubmit.AddListener(()=>RefreshTable());
    }

    public override void OnEnable()
    {
        base.OnEnable();
        RefreshTable();
    }

    private void SpawnCardSeason(string text)
    {
        cardSeasons = JsonExtension.getJsonArray<CardSeasonTable>(text);
        //Debug.Log(cardSeasons.Length);
        foreach (var cardSeason in cardSeasons)
        {
            GameObject go = Instantiate(cardSeasonItemPrefab, content);
            cardSeasonItemsGO.Add(go);
            go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = cardSeason.CardSeasonName;
            go.transform.Find("BtnEdit").GetComponent<Button>().onClick.AddListener(() => EditCardSeason(cardSeason));
            /*
            var buttonMenu = go.transform.Find("BtnEdit").GetComponent<ButtonMenu>();
            buttonMenu.closeGO = this;
            buttonMenu.openGO = rankEdit;
            */
            go.transform.Find("BtnDelete").GetComponent<Button>().onClick.AddListener(() => DeleteCardSeason(cardSeason));
        }
    }

    private void EditCardSeason(CardSeasonTable tag)
    {
        if (cardSeasonCreate.gameObject.activeSelf)
        {
            cardSeasonCreate.Open();
        }
        cardSeasonEdit.SetSeason(tag);
    }

    public void CreateSeason()
    {
        if (cardSeasonEdit.gameObject.activeSelf)
        {
            cardSeasonEdit.SetSeason(null);
        }
        cardSeasonCreate.Open();
    }

    public void DeleteCardSeason(CardSeasonTable tag)
    {
        ServerConnection.Instance.ExecutePHP("DeleteCardSeason.php",$"cardSeasonId={tag.CardSeasonID}",RefreshTable);
    }

    private void RefreshTable(string text="")
    {
        //Deletes for now, but should be a pool for performance
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        cardSeasonItemsGO = new List<GameObject>();
        ServerConnection.Instance.ExecutePHP("GetCardSeasons.php", SpawnCardSeason);
    }
}
