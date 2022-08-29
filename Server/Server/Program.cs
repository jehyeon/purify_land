using System;
using System.Net;
using System.Threading;
using ServerCore;

namespace Server
{
    class Program
    {
        //static Listener _listener = new Listener();
        public static GameRoom Room = new GameRoom();

        //static void FlushRoom()
        //{
        //    Room.Push(() => Room.Flush());
        //    JobTimer.Instance.Push(FlushRoom, 250);
        //}

        static void Main(string[] args)
        {
            SQLiteManager sqliteManager = new SQLiteManager();
            sqliteManager.TryGetPlayer("sd");
            //string host = Dns.GetHostName();
            //IPHostEntry ipHost = Dns.GetHostEntry(host);
            //IPAddress ipAddr = ipHost.AddressList[0];
            //IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            //_listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });

            //FlushRoom();

            //while (true)
            //{
            //    JobTimer.Instance.Flush();
            //}
        }
    }
}
