using System.Collections.Generic;
using System.Linq;
using _Script.Communication;
using _Script.Tables;
using UnityEngine;

namespace _Script.Extensions
{
    public class TableParser
    {
        static List<CardTable> cards = new List<CardTable>();
        private static string[] ids;
        public static List<CardTable> StringToCardID(string text)
        {
            cards = new List<CardTable>();
            ids = text.Trim(' ').Split('|');
            ServerConnection.Instance.ExecutePHP("GetCards.php",StringToCardID_ProcessResults);
            return cards;
        }

        private static void StringToCardID_ProcessResults(string text)
        {
            List<CardTable> tempCards = JsonExtension.getJsonArray<CardTable>(text).ToList();
            foreach (var temp in tempCards)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if (temp.CardID.ToString()==ids[i])
                    {
                    
                    }
                }
                
            }
        }
    }
}