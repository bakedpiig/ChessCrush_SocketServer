using System.Collections.Generic;

namespace ChessCrush_SocketServer
{
    public class GameRoom
    {
        private static int roomIDCount = 0;
        public int roomID;
        private List<string> players;
        public bool ended { get; private set; }

        public GameRoom(string player1, string player2)
        {
            roomID = roomIDCount++;
            players = new List<string>();
            players.Add(player1);
            players.Add(player2);

            OutputMemoryStream outputMemoryStream1 = new OutputMemoryStream();
            outputMemoryStream1.Write(roomID);
            outputMemoryStream1.Write(player2);
            ServerMain.socketsByUserName[player1].Send(outputMemoryStream1.buffer);

            OutputMemoryStream outputMemoryStream2 = new OutputMemoryStream();
            outputMemoryStream2.Write(roomID);
            outputMemoryStream2.Write(player1);
            ServerMain.socketsByUserName[player2].Send(outputMemoryStream2.buffer);
        }
    }
}
