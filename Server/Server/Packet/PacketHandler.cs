using Server;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
    public static void C_LeaveGameHandler(PacketSession session, IPacket packet)
    {
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
        {
            return;
        }

        Console.WriteLine($"{clientSession.SessionId}: 나감");

        GameRoom room = clientSession.Room;
        room.Push(() => room.Leave(clientSession));
    }
    public static void C_MoveHandler(PacketSession session, IPacket packet)
    {
        C_Move movePacket = packet as C_Move;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
        {
            return;
        }

        Console.WriteLine($"{clientSession.SessionId}: 이동 ({movePacket.posX}, {movePacket.posY})");

        GameRoom room = clientSession.Room;
        room.Push(() => room.Move(clientSession, movePacket));
    }

    public static void C_ActHandler(PacketSession session, IPacket packet)
    {
        C_Act actPacket = packet as C_Act;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
        {
            return;
        }

         Console.WriteLine($"{clientSession.SessionId}: 동작 {actPacket.actionType} (right: {actPacket.right}");

        GameRoom room = clientSession.Room;
        room.Push(() => room.Act(clientSession, actPacket));
    }

    public static void C_PlayerHpHandler(PacketSession session, IPacket packet)
    {
        C_PlayerHp hpPacket = packet as C_PlayerHp;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
        {
            return;
        }

         Console.WriteLine($"{hpPacket.playerId}: {hpPacket.hp}/{hpPacket.maxHp}");

        GameRoom room = clientSession.Room;
        room.Push(() => room.SyncHp(clientSession, hpPacket));
    }

    public static void C_SpawnCallEnemyHandler(PacketSession session, IPacket packet)
    {
        C_SpawnCallEnemy spawnCallPacket = packet as C_SpawnCallEnemy;
        ClientSession clientSession = session as ClientSession;
        
        if (clientSession.Room == null)
        {
            return;
        }

        Console.WriteLine($"{clientSession.SessionId}: {spawnCallPacket.enemyId} Enemy 스폰 요청");

        GameRoom room = clientSession.Room;
        room.Push(() => room.SpawnCallEnemy(clientSession, spawnCallPacket));
    }

    public static void C_EnemyMoveHandler(PacketSession session, IPacket packet)
    {
        C_EnemyMove enemyMovePacket = packet as C_EnemyMove;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
        {
            return;
        }

        Console.WriteLine($"Enemy {enemyMovePacket.id}: 이동 ({enemyMovePacket.posX}, {enemyMovePacket.posY})");

        GameRoom room = clientSession.Room;
        room.Push(() => room.EnemyMove(clientSession, enemyMovePacket));
    }
    
    public static void C_EnemyTargetHandler(PacketSession session, IPacket packet)
    {
        C_EnemyTarget enemyTargetPacket = packet as C_EnemyTarget;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
        {
            return;
        }

        Console.WriteLine($"Enemy {enemyTargetPacket.id}: 타겟 지정 ({enemyTargetPacket.playerId})");

        GameRoom room = clientSession.Room;
        room.Push(() => room.EnemyTarget(clientSession, enemyTargetPacket));
    }

    public static void C_EnemyStateHandler(PacketSession session, IPacket packet)
    {
        C_EnemyState enemyStatePacket = packet as C_EnemyState;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
        {
            return;
        }

        Console.WriteLine($"Enemy {enemyStatePacket.id}: 상태 변화 ({enemyStatePacket.state})");

        GameRoom room = clientSession.Room;
        room.Push(() => room.EnemyState(clientSession, enemyStatePacket));
    }

    public static void C_EnemyActHandler(PacketSession session, IPacket packet)
    {
        C_EnemyAct enemyActPacket = packet as C_EnemyAct;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
        {
            return;
        }

        Console.WriteLine($"Enemy {enemyActPacket.id}: Act ({enemyActPacket.actionType})");

        GameRoom room = clientSession.Room;
        room.Push(() => room.EnemyAct(clientSession, enemyActPacket));
    }

    public static void C_EnemyHpHandler(PacketSession session, IPacket packet)
    {
        C_EnemyHp enemyHpPacket = packet as C_EnemyHp;
        ClientSession clientSession = session as ClientSession;

        if (clientSession.Room == null)
        {
            return;
        }

        Console.WriteLine($"Enemy {enemyHpPacket.id}: {enemyHpPacket.hp} / {enemyHpPacket.maxHp}");

        GameRoom room = clientSession.Room;
        room.Push(() => room.EnemyHp(clientSession, enemyHpPacket));
    }
}
