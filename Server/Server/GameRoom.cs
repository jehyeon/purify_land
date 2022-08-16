using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class GameRoom : IJobQueue
    {
        private int hostSessionId;
        Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();

        // Enemy
        Dictionary<int, Enemy> _enemies = new Dictionary<int, Enemy>();
        private int enemyId = 0;

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

            if (_sessions.Count == 0)
            {
                // 처음 입장한 플레이어가 Host
                // !!! Host 플레이어가 나갔을 때 로직 추가해야 함
                hostSessionId = session.SessionId;
                Console.WriteLine("호스트 입장");
            }
            _sessions.Add(session.SessionId, session);
            session.Room = this;
            session.Player = new Player();
            session.Player.Hp = 100;    // !!! DB를 통해 정보를 가져와야 함 + 스탯 정보 세분화
            session.Player.MaxHp = 100;

            // 입장한 플레이어에게 플레이어 List 전달
            S_PlayerList players = new S_PlayerList();
            foreach (int sessionId in _sessions.Keys)
            {
                ClientSession s = _sessions[sessionId];
                players.playerList.Add(new S_PlayerList.Player()
                {
                    isHost = (hostSessionId == sessionId),
                    isSelf = (s == session),
                    playerId = s.SessionId,
                    posX = s.Player.PosX,
                    posY = s.Player.PosY,
                    hp = s.Player.Hp,
                    maxHp = s.Player.MaxHp
                });
            }
            session.Send(players.Write());

            // Enemy List 전달
            S_EnemyList enemies = new S_EnemyList();
            foreach (int id in _enemies.Keys)
            {
                enemies.enemyList.Add(new S_EnemyList.Enemy()
                {
                    enemyId = _enemies[id].EnemyId,
                    id = _enemies[id].Id,
                    posX = _enemies[id].PosX,
                    posY = _enemies[id].PosY,
                    hp = _enemies[id].Hp,
                    maxHp = _enemies[id].MaxHp
                });
            }
            session.Send(enemies.Write());

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
        public void SpawnCallEnemy(ClientSession session, C_SpawnCallEnemy packet)
        {
            // 스폰 요청
            // Enemies에 추가
            _enemies.Add(++enemyId, new Enemy()
            {
                EnemyId = packet.enemyId,
                Id = enemyId,
                PosX = packet.posX,
                PosY = packet.posY,
                Hp = 100,           // !!! 나중에 DB에서 가져오도록 처리
                MaxHp = 100
            });

            S_BroadcastSpawnEnemy spawn = new S_BroadcastSpawnEnemy();
            spawn.enemyId = _enemies[enemyId].EnemyId;
            spawn.id = _enemies[enemyId].Id;
            spawn.posX = _enemies[enemyId].PosX;
            spawn.posY = _enemies[enemyId].PosY;
            spawn.hp = _enemies[enemyId].Hp;
            spawn.maxHp = _enemies[enemyId].MaxHp;

            Broadcast(spawn.Write());
        }

        public void EnemyMove(ClientSession session, C_EnemyMove packet)
        {
            // Enemy 이동
            // !!! 목적지 패킷이 아니라 현재 위치를 기준으로 전달해야 할듯
            _enemies[packet.id].PosX = packet.posX;
            _enemies[packet.id].PosY = packet.posY;

            // 모든 플레이어에게 전달
            S_BroadcastEnemyMove enemyMovePacket = new S_BroadcastEnemyMove();
            enemyMovePacket.id = packet.id;
            enemyMovePacket.posX = _enemies[packet.id].PosX;
            enemyMovePacket.posY = _enemies[packet.id].PosY;

            Broadcast(enemyMovePacket.Write());
        }

        public void EnemyTarget(ClientSession session, C_EnemyTarget packet)
        {
            _enemies[packet.id].TargetPlayerId = packet.playerId;

            S_BroadcastEnemyTarget enemyTargetPacket = new S_BroadcastEnemyTarget();
            enemyTargetPacket.id = packet.id;
            enemyTargetPacket.playerId = _enemies[packet.id].TargetPlayerId;

            Broadcast(enemyTargetPacket.Write());
        }

        public void EnemyState(ClientSession session, C_EnemyState packet)
        {
            _enemies[packet.id].State = packet.state;

            S_BroadcastEnemyState enemyStatePacket = new S_BroadcastEnemyState();
            enemyStatePacket.id = packet.id;
            enemyStatePacket.state = _enemies[packet.id].State;

            Broadcast(enemyStatePacket.Write());
        }

        public void EnemyAct(ClientSession session, C_EnemyAct packet)
        {
            S_BroadcastEnemyAct enemyActPacket = new S_BroadcastEnemyAct();
            enemyActPacket.id = packet.id;
            enemyActPacket.actionType = packet.actionType;

            Broadcast(enemyActPacket.Write());
        }
    }
}