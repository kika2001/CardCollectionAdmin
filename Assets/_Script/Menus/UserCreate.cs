using System;
using System.Collections.Generic;
using _Script.Communication;
using _Script.Extensions;
using _Script.MenuManager;
using _Script.Tables;
using TMPro;
using UnityEngine;

namespace _Script.Menus
{
   public class UserCreate : Menu
   {
      [SerializeField] private TMP_InputField IFName;
      [SerializeField] private TMP_Dropdown rankDropDown;
      private RankTable[] ranks;


      [Header("Filter")] [SerializeField] private FilterUserTags filter;
   
      private const string DefaultInitial ="FMQ_";
      private const string DefaultFinal = "";

      public override void OnEnable()
      {
         base.OnEnable();
         
         ServerConnection.Instance.ExecutePHP("GetRanks.php", GetRanks);
      }

      private void GetRanks(string text)
      {
         //Debug.Log(text);
         rankDropDown.ClearOptions();
         ranks = JsonExtension.getJsonArray<RankTable>(text);
         List<string> rankNames = new List<string>(); 
         foreach (var rarity in ranks)
         {
            rankNames.Add(rarity.RankName);
         }
         rankDropDown.AddOptions(rankNames);
         filter.Refresh();
      }
      public void Create()
      {
         //NomeTeste - Ricardo Ferreira Teixeira
         //Userdefault - userRicardo2012
         //PassDefault - passRicardo2012
         //Nick- FMQ_RicardoTeixeira
         //FullName - Ricardo Ferreira Teixeira
      
         string[] nameParse = IFName.text.Split();
         if (nameParse.Length<2)
         {
            ConsoleLog.UpdateLog("0 | Please enter full name.");
         }
         else
         {
            string user = $"user{nameParse[0]}{DateTime.Today.Day}{DateTime.Today.Month}";
            string pass = $"pass{nameParse[0]}{DateTime.Today.Day}{DateTime.Today.Month}";
            string nick = $"{DefaultInitial + nameParse[0] + nameParse[nameParse.Length - 1] + DefaultFinal}";
            int rank = ranks[rankDropDown.value].RankID;

            string tags = "";
            var tagsSelected=filter.CheckedValues();
            if (tagsSelected.Count>0)
            {
               tags = $"{tagsSelected[0].UserTagID}";
               for (int i = 1; i < tagsSelected.Count; i++)
               {
                  tags += $",{tagsSelected[i].UserTagID}";
               }
            }
            
            ServerConnection.Instance.ExecutePHP("CreateUser.php",$"user={user}&pass={pass}&nick={nick}&name={IFName.text} &rank={rank}&link=&tags={tags} ",Result);
         }
      
      }

      private void Result(string text)
      {
         //Debug.Log(text);
         ConsoleLog.UpdateLog(text);
      }

      
   }
}
