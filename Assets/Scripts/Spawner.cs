using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int enemyCount;

    private void Awake()
    {
        enemyCount = 0;
    }

    //private void Update()
    //{
    //    Spawn();    
    //}

    public void Spawn()
    {
        EnemyManager.Instance.Get();
    }
}
