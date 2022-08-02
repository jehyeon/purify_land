using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerManager
{
    MyPlayer _myPlayer;
    Dictionary<int, Player> _players = new Dictionary<int, Player>();

    public static NetworkPlayerManager Instance { get; } = new NetworkPlayerManager();

    // -------------------------------------------------------------------------
    // 서버 접속, 나가기
    // -------------------------------------------------------------------------
    public void Add(S_PlayerList packet)
    {
        Object obj = Resources.Load("Prefabs/Player");

        foreach (S_PlayerList.Player p in packet.players)
        {
            GameObject go = Object.Instantiate(obj) as GameObject;

            if (p.isSelf)
            {
                // 자기 자신인 경우
                MyPlayer myPlayer = go.transform.GetChild(0).gameObject.AddComponent<MyPlayer>();
                myPlayer.PlayerId = p.playerId;
                myPlayer.transform.position = Vector2.zero;
                _myPlayer = myPlayer;
            }
            else
            {
                Player player = go.transform.GetChild(0).gameObject.AddComponent<Player>();
                player.PlayerId = p.playerId;
                _players.Add(p.playerId, player);
                player.transform.position = new Vector2(p.posX, p.posY);
            }
        }
    }

    public void EnterGame(S_BroadcastEnterGame packet)
    {
        if (packet.playerId == _myPlayer.PlayerId)
        {
            return;
        }
        
        Object obj = Resources.Load("Prefabs/Player");
        GameObject go = Object.Instantiate(obj) as GameObject;

        Player player = go.transform.GetChild(0).gameObject.AddComponent<Player>();
        _players.Add(packet.playerId, player);
        player.transform.position = Vector2.zero;
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
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                GameObject.Destroy(player.gameObject);
                _players.Remove(packet.playerId);
            }
        }
    }

    // -------------------------------------------------------------------------
    // 캐릭터 이동
    // -------------------------------------------------------------------------    
    public void Move(S_BroadcastMove packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
        {
            _myPlayer.DestinationPos = new Vector2(packet.posX, packet.posY);
        }
        else
        {
            // 다른 유저의 이동 패킷을 받은 경우
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                // Vector3 nextPlayerPos = new Vector3(packet.posX, packet.posY, 0f);
                // Vector3 vectDiff = nextPlayerPos - player.transform.position;

                player.DestinationPos = new Vector2(packet.posX, packet.posY);
            }
        }
    }

    // -------------------------------------------------------------------------
    // 캐릭터 애니메이션 재생
    // -------------------------------------------------------------------------
    public void Act(S_BroadcastAct packet)
    {
        if (_myPlayer.PlayerId != packet.playerId)
        {
            // 다른 유저의 이동 패킷을 받은 경우
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                player.ActAnimation(packet.actionType);
            }
        }
    }
}
