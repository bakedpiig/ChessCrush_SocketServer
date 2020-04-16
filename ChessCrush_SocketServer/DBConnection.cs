using ChessCrush_SocketServer.OperationResultCode;
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

        public DBConnection(int dbPort, string uid, string password, string databaseName)
        {
            this.dbPort = dbPort;
            this.uid = uid;
            this.password = password;
            this.databaseName = databaseName;

            string conStr = $"server={"127.0.0.1"};port={this.dbPort.ToString()};uid={this.uid};pwd={this.password};database={this.databaseName};charset=utf8 ;";
            sqlConnection = new MySqlConnection(conStr);
            sqlConnection.Open();
        }

        public SignUpCode SignUp(string newUserName, string newUserPassword)
        {
            string query = $"select * from users where user_id = \"{newUserName}\";";
            MySqlCommand comm = new MySqlCommand(query, sqlConnection);
            var reader = comm.ExecuteReader();
            var result = reader.Read();
            reader.Close();
            if (result) return SignUpCode.UsingID;

            string insertQuery = $"insert into users values (\"{newUserName}\",\"{newUserPassword}\");";
            var newComm = new MySqlCommand(insertQuery, sqlConnection);
            newComm.ExecuteNonQuery();


            return SignUpCode.Success;
        }

        public SignInCode SignIn(string signUserName, string signUserPassword)
        {
            string signInQuery = $"select * from users where user_id = \"{signUserName}\";";
            MySqlCommand comm = new MySqlCommand(signInQuery, sqlConnection);
            var reader = comm.ExecuteReader();
            var result = reader.Read();

            if (!result)
            {
                reader.Close();
                return SignInCode.MissingID;
            }
            if (reader.GetString("password") != signUserPassword)
            {
                reader.Close();
                return SignInCode.WrongPW;
            }

            reader.Close();
            return SignInCode.Success;
        }
    }
}