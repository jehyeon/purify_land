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
        // StartCoroutine("CoSendPacket");
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
            // 우클릭
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 땅 클릭하면 그 위치로 이동
                if (hit.collider.CompareTag("Ground"))
                {
                    this.DestinationPos = hit.point;
                    CoSendPacket();
                    return;
                }
            }
        }
        // 이동 벡터 계산
        // _h = Input.GetAxisRaw("Horizontal");
        // _v = Input.GetAxisRaw("Vertical");

        // this._moveVec = new Vector2(_h, _v).normalized;
        // // this.DestinationPos = this.transform.position + MoveVec;
        // if (this._moveVec != Vector3.zero)
        // {
        // }
    }

    // -------------------------------------------------------------------------
    // 서버 패킷 전송
    // -------------------------------------------------------------------------
    private void CoSendPacket()
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
    // IEnumerator CoSendPacket()
    // {
    //     // 현재 위치 패킷 전송
    //     while (true)
    //     {
    //         yield return new WaitForSeconds(0.017f);

    //         C_Move movePacket = new C_Move();

    //         movePacket.posX = this._moveVec.x;
    //         movePacket.posY = this._moveVec.y;
    //         // if (this.MoveVec != Vector3.zero)
    //         // {
    //         //     // 정지한 경우 현재 위치를 보냄
    //         //     movePacket.posX = this.transform.position.x;
    //         //     movePacket.posY = this.transform.position.y;
    //         // }
    //         // else
    //         // {
    //         //     // 이동 중인 경우 moveVec을 포함하여 갈 위치를 보냄
    //         //     movePacket.posX = this.DestinationPos.x;
    //         //     movePacket.posY = this.DestinationPos.y;
    //         // }

    //         if (_network == null)
    //         {
    //             _network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    //         }

    //         _network.Send(movePacket.Write());
    //     }
    // }

    public void OK(string text)
    {
        Debug.Log(text);
    }
}
