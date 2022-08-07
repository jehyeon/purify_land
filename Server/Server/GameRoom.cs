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

        // -------------------------------------------------------------------------
        // 캐릭터 접속, 종료
        // -------------------------------------------------------------------------
        public void Enter(ClientSession session)
        {
            Console.WriteLine(session.SessionId);
            // 플레이어 입장
            _sessions.Add(session.SessionId, session);
            session.Room = this;
            session.hp = 100;     // !!! DB를 통해 정보를 가져와야 함 + 스탯 정보 세분화
            session.maxHp = 100;

            // 입장한 플레이어에게 플레이어 List 전달
            S_PlayerList players = new S_PlayerList();
            foreach (int sessionId in _sessions.Keys)
            {
                ClientSession s = _sessions[sessionId];
                players.players.Add(new S_PlayerList.Player()
                {
                    isSelf = (s == session),
                    playerId = s.SessionId,
                    posX = s.PosX,
                    posY = s.PosY,
                    hp = s.hp,
                    maxHp = s.maxHp
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

        // -------------------------------------------------------------------------
        // 이동
        // -------------------------------------------------------------------------
        public void Move(ClientSession session, C_Move packet)
        {
            // 좌표 이동
            session.PosX = packet.posX;
            session.PosY = packet.posY;

            // 다른 플레이어에게 전달
            S_BroadcastMove move = new S_BroadcastMove();
            move.playerId = session.SessionId;
            move.posX = session.PosX;
            move.posY = session.PosY;

            Broadcast(move.Write());
        }

        // -------------------------------------------------------------------------
        // 체력
        // -------------------------------------------------------------------------
        public void SyncHp(ClientSession session, C_PlayerHp packet)
        {
            // 체력 동기화
            session.hp = packet.hp;
            session.maxHp = packet.maxHp;

            // 다른 플레이어에게 전달
            S_BroadcastPlayerHp hpPacket = new S_BroadcastPlayerHp();
            hpPacket.playerId = packet.playerId;
            hpPacket.hp = session.hp;
            hpPacket.maxHp = session.maxHp;

            Broadcast(hpPacket.Write());
        }

        // -------------------------------------------------------------------------
        // 애니메이션 재생
        // -------------------------------------------------------------------------
        public void Act(ClientSession session, C_Act packet)
        {
            // 다른 플레이어에게 전달
            S_BroadcastAct act = new S_BroadcastAct();
            act.playerId = session.SessionId;
            act.actionType = packet.actionType;
            act.right = packet.right;

            Broadcast(act.Write());
        }
    }
}
