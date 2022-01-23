using System;
using UnityEngine;
namespace _Script.Tables
{
    [Serializable]
    public class CardTable
    {
        [SerializeField] public int CardID ;
        [SerializeField] public string CardName;
        [SerializeField] public int RarityID;
        [SerializeField] public string CardTags;
        [SerializeField] public int CardSeasonID;

    }
    [Serializable]
    public class InventoryCardsTable
    {
        [SerializeField] public int ID;
        [SerializeField] public int PlayerID;
        [SerializeField] public int CardID;
        [SerializeField] public string DateCreated;
    }
    [Serializable]
    public class InventoryBoostersTable
    {
        [SerializeField] public int ID;
        [SerializeField] public int PlayerID;
        [SerializeField] public int BoosterID;
        [SerializeField] public string DateCreated;
    }
    
    
    [Serializable]
    public class UserTable
    {
        [SerializeField] public int UserID;
        [SerializeField] public string UserUsername;
        [SerializeField] public string UserPass;
        [SerializeField] public string UserNick;
        [SerializeField] public string UserName;
        [SerializeField] public int UserRank;
        [SerializeField] public string UserLink;
        [SerializeField] public string UserTags;
    }
    [Serializable]
    public class RankTable
    {
        [SerializeField] public int RankID;
        [SerializeField] public string RankName;
    }

    [Serializable]
    public class UserTagTable
    {
        [SerializeField] public int UserTagID;
        [SerializeField] public string UserTagName;
    }
    [Serializable]
    public class CardTagTable
    {
        [SerializeField] public int CardTagID;
        [SerializeField] public string CardTagName;
    }
    [Serializable]
    public class CardSeasonTable
    {
        [SerializeField] public int CardSeasonID;
        [SerializeField] public string CardSeasonName;
    }
    
    [Serializable]
    public class RarityTable
    {
        [SerializeField] public int RarityID;
        [SerializeField] public string RarityName;
        [SerializeField] public float RarityPercentage;
    }

    [Serializable]
    public class AdminsTable
    {
        [SerializeField] public int AdminID;
        [SerializeField] public string AdminUser;
        [SerializeField] public string AdminPass;
        [SerializeField] public string AdminNick;
    }

    [Serializable]
    public class BoostersTable
    {
        [SerializeField] public int BoosterID;
        [SerializeField] public string BoosterName;
        [SerializeField] public string BoosterDescription;
        [SerializeField] public string BoosterCards;
        [SerializeField] public string BoosterImage;
        [SerializeField] public string BoosterTags;
    }

    [Serializable]
    public class TicketTable
    {
        [SerializeField] public int TicketID;
        [SerializeField] public int PlayerID;
        [SerializeField] public string TicketTitle;
        [SerializeField] public string TicketDescription;
        [SerializeField] public int TicketState;
    }
}