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
    }

    private new void Update()
    {
        MouseEvent();
        base.Update();
    }

    private void MouseEvent()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 좌클릭
            // !!! 임시
            this.ActAnimation(1);
            this.SendActPacket(1);

            int tempDamage = Random.Range(1, 15);
            SendHpPacket(tempDamage);
            this.Attacked(tempDamage);
        }

        if (Input.GetMouseButtonDown(1))
        {
            // 우클릭
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 땅 클릭하면 그 위치로 이동
                if (hit.collider.CompareTag("Ground"))
                {
                    this.DestinationPos = hit.point;
                    SendMovePacket();
                    return;
                }
            }
        }
    }

    // -------------------------------------------------------------------------
    // 서버 패킷 전송
    // -------------------------------------------------------------------------
    private void SendMovePacket()
    {
        C_Move movePacket = new C_Move();

        movePacket.posX = this.DestinationPos.x;
        movePacket.posY = this.DestinationPos.y;

        if (_network == null)
        {
            _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        }

        _network.Send(movePacket.Write());
    }

    private void SendActPacket(int actionType)
    {
        C_Act actPacket = new C_Act();

        actPacket.actionType = actionType;

        if (_network == null)
        {
            _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        }

        _network.Send(actPacket.Write());
    }

    private void SendHpPacket(int change)
    {
        C_PlayerHp hpPacket = new C_PlayerHp();

        hpPacket.change = change;

        if (_network == null)
        {
            _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        }

        _network.Send(hpPacket.Write());
    }
}
