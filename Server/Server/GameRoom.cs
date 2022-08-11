using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class GameRoom : IJobQueue
    {
        //List<ClientSession> _sessions = new List<ClientSession>();
        Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();

        // Enemy
        Dictionary<int, Enemy> _enemies = new Dictionary<int, Enemy>();

        JobQueue _jobQueue = new JobQueue();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

        public void Push(Action job)
        {
            _jobQueue.Push(job);
        }

        public void Flush()
        {
            foreach (int sessionId in _sessions.Keys)
            {
                _sessions[sessionId].Send(_pendingList);
            }

            _pendingList.Clear();
        }

        public void Broadcast(ArraySegment<byte> segment)
        {
            _pendingList.Add(segment);
        }

        // --------------------------------------------------------------------------
        // Player
        // --------------------------------------------------------------------------
        public void Enter(ClientSession session)
        {
            // 플레이어 입장
            Console.WriteLine(session.SessionId);
            _sessions.Add(session.SessionId, session);
            session.Room = this;
            session.Player.Hp = 100;    // !!! DB를 통해 정보를 가져와야 함 + 스탯 정보 세분화
            session.Player.MaxHp = 100;

            // 입장한 플레이어에게 플레이어 List 전달
            S_PlayerList players = new S_PlayerList();
            foreach (int sessionId in _sessions.Keys)
            {
                ClientSession s = _sessions[sessionId];
                players.players.Add(new S_PlayerList.Player()
                {
                    isSelf = (s == session),
                    playerId = s.SessionId,
                    posX = s.Player.PosX,
                    posY = s.Player.PosY,
                    hp = s.Player.Hp,
                    maxHp = s.Player.MaxHp
                });
            }

            session.Send(players.Write());

            // 기존 플레이어들에게 새로운 플레이어 정보 전달
            S_BroadcastEnterGame enter = new S_BroadcastEnterGame();
            enter.playerId = session.SessionId;
            enter.posX = 0;
            enter.posY = 0;
            enter.hp = 100;     // !!! DB를 통해 정보를 가져와야 함 + 스탯 정보 세분화
            enter.maxHp = 100;

            Broadcast(enter.Write());
        }

        public void Leave(ClientSession session)
        {
            // 플레이어 제거
            _sessions.Remove(session.SessionId);

            // 다른 플레이어들에게 나간 플레이어 정보 전달
            S_BroadcastLeaveGame leave = new S_BroadcastLeaveGame();
            leave.playerId = session.SessionId;

            Broadcast(leave.Write());
        }

        public void Move(ClientSession session, C_Move packet)
        {
            // 플레이어 이동
            session.Player.PosX = packet.posX;
            session.Player.PosY = packet.posY;

            // 다른 플레이어에게 전달
            S_BroadcastMove move = new S_BroadcastMove();
            move.playerId = session.SessionId;
            move.posX = session.Player.PosX;
            move.posY = session.Player.PosY;

            Broadcast(move.Write());
        }

        public void SyncHp(ClientSession session, C_PlayerHp packet)
        {
            // 체력 동기화
            session.Player.Hp = packet.hp;
            session.Player.MaxHp = packet.maxHp;

            // 다른 플레이어에게 전달
            S_BroadcastPlayerHp hpPacket = new S_BroadcastPlayerHp();
            hpPacket.playerId = packet.playerId;
            hpPacket.hp = session.Player.Hp;
            hpPacket.maxHp = session.Player.MaxHp;

            Broadcast(hpPacket.Write());
        }

        public void Act(ClientSession session, C_Act packet)
        {
            // 플레이어 애니메이션 재생
            S_BroadcastAct act = new S_BroadcastAct();
            act.playerId = session.SessionId;
            act.actionType = packet.actionType;
            act.right = packet.right;

            Broadcast(act.Write());
        }

        // --------------------------------------------------------------------------
        // Enemy
        // --------------------------------------------------------------------------
        public void SpawnCallEnemy(ClientSession session, int enemyId)
        {
            // 스폰 요청
        }
    }
}