using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerManager
{
    public static NetworkPlayerManager Instance { get; } = new NetworkPlayerManager();

    MyPlayer _myPlayer;
    private Dictionary<int, Player> _players = new Dictionary<int, Player>();
    public Dictionary<int, Player> Players { get { return _players; } }

    // -------------------------------------------------------------------------
    // 서버 접속, 나가기
    // -------------------------------------------------------------------------
    public void SyncPlayerList(S_PlayerList packet)
    {
        // 새로 접속한 유저에게 플레이어 정보 전달
        Object obj = Resources.Load("Prefabs/Player");

        foreach (S_PlayerList.Player p in packet.playerList)
        {
            GameObject go = Object.Instantiate(obj) as GameObject;

            if (p.isSelf)
            {
                // 자기 자신인 경우
                if (p.isHost)
                {
                    // 호스트인 경우
                    NetworkEnemyManager.Instance.IsHost = true;
                    Object hostManager = Resources.Load("Prefabs/HostManager");
                    GameObject goHostManager = Object.Instantiate(hostManager) as GameObject;
                }

                MyPlayer myPlayer = go.transform.GetChild(0).gameObject.AddComponent<MyPlayer>();
                myPlayer.PlayerId = p.playerId;
                myPlayer.transform.position = Vector2.zero;
                myPlayer.DestinationPos = Vector2.zero;
                myPlayer.transform.parent.gameObject.name = "MyPlayer";      // !!! temp;
                myPlayer.SyncHp(100, 100);     // !!! 처음 접속할 경우 최대 체력
                _myPlayer = myPlayer;
            }
            else
            {
                Player player = go.transform.GetChild(0).gameObject.AddComponent<Player>();
                player.PlayerId = p.playerId;
                _players.Add(p.playerId, player);
                player.transform.position = new Vector2(p.posX, p.posY);
                player.DestinationPos = new Vector2(p.posX, p.posY);
                player.SyncHp(p.hp, p.maxHp);
            }
        }
    }

    public void EnterGame(S_BroadcastEnterGame packet)
    {
        // 기존 유저에게 새로운 유저 접속 정보를 전달
        if (packet.playerId == _myPlayer.PlayerId)
        {
            // 자신의 입장 패킷은 무시
            return;
        }

        Object obj = Resources.Load("Prefabs/Player");
        GameObject go = Object.Instantiate(obj) as GameObject;

        Player player = go.transform.GetChild(0).gameObject.AddComponent<Player>();
        player.PlayerId = packet.playerId;
        _players.Add(packet.playerId, player);
        player.transform.position = new Vector2(packet.posX, packet.posY);
        player.DestinationPos = new Vector2(packet.posX, packet.posY);
        player.SyncHp(packet.hp, packet.maxHp);
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
    // 캐릭터 스탯 변경
    // -------------------------------------------------------------------------  
    public void SyncHp(S_BroadcastPlayerHp packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
        {
            _myPlayer.SyncHp(packet.hp, packet.maxHp);
        }
        else
        {
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                player.SyncHp(packet.hp, packet.maxHp);
            }
            else
            {
                Debug.Log(packet.playerId);
                Debug.Log($"{packet.hp}/{packet.maxHp}");
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
                player.Rotate(packet.right);
                player.ActAnimation(packet.actionType);
            }
        }
    }
}
