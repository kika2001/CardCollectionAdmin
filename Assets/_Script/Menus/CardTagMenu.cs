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

public class CardTagMenu : Menu
{
    [SerializeField] private GameObject cardTagItemPrefab;
    [SerializeField] private CardTagTable[] cardTags;
    [SerializeField] private Transform content;

    [SerializeField]private List<GameObject> cardTagsItemsGO = new List<GameObject>();

    [SerializeField] private CardTagEdit cardTagEdit;
    [SerializeField] private CardTagCreate cardTagCreate;
    private void Awake()
    {
        cardTagEdit.OnEdit.AddListener(()=>RefreshTable());
        cardTagCreate.OnSubmit.AddListener(()=>RefreshTable());
    }

    public override void OnEnable()
    {
        base.OnEnable();
        RefreshTable();
    }

    private void SpawnCardTags(string text)
    {
        cardTags = JsonExtension.getJsonArray<CardTagTable>(text);
        Debug.Log(cardTags.Length);
        foreach (var cardTag in cardTags)
        {
            GameObject go = Instantiate(cardTagItemPrefab, content);
            cardTagsItemsGO.Add(go);
            go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = cardTag.CardTagName;
            go.transform.Find("BtnEdit").GetComponent<Button>().onClick.AddListener(() => EditCardTag(cardTag));
            /*
            var buttonMenu = go.transform.Find("BtnEdit").GetComponent<ButtonMenu>();
            buttonMenu.closeGO = this;
            buttonMenu.openGO = rankEdit;
            */
            go.transform.Find("BtnDelete").GetComponent<Button>().onClick.AddListener(() => DeleteCardTag(cardTag));
        }
    }

    private void EditCardTag(CardTagTable tag)
    {
        if (cardTagCreate.gameObject.activeSelf)
        {
            cardTagCreate.Open();
        }
        cardTagEdit.SetRank(tag);
    }

    public void CreateTag()
    {
        if (cardTagEdit.gameObject.activeSelf)
        {
            cardTagEdit.SetRank(null);
        }
        cardTagCreate.Open();
    }

    public void DeleteCardTag(CardTagTable tag)
    {
        ServerConnection.Instance.ExecutePHP("DeleteCardTag.php",$"cardTagId={tag.CardTagID}",RefreshTable);
    }

    private void RefreshTable(string text="")
    {
        //Deletes for now, but should be a pool for performance
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        cardTagsItemsGO = new List<GameObject>();
        ServerConnection.Instance.ExecutePHP("GetCardTags.php", SpawnCardTags);
    }
}
