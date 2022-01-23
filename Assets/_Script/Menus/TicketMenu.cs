using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Script.Communication;
using _Script.Extensions;
using _Script.MenuManager;
using _Script.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TicketMenu : Menu
{
    [SerializeField] private List<TicketTable> tickets;
    private List<GameObject> ticketGO = new List<GameObject>();
    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject ticketTemplate;

    [Header("Menus")] 
    [SerializeField] private TicketViewMenu viewMenu;
    [SerializeField] private Menu displayMenu;
    
    
    public override void OnEnable()
    {
        base.OnEnable();
        RefreshTable("");
    }
    private void GetTickets(string text)
    {
        //Debug.Log(text);
        tickets = JsonExtension.getJsonArray<TicketTable>(text).ToList();
        SpawnTickets();
    }

    private void SpawnTickets()
    {
        for (int i = 0; i < tickets.Count; i++)
        {
            GameObject go = Instantiate(ticketTemplate, contentTransform);
            ticketGO.Add(go);
            go.transform.Find("userId").GetComponent<TextMeshProUGUI>().text = tickets[i].PlayerID.ToString();
            go.transform.Find("subject").GetComponent<TextMeshProUGUI>().text = tickets[i].TicketTitle;

            Transform state = go.transform.Find("state");
            
            Toggle todo = state.Find("todo").GetComponent<Toggle>();
            Toggle doing = state.Find("doing").GetComponent<Toggle>();
            Toggle done = state.Find("done").GetComponent<Toggle>();
            todo.onValueChanged.RemoveAllListeners();
            doing.onValueChanged.RemoveAllListeners();
            done.onValueChanged.RemoveAllListeners();
            
            todo.isOn = false;
            doing.isOn = false;
            done.isOn = false;
            switch (tickets[i].TicketState)
            {
                case -1: //To Do
                    todo.isOn = true;
                    break;
                case 0: //Doing
                    doing.isOn = true;
                    break;
                case 1:
                    done.isOn = true;
                    break;
            }
            
            todo.onValueChanged.AddListener((b)=>ChangeState(b,-1,i));
            doing.onValueChanged.AddListener((b)=>ChangeState(b,0,i));
            done.onValueChanged.AddListener((b)=>ChangeState(b,1,i));
            go.GetComponent<Button>().onClick.AddListener(()=>
            {
                Debug.Log($"Criou Ticket. ID na lista {i}");
                ViewTicket(i);
            });
        }
    }

    private void ViewTicket(int id)
    {
        displayMenu.gameObject.SetActive(false);
        Debug.Log($"ID lista: {id}");
        viewMenu.SetTicket(tickets[id]);
        viewMenu.gameObject.SetActive(true);
        
    }

    private void ChangeState(bool b,int state,int id)
    {
        if (b)
        {
            tickets[id].TicketState = state;
            ServerConnection.Instance.ExecutePHP("EditTicket.php",$"state={state}&ticketId={tickets[id].TicketID}", RefreshTable);
        }
    }

    public void RefreshTable(string text)
    {
        //Deletes for now, but should be a pool for performance
        for (int i = 0; i < contentTransform.childCount; i++)
        {
            Destroy(contentTransform.GetChild(i).gameObject);
            //Debug.LogWarning("Destroyed ticket");
        }
        
        ticketGO = new List<GameObject>();
            
        ServerConnection.Instance.ExecutePHP("GetTickets.php", GetTickets);
    }
}
