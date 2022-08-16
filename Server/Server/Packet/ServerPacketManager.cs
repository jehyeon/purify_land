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
		_makeFunc.Add((ushort)PacketID.C_LeaveGame, MakePacket<C_LeaveGame>);
		_handler.Add((ushort)PacketID.C_LeaveGame, PacketHandler.C_LeaveGameHandler);
		_makeFunc.Add((ushort)PacketID.C_Move, MakePacket<C_Move>);
		_handler.Add((ushort)PacketID.C_Move, PacketHandler.C_MoveHandler);
		_makeFunc.Add((ushort)PacketID.C_Act, MakePacket<C_Act>);
		_handler.Add((ushort)PacketID.C_Act, PacketHandler.C_ActHandler);
		_makeFunc.Add((ushort)PacketID.C_PlayerHp, MakePacket<C_PlayerHp>);
		_handler.Add((ushort)PacketID.C_PlayerHp, PacketHandler.C_PlayerHpHandler);
		_makeFunc.Add((ushort)PacketID.C_SpawnCallEnemy, MakePacket<C_SpawnCallEnemy>);
		_handler.Add((ushort)PacketID.C_SpawnCallEnemy, PacketHandler.C_SpawnCallEnemyHandler);
		_makeFunc.Add((ushort)PacketID.C_EnemyMove, MakePacket<C_EnemyMove>);
		_handler.Add((ushort)PacketID.C_EnemyMove, PacketHandler.C_EnemyMoveHandler);
		_makeFunc.Add((ushort)PacketID.C_EnemyTarget, MakePacket<C_EnemyTarget>);
		_handler.Add((ushort)PacketID.C_EnemyTarget, PacketHandler.C_EnemyTargetHandler);
		_makeFunc.Add((ushort)PacketID.C_EnemyState, MakePacket<C_EnemyState>);
		_handler.Add((ushort)PacketID.C_EnemyState, PacketHandler.C_EnemyStateHandler);
		_makeFunc.Add((ushort)PacketID.C_EnemyAct, MakePacket<C_EnemyAct>);
		_handler.Add((ushort)PacketID.C_EnemyAct, PacketHandler.C_EnemyActHandler);
		_makeFunc.Add((ushort)PacketID.C_EnemyHp, MakePacket<C_EnemyHp>);
		_handler.Add((ushort)PacketID.C_EnemyHp, PacketHandler.C_EnemyHpHandler);

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