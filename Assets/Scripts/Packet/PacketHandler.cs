using Client;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

class PacketHandler
{
    public static void S_BroadcastEnterGameHandler (PacketSession session, IPacket packet)
    {
        S_BroadcastEnterGame pkt = packet as S_BroadcastEnterGame;
        ServerSession serverSession = session as ServerSession;

        NetworkPlayerManager.Instance.EnterGame(pkt);
    }

    public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastLeaveGame pkt = packet as S_BroadcastLeaveGame;
        ServerSession serverSession = session as ServerSession;

        NetworkPlayerManager.Instance.LeaveGame(pkt);
    }

    public static void S_PlayerListHandler(PacketSession session, IPacket packet)
    {
        S_PlayerList pkt = packet as S_PlayerList;
        ServerSession serverSession = session as ServerSession;

        NetworkPlayerManager.Instance.SyncPlayerList(pkt);
    }

    public static void S_BroadcastMoveHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastMove pkt = packet as S_BroadcastMove;
        ServerSession serverSession = session as ServerSession;
        
        NetworkPlayerManager.Instance.Move(pkt);
    }
    
    public static void S_BroadcastActHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastAct pkt = packet as S_BroadcastAct;
        ServerSession serverSession = session as ServerSession;
        
        NetworkPlayerManager.Instance.Act(pkt);
    }

    public static void S_BroadcastPlayerHpHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastPlayerHp pkt = packet as S_BroadcastPlayerHp;
        ServerSession serverSession = session as ServerSession;

        NetworkPlayerManager.Instance.SyncHp(pkt);
    }

    public static void S_EnemyListHandler(PacketSession session, IPacket packet)
    {
        S_EnemyList pkt = packet as S_EnemyList;
        ServerSession serverSession = session as ServerSession;

        NetworkEnemyManager.Instance.EnterGame(pkt);
    }

    public static void S_BroadcastSpawnEnemyHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastSpawnEnemy pkt = packet as S_BroadcastSpawnEnemy;
        ServerSession serverSession = session as ServerSession;

        NetworkEnemyManager.Instance.Spawn(pkt);
    }

    public static void S_BroadcastEnemyMoveHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastEnemyMove pkt = packet as S_BroadcastEnemyMove;
        ServerSession serverSession = session as ServerSession;

        NetworkEnemyManager.Instance.Move(pkt);
    }

    public static void S_BroadcastEnemyTargetHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastEnemyTarget pkt = packet as S_BroadcastEnemyTarget;
        ServerSession serverSession = session as ServerSession;

        NetworkEnemyManager.Instance.Target(pkt);
    }

    public static void S_BroadcastEnemyStateHandler(PacketSession session, IPacket packet)
    {
        S_BroadcastEnemyState pkt = packet as S_BroadcastEnemyState;
        ServerSession serverSession = session as ServerSession;

        NetworkEnemyManager.Instance.State(pkt);
    }
}
