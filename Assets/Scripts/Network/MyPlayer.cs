using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : Player
{
    // [SerializeField] 
    // private InventoryUI inventory;
    private NetworkManager _network;
    private float _h;
    private float _v;

    protected new void Start()
    {
        base.Start();
        NetworkManager _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        StartCoroutine("CoSendPacket");
    }

    private new void Update()
    {
        ComputeMoveVec();
        base.Update();
    }

    private void ComputeMoveVec()
    {
        // 이동 벡터 계산
        _h = Input.GetAxisRaw("Horizontal");
        _v = Input.GetAxisRaw("Vertical");

        this.MoveVec = new Vector2(_h, _v).normalized;
        this.DetinationPos = this.transform.position + MoveVec;
    }

    // -------------------------------------------------------------------------
    // 서버 패킷 전송
    // -------------------------------------------------------------------------
    IEnumerator CoSendPacket()
    {
        // 현재 위치 패킷 전송
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            C_Move movePacket = new C_Move();
            movePacket.posX = this.transform.position.x;
            movePacket.posY = this.transform.position.y;

            if (_network == null)
            {
                _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
            }

            _network.Send(movePacket.Write());
        }
    }

    public void OK(string text)
    {
        Debug.Log(text);
    }
}
