using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkEnemyManager
{
    Dictionary<int, Enemy> _enemies = new Dictionary<int, Enemy>();

    public static NetworkEnemyManager Instance { get; } = new NetworkEnemyManager();

    public void EnterGame(S_EnemyList packet)
    {
        // 게임 접속 시 스폰된 Enemy 정보 Load
        for (int i = 0; i < packet.enemyList.Count; i++)
        {
            // !!! packet.enemyList[i].enemyId에 따른 처리는 나중에
            GameObject goEnemy = EnemyManager.Instance.Get();        // !!! zombie로 고정
            Enemy enemy = goEnemy.GetComponent<Enemy>();

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

        Vector3 pos = new Vector3(packet.posX, packet.posY, 0f);
        enemy.Set(packet.id, pos, packet.hp, packet.maxHp);

        _enemies.Add(packet.id, enemy);
    }

    public void Move(S_BroadcastEnemyMove packet)
    {
        Enemy enemy = null;
        if (_enemies.TryGetValue(packet.id, out enemy))
        {

        }
    }
}
