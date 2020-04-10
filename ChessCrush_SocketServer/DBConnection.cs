using MySql.Data.MySqlClient;

namespace ChessCrush_SocketServer
{
    public class DBConnection
    {
        private int dbPort;
        private string uid;
        private string password;
        private string databaseName;
        public MySqlConnection sqlConnection { get; private set; }

        public DBConnection()
        {
            string conStr = $"server={"127.0.0.1"};port={dbPort.ToString()};uid={uid};pwd={password};database={databaseName};charset=utf8 ;";
            sqlConnection = new MySqlConnection(conStr);
        }
    }
}