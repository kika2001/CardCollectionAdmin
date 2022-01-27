using System;
using System.Collections;
using System.Collections.Generic;
using _Script.Communication;
using _Script.Extensions;
using _Script.MenuManager;
using _Script.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TicketViewMenu : Menu
{
    [SerializeField] private TextMeshProUGUI description, title, worker;
    [SerializeField] private Transform state;
    private bool init=false;
    private bool intializeSubs = false;
    private TicketTable currentTicket;
    private Toggle todo, doing, done;


    public void SetTicket(TicketTable t)
    {
        intializeSubs = false;
        Debug.Log($"Ticket Received. Ticket Player ID:{t.PlayerID}. Ticket state:{t.TicketState}");
        description.text = t.TicketDescription;
        title.text = t.TicketTitle;
        GetPlayerName(t.PlayerID);
        currentTicket = t;
        if (!init)
        {
            todo = state.Find("todo").GetComponent<Toggle>();
            doing = state.Find("doing").GetComponent<Toggle>();
            done = state.Find("done").GetComponent<Toggle>();
            init = true;
        }
        todo.isOn = false;
        doing.isOn = false;
        done.isOn = false;
        switch (currentTicket.TicketState)
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
        todo.onValueChanged.RemoveAllListeners();
        doing.onValueChanged.RemoveAllListeners();
        done.onValueChanged.RemoveAllListeners();
        
        todo.onValueChanged.AddListener((b)=>ChangeState(b,-1,currentTicket.TicketID));
        doing.onValueChanged.AddListener((b)=>ChangeState(b,0,currentTicket.TicketID));
        done.onValueChanged.AddListener((b)=>ChangeState(b,1,currentTicket.TicketID));
        intializeSubs = true;


    }

    private void ChangeState(bool b,int state,int ticketeId)
    {
        if (b && intializeSubs)
        {
            currentTicket.TicketState = state;
            ServerConnection.Instance.ExecutePHP("EditTicket.php",$"state={state}&ticketId={ticketeId}");
        }
    }
    
    private void GetPlayerName(int ID)
    {
        ServerConnection.Instance.ExecutePHP("GetUserByID.php",$"playerId={ID}",PlayerName);
    }

    private void PlayerName(string text)
    {
        worker.text = JsonUtility.FromJson<UserTable>(text).UserNick;
    }
}
