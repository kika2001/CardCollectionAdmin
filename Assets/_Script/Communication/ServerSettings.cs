using UnityEngine;

namespace _Script.Communication
{
    [CreateAssetMenu(fileName = "ServerSettings", menuName = "CardCollection/ServerSettings", order = 1)]
    public class ServerSettings : ScriptableObject
    {
        public string servername = "localhost";
        public string username = "username";
        public string password = "password";
        public string databaseName = "databaseName";
        public string url = "URL of the card collection folder";

        public string LoginInfo
        {
            get
            {
                return $"db_server={servername}&db_user={username}&db_pass={password}&db_name={databaseName}";
            }
        }
    }
}
