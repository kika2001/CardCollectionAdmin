using System.Collections;
using System.Collections.Generic;
using _Script;
using _Script.Communication;
using _Script.MenuManager;
using _Script.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RankCreate : Menu
{
    [SerializeField] private TMP_InputField IfRankName;
    public UnityEvent OnSubmit;

    public void Open()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            IfRankName.text = "";
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void CreateRank()
    {
        ServerConnection.Instance.ExecutePHP("CreateRank.php",$"rankName={IfRankName.text}", Create);
    }

    private void Create(string text)
    {
        ConsoleLog.UpdateLog(text);
        OnSubmit.Invoke();
        gameObject.SetActive(false);
    }
}
