using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using UnityEngine;
using Client;
using ServerCore;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _instance = null;
    public static NetworkManager Instance { get { return _instance; } }

    ServerSession _session = new ServerSession();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = ipHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        Connector connector = new Connector();

        connector.Connect(endPoint, () => { return _session; }, 1);
    }

    private void Update()
    {
        List<IPacket> list = PacketQueue.Instance.PopAll();
        
        foreach (IPacket packet in list)
        {
            PacketManager.Instance.HandlerPacket(_session, packet);
        }
    }

    public void Send(ArraySegment<byte> sendBuff)
    {
        _session.Send(sendBuff);
    }
}
