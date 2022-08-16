using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkEnemyManager
{
    public bool IsHost { get; set; } = false;
    Dictionary<int, Enemy> _enemies = new Dictionary<int, Enemy>();
    public static NetworkEnemyManager Instance { get; } = new NetworkEnemyManager();

    public void EnterGame(S_EnemyList packet)
    {
        // 게임 접속 시 스폰된 Enemy 정보 Load
        for (int i = 0; i < packet.enemyList.Count; i++)
        {
            // !!! packet.enemyList[i].enemyId에 따른 처리는 나중에
            GameObject goEnemy = EnemyManager.Instance.Get();        // !!! zombie로 고정
            Enemy enemy = goEnemy.transform.GetChild(0).GetComponent<Enemy>();

            Vector3 pos = new Vector3(packet.enemyList[i].posX, packet.enemyList[i].posY, 0f);
            enemy.Set(packet.enemyList[i].id, pos, packet.enemyList[i].hp, packet.enemyList[i].maxHp);

            _enemies.Add(packet.enemyList[i].id, enemy);
        }
    }

    public void Spawn(S_BroadcastSpawnEnemy packet)
    {
        // !!! packet.enemyList[i].enemyId에 따른 처리는 나중에
        GameObject goEnemy = EnemyManager.Instance.Get();        // !!! zombie로 고정
        Enemy enemy = goEnemy.transform.GetChild(0).GetComponent<Enemy>() as Enemy;
        if (IsHost)
        {
            // 호스트인 경우
            // Detector 추가
            Object obj = Resources.Load("Prefabs/Detector");
            GameObject goDetector = Object.Instantiate(obj) as GameObject;
            goDetector.transform.SetParent(enemy.transform);
            Detector detector = goDetector.gameObject.GetComponent<Detector>();

            // HostEnemy 컴포넌트 추가
            HostEnemy hostEnemy = enemy.gameObject.AddComponent<HostEnemy>();
            hostEnemy.SetDetector(detector);
        }

        Vector3 pos = new Vector3(packet.posX, packet.posY, 0f);
        enemy.Set(packet.id, pos, packet.hp, packet.maxHp);

        _enemies.Add(packet.id, enemy);
    }

    public void Move(S_BroadcastEnemyMove packet)
    {
        if (!IsHost)
        {
            Enemy enemy = null;
            if (_enemies.TryGetValue(packet.id, out enemy))
            {
                enemy.DestinationPos = new Vector3(packet.posX, packet.posY, 0f);
            }
        }
    }

    public void Target(S_BroadcastEnemyTarget packet)
    {
        if (!IsHost)
        {
            Enemy enemy = null;
            if (_enemies.TryGetValue(packet.id, out enemy))
            {
                Player target = null;
                if (NetworkPlayerManager.Instance.Players.TryGetValue(packet.playerId, out target))
                {
                    enemy.SetTarget(target);
                }
                else
                {
                    Debug.Log("플레이어를 못찾음");
                }
            }
        }
    }

    public void State(S_BroadcastEnemyState packet)
    {
        if (!IsHost)
        {
            Enemy enemy = null;
            if (_enemies.TryGetValue(packet.id, out enemy))
            {
                enemy.SetState(packet.state);
            }
        }
    }

    public void Act(S_BroadcastEnemyAct packet)
    {
        if (!IsHost)
        {
            Enemy enemy = null;
            if (_enemies.TryGetValue(packet.id, out enemy))
            {
                enemy.ActAnimation(packet.actionType);
            }
        }
    }

    public void Hp(S_BroadcastEnemyHp packet)
    {
        // !!! 체력 전송 패킷이 몰리면 버그가 발생할 수 있음
        // !!! 입은 데미지로 수정해야할듯
        Enemy enemy = null;
        if (_enemies.TryGetValue(packet.id, out enemy))
        {
            enemy.SyncHp(packet.hp, packet.maxHp);
        }
    }
}
