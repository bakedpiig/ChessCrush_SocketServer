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
            
            if(waitingUserNameList.Count>=2)
            {
                var newGameRoom = new GameRoom(waitingUserNameList[0], waitingUserNameList[1]);
            }
        }
    }
}
