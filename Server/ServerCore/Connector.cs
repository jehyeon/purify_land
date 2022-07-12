using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
    public class Connector
    {
        Func<Session> _sessionFactory;

        public void Connect(IPEndPoint endPoint, Func<Session> sessionFactory)
        {
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionFactory = sessionFactory;
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += OnConnnectedCompleted;
            args.RemoteEndPoint = endPoint;
            args.UserToken = socket;

            RegisterConnect(args);
        }

        void RegisterConnect(SocketAsyncEventArgs args)
        {
            Socket socket = args.UserToken as Socket;

            if (socket == null)
            {
                return;
            }

            bool pending = socket.ConnectAsync(args);
            if (pending == false)
            {
                OnConnnectedCompleted(null, args);
            }
        }

        void OnConnnectedCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                Session session = _sessionFactory.Invoke();
                session.Start(args.ConnectSocket);
                session.OnConneteced(args.RemoteEndPoint);
            }
            else
            {
                Console.WriteLine($"OnConnectedCompleted Fail: {args.SocketError}");
            }
        }
    }
}
