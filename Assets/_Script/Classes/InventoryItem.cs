using _Script.Tables;

namespace _Script.Classes
{
    public class InventoryItem
    {
        public InventoryCardsTable card { get; private set; }
        public InventoryBoostersTable booster{ get; private set; }
        public string dateCreated;
        
        public InventoryItem(InventoryCardsTable c)
        {
            card = c;
            booster = null;
            dateCreated = c.DateCreated;

        }

        public InventoryItem(InventoryBoostersTable b)
        {
            card = null;
            booster = b;
            dateCreated = b.DateCreated;
        }
    }
}