using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    [SerializeField]
    private GameObject[] poolingObjectPrefab;

    Queue<EnemyMovement> poolingObjectQueue = new Queue<EnemyMovement>();

    float oriSpawnTime = 2;
    float spawnTime = 1;
    int idx = 0;
    
    public int Idx
    {
        get { return idx; }
    }

    private void Awake()
    {
        Instance = this;

        Initialize(poolingObjectPrefab.Length);
    }

    private void Update()
    {
        spawnTime -= Time.deltaTime;
        if (spawnTime <= 0)
        {
            GetObject();
            // �÷��̾� ���� ��ġ���� Ư�� �Ÿ� �� �����ϰ� �����Ǹ�, ���Ͱ� ��ġ�� �ʰ� �����. 
            // �ʱ⿡ �����Ǹ�, �ٸ������� ��ġ�� �̵�.
            // ���ʿ� �������� �ٱ��� ����.
            idx++;
            spawnTime = oriSpawnTime;
        }
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject(i));
        }
    }

    private EnemyMovement CreateNewObject(int index)
    {
        EnemyMovement newObj = Instantiate(poolingObjectPrefab[index]).GetComponent<EnemyMovement>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static EnemyMovement GetObject()
    {
        if (Instance.poolingObjectQueue.Count > 0)
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            obj.transform.position = new Vector3(Random.Range(-25, 25), Random.Range(-15, 15));
            return obj;
        }
        else
        {
            //var newObj = Instance.CreateNewObject();
            //newObj.gameObject.SetActive(true);
            //newObj.transform.SetParent(null);
            //return newObj;
            return null;
        }
    }

    // ������Ʈ Active false�� ����.(Spawner�� ������)
    public static void ReturnObject(EnemyMovement obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }
}
