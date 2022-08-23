using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace Server
{
    class ClientSession : PacketSession
    {
        public int SessionId { get; set; }
        public GameRoom Room { get; set; }
        public Player Player { get; set; }

        public override void OnConneteced(EndPoint endPoint)
        {
            Console.WriteLine($"OnConneteced : {endPoint}");
            Program.Room.Push(() => Program.Room.EnterSession(this));
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            Console.WriteLine("뭔가 받음");
            PacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            SessionManager.Instance.Remove(this);
            if (Room != null)
            {
                GameRoom room = Room;
                room.Push(() => room.Leave(this));
                Room = null;
            }
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnSend(int numOfBytes)
        {
            //Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }
}
