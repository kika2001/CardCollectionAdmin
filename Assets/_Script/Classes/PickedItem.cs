using System;
using _Script.Tables;

namespace _Script.Classes
{
    [Serializable]
    public class PickedItem
    {
        public CardTable card { get; set; }
        public BoostersTable booster{ get; set; }
        public ItemType itemType { get; set; }
        public int amount;

        public PickedItem(CardTable c, int a)
        {
            itemType = ItemType.Card;
            card = c;
            booster = null;
            amount = a;
			
        }

        public PickedItem(BoostersTable b, int a)
        {
            itemType = ItemType.Booster;
            card = null;
            booster = b;
            amount = a;
        }
    }

    public enum ItemType
    {
        Card,
        Booster
    }
}