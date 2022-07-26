using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : NetworkPlayer
{
    private NetworkManager _network;
    private Rigidbody2D rigid;
    private float _h;
    private float _v;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        NetworkManager _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        StartCoroutine("CoSendPacket");
    }

    private void Update()
    {
        _h = Input.GetAxisRaw("Horizontal");
        _v = Input.GetAxisRaw("Vertical");
        Vector2 moveVec = new Vector2(_h, _v);

        rigid.velocity = moveVec * 5 * 2;   // !!!
    }

    IEnumerator CoSendPacket()
    {
        // 현재 위치 패킷 전송
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            C_Move movePacket = new C_Move();
            movePacket.posX = Mathf.Round(this.transform.position.x * 100) * 0.01f;
            movePacket.posY = Mathf.Round(this.transform.position.y * 100) * 0.01f;
            Debug.Log(movePacket.posX);

            if (_network == null)
            {
                _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
                Debug.Log("못 찾음");
            }
            _network.Send(movePacket.Write());
        }
    }
}
