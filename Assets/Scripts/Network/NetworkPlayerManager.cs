using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerManager
{
    MyPlayer _myPlayer;
    Dictionary<int, NetworkPlayer> _players = new Dictionary<int, NetworkPlayer>();

    public static NetworkPlayerManager Instance { get; } = new NetworkPlayerManager();
    
    public void Add(S_PlayerList packet)
    {
        Object obj = Resources.Load("Player");

        foreach (S_PlayerList.Player p in packet.players)
        {
            GameObject go = Object.Instantiate(obj) as GameObject;

            if (p.isSelf)
            {
                MyPlayer myPlayer = go.AddComponent<MyPlayer>();
                myPlayer.PlayerId = p.playerId;
                myPlayer.transform.position = new Vector3(p.posX, p.posY, 0f);
                _myPlayer = myPlayer;
            }
            else
            {
                NetworkPlayer player = go.AddComponent<NetworkPlayer>();
                player.PlayerId = p.playerId;
                _players.Add(p.playerId, player);
                player.transform.position = new Vector3(p.posX, p.posY, 0f);
            }
        }
    }

    public void Move(S_BroadcastMove packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
        {
            // _myPlayer.transform.position = new Vector3(packet.posX, packet.posY, 0f);
        }
        else
        {
            NetworkPlayer player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                player.transform.position = new Vector3(packet.posX, packet.posY, 0f);
            }
        }
    }

    public void EnterGame(S_BroadcastEnterGame packet)
    {
        if (packet.playerId == _myPlayer.PlayerId)
        {
            return;
        }
        
        Object obj = Resources.Load("Player");
        GameObject go = Object.Instantiate(obj) as GameObject;

        NetworkPlayer player = go.AddComponent<NetworkPlayer>();
        _players.Add(packet.playerId, player);
        player.transform.position = new Vector3(packet.posX, packet.posY, 0f);
    }

    public void LeaveGame(S_BroadcastLeaveGame packet)
    {
        if (packet.playerId == _myPlayer.PlayerId)
        {
            GameObject.Destroy(_myPlayer.gameObject);
            _myPlayer = null;
        }
        else
        {
            NetworkPlayer player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                GameObject.Destroy(player.gameObject);
                _players.Remove(packet.playerId);
            }
        }
    }
}
