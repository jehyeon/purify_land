using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostEnemy : MonoBehaviour
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

    private Enemy _enemy;
    private Detector _detector;
    private bool isPatrolling = false;
    private bool isAttacking = false;

    private float distanceFromStart;
    private float distanceFromTarget;

    private float temp = 0;
    private float startAttack = 0;
    private float endAttack = 0;

    private void Start()
    {
        _enemy = this.GetComponent<Enemy>();
    }

    private void Update()
    {
        temp += Time.deltaTime;
        if (isAttacking)
        {
            // 공격 중에는 상태 변화, 이동 없음
            return;
        }

        // 벡터, 거리 계산
        distanceFromStart = (_enemy.StartPos - _enemy.transform.position).sqrMagnitude;
        distanceFromTarget = _enemy.Target == null
            ? float.MaxValue
            : (_enemy.Target.transform.position - _enemy.transform.position).sqrMagnitude;

        if (distanceFromStart > _enemy.BackRange)
        {
            // 거리가 초과되는 경우
            if (_enemy.State != State.Back)
            {
                SetSyncState(State.Back);
            }
        }

        if (_enemy.State == State.Idle)
        {
            if (!isPatrolling)
            {
                StartPatrol();
                return;
            }
        }

        if (_enemy.State == State.Back)
        {
            if (distanceFromStart < 0.01)
            {
                SetSyncState(State.Idle);

                StartPatrol();
                return;
            }
        }

        if (_enemy.State == State.Detect)
        {
            if (_enemy.Target == null)
            {
                // !!! TEMP
                return;
            }

            if (distanceFromTarget < _enemy.AttackRange)
            {
                if (!isAttacking)
                {
                    StartCoroutine("AttackToTarget");
                }
            }
        }
    }

    public void SetDetector(Detector detector)
    {
        _detector = detector;
        _detector.SetParent(this);
    }

    // -------------------------------------------------------------------------
    // Sync
    // -------------------------------------------------------------------------
    private void SetSyncState(State state)
    {
        _enemy.SetState(state);
        SendStatePacket(state);
    }

    private void SyncAct(int actionType, bool right)
    {
        _enemy.ActAnimation(actionType);
        this.SendActPacket(actionType, right);
    }

    // -------------------------------------------------------------------------
    // Detect, Target
    // -------------------------------------------------------------------------
    public void DetectPlayer(Player target)
    {
        if (_enemy.State != State.Idle)
        {
            // Idle 상태일때만 감지
            return;
        }

        StopPatrol();
        _enemy.SetTarget(target);

        SendTargetPacket(target.PlayerId);
    }

    // -------------------------------------------------------------------------
    // 공격
    // -------------------------------------------------------------------------
    IEnumerator AttackToTarget()
    {
        startAttack = temp;
        // 공격 준비
        float delay = 1f;        // temp (same Player)
        isAttacking = true;

        // 정지
        SetSyncState(State.Attack);
        _enemy.DestinationPos = _enemy.transform.position;
        SendMovePacket();

        // 공격 애니메이션 재생
        bool right = _enemy.Rotate(_enemy.Target.transform.position - this.transform.position);
        SyncAct(1, right);
        yield return new WaitForSeconds(delay * 0.5f);

        // 거리 갱신
        distanceFromTarget = _enemy.Target == null
            ? float.MaxValue
            : (_enemy.Target.transform.position - _enemy.transform.position).sqrMagnitude;

        if (distanceFromTarget < _enemy.AttackRange * 1.5f)     // !!! TEMP
        {
            // 타겟과의 거리가 멀어지지 않았으면
            DamageToTarget();
        }

        yield return new WaitForSeconds(delay * 0.5f);

        // 공격 종료
        isAttacking = false;
        SetSyncState(State.Detect);
        endAttack = temp;
        Debug.Log($"공격 딜레이 {endAttack - startAttack}");
    }

    private void DamageToTarget()
    {
        if (_enemy.Target == null)
        {
            return;
        }

        int tempDamage = Random.Range(1, 5);
        int[] hpInfo = (_enemy.Target as Character).TakeDamage(tempDamage);    // 대상 체력 감소
        SendHpPacket(_enemy.Target.PlayerId, hpInfo);   // 체력 패킷 전송
    }

    // -------------------------------------------------------------------------
    // 순찰
    // -------------------------------------------------------------------------
    private void StartPatrol()
    {
        StopPatrol();
        isPatrolling = true;
        StartCoroutine("Patrol");
    }

    private void StopPatrol()
    {
        isPatrolling = false;
        StopCoroutine("Patrol");
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            // start position을 기준으로 주변을 순찰
            float patrolX = _enemy.StartPos.x + Random.Range(-1f, 1f);
            float patrolY = _enemy.StartPos.y + Random.Range(-1f, 1f);
            _enemy.DestinationPos = new Vector3(patrolX, patrolY, 0f);

            SendMovePacket();

            yield return new WaitForSeconds(Random.Range(4f, 6f));
        }
    }

    // -------------------------------------------------------------------------
    // 서버 패킷 전송
    // -------------------------------------------------------------------------
    private void SendMovePacket()
    {
        C_EnemyMove movePacket = new C_EnemyMove();

        movePacket.id = _enemy.Id;
        movePacket.posX = _enemy.DestinationPos.x;
        movePacket.posY = _enemy.DestinationPos.y;

        this.NetworkManager.Send(movePacket.Write());
    }

    private void SendTargetPacket(int playerId)
    {
        C_EnemyTarget targetPacket = new C_EnemyTarget();

        targetPacket.id = _enemy.Id;
        targetPacket.playerId = playerId;

        this.NetworkManager.Send(targetPacket.Write());
    }

    private void SendStatePacket(State state)
    {
        C_EnemyState statePacket = new C_EnemyState();

        statePacket.id = _enemy.Id;
        statePacket.state = (int)state;

        this.NetworkManager.Send(statePacket.Write());
    }

    private void SendHpPacket(int playerId, int[] hpInfo)
    {
        // !!! duplicated (same MyPlayer.cs)
        C_PlayerHp hpPacket = new C_PlayerHp();
        hpPacket.playerId = playerId;
        hpPacket.hp = hpInfo[0];
        hpPacket.maxHp = hpInfo[1];

        this.NetworkManager.Send(hpPacket.Write());
    }

    private void SendActPacket(int actionType, bool right)
    {
        C_EnemyAct actPacket = new C_EnemyAct();
        actPacket.id = _enemy.Id;
        actPacket.actionType = actionType;
        actPacket.right = right;

        this.NetworkManager.Send(actPacket.Write());
    }
}
