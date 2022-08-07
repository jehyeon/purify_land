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
}
