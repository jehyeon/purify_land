using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance = null;
    public static EnemyManager Instance { get { return instance; } }

    private Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();

    [SerializeField]
    private Transform spawned;
    [SerializeField]
    private Transform notSpawned;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        Initialize(10);     // !!! temp
    }

    // -------------------------------------------------------------------------
    // 생성
    // -------------------------------------------------------------------------
    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateEnemy());
        }
    }

    private GameObject CreateEnemy()
    {
        // !!! enemy 종류를 늘린 후 스폰 enemy를 선택할 수 있도록
        Object enemyPref = Resources.Load("Prefabs/Enemy/Zombie");
        GameObject enemyObject = Instantiate(enemyPref) as GameObject;
        enemyObject.transform.GetChild(0).GetComponent<Enemy>().Reset();      // 초기화
        enemyObject.gameObject.SetActive(false);
        enemyObject.transform.SetParent(notSpawned);

        return enemyObject;
    }

    // -------------------------------------------------------------------------
    // Get, Return
    // -------------------------------------------------------------------------
    public GameObject Get()
    {
        if (instance.poolingObjectQueue.Count > 0)
        {
            GameObject enemyObject = instance.poolingObjectQueue.Dequeue();
            enemyObject.transform.SetParent(instance.spawned);
            enemyObject.SetActive(true);

            return enemyObject;
        }
        else
        {
            Initialize(10);
            return Get();
        }
    }

    public void Return(GameObject enemyObject)
    {
        enemyObject.SetActive(false);
        enemyObject.transform.SetParent(instance.notSpawned);
        // !!! enemy 종류가 늘어나면 enemy별로 queue 추가
        instance.poolingObjectQueue.Enqueue(enemyObject);
    }
}
