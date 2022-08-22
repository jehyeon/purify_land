using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : Player
{
    private NetworkManager _networkManager;
    private NetworkManager NetworkManager
    {
        get
        {
            if (_networkManager == null)
            {
                _networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
                return _networkManager;
            }
            else
            {
                return _networkManager;
            }
        }
    }

    private AttackRange attackRange;
    private bool isAttacking;

    private bool canMove = true;

    protected override void Start()
    {
        base.Start();

        // 공격 범위
        attackRange = this.transform.Find("Attack Range").GetComponent<AttackRange>();
        attackRange.Activate();     // MyPlayer만 공격 범위 활성화
    }

    protected override void Update()
    {
        MouseEvent();
        base.Update();
    }

    private void MouseEvent()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 좌클릭
            if (!isAttacking)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    StartCoroutine("Attack", hit.point);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            // 우클릭
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (!canMove)
                {
                    return;
                }

                // 땅 클릭하면 그 위치로 이동
                if (hit.collider.CompareTag("Ground"))
                {
                    Move(hit.point);
                    return;
                }
            }
        }

        // !!! TEMP
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance.IsActivatedOptionUI)
            {
                UIManager.Instance.CloseOptionUI();
            }
            else
            {
                UIManager.Instance.OpenOptionUI();
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ItemManager.Instance.Get(0);
            if (UIManager.Instance.IsActivatedInventoryUI)
            {
                UIManager.Instance.CloseInventoryUI();
            }
            else
            {
                UIManager.Instance.OpenInventoryUI();
            }
        }
    }

    // -------------------------------------------------------------------------
    // 공격, 피격
    // -------------------------------------------------------------------------
    IEnumerator Attack(Vector3 clickPoint)
    {
        // !!! temp 임시로 자연스럽게 공격 속도 조정
        // 나중에 스탯 연동
        // 공격 애니메이션 0.417 * 1.25(x0.75) = 0.52
        float delay = 0.52f;

        Stop();                     // 공격시 정지
        DisableMove();
        bool right = Rotate(clickPoint - this.transform.position);   // 공격 방향으로 회전
        isAttacking = true;

        this.ActAnimation(1);       // 애니메이션 패킷 전송
        this.SendActPacket(1, right);
        yield return new WaitForSeconds(delay * 0.5f);
        DamageToTargets();
        yield return new WaitForSeconds(delay * 0.5f);
        isAttacking = false;            // 공격 중지
        EnableMove();
    }

    private void DamageToTargets()
    {
        int tempDamage = Random.Range(1, 15);
        foreach (Collider2D target in attackRange.Targets)
        {
            // Player
            //Player targetPlayer = target.GetComponent<Player>();
            //int[] hpInfo = (targetPlayer as Character).TakeDamage(tempDamage);     // 대상 체력 감소
            //SendHpPacket(targetPlayer.PlayerId, hpInfo);   // 체력 패킷 전송

            // Enemy
            Enemy targetEnemy = target.GetComponent<Enemy>();
            int[] hpInfo = (targetEnemy as Character).TakeDamage(tempDamage);     // 대상 체력 감소
            SendEnemyHpPacket(targetEnemy.Id, hpInfo);   // 체력 패킷 전송
        }
    }

    // -------------------------------------------------------------------------
    // 이동, 정지, 방향 전환
    // -------------------------------------------------------------------------
    private void Move(Vector3 destination)
    {
        this.DestinationPos = destination;
        SendMovePacket();       // DestinationPos로 패킷 전송
    }

    private void Stop()
    {
        Move(this.transform.position);
    }

    private void EnableMove()
    {
        canMove = true;
    }

    private void DisableMove()
    {
        canMove = false;
    }

    // -------------------------------------------------------------------------
    // 서버 패킷 전송
    // -------------------------------------------------------------------------
    private void SendMovePacket()
    {
        C_Move movePacket = new C_Move();

        movePacket.posX = this.DestinationPos.x;
        movePacket.posY = this.DestinationPos.y;

        NetworkManager.Send(movePacket.Write());
    }

    private void SendActPacket(int actionType, bool right)
    {
        C_Act actPacket = new C_Act();

        actPacket.actionType = actionType;
        actPacket.right = right;

        NetworkManager.Send(actPacket.Write());
    }

    private void SendHpPacket(int playerId, int[] hpInfo)
    {
        C_PlayerHp hpPacket = new C_PlayerHp();
        hpPacket.playerId = playerId;
        hpPacket.hp = hpInfo[0];
        hpPacket.maxHp = hpInfo[1];

        NetworkManager.Send(hpPacket.Write());
    }

    private void SendEnemyHpPacket(int enemyId, int[] hpInfo)
    {
        C_EnemyHp hpPacket = new C_EnemyHp();
        hpPacket.id = enemyId;
        hpPacket.hp = hpInfo[0];
        hpPacket.maxHp = hpInfo[1];

        NetworkManager.Send(hpPacket.Write());
    }
}
