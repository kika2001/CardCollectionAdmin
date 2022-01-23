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

public class UserTagMenu : Menu
{
    [SerializeField] private GameObject userTagItemPrefab;
    [SerializeField] private UserTagTable[] userTags;
    [SerializeField] private Transform content;

    [SerializeField]private List<GameObject> userTagsItemsGO = new List<GameObject>();

    [SerializeField] private UserTagEdit userTagEdit;
    [SerializeField] private UserTagCreate userTagCreate;
    private void Awake()
    {
        userTagEdit.OnEdit.AddListener(()=>RefreshTable());
        userTagCreate.OnSubmit.AddListener(()=>RefreshTable());
    }

    public override void OnEnable()
    {
        base.OnEnable();
        RefreshTable();
    }

    private void SpawnUserTags(string text)
    {
        userTags = JsonExtension.getJsonArray<UserTagTable>(text);
        Debug.Log(userTags.Length);
        foreach (var userTag in userTags)
        {
            GameObject go = Instantiate(userTagItemPrefab, content);
            userTagsItemsGO.Add(go);
            go.transform.Find("name").GetComponent<TextMeshProUGUI>().text = userTag.UserTagName;
            go.transform.Find("BtnEdit").GetComponent<Button>().onClick.AddListener(() => EditUserTag(userTag));
            /*
            var buttonMenu = go.transform.Find("BtnEdit").GetComponent<ButtonMenu>();
            buttonMenu.closeGO = this;
            buttonMenu.openGO = rankEdit;
            */
            go.transform.Find("BtnDelete").GetComponent<Button>().onClick.AddListener(() => DeleteUserTag(userTag));
        }
    }

    private void EditUserTag(UserTagTable tag)
    {
        if (userTagCreate.gameObject.activeSelf)
        {
            userTagCreate.Open();
        }
        userTagEdit.SetRank(tag);
    }

    public void CreateTag()
    {
        if (userTagEdit.gameObject.activeSelf)
        {
            userTagEdit.SetRank(null);
        }
        userTagCreate.Open();
    }

    public void DeleteUserTag(UserTagTable tag)
    {
        ServerConnection.Instance.ExecutePHP("DeleteUserTag.php",$"userTagId={tag.UserTagID}",RefreshTable);
    }

    private void RefreshTable(string text="")
    {
        //Deletes for now, but should be a pool for performance
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        userTagsItemsGO = new List<GameObject>();
        ServerConnection.Instance.ExecutePHP("GetUserTags.php", SpawnUserTags);
    }
}
