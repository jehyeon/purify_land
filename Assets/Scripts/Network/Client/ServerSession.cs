using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ServerCore;

namespace Client
{
    class ServerSession : PacketSession
    {
        public override void OnConneteced(EndPoint endPoint)
        {
            Console.WriteLine($"OnConneteced : {endPoint}");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.OnRecvPacket(this, buffer, (s, p) => PacketQueue.Instance.Push(p));
        }

        public override void OnSend(int numOfBytes)
        {
        }
    }
}
