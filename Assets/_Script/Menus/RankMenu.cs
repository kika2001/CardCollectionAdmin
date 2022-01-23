using System;
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

public class RankMenu : Menu
{
    [SerializeField] private GameObject rankItemPrefab;
    [SerializeField] private RankTable[] ranks;
    [SerializeField] private Transform content;

    [SerializeField]private List<GameObject> ranksItemsGO = new List<GameObject>();

    [SerializeField] private RankEdit rankEdit;
    [SerializeField] private RankCreate rankCreate;
    private void Awake()
    {
        rankEdit.OnEdit.AddListener(()=>RefreshTable());
        rankCreate.OnSubmit.AddListener(()=>RefreshTable());
    }

    public override void OnEnable()
    {
        base.OnEnable();
        RefreshTable();
    }

    private void SpawnRanks(string text)
    {
        ranks = JsonExtension.getJsonArray<RankTable>(text);
        Debug.Log(ranks.Length);
        foreach (var rank in ranks)
        {
            GameObject go = Instantiate(rankItemPrefab, content);
            ranksItemsGO.Add(go);
            go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = rank.RankName;
            go.transform.Find("BtnEdit").GetComponent<Button>().onClick.AddListener(() => EditRank(rank));
            /*
            var buttonMenu = go.transform.Find("BtnEdit").GetComponent<ButtonMenu>();
            buttonMenu.closeGO = this;
            buttonMenu.openGO = rankEdit;
            */
            go.transform.Find("BtnDelete").GetComponent<Button>().onClick.AddListener(() => DeleteRank(rank));
        }
    }

    private void EditRank(RankTable rank)
    {
        if (rankCreate.gameObject.activeSelf)
        {
            rankCreate.Open();
        }
        rankEdit.SetRank(rank);
    }

    public void CreateRank()
    {
        if (rankEdit.gameObject.activeSelf)
        {
            rankEdit.SetRank(null);
        }
        rankCreate.Open();
    }

    public void DeleteRank(RankTable rank)
    {
        ServerConnection.Instance.ExecutePHP("DeleteRank.php",$"rankId={rank.RankID}",RefreshTable);
    }

    private void RefreshTable(string text="")
    {
        //Deletes for now, but should be a pool for performance
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        ranksItemsGO = new List<GameObject>();
        ServerConnection.Instance.ExecutePHP("GetRanks.php", SpawnRanks);
    }
}
