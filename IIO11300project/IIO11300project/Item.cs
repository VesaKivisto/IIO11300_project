using System;

namespace IIO11300project
{
    // A class for displaying item data.
    // All needed item data is stored in database and fetched from there with GetItemData function defined ein DBHandler class.
    // Item IDs are some of the data returned with match history and match detail requests and are used to fetch the rest of the data from the database.
    public class Item
    {
        public string ID { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Descr { get; set; }
        public string ToolTip
        {
            get
            {
                if (!String.IsNullOrEmpty(Value))
                {
                    return Name + "\n" + Value + " Gold\n" + Descr;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}