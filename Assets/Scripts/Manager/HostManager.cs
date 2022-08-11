using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostManager : MonoBehaviour
{
    // Host에서 일괄적으로 연산 및 패킷 전송

    private static HostManager instance = null;
    public static HostManager Instance { get { return instance; } }

    private NetworkManager _network;

    private float temp = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        NetworkManager _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    private void Update()
    {
        temp += Time.deltaTime;

        if (temp > 5f)
        {
            temp = 0;

            Vector3 tempPos = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0f);
            SendSpawnCallPacket(1, tempPos);
            Debug.Log("Enemy 스폰");
        }
    }

    // -------------------------------------------------------------------------
    // 서버 패킷 전송
    // -------------------------------------------------------------------------
    private void SendSpawnCallPacket(int enemyId, Vector3 pos)
    {
        C_SpawnCallEnemy spawnCallPacket = new C_SpawnCallEnemy();

        spawnCallPacket.enemyId = enemyId;
        spawnCallPacket.posX = pos.x;
        spawnCallPacket.posY = pos.y;

        if (_network == null)
        {
            _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        }

        _network.Send(spawnCallPacket.Write());
    }
}
