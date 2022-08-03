using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class GameRoom : IJobQueue
    {
        List<ClientSession> _sessions = new List<ClientSession>();
        JobQueue _jobQueue = new JobQueue();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

        public void Push(Action job)
        {
            _jobQueue.Push(job);
        }

        public void Flush()
        {
            foreach (ClientSession s in _sessions)
            {
                s.Send(_pendingList);
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
            // 플레이어 입장
            _sessions.Add(session);
            session.Room = this;

            // 입장한 플레이어에게 플레이어 List 전달
            S_PlayerList players = new S_PlayerList();
            foreach (ClientSession s in _sessions)
            {
                players.players.Add(new S_PlayerList.Player()
                {
                    isSelf = (s == session),
                    playerId = s.SessionId,
                    posX = s.PosX,
                    posY = s.PosY
                });
            }

            session.Send(players.Write());

            // 기존 플레이어들에게 새로운 플레이어 정보 전달
            S_BroadcastEnterGame enter = new S_BroadcastEnterGame();
            enter.playerId = session.SessionId;
            enter.posX = 0;
            enter.posY = 0;

            Broadcast(enter.Write());
        }

        public void Leave(ClientSession session)
        {
            // 플레이어 제거
            _sessions.Remove(session);

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
        // 피격
        // -------------------------------------------------------------------------
        public void Attacked(ClientSession session, C_PlayerHp packet)
        {
            S_BroadcastPlayerHp act = new S_BroadcastPlayerHp();
            act.playerId = session.SessionId;
            act.change = packet.change;

            Broadcast(act.Write());
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

            Broadcast(act.Write());
        }
    }
}
