using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChessCrush_SocketServer
{
    public class Game
    {
        private List<string> waitingUserNameList;

        public Game()
        {
            waitingUserNameList = new List<string>();
        }

        public void ParticipateGame(string userName)
        {
            waitingUserNameList.Add(userName);

            var beforeTime = DateTime.Now;
            while (DateTime.Now - beforeTime <= new TimeSpan(0, 0, 10)) 
            {
                if (waitingUserNameList.Count >= 2)
                {
                    var newGameRoom = new GameRoom(waitingUserNameList[0], waitingUserNameList[1]);
                    waitingUserNameList.RemoveAt(0);
                    waitingUserNameList.RemoveAt(1);
                    return;
                }
                Task.Delay(1000);
            }

            var oms = new OutputMemoryStream();
            oms.Write(false);
            ServerMain.socketsByUserName[userName].Send(oms.buffer);
        }
    }
}
