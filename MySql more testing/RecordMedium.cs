using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql_more_testing
{
    public class RecordMedium
    {
        public string _serverName { get; set; }
        public string _port { get; set; }
        public string _UID { get; set; }
        public string _password { get; set; }
        public string _database { get; set; }

        public RecordMedium(string serverName, string port, string uID, string password, string database)
        {
            _serverName = serverName;
            _port = port;
            _UID = uID;
            _password = password;
            _database = database;
        }
    }

    public class RecordMenuMedium
    {
        public string _menuID { get; set; }
        public string _menuName { get; set; }
        public string _menuPrice { get; set; }

        public RecordMenuMedium(string menuID, string menuName, string menuPrice)
        {
            _menuID = menuID;
            _menuName = menuName;
            _menuPrice = menuPrice;
        }
    }

    public class SellRecording
    {
        public string MenuCode { get; set; }
        public string UserCode { get; set; }
        public int SellId { get; set; }
        public string Amount { get; set; }
        public string Price { get; set; }
        public SellRecording(string menuCode, string userCode, string amount)
        {
            MenuCode = menuCode;
            UserCode = userCode;
            Amount = amount;
        }

        public SellRecording(int sellId)
        {
            SellId = sellId;
        }
        public SellRecording(string price)
        {
            Price = price;
        }
    }

}
