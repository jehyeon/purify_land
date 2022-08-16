using ServerCore;
using System;
using System.Collections.Generic;

public class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
	Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();
		
	public void Register()
	{
		_makeFunc.Add((ushort)PacketID.S_BroadcastEnterGame, MakePacket<S_BroadcastEnterGame>);
		_handler.Add((ushort)PacketID.S_BroadcastEnterGame, PacketHandler.S_BroadcastEnterGameHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastLeaveGame, MakePacket<S_BroadcastLeaveGame>);
		_handler.Add((ushort)PacketID.S_BroadcastLeaveGame, PacketHandler.S_BroadcastLeaveGameHandler);
		_makeFunc.Add((ushort)PacketID.S_PlayerList, MakePacket<S_PlayerList>);
		_handler.Add((ushort)PacketID.S_PlayerList, PacketHandler.S_PlayerListHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastMove, MakePacket<S_BroadcastMove>);
		_handler.Add((ushort)PacketID.S_BroadcastMove, PacketHandler.S_BroadcastMoveHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastAct, MakePacket<S_BroadcastAct>);
		_handler.Add((ushort)PacketID.S_BroadcastAct, PacketHandler.S_BroadcastActHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastPlayerHp, MakePacket<S_BroadcastPlayerHp>);
		_handler.Add((ushort)PacketID.S_BroadcastPlayerHp, PacketHandler.S_BroadcastPlayerHpHandler);
		_makeFunc.Add((ushort)PacketID.S_EnemyList, MakePacket<S_EnemyList>);
		_handler.Add((ushort)PacketID.S_EnemyList, PacketHandler.S_EnemyListHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastSpawnEnemy, MakePacket<S_BroadcastSpawnEnemy>);
		_handler.Add((ushort)PacketID.S_BroadcastSpawnEnemy, PacketHandler.S_BroadcastSpawnEnemyHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastEnemyMove, MakePacket<S_BroadcastEnemyMove>);
		_handler.Add((ushort)PacketID.S_BroadcastEnemyMove, PacketHandler.S_BroadcastEnemyMoveHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastEnemyTarget, MakePacket<S_BroadcastEnemyTarget>);
		_handler.Add((ushort)PacketID.S_BroadcastEnemyTarget, PacketHandler.S_BroadcastEnemyTargetHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastEnemyState, MakePacket<S_BroadcastEnemyState>);
		_handler.Add((ushort)PacketID.S_BroadcastEnemyState, PacketHandler.S_BroadcastEnemyStateHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastEnemyAct, MakePacket<S_BroadcastEnemyAct>);
		_handler.Add((ushort)PacketID.S_BroadcastEnemyAct, PacketHandler.S_BroadcastEnemyActHandler);

	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
		if (_makeFunc.TryGetValue(id, out func))
		{
			IPacket packet = func.Invoke(session, buffer);
			if (onRecvCallback != null)
			{
				onRecvCallback.Invoke(session, packet);
			}
			else
			{
				HandlerPacket(session, packet);
			}
		}
	}

	T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
	{
		T pkt = new T();
		pkt.Read(buffer);

		return pkt;		
	}

	public void HandlerPacket(PacketSession session, IPacket packet)
	{
		Action<PacketSession, IPacket> action = null;
		if (_handler.TryGetValue(packet.Protocol, out action))
			action.Invoke(session, packet);
	}
}